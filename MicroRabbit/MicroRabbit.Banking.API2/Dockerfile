FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MicroRabbit.Banking.API2/MicroRabbit.Banking.API2.csproj", "MicroRabbit.Banking.API2/"]
RUN dotnet restore "MicroRabbit.Banking.API2/MicroRabbit.Banking.API2.csproj"
COPY . .
WORKDIR "/src/MicroRabbit.Banking.API2"
RUN dotnet build "MicroRabbit.Banking.API2.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroRabbit.Banking.API2.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroRabbit.Banking.API2.dll"]