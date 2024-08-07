FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
COPY . .
RUN dotnet build -o release

FROM  mcr.microsoft.com/dotnet/aspnet:8.0 as publish
EXPOSE 5000
WORKDIR /app
COPY --from=build /app/release .
ENTRYPOINT [ "dotnet", "portfolio.dll" ]