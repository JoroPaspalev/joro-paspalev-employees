ARG appVersion="1.0.1-beta"

FROM mcr.microsoft.com/dotnet/framework/sdk:4.8.1 AS base
WORKDIR /app
EXPOSE 5599

WORKDIR /src

COPY . .
RUN dotnet restore Couple_Employees.sln

WORKDIR /src/Couple_Employees
RUN dotnet build Couple_Employees.csproj /p:Version=$appVersion -c Release -o /app

FROM base AS publish
ARG appVersion
RUN dotnet publish Couple_Employees.csproj /p:Version=$appVersion -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Couple_Employees.dll", "--urls", "http://0.0.0.0:5599"]
