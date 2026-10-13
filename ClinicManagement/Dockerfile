# ---------- Build .NET 8 API ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore first (cached while only code changes)
COPY ClinicManagement.Model/ClinicManagement.Model.csproj ClinicManagement.Model/
COPY ClinicManagement.Data/ClinicManagement.Data.csproj ClinicManagement.Data/
COPY ClinicManagement.Services/ClinicManagement.Services.csproj ClinicManagement.Services/
COPY ClinicManagement.WebApi/ClinicManagement.WebApi.csproj ClinicManagement.WebApi/

# ClinicManagement.Generator is a .NET Framework 4.8 T4 project: not used at runtime and
# not buildable on Linux, so the reference is dropped inside the image only.
RUN sed -i '/ClinicManagement.Generator.csproj/d' ClinicManagement.WebApi/ClinicManagement.WebApi.csproj \
 && dotnet restore ClinicManagement.WebApi/ClinicManagement.WebApi.csproj

COPY ClinicManagement.Model/ ClinicManagement.Model/
COPY ClinicManagement.Data/ ClinicManagement.Data/
COPY ClinicManagement.Services/ ClinicManagement.Services/
COPY ClinicManagement.WebApi/ ClinicManagement.WebApi/

RUN sed -i '/ClinicManagement.Generator.csproj/d' ClinicManagement.WebApi/ClinicManagement.WebApi.csproj \
 && dotnet publish ClinicManagement.WebApi/ClinicManagement.WebApi.csproj \
    -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Writable folders for uploads and logs (mounted as volumes in docker-compose)
RUN mkdir -p wwwroot/Import wwwroot/Logs wwwroot/Template \
 && chown -R app:app wwwroot
USER app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    TZ=Asia/Ho_Chi_Minh

EXPOSE 8080
ENTRYPOINT ["dotnet", "ClinicManagement.WebApi.dll"]
