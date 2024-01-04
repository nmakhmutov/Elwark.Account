FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#RUN apt-get update -y
#RUN apt-get install -y python3
#RUN dotnet workload install wasm-tools

COPY "src/Elwark.Account" "src/Elwark.Account"
WORKDIR /src/Elwark.Account
RUN dotnet publish Elwark.Account.csproj -c Release -o /app

FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build /app/wwwroot .
COPY src/Elwark.Account/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
