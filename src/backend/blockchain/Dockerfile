FROM node:18-alpine

WORKDIR /app

COPY package*.json ./
RUN npm install --legacy-peer-deps

COPY . .

RUN chmod +x entrypoint.sh

EXPOSE 8545

ENTRYPOINT ["sh", "./entrypoint.sh"]
