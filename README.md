# Hotel Booking System

## Опис проєкту

Це десктопний додаток для бронювання готельних номерів, написаний на WPF з патерном MVVM. Користувачі можуть переглядати доступні кімнати, вибирати дати заїзду та виїзду, бронювати, редагувати і скасовувати бронювання. Логіка роботи із даними реалізована через сервісний шар.

---

## Локальний запуск

1. Клонувати репозиторій:
2. Відкрити рішення у Visual Studio (2019 або новіша версія).
3. Відновити NuGet пакети.
4. Запустити проєкт .
5. Авторизуватися або створити користувача і почати бронювання.

---

## Programming Principles

В проєкті дотримано такі принципи:

- **SRP (Single Responsibility Principle)** — кожен клас відповідає за одну задачу.
- **OCP (Open/Closed Principle)** — розширення без зміни існуючого коду.
- **LSP (Liskov Substitution Principle)** — замінність об'єктів базового класу.
- **ISP (Interface Segregation Principle)** — вузькі інтерфейси.
- **Dependency Injection** — впровадження залежностей для легшого тестування.

---

## Design Patterns

- **MVVM (Model-View-ViewModel)**  
  Файли: [BookingView.xaml](https://github.com/parfik110/Lab_6/blob/main/HotelBookingSystem/Views/BookingView.xaml), [BookingViewModel.cs](https://github.com/parfik110/Lab_6/blob/main/HotelBookingSystem/ViewModels/BookingViewModel.cs)  
  Відокремлення UI від логіки і даних.

- **Command Pattern (RelayCommand)**  
  Файли: [RelayCommand.cs](https://github.com/parfik110/Lab_6/blob/main/HotelBookingSystem/ViewModels/RelayCommand.cs), [BookingViewModel.cs](https://github.com/parfik110/Lab_6/blob/main/HotelBookingSystem/ViewModels/BookingViewModel.cs) 
  Команди для кнопок, підтримка MVVM.

- **Repository Pattern (через BookingService)**  
  Файли: [BookingService.cs](https://github.com/parfik110/Lab_6/blob/main/HotelBookingSystem/Services/BookingService.cs)  
  Інкапсуляція логіки роботи з даними.

---

## Refactoring Techniques

Використано такі техніки:

- **Extract Method** — виділення логіки в окремі методи ([CreateBooking](https://github.com/parfik110/Lab_6/blob/0e7dac5308fd83d8c99c11d06ececffd4c1656bb/HotelBookingSystem/Services/BookingService.cs#L20), [CancelBooking](https://github.com/parfik110/Lab_6/blob/0e7dac5308fd83d8c99c11d06ececffd4c1656bb/HotelBookingSystem/Services/BookingService.cs#L47)).
- **Rename Variables** — зрозумілі імена змінних.
- **Encapsulate Field** — властивості з [OnPropertyChanged](https://github.com/parfik110/Lab_6/blob/0e7dac5308fd83d8c99c11d06ececffd4c1656bb/HotelBookingSystem/ViewModels/BookingViewModel.cs#L31-L96) для MVVM.
- **Introduce Command** — заміна обробників подій на команди.
- **Simplify Conditionals** — спрощення умов у перевірках ([CanBookRoom](https://github.com/parfik110/Lab_6/blob/0e7dac5308fd83d8c99c11d06ececffd4c1656bb/HotelBookingSystem/ViewModels/BookingViewModel.cs#L210)).
![Кількість рядків коду](https://github.com/user-attachments/assets/c927b089-b377-4f75-9a66-64729f10480b)
