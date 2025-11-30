# Stage 1: Build the React Application
FROM node:25-alpine as build
WORKDIR /app

# Copy package.json first to cache npm install
COPY package.json package-lock.json ./
RUN npm ci

# Copy source code and build
COPY . .
RUN npm run build

# Stage 2: Serve with Nginx
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# 1. Remove the default Nginx configuration
RUN rm /etc/nginx/conf.d/default.conf

# 2. Copy your custom configuration file
COPY nginx.conf /etc/nginx/conf.d/default.conf

# 3. Copy the React build output (Use /dist if using Vite, /build if CRA)
COPY --from=build /app/dist .

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]