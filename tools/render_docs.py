"""Render docs/test-cases.md and docs/traceability.md from docs/test-cases.json.

Usage: python tools/render_docs.py
"""

import json
from collections import defaultdict
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
DOCS = ROOT / "docs"


def main() -> None:
    data = json.loads((DOCS / "test-cases.json").read_text(encoding="utf-8"))
    cases = data["test_cases"]
    reset = data["reset_step"]

    lines = [
        "# Test cases: RoomBooking",
        "",
        "Generated from `test-cases.json` by `tools/render_docs.py`. Edit the JSON, then re-run.",
        "",
        "Common preconditions for every case:",
        "",
        *[f"- {item}" for item in data["common_preconditions"]],
        "",
        f"Every case starts with a reset step: {reset['action']} Expected: {reset['expected']}",
        "",
        "| ID | Title | Requirements | Persona | Priority | Type |",
        "| --- | --- | --- | --- | --- | --- |",
    ]
    for case in cases:
        lines.append(
            f"| {case['id']} | {case['title']} | {', '.join(case['requirements'])} | "
            f"{case['persona'] or 'Signed out'} | {case['priority']} | {case['type']} |"
        )
    for case in cases:
        lines += [
            "",
            f"## {case['id']} {case['title']}",
            "",
            case["description"],
            "",
            f"- Requirements: {', '.join(case['requirements'])}",
            f"- Persona: {case['persona'] or 'Signed out'}",
            f"- Priority: {case['priority']}; type: {case['type']}",
            f"- Automated reference: {case['automated_test'] or 'none (UI only)'}",
            "",
            "Preconditions:",
            "",
            *[f"- {item}" for item in case["preconditions"]],
            "",
            "| # | Action | Test data | Expected result |",
            "| --- | --- | --- | --- |",
        ]
        steps = [{**reset, "data": None}, *case["steps"]]
        for number, step in enumerate(steps, start=1):
            data_text = step.get("data") or ""
            lines.append(
                f"| {number} | {_cell(step['action'])} | {_cell(data_text)} | {_cell(step['expected'])} |"
            )
        lines += ["", "Code touchpoints: " + ", ".join(f"`{item}`" for item in case["code_touchpoints"])]
    (DOCS / "test-cases.md").write_text("\n".join(lines) + "\n", encoding="utf-8")

    by_requirement: dict[str, list[str]] = defaultdict(list)
    by_symbol: dict[str, list[str]] = defaultdict(list)
    for case in cases:
        for requirement in case["requirements"]:
            by_requirement[requirement].append(case["id"])
        for symbol in case["code_touchpoints"]:
            by_symbol[symbol].append(case["id"])

    trace = [
        "# Traceability: RoomBooking",
        "",
        "Generated from `test-cases.json` by `tools/render_docs.py`.",
        "",
        "## Requirement to test cases",
        "",
        "| Requirement | Test cases |",
        "| --- | --- |",
        *[
            f"| {requirement} | {', '.join(ids)} |"
            for requirement, ids in sorted(by_requirement.items())
        ],
        "",
        "## Code symbol to test cases",
        "",
        "This is the hand-written ground truth for change-impact analysis: a change to a symbol",
        "should select at least the test cases listed for it.",
        "",
        "| Symbol | Test cases |",
        "| --- | --- |",
        *[f"| `{symbol}` | {', '.join(ids)} |" for symbol, ids in sorted(by_symbol.items())],
    ]
    (DOCS / "traceability.md").write_text("\n".join(trace) + "\n", encoding="utf-8")
    print(f"Rendered {len(cases)} test cases.")


def _cell(text: str) -> str:
    return text.replace("|", "\\|").replace("\n", " ")


if __name__ == "__main__":
    main()
