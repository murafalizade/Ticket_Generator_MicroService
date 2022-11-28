#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["QRTicketGenerator.API/QRTicketGenerator.API.csproj", "QRTicketGenerator.API/"]
RUN dotnet restore "QRTicketGenerator.API/QRTicketGenerator.API.csproj"
COPY . .
WORKDIR "/src/QRTicketGenerator.API"
RUN dotnet build "QRTicketGenerator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QRTicketGenerator.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QRTicketGenerator.API.dll"]