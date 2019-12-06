FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["*.sln", "./"]
COPY ["*/*.csproj", "./"]
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore 
COPY . .
RUN dotnet publish --no-restore "CombinaryStream/CombinaryStream.csproj" -c Release -o /app
RUN chmod -R a-w-x+X /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
ENV ASPNETCORE_URLS="http://+:8080"
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
USER nobody
ENTRYPOINT ["dotnet", "CombinaryStream.dll"]