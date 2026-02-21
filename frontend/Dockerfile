# Use Node.js for building
FROM node:20-alpine as build

WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy source code
COPY . .

# Build the app
RUN npm run build

# Use Nginx to serve the built app
FROM nginx:alpine

# Copy the build output to Nginx's html directory
COPY --from=build /app/dist /usr/share/nginx/html

# Copy custom Nginx configuration if needed (optional)
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
