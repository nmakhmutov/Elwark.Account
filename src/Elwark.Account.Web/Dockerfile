FROM node:14 As node
WORKDIR /app
COPY Elwark.Account.Web/package*.json ./
RUN npm install
COPY Elwark.Account.Web .
RUN npm run build

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
COPY . ./
RUN dotnet publish Elwark.Account.Web -c Release -o /app
COPY --from=node /app/wwwroot/fonts ./wwwroot/fonts
COPY --from=node /app/wwwroot/styles.css ./wwwroot/styles.css
COPY --from=node /app/wwwroot/bundle.js ./wwwroot/bundle.js

FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build /app/wwwroot .
COPY Elwark.Account.Web/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80