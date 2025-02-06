FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /src
COPY ["./src", "/src"]

WORKDIR /src/ChefKnife.Display.WebAPI

RUN dotnet publish -c Release -o /app/publish

CMD ["dotnet", "/app/publish/ChefKnife.Display.WebAPI.dll"]

# Use this line to build image: docker build -f src/ChefKnife.Display.WebAPI/Dockerfile -t display-api-image .
