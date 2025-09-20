using System;
using System.Globalization;

namespace ExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Учет ежедневных расходов ===");

            // Ввод количества операций
            int operationsCount = GetOperationsCount();

            // Создание массивов для хранения данных
            string[] names = new string[operationsCount];
            decimal[] amounts = new decimal[operationsCount];

            // Ввод данных о расходах
            InputExpenses(names, amounts);

            // Главное меню
            ShowMainMenu(names, amounts);
        }

        // Метод для получения количества операций
        static int GetOperationsCount()
        {
            int count;
            while (true)
            {
                Console.Write("Введите количество операций (2-40): ");
                if (int.TryParse(Console.ReadLine(), out count) && count >= 2 && count <= 40)
                {
                    return count;
                }
                Console.WriteLine("Ошибка! Введите число от 2 до 40.");
            }
        }

        // Метод для ввода данных о расходах
        static void InputExpenses(string[] names, decimal[] amounts)
        {
            Console.WriteLine("\nВведите данные о расходах в формате: Название; Сумма");
            Console.WriteLine("Пример: Влажные салфетки \"Лента\"; 235");

            for (int i = 0; i < names.Length; i++)
            {
                while (true)
                {
                    Console.Write($"Операция {i + 1}: ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Console.WriteLine("Ошибка! Ввод не может быть пустым.");
                        continue;
                    }

                    string[] parts = input.Split(';');

                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Ошибка! Используйте формат: Название; Сумма");
                        continue;
                    }

                    // Обработка названия
                    string name = parts[0].Trim();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Ошибка! Название не может быть пустым.");
                        continue;
                    }

                    // Обработка суммы
                    if (decimal.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) && amount > 0)
                    {
                        names[i] = name;
                        amounts[i] = amount;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка! Введите корректную сумму (положительное число).");
                    }
                }
            }
        }

        // Главное меню
        static void ShowMainMenu(string[] names, decimal[] amounts)
        {
            while (true)
            {
                Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
                Console.WriteLine("1. Вывод данных");
                Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
                Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
                Console.WriteLine("4. Конвертация валюты");
                Console.WriteLine("5. Поиск по названию");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayData(names, amounts);
                        break;
                    case "2":
                        ShowStatistics(names, amounts);
                        break;
                    case "3":
                        BubbleSort(names, amounts);
                        Console.WriteLine("Данные отсортированы по возрастанию цены.");
                        break;
                    case "4":
                        ConvertCurrency(names, amounts);
                        break;
                    case "5":
                        SearchByName(names, amounts);
                        break;
                    case "0":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор! Попробуйте снова.");
                        break;
                }
            }
        }

        // Вывод всех данных
        static void DisplayData(string[] names, decimal[] amounts)
        {
            Console.WriteLine("\n=== ВСЕ РАСХОДЫ ===");
            Console.WriteLine("№ Название\t\t\tСумма (руб)");
            Console.WriteLine("----------------------------------------");

            decimal total = 0;
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{i + 1,2}. {names[i],-25} {amounts[i],10:F2}");
                total += amounts[i];
            }
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Итого: {total,35:F2} руб.");
        }

        // Статистика
        static void ShowStatistics(string[] names, decimal[] amounts)
        {
            if (amounts.Length == 0) return;

            decimal total = 0;
            decimal max = amounts[0];
            decimal min = amounts[0];
            int maxIndex = 0;
            int minIndex = 0;

            foreach (decimal amount in amounts)
            {
                total += amount;
            }

            for (int i = 1; i < amounts.Length; i++)
            {
                if (amounts[i] > max)
                {
                    max = amounts[i];
                    maxIndex = i;
                }
                if (amounts[i] < min)
                {
                    min = amounts[i];
                    minIndex = i;
                }
            }

            decimal average = total / amounts.Length;

            Console.WriteLine("\n=== СТАТИСТИКА ===");
            Console.WriteLine($"Общая сумма: {total:F2} руб.");
            Console.WriteLine($"Средняя сумма: {average:F2} руб.");
            Console.WriteLine($"Максимальная трата: {max:F2} руб. ({names[maxIndex]})");
            Console.WriteLine($"Минимальная трата: {min:F2} руб. ({names[minIndex]})");
            Console.WriteLine($"Количество операций: {amounts.Length}");
        }

        // Пузырьковая сортировка
        static void BubbleSort(string[] names, decimal[] amounts)
        {
            for (int i = 0; i < amounts.Length - 1; i++)
            {
                for (int j = 0; j < amounts.Length - i - 1; j++)
                {
                    if (amounts[j] > amounts[j + 1])
                    {
                        // Обмен суммами
                        (amounts[j], amounts[j + 1]) = (amounts[j + 1], amounts[j]);

                        // Обмен названиями
                        (names[j], names[j + 1]) = (names[j + 1], names[j]);
                    }
                }
            }
        }

        // Конвертация валюты
        static void ConvertCurrency(string[] names, decimal[] amounts)
        {
            Console.WriteLine("\n=== КОНВЕРТАЦИЯ ВАЛЮТЫ ===");
            Console.WriteLine("Доступные валюты:");
            Console.WriteLine("1. Доллар США (USD)");
            Console.WriteLine("2. Евро (EUR)");
            Console.WriteLine("3. Фунт стерлингов (GBP)");
            Console.WriteLine("4. Йена (JPY)");
            Console.WriteLine("5. Ввести свой курс");
            Console.Write("Выберите валюту: ");

            decimal rate = 0;
            string currencySymbol = "";

            string currencyChoice = Console.ReadLine();
            switch (currencyChoice)
            {
                case "1":
                    rate = 90m; // Примерный курс USD
                    currencySymbol = "USD";
                    break;
                case "2":
                    rate = 98m; // Примерный курс EUR
                    currencySymbol = "EUR";
                    break;
                case "3":
                    rate = 115m; // Примерный курс GBP
                    currencySymbol = "GBP";
                    break;
                case "4":
                    rate = 0.6m; // Примерный курс JPY
                    currencySymbol = "JPY";
                    break;
                case "5":
                    Console.Write("Введите курс конвертации (рублей за 1 единицу валюты): ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal customRate) && customRate > 0)
                    {
                        rate = customRate;
                        Console.Write("Введите символ валюты: ");
                        currencySymbol = Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Неверный курс! Возврат в меню.");
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("Неверный выбор! Возврат в меню.");
                    return;
            }

            Console.WriteLine($"\n=== РАСХОДЫ В {currencySymbol} (курс: 1 {currencySymbol} = {rate} руб.) ===");
            Console.WriteLine("№ Название\t\t\tСумма");
            Console.WriteLine("----------------------------------------");

            decimal total = 0;
            for (int i = 0; i < names.Length; i++)
            {
                decimal convertedAmount = amounts[i] / rate;
                Console.WriteLine($"{i + 1,2}. {names[i],-25} {convertedAmount,10:F2} {currencySymbol}");
                total += convertedAmount;
            }
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Итого: {total,35:F2} {currencySymbol}");
        }

        // Поиск по названию
        static void SearchByName(string[] names, decimal[] amounts)
        {
            Console.WriteLine("\n=== ПОИСК ПО НАЗВАНИЮ ===");
            Console.Write("Введите название для поиска: ");
            string searchTerm = Console.ReadLine().ToLower();

            bool found = false;
            Console.WriteLine("\nРезультаты поиска:");
            Console.WriteLine("№ Название\t\t\tСумма (руб)");
            Console.WriteLine("----------------------------------------");

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].ToLower().Contains(searchTerm))
                {
                    Console.WriteLine($"{i + 1,2}. {names[i],-25} {amounts[i],10:F2}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Ничего не найдено.");
            }
        }
    }
}
