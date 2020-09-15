FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MicroRabbit.Transfer.API/MicroRabbit.Transfer.API.csproj", "MicroRabbit.Transfer.API/"]
RUN dotnet restore "MicroRabbit.Transfer.API/MicroRabbit.Transfer.API.csproj"
COPY . .
WORKDIR "/src/MicroRabbit.Transfer.API"
RUN dotnet build "MicroRabbit.Transfer.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroRabbit.Transfer.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroRabbit.Transfer.API.dll"]