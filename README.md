# Веб-приложение для Московского зоопарка
# Мусаев Умахан Рашидович БПИ234
## Описание проекта
Данное веб-приложение разработано для автоматизации ключевых бизнес-процессов Московского зоопарка, включая управление животными, вольерами и расписанием кормления. Приложение построено с использованием принципов **Clean Architecture** и концепций **Domain-Driven Design (DDD)**. Так как это домашнее задание является продолжением прошлого, в решении также присутсвует прошлый проект, и некоторые решения были реализованы с соответсвием прошлого домашнего задания(например, добавлены характеристики для некоторых из животных, iq, длина хвоста и тд.)

---

## Основные возможности

### Управление животными
- Добавление и удаление животных.
- Перемещение животных между вольерами с проверкой совместимости типов.
- Просмотр информации о животных.

### Управление вольерами
- Добавление и удаление вольеров.
- Просмотр информации о вольерах (тип, вместимость, текущее количество животных).
- Уборка вольеров.

### Управление расписанием кормления
- Просмотр расписания кормления для всех животных или конкретного животного.
- Добавление нового расписания кормления.
- Изменение времени кормления.
- Отметка выполнения кормления.

### Статистика зоопарка
- Подсчет количества животных, свободных вольеров и других метрик.
- Просмотр сводной статистики по зоопарку.

---

## Архитектура

### Применение Clean Architecture
- **Domain**:
  - Основные модели: `Animal`, `Enclosure`, `FeedingSchedule`.
  - Доменные события: `AnimalMovedEvent`, `FeedingTimeEvent`.
  - Полностью изолирован от других слоев.
- **Application**:
  - Сервисы: `AnimalTransferService`, `FeedingOrganizationService`, `ZooStatisticsService`.
  - Инкапсулирует бизнес-логику и взаимодействует с репозиториями через интерфейсы.
- **Infrastructure**:
  - Реализует in-memory хранилища: `InMemoryAnimalRepository`, `InMemoryEnclosureRepository`, `InMemoryFeedingScheduleRepository`.
  - Отвечает за взаимодействие с внешними системами.
- **Presentation**:
  - REST API контроллеры: `AnimalController`, `EnclosureController`, `FeedingScheduleController`, `StatisticsController`.
  - Обрабатывает HTTP-запросы и возвращает ответы клиентам.

### Применение Domain-Driven Design
- **Value Objects**:
  - `Food`: Описывает тип и количество пищи.
- **Инкапсуляция бизнес-правил**:
  - `Animal`: Методы `Feed`, `Heal`, `MoveToEnclosure` инкапсулируют логику кормления, лечения и перемещения.
  - `Enclosure`: Методы `AddAnimal`, `RemoveAnimal`, `CanAddAnimal` проверяют совместимость и вместимость вольера.
  - `FeedingSchedule`: Методы `UpdateSchedule`, `MarkAsCompleted` управляют расписанием кормления.
- **Доменные события**:
  - `AnimalMovedEvent`: Генерируется при перемещении животного.
  - `FeedingTimeEvent`: Генерируется при наступлении времени кормления.

---

## Тестирование

### Покрытие тестами
- Покрыто более 65% кода:
  - Контроллеры: `AnimalController`, `EnclosureController`, `FeedingScheduleController`.
  - Доменные объекты: `Animal`, `Enclosure`, `FeedingSchedule`.

### Примеры тестов
- **AnimalControllerTests**:
  - Тесты для добавления, удаления, перемещения животных.
- **FeedingScheduleControllerTests**:
  - Тесты для добавления, изменения и отметки выполнения расписания кормления.
- **EnclosureControllerTests**:
  - Тесты для добавления, удаления и уборки вольеров.

---

## Использование
### Установленные Nuget-пакеты:
WebApp:
![image](https://github.com/user-attachments/assets/87d63052-8b92-4938-92b0-725bea7f64c0)
WebTest:
![image](https://github.com/user-attachments/assets/0464cf35-5139-4072-a333-ec120ab4de79)


### Запуск приложения
1. Склонируйте репозиторий.
2. Перейдите в папку проекта.
3. Выполните команду:
   `dotnet run --project WebApp`
4. Откройте браузер и перейдите по адресу `https://localhost:7028/swagger/index.html` или `https://localhost:5200/swagger/index.html`.

### Тестирование
1. Перейдите в папку тестового проекта.
2. Выполните команду:
   `dotnet test`


---

## Соответствие требованиям

| Требование                          | Реализация                                                            | Местоположение                          |
|-------------------------------------|-----------------------------------------------------------------------|-----------------------------------------|
| Добавить/удалить животное           | Реализовано                                                           | `AnimalController`                      |
| Добавить/удалить вольер             | Реализовано                                                           | `EnclosureController`                   |
| Переместить животное между вольерами| Реализовано                                                           | `AnimalController`, `AnimalTransferService` |
| Просмотреть расписание кормления    | Реализовано                                                           | `FeedingScheduleController`             |
| Добавить новое кормление            | Реализовано                                                           | `FeedingScheduleController`             |
| Просмотреть статистику зоопарка     | Реализовано                                                           | `StatisticsController`, `ZooStatisticsService` |

---
## Ручное тестирование
#### 1. Добавление/удаление животного:
1. Выбираем добавить животное, прописываем характеристики, пример:

![image](https://github.com/user-attachments/assets/72e73f1e-3693-444f-a3a4-ebff8ac9bf58)



!!!ВНИМАНИЕ:
type:
- 0 - Хищное
- 1 - Травоядное
- 2 - Рыба
- 3 - Птица

2. Для удаления животного, с помощью вывода всех животных, смотрим на id нужного нам животного и вводим его в разделе `DELETE`.
   
### 2. Добавление/удаление вольера:
1. Выбираем добавить вольер, прописываем характеристики, пример:

![image](https://github.com/user-attachments/assets/83adc58b-9f04-49b9-898b-92b8ad9656a3)

!!!ВНИМАНИЕ, если `maxCapacity` указать 0, в вольер ничего добавить не получится, так как его иаксимальная вместимость 0.

3. Для удаления вольера, с помощью вывода всех вольеров, смотрим на id нужного нам вольера и вводим его в разделе `DELETE`.

### 3. Перемещение животного между вольерами:

1. Узнаем id нужного нам животного и вольера и в соответствущем разделе вводим их.
   !!!Внимание, type у вольера и животного должны быть одинаковыми для добавления.
   ![image](https://github.com/user-attachments/assets/f73bf7fd-d40e-4d96-b65a-6f7a2fe7e013)

   ![image](https://github.com/user-attachments/assets/e5d4044b-9568-49cf-9ae7-d946b5cc3df6)

### 4. Просмотр расписания кормления
1. Переходим в соответствующий раздел и смотрим расписание
 ![image](https://github.com/user-attachments/assets/f08f5949-c6cd-409d-ab80-4ba5580d17d9)

### 5. Добавление расписания кормления
1. Переходим в соответствующий раздел и добавляем расписание:
![image](https://github.com/user-attachments/assets/d128d0cd-c6c9-45f6-99c8-1ea884aa0ab6)

### 6. Статистика:
1. Выбираем нужную нам статистику и наблюдаем данные, к примеру количество животных по типам:
2. ![image](https://github.com/user-attachments/assets/b7076ab5-985b-4dc7-9d22-6a09796d5c74)

---

# Изменения после продления дедлайна.
Так как дедлайн был продлен на 30 часов, мною было принято решение усовершеннствовать проект, теперь добавление животного, вольера и кормления происходит в более удобном формате:

![image](https://github.com/user-attachments/assets/d67b0722-a9ff-4a39-bd0c-b0f20a1d2c8a)

Тут уже более понятно, как добавить животное, пример заполнения:

![image](https://github.com/user-attachments/assets/268213e2-19d1-4e4b-9d41-cb7ddf9090f2)

Незаполненные разделы являются необязательными, это добавленные специфические свойства.

## Вывод
Программа полностью соответствует требованиям:
- Реализованы все основные функции.
- Соблюдены принципы Clean Architecture.
- Применены концепции Domain-Driven Design.
- Покрытие тестами превышает 65%.

