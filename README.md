# Демонстрационный экзамен 2026 | Программист 💻

## 📌 О проекте
Данный проект представляет собой решение задания Демонстрационного экзамена 2026 года по специальности **09.02.07 Информационные системы и программирование**. 
**Уровень:** Профильный уровень (ПУ).

Репозиторий проекта: [https://github.com/ivankolt/DEMO1](https://github.com/ivankolt/DEMO1)

---

## 🛠 Технологии и база данных
Проект разработан с использованием современных технологий и паттернов:
* **База данных:** PostgreSQL 15.13, compiled by Visual C++ build 1944, 64-bit.
* **Резервная копия:** В корне репозитория находится файл дампа базы данных `DB.sql` для быстрого развертывания.
* **Фреймворк:** Avalonia UI (паттерн MVVM).
* **ORM:** Entity Framework Core.

---

## 🗄 Проектирование базы данных

**Шаг 1. Схема «Сущность-связь» (ER-диаграмма)**
Разработана концептуальная схема, отражающая основные сущности предметной области и связи между ними.

<img width="770" height="789" alt="image" src="https://github.com/user-attachments/assets/6acddcbe-2d1d-4293-8335-4ae61bb12482" />

**Шаг 2. Логическая модель базы данных**
Спроектирована логическая структура БД с указанием типов данных и первичных/внешних ключей (соответствует 3НФ).

<img width="621" height="758" alt="image" src="https://github.com/user-attachments/assets/94b5c589-b0f1-4333-bf48-bea3584a5230" />


**Шаг 3. Физическая модель (ERD)**
Физическая модель базы данных была автоматически сгенерирована с использованием инструмента pgAdmin4.

<img width="1656" height="1533" alt="Untitled" src="https://github.com/user-attachments/assets/fcb87f36-58ce-4427-b463-7c6e956e9438" />

---

## 📥 Заполнение базы данных
Таблицы базы данных были заполнены при помощи встроенного инструмента импорта данных (файлы с пометкой `import` из папки ресурсов). Остальные таблицы, для которых не было исходных файлов, были заполнены вручную тестовыми данными в соответствии с требованиями технического задания.

<img width="1616" height="445" alt="image" src="https://github.com/user-attachments/assets/81a9f979-473e-4b91-b8fc-06a9184a663c" />


---

## 📐 Проектирование системы

**Диаграмма прецедентов (Use Case)**
Отражает роли пользователей (Гость, Клиент, Менеджер, Администратор) и доступный им функционал.
<img width="790" height="586" alt="image" src="https://github.com/user-attachments/assets/1fc01142-6306-4c9f-bd0a-6aa4a3a8b9d2" />


**Блок-схема алгоритма**
Алгоритм работы основного функционала приложения (согласно ГОСТ 19.701-90).
<img width="609" height="779" alt="image" src="https://github.com/user-attachments/assets/d42f1d53-1ffa-43c0-a138-f6e90e796fe0" />


**Проволочная диаграмма (Wireframe)**
Макеты пользовательского интерфейса, спроектированные с учетом требований к юзабилити.
<img width="795" height="671" alt="image" src="https://github.com/user-attachments/assets/ba5e22fc-64f9-42ef-85d9-a1eeb977ceda" />


---

## 💻 Разработка приложения

Проект создан на базе кроссплатформенного фреймворка **Avalonia** с использованием архитектурного паттерна **MVVM** (Model-View-ViewModel).

### 📦 Установка зависимостей (NuGet)
Для работы графического интерфейса и подключения к базе данных через Entity Framework Core в терминале были выполнены следующие команды:

```bash
dotnet add package Avalonia --version 11.3.11
dotnet add package Avalonia.Desktop --version 11.3.11
dotnet add package Avalonia.Themes.Fluent --version 11.3.11
dotnet add package Avalonia.Fonts.Inter --version 11.3.11
dotnet add package Avalonia.Diagnostics --version 11.3.11
dotnet add package CommunityToolkit.Mvvm --version 8.2.1
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.3
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.3
dotnet add package Npgsql --version 9.0.3
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.3
```

### ⚙️ Генерация моделей (Scaffold)
Классы моделей и контекст базы данных (`DemoContext`) были сгенерированы автоматически (Reverse Engineering) при помощи команды:

```bash
dotnet ef dbcontext scaffold "Host=localhost;Database=NAME;Username=NAME;Password=;" Npgsql.EntityFrameworkCore.PostgreSQL -o Models -f
```
---

### Описание графического интерфейса
1. Окно авторизации Первый экран, который видит пользователь. Позволяет ввести логин и пароль для входа в систему под определенной ролью, либо продолжить работу в режиме "Гостя".
<img width="1195" height="716" alt="image" src="https://github.com/user-attachments/assets/cac3f722-4692-497a-af83-f7a630167d9a" />


2. Главное окно (Список товаров) Отображает список товаров, загруженных из БД. Реализован функционал поиска, фильтрации (по поставщику) и сортировки (по количеству). Строки подсвечиваются в зависимости от размера скидки и наличия на складе. Если фото отсутствует, выводится изображение-заглушка.
<img width="1194" height="711" alt="image" src="https://github.com/user-attachments/assets/043587a4-89a9-403a-87c9-fb4df0d64143" />


3. Окно управления товаром (Добавление / Редактирование) Доступно только Администратору. Позволяет изменять данные существующего товара или добавлять новый. Реализована загрузка изображений с сохранением в папку проекта и защита от ввода некорректных данных.
<img width="1199" height="936" alt="image" src="https://github.com/user-attachments/assets/2b953dbe-bd1b-41bf-8124-610bf4cbf210" />


4. Список заказов Раздел, доступный для ролей Менеджер и Администратор. Выводит таблицу с заказами: артикул, статус, адрес пункта выдачи, дата заказа и дата доставки.
<img width="1196" height="710" alt="image" src="https://github.com/user-attachments/assets/ea4256be-081d-4858-9fb2-f29e29100c78" />


---

## 🚀 Как запустить проект

### 1. Клонируйте репозиторий
```bash
git clone https://github.com/ivankolt/DEMO1.git
```

### 2. Подготовьте базу данных
Откройте pgAdmin4 и создайте пустую базу данных, например Auto_Service.

Восстановите структуру и данные, выполнив SQL-скрипт из файла DB.sql, который находится в корне репозитория.

### 3. Настройте подключение
Откройте проект в вашей IDE (Visual Studio, VS Code или Rider).

При необходимости обновите строку подключения (логин, пароль, название БД) в файле:

```text
Models/DemoContext.cs
```

### 4. Запустите приложение
Перейдите в корневую папку проекта и выполните команду:

```bash
dotnet run
```
