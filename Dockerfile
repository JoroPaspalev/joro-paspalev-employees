ARG appVersion="1.0.1-beta"

FROM mcr.microsoft.com/dotnet/sdk AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk AS build
ARG appVersion
WORKDIR /src

COPY . .
RUN dotnet restore Couple_Employees.sln

WORKDIR /src/Couple_Employees
RUN dotnet build Couple_Employees.csproj /p:Version=$appVersion -c Release -o /app

FROM build AS publish
ARG appVersion
RUN dotnet publish Couple_Employees.csproj /p:Version=$appVersion -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Couple_Employees.dll"]
