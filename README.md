package main

import (
    "database/sql"
    "log"
    "net/http"
    "os"
    "my-microservice/internal/api"
    "my-microservice/internal/cache"
    "my-microservice/internal/logger"
    "my-microservice/internal/storage"

    _ "github.com/lib/pq"
    "github.com/go-redis/redis/v8"
    "github.com/elastic/go-elasticsearch/v8"
)

func main() {
    // Инициализация логгера Elasticsearch
    es, err := elasticsearch.NewDefaultClient()
    if err != nil {
        log.Fatalf("Error creating Elasticsearch client: %v", err)
    }
    logService := logger.New(es)

    // Инициализация клиента Redis
    redisClient := redis.NewClient(&redis.Options{
        Addr:     "localhost:6379", // адрес и порт Redis
        Password: "",               // пароль (если есть)
        DB:       0,                // используемая база данных
    })
    cacheService := cache.New(redisClient, logService)

    // Инициализация подключения к PostgreSQL
    db, err := sql.Open("postgres", "postgres://user:password@localhost/dbname?sslmode=disable")
    if err != nil {
        logService.Fatal("Error connecting to the database: ", err)
    }
    defer db.Close()
    storageService := storage.NewDBClient(db, logService)

    // Инициализация и настройка HTTP сервера
    httpHandler := api.NewHandler(cacheService, storageService, logService)
    http.HandleFunc("/", httpHandler.ServeHTTP)

    // Запуск HTTP сервера
    port := ":8080"
    if envPort := os.Getenv("PORT"); envPort != "" {
        port = ":" + envPort
    }
    logService.Info("Starting server on port", port)
    if err := http.ListenAndServe(port, nil); err != nil {
        logService.Fatal("Error starting HTTP server: ", err)
    }
}
