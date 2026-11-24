FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY RoomBooking.sln ./
COPY src/RoomBooking.Core/RoomBooking.Core.csproj src/RoomBooking.Core/
COPY src/RoomBooking.Infrastructure/RoomBooking.Infrastructure.csproj src/RoomBooking.Infrastructure/
COPY src/RoomBooking.Web/RoomBooking.Web.csproj src/RoomBooking.Web/
RUN dotnet restore src/RoomBooking.Web/RoomBooking.Web.csproj
COPY src/ src/
RUN dotnet publish src/RoomBooking.Web/RoomBooking.Web.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
USER app
ENTRYPOINT ["dotnet", "RoomBooking.Web.dll"]
