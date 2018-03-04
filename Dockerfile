# https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-prod

# FROM microsoft/dotnet:2.0-sdk AS build-env
FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
#COPY *.csproj ./
#RUN dotnet restore

# copy everything else and build
COPY ./ ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# build runtime image
# FROM microsoft/dotnet:2.0-runtime 
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/ciexe/out ./
ENTRYPOINT ["dotnet", "ciexe.dll"]