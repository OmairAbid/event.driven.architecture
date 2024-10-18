#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Framework/API/API.csproj", "Framework/API/"]
COPY ["Framework/IdentityRepository/IdentityRepository.csproj", "Framework/IdentityRepository/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Framework/EventsourcingRepository/EventSourcingRepository.csproj", "Framework/EventsourcingRepository/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Framework/QueryRepository/QueryRepository.csproj", "Framework/QueryRepository/"]
RUN dotnet restore "Framework/API/API.csproj"
COPY . .
WORKDIR "/src/Framework/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]