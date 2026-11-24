"""Import the hand-written RoomBooking test cases into a Jawdah draft test set.

Local development helper. Jawdah has no API for adding hand-written cases, so this script
writes them with Jawdah's own models, in the same shape the Author agent produces, and moves
the set to review. Approve and publish in the Jawdah UI (or pass --approve).

Run inside the Jawdah backend container:

    docker cp tools/jawdah-import/import_test_cases.py jawdah-backend-1:/tmp/
    docker cp docs/test-cases.json jawdah-backend-1:/tmp/
    docker exec -w /app jawdah-backend-1 .venv/bin/python /tmp/import_test_cases.py \
        --set <test-case-set-id> --cases /tmp/test-cases.json [--list] [--approve] \
        [--map REQ-08=REQ-0042,REQ-0043 ...]

--list prints the set's requirements and the proposed REQ mapping, and writes nothing.
"""

import argparse
import asyncio
import json
import re
import sys
from pathlib import Path
from uuid import UUID

sys.path.insert(0, "/app")

from sqlmodel import select  # noqa: E402

from app.db import session_factory  # noqa: E402
from app.models import CanonicalRequirement, SnapshotPersona  # noqa: E402
from app.models.test_case import (  # noqa: E402
    TestCase,
    TestCaseGenerationStatus,
    TestCaseRequirement,
    TestCaseReviewState,
    TestCaseSet,
    TestCaseSetRequirement,
    TestCaseSetStage,
    TestCaseSetStatus,
    TestCaseStep,
    utc_now,
)

# Keywords that identify each BRD requirement in the Analyst's extracted statements.
KEYWORDS = {
    "REQ-01": [r"sign[- ]?in", r"credential", r"access denied", r"role"],
    "REQ-02": [r"catalogue", r"active rooms", r"list of rooms", r"status column"],
    "REQ-03": [r"add rooms?", r"edit room", r"duplicate name", r"capacity must", r"room name"],
    "REQ-04": [r"deactivat", r"activat"],
    "REQ-05": [r"search", r"available rooms", r"no rooms are available"],
    "REQ-06": [r"22:00", r"in the future", r"on the hour", r"1 to 8"],
    "REQ-07": [r"booking .*confirmed", r"book a room", r"create a booking"],
    "REQ-08": [r"peak"],
    "REQ-09": [r"discount"],
    "REQ-10": [r"invoice", r"vat"],
    "REQ-11": [r"cancel"],
    "REQ-12": [r"reschedul"],
    "REQ-13": [r"notification"],
    "REQ-14": [r"my bookings", r"all bookings", r"booking not found", r"own bookings"],
    "REQ-15": [r"admin.*cancel", r"waive"],
}


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser()
    parser.add_argument("--set", required=True, type=UUID, help="Draft test case set ID")
    parser.add_argument("--cases", required=True, type=Path, help="Path to test-cases.json")
    parser.add_argument("--list", action="store_true", help="Show the mapping and write nothing")
    parser.add_argument("--approve", action="store_true", help="Mark imported cases approved")
    parser.add_argument(
        "--allow-missing-personas",
        action="store_true",
        help="Import cases whose persona is not in the snapshot without a snapshot persona link",
    )
    parser.add_argument(
        "--map",
        action="append",
        default=[],
        help="Override a mapping, for example REQ-08=REQ-0042,REQ-0043",
    )
    return parser.parse_args()


async def main() -> int:
    args = parse_args()
    data = json.loads(args.cases.read_text(encoding="utf-8"))
    cases = data["test_cases"]
    reset = data["reset_step"]

    async with session_factory() as session:
        test_case_set = await session.get(TestCaseSet, args.set)
        if test_case_set is None:
            print("Test case set not found.")
            return 1

        selected = list(
            await session.exec(
                select(TestCaseSetRequirement, CanonicalRequirement)
                .join(
                    CanonicalRequirement,
                    CanonicalRequirement.id == TestCaseSetRequirement.canonical_requirement_id,
                )
                .where(TestCaseSetRequirement.test_case_set_id == args.set)
                .order_by(TestCaseSetRequirement.ordinal)
            )
        )
        mapping = propose_mapping(selected, args.map)
        personas = {
            persona.name.casefold(): persona
            for persona in await session.exec(
                select(SnapshotPersona).where(
                    SnapshotPersona.snapshot_id == test_case_set.application_snapshot_id
                )
            )
        }

        print(f"Set {test_case_set.name} ({test_case_set.status}), {len(selected)} requirements.")
        for item, requirement in selected:
            print(f"  {item.reference}: {requirement.statement[:110]}")
        print("Mapping:")
        for brd_id in sorted(KEYWORDS):
            refs = [requirement.reference for requirement in mapping.get(brd_id, [])]
            print(f"  {brd_id} -> {', '.join(refs) or 'NOT MAPPED'}")
        print(f"Snapshot personas: {', '.join(sorted(personas)) or 'none configured'}")

        unmapped = sorted({req for case in cases for req in case["requirements"]} - set(mapping))
        missing_personas = sorted(
            {
                case["persona"]
                for case in cases
                if case["persona"] and case["persona"].casefold() not in personas
            }
        )
        if personas and missing_personas:
            print(f"Personas missing from the snapshot: {', '.join(missing_personas)}")
        if args.list:
            return 0
        if unmapped:
            print(f"Refusing to import: unmapped requirements {', '.join(unmapped)}. Use --map.")
            return 1
        if personas and missing_personas and not args.allow_missing_personas:
            print("Refusing to import: add the missing personas to the snapshot or rename them.")
            return 1
        if test_case_set.status not in {TestCaseSetStatus.DRAFT, TestCaseSetStatus.REVIEW}:
            print(f"Refusing to import into a {test_case_set.status} set.")
            return 1
        existing = list(
            await session.exec(select(TestCase).where(TestCase.test_case_set_id == args.set))
        )
        if existing:
            print(f"Refusing to import: the set already has {len(existing)} test cases.")
            return 1

        review_state = TestCaseReviewState.APPROVED if args.approve else TestCaseReviewState.PENDING
        for ordinal, case in enumerate(cases, start=1):
            persona = personas.get(case["persona"].casefold()) if case["persona"] else None
            title = f"{case['id']} {case['title']}"[:255]
            note = (
                f"Hand-written {case['id']}. Code touchpoints: "
                + ", ".join(case["code_touchpoints"])
                + f". Automated reference: {case['automated_test'] or 'none'}."
            )
            test_case = TestCase(
                test_case_set_id=args.set,
                ordinal=ordinal,
                generated_title=title,
                reviewed_title=title,
                generated_description=case["description"],
                reviewed_description=case["description"],
                preconditions=[*data["common_preconditions"], *case["preconditions"]],
                generated_start_unauthenticated=case["start_unauthenticated"],
                start_unauthenticated=case["start_unauthenticated"],
                exercises=[],
                persona=persona.name if persona else case["persona"],
                snapshot_persona_id=persona.id if persona else None,
                case_type=case["type"],
                priority=case["priority"],
                grounding_note=note,
                review_state=review_state,
                edit_origin="import",
            )
            session.add(test_case)
            await session.flush()
            steps = [{"action": reset["action"], "expected": reset["expected"]}, *case["steps"]]
            for step_ordinal, step in enumerate(steps, start=1):
                session.add(
                    TestCaseStep(
                        test_case_id=test_case.id,
                        ordinal=step_ordinal,
                        action=step["action"],
                        expected_result=step["expected"],
                        test_data=step.get("data"),
                    )
                )
            linked: set[UUID] = set()
            for brd_id in case["requirements"]:
                for requirement in mapping[brd_id]:
                    if requirement.id in linked:
                        continue
                    linked.add(requirement.id)
                    session.add(
                        TestCaseRequirement(
                            test_case_id=test_case.id,
                            canonical_requirement_id=requirement.id,
                            reference=requirement.reference,
                        )
                    )

        for item, _ in selected:
            item.status = TestCaseGenerationStatus.GENERATED
            item.completed_at = utc_now()
            session.add(item)
        test_case_set.status = TestCaseSetStatus.REVIEW
        test_case_set.stage = TestCaseSetStage.REVIEW
        test_case_set.updated_at = utc_now()
        test_case_set.completed_at = utc_now()
        session.add(test_case_set)
        await session.commit()
        print(f"Imported {len(cases)} test cases. The set is now in review.")
    return 0


def propose_mapping(selected, overrides: list[str]) -> dict[str, list[CanonicalRequirement]]:
    by_reference = {requirement.reference: requirement for _, requirement in selected}
    mapping: dict[str, list[CanonicalRequirement]] = {}
    for brd_id, patterns in KEYWORDS.items():
        matches = [
            requirement
            for _, requirement in selected
            if brd_id in requirement.statement
            or any(re.search(pattern, requirement.statement, re.IGNORECASE) for pattern in patterns)
        ]
        if matches:
            mapping[brd_id] = matches
    for override in overrides:
        brd_id, _, refs = override.partition("=")
        chosen = [by_reference[ref] for ref in refs.split(",") if ref in by_reference]
        if chosen:
            mapping[brd_id.strip()] = chosen
    return mapping


if __name__ == "__main__":
    raise SystemExit(asyncio.run(main()))
