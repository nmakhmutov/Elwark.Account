FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . ./
RUN dotnet publish Elwark.Account -c Release -o /app

FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build /app/wwwroot .
COPY Elwark.Account/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
