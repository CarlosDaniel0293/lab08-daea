# Usar una imagen base de Node.js
FROM node:14

# Crear el directorio de la aplicación
WORKDIR /app

# Copiar el archivo de configuración y las dependencias
COPY package*.json ./
RUN npm install

# Copiar el resto de la aplicación
COPY . .

# Exponer el puerto que usa la aplicación
EXPOSE 3000

# Comando para ejecutar la aplicación
CMD ["node", "server.js"] 
