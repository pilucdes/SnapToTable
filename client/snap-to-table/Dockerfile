FROM node:20-bullseye
WORKDIR /app

ENV CI=true

# Install deps first for better caching
COPY client/snap-to-table/package.json ./package.json
COPY client/snap-to-table/package-lock.json ./package-lock.json
RUN npm ci

# Copy source
COPY client/snap-to-table/ .

# Expo dev server ports
EXPOSE 8081

# Default API URL can be overridden at runtime
ENV EXPO_PUBLIC_API_BASE_URL=http://localhost:5261

CMD ["npm", "run", "web"]


