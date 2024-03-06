FROM mcr.microsoft.com/dotnet/aspnet:8.0.2 AS base
WORKDIR /app

RUN adduser -u 5679 --disabled-password --gecos "" amorphie-consentuser && chown -R amorphie-consentuser:amorphie-consentuser /app
USER amorphie-consentuser

FROM mcr.microsoft.com/dotnet/sdk:8.0.200 AS build
WORKDIR /app
COPY . .

ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false
RUN dotnet restore "./amorphie.consent/amorphie.consent.csproj"

WORKDIR "/app/."
RUN dotnet build "./amorphie.consent/amorphie.consent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./amorphie.consent/amorphie.consent.csproj" -c Release -o /app/publish

FROM base AS final
COPY amorphie.consent/0125_480.pfx /app
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "amorphie.consent.dll"]