FROM node:22-alpine

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci 

COPY . .

RUN chmod +x entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]
