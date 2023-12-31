FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

RUN adduser -u 5679 --disabled-password --gecos "" amorphie-consentuser && chown -R amorphie-consentuser:amorphie-consentuser /app
USER amorphie-consentuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./amorphie.consent/amorphie.consent.csproj"

WORKDIR "/src/."
RUN dotnet build "./amorphie.consent/amorphie.consent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./amorphie.consent/amorphie.consent.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "amorphie.consent.dll"]

