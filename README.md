Данные pet-проект все еще находится в разработке.

На текущий момент в приложение реализована тривиальная работа форму, в котором можно регистрироваться и авторизироваться, создавать форумы и топики к ним. Для данного приложения был реализован простой функционал авторизации.
Большая часть работы была направлена на:
  1. Применение на практики различных сторонних сервисов, таких как Prometheus, Grafana, OpenSearch
  2. Написание xUnit и E2E тестов

В будущем планируется расширение функционала, интеграция с Kafka, увеличение метрик, добавление трейсов.

Для запуска приложения требуется требуется docker desktop. Выполните команду находясь в директории репозитория:
```
docker compose up -d
```

Для работы приложения требуется dotnet 8 и применить миграции к базе данных:
```
dotnet ef database update -p Storage -s API
```
или если с помощью PMC необходимо выбрать проект по умолчанию Storage и выполнить команду:
```
Update-Database
```
