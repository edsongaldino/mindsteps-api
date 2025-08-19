# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1) Copie SOMENTE os .csproj (melhor cache)
COPY MindSteps.API/MindSteps.API.csproj MindSteps.API/
COPY MindSteps.Application/MindSteps.Application.csproj MindSteps.Application/
COPY MindSteps.Domain/MindSteps.Domain.csproj MindSteps.Domain/
COPY MindSteps.Infrastructure/MindSteps.Infrastructure.csproj MindSteps.Infrastructure/
COPY MindSteps.SharedKernel/MindSteps.SharedKernel.csproj MindSteps.SharedKernel/

# 2) Restore baseado no csproj da API (que referencia as demais)
RUN dotnet restore MindSteps.API/MindSteps.API.csproj

# 3) Agora copie o restante do código
COPY . .

# 4) Publish (Release)
RUN dotnet publish MindSteps.API/MindSteps.API.csproj -c Release -o /out

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out ./

# API escutando na porta 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# DLL exata (case-sensitive em Linux)
ENTRYPOINT ["dotnet", "MindSteps.API.dll"]
