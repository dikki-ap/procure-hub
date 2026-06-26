# Stage 1: Build React frontend
FROM node:20-alpine AS frontend-build
WORKDIR /app/frontend
COPY frontend/package*.json ./
RUN npm ci --silent
COPY frontend/ ./
RUN npm run build

# Stage 2: Restore .NET dependencies (layer-cached)
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS backend-restore
WORKDIR /src
COPY Directory.Packages.props .
COPY ["src/ProcureHub.API/ProcureHub.API.csproj",                                          "src/ProcureHub.API/"]
COPY ["src/ProcureHub.SharedKernel/ProcureHub.SharedKernel.csproj",                        "src/ProcureHub.SharedKernel/"]
COPY ["src/Modules/MasterData/ProcureHub.Modules.MasterData.csproj",                       "src/Modules/MasterData/"]
COPY ["src/Modules/VendorManagement/ProcureHub.Modules.VendorManagement.csproj",           "src/Modules/VendorManagement/"]
COPY ["src/Modules/Procurement/ProcureHub.Modules.Procurement.csproj",                     "src/Modules/Procurement/"]
COPY ["src/Modules/ApprovalEngine/ProcureHub.Modules.ApprovalEngine.csproj",               "src/Modules/ApprovalEngine/"]
COPY ["src/Modules/DocumentManagement/ProcureHub.Modules.DocumentManagement.csproj",       "src/Modules/DocumentManagement/"]
COPY ["src/Modules/Notifications/ProcureHub.Modules.Notifications.csproj",                 "src/Modules/Notifications/"]
COPY ["src/Modules/Analytics/ProcureHub.Modules.Analytics.csproj",                         "src/Modules/Analytics/"]
RUN dotnet restore "src/ProcureHub.API/ProcureHub.API.csproj"

# Stage 3: Build and publish
FROM backend-restore AS backend-build
COPY . .
RUN dotnet publish "src/ProcureHub.API/ProcureHub.API.csproj" \
    -c Release -o /publish --no-restore

# Stage 4: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
COPY --from=backend-build /publish .
COPY --from=frontend-build /app/frontend/dist ./wwwroot

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ProcureHub.API.dll"]
