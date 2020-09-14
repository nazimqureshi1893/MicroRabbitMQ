FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["MicroRabbit.Banking.API/MicroRabbit.Banking.API.csproj", "MicroRabbit.Banking.API/"]
RUN dotnet restore "MicroRabbit.Banking.API/MicroRabbit.Banking.API.csproj"
COPY . .
WORKDIR "/src/MicroRabbit.Banking.API"
RUN dotnet build "MicroRabbit.Banking.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroRabbit.Banking.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroRabbit.Banking.API.dll"]