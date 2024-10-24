# File ./Dockerfile
FROM node:20.11.1-alpine3.19 AS build
WORKDIR /app
COPY package.json ./
RUN yarn install
ENV PATH /app/node_modules/.bin:$PATH
COPY . .
RUN yarn run build

FROM alpine:latest
RUN mkdir /app-output
COPY --from=build /app/dist /app-output
CMD ["cp", "-r", "/app-output/.", "/data/app"]