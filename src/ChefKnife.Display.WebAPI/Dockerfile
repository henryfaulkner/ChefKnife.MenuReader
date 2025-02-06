FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /src
COPY ["./src", "/src"]

EXPOSE 5268

ENV ASPNETCORE_URLS="http://0.0.0.0:5268" \
    ASPNETCORE_ENVIRONMENT=Development

WORKDIR /src/ChefKnife.Display.WebAPI

RUN dotnet publish -c Release -o /app/publish

CMD ["dotnet", "/app/publish/ChefKnife.Display.WebAPI.dll"]

# Build image with: docker build -f src/ChefKnife.Display.WebAPI/Dockerfile -t display-api-image .
# Run image with: docker run -d -p 5268:5268 --name display-api-container display-api-image