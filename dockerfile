#base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /app

COPY *.sln ./
COPY StoreBL/*.csproj StoreBL/
COPY StoreDL/*.csproj StoreDL/
COPY StoreTests/*.csproj StoreTests/
COPY StoreWebUI/*.csproj StoreWebUI/
COPY StoreModels/*.csproj StoreModels/

RUN cd StoreWebUI && dotnet restore

COPY . ./
# CMD /bin/bash

RUN dotnet publish StoreWebUI -c Release -o publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish ./

CMD ["dotnet", "StoreWebUI.dll"]