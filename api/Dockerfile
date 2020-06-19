FROM mcr.microsoft.com/dotnet/core/sdk:3.1.301-buster AS build
WORKDIR /src
COPY . .
RUN dotnet publish ./ --configuration Release --output /publish -warnaserror

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.5-buster-slim
WORKDIR /app
COPY --from=build /publish ./
EXPOSE 80
ENTRYPOINT [ "dotnet", "api.dll" ]


