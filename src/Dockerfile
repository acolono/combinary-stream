FROM microsoft/dotnet:2.2-sdk-alpine AS build
WORKDIR /src
COPY ["*.sln", "./"]
COPY ["*/*.csproj", "./"]
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet restore 
COPY . .
RUN dotnet publish --no-restore "CombinaryStream/CombinaryStream.csproj" -c Release -o /app
RUN chmod -R a-w-x+X /app

FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /app
ENV ASPNETCORE_URLS="http://+:8080"
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
USER nobody
ENTRYPOINT ["dotnet", "CombinaryStream.dll"]