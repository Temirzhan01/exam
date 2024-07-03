# Использование базового образа .NET
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

# Установка зависимостей для libwkhtmltox
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    libgdiplus \
    libc6-dev \
    libx11-dev \
    libgdk-pixbuf2.0-0 \
    libxext6 \
    libxrender1 \
    libfontconfig1 \
    libxtst6 \
    libglib2.0-0 \
    libgl1-mesa-glx \
    xfonts-75dpi \
    xfonts-base \
    wget && \
    rm -rf /var/lib/apt/lists/*

# Загрузка и установка libwkhtmltox
RUN wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6-1/wkhtmltox_0.12.6-1.bionic_amd64.deb && \
    dpkg -i wkhtmltox_0.12.6-1.bionic_amd64.deb && \
    apt-get install -f && \
    rm wkhtmltox_0.12.6-1.bionic_amd64.deb

# Копирование папки HTMLDocuments и ее содержимого в контейнер
COPY HTMLDocuments /app/HTMLDocuments/

# Публикация и сборка проекта
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish "YourProjectName.csproj" -c Release -o /app/publish

# Запуск приложения
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "YourProjectName.dll"]
