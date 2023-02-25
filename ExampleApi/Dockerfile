FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /api
COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
EXPOSE 4000
ENV ASPNETCORE_URLS=http://+:4000
WORKDIR /api
COPY --from=build /api/out .
ENTRYPOINT ["dotnet", "ExampleApi.dll"]