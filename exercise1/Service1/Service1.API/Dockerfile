FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
 
COPY ["Service1.API/Service1.API.csproj", "Service1.API/"]
COPY ["Service1.BLL/Service1.BLL.csproj", "Service1.BLL/"]
COPY ["Service1.DAL/Service1.DAL.csproj", "Service1.DAL/"]
RUN dotnet restore "Service1.API/Service1.API.csproj"
 
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
 
COPY --from=build /app/publish .
 
EXPOSE 8199
 
CMD ["dotnet", "Service1.API.dll"]