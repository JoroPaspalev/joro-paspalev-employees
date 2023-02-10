ARG appVersion="1.0.1-beta"

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG appVersion
WORKDIR /src
COPY Distress.Service/Distress.Service.csproj Distress.Service/
RUN dotnet restore Distress.Service/Distress.Service.csproj
COPY Retry/Retry.csproj Retry/
RUN dotnet restore Retry/Retry.csproj
COPY Common/CommonProperties/CommonProperties.csproj CommonProperties/
RUN dotnet restore CommonProperties/CommonProperties.csproj
COPY . .
WORKDIR /src/Distress.Service
RUN dotnet build Distress.Service.csproj /p:Version=$appVersion -c Release -o /app

FROM build AS publish
ARG appVersion
RUN dotnet publish Distress.Service.csproj /p:Version=$appVersion -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Distress.Service.dll"]
