FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . ./
RUN apt-get update -y
RUN apt-get install -y python3
RUN dotnet workload install wasm-tools
RUN dotnet publish Elwark.Account -c Release -o /app

FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build /app/wwwroot .
COPY Elwark.Account/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
