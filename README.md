# Task planner
Многопользовательский планировщик задач. Пользователи могут использовать его в качестве TODO листа.

## Использованные технологии

- .NET 8.0
- ASP.NET Core 
- Dapper, EF Core, AutoMapper, Coravel, Swagger, SmtpClient
- PostgreSQL
- Kafka
- JWT
- Docker
- Amazon EC2 

## Микросервисы

При разработке проекта использовался микросервисный подход. Проект включает в себя 4 микросервиса:

- [taskplanner-user-service](https://github.com/me1ncun/task-tracker/tree/master/taskplanner-user-service)
- [taskplanner-scheduler](https://github.com/me1ncun/task-tracker/tree/master/taskplanner-scheduler)
- [taskplanner-mailservice](https://github.com/me1ncun/task-tracker/tree/master/taskplanner-mailservice)
- [taskplanner-ui](https://github.com/me1ncun/task-tracker/tree/master/taskplanner-ui)

### [taskplanner-user-service](https://github.com/me1ncun/task-tracker/tree/master/taskplanner-user-service)
ASP.NET Core приложение, реализующее REST API для работы с пользователями и задачами.

Предоставление доступа к функционалу реализовано с помощью JWT-аутентификации.

Работа с пользователями:

- Регистрация
- Авторизация

Работа с задачами (доступно только авторизованым пользователям):

- Создание
- Чтение
- Редактирование(Изменение заголовка и описания, пометка задачи, как сделанной)
- Удаление

### [taskplanner-mailservice](https://github.com/me1ncun/task-tracker/tree/master/taskplanner-mailservice)
ASP.NET Core приложение с двумя модулями - Net.Mail и Kafka (Consumer).

С помощью Kafka приложение подключается  и создает очередь, после чего подписывается на сообщения, поступающие от планировщика и бэкенд-сервиса.

Для каждого полученного сообщения, содержимое которого десериализуется в экземпляр модели, используется Net.Mail, чтобы отправить письмо.

### [taskplanner-scheduler](https://github.com/me1ncun/task-tracker/tree/master/taskplanner-scheduler)
ASP.NET Core приложение с двумя модулями - Coravel и и Kafka (Producer).

Задача сервиса - раз в сутки итерировать всех пользователей, формировать для них отчёты об отогах дня, а также формировать email для отправки. Сформированные сообщения отправляются в Kafka очередь.

### [taskplanner-ui](https://github.com/me1ncun/task-tracker/tree/master/taskplanner-ui)
Веб-приложение является одностраничным, всё общение с сервером происходит с помощью Javascript/Ajax, интерфейс обновляется через jQuery.

## Запуск на локальном компьютере
Необходимо:

- Установить Docker на локальную машину — https://docs.docker.com/get-docker/
- Склонировать репозиторий task-planner
```bash
git clone https://github.com/me1ncun/task-planner.git
```
- Перейти в каталог приложение
```bash
cd task-planner
```

В .env файл вставить свои данные для настройки портов и подключению к базе данных.
- Запустить docker-compose файл
```bash
docker-compose up
```
- Страница с документацией будет доступна по ссылке: http://localhost:8080/swagger/index.html

