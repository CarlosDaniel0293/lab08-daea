# Dockerfile for Flask (frontend)
FROM python:3.8-slim

# Crear directorio de trabajo
WORKDIR /app

# Copiar los archivos requeridos
COPY . /app

# Instalar las dependencias
RUN pip install -r requirements.txt

# Exponer el puerto en el que correrá la app
EXPOSE 5000

# Comando para correr la aplicación
CMD ["python", "app.py"]
