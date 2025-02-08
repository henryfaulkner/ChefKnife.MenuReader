FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /src
COPY ["./src", "/src"]

EXPOSE 8080

ENV ASPNETCORE_URLS="http://0.0.0.0:8080"

WORKDIR /src/ChefKnife.Display.UI

RUN dotnet publish -c Release -o /app/publish

CMD ["dotnet", "/app/publish/ChefKnife.Display.UI.dll"]

# Build image with: docker build -f src/ChefKnife.Display.UI/Dockerfile -t display-ui-image .
# Run image with: docker run -d -p 8080:8080 --name display-ui-container display-ui-image