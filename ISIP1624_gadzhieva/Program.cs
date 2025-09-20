using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreManagement
{
    // Перечисление категорий товаров
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Food,
        Books,
        Sports
    }

    // Класс товара
    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public ProductCategory Category { get; set; }

        public Product(string code, string name, decimal price, int quantity, ProductCategory category)
        {
            Code = code;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price:C}, Количество: {Quantity}, " +
            $"В наличии: {(InStock ? "Да" : "Нет")}, Категория: {Category}";
        }
    }

    class Program
    {
        private static List<StoreManagement.Product> products = new List<StoreManagement.Product>();
        private static int productCounter = 1;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Система управления товарами магазина ===");
            InitializeTestData();
            ShowMainMenu();
        }

        // Инициализация тестовых данных
        static void InitializeTestData()
        {
            AddProduct("Ноутбук HP", 45000m, 10, ProductCategory.Electronics);
            AddProduct("Футболка", 1500m, 25, ProductCategory.Clothing);
            AddProduct("Хлеб", 50m, 100, ProductCategory.Food);
            AddProduct("Война и мир", 800m, 15, ProductCategory.Books);
            AddProduct("Футбольный мяч", 2500m, 8, ProductCategory.Sports);

            Console.WriteLine("Добавлено 5 тестовых товаров.");
        }

        // Генерация уникального кода
        static string GenerateProductCode()
        {
            return "1" + productCounter++.ToString("D4");
        }

        // Главное меню
        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Заказать поставку товара");
                Console.WriteLine("4. Продать товар");
                Console.WriteLine("5. Поиск товаров");
                Console.WriteLine("6. Показать все товары");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProductMenu();
                        break;
                    case "2":
                        DeleteProductMenu();
                        break;
                    case "3":
                        OrderSupplyMenu();
                        break;
                    case "4":
                        SellProductMenu();
                        break;
                    case "5":
                        SearchProductsMenu();
                        break;
                    case "6":
                        DisplayAllProducts();
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

        // Метод для добавления товара (базовый)
        static void AddProduct(string name, decimal price, int quantity, ProductCategory category)
        {
            string code = GenerateProductCode();
            products.Add(new Product(code, name, price, quantity, category));
        }

        // Отображение всех товаров
        static void DisplayAllProducts()
        {
            Console.WriteLine("\n=== ВСЕ ТОВАРЫ ===");
            if (products.Count == 0)
            {
                Console.WriteLine("Товаров нет.");
                return;
            }

            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }

        // Меню добавления товара
        static void AddProductMenu()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ТОВАРА ===");

            // Ввод названия
            string name;
            while (true)
            {
                Console.Write("Введите название товара: ");
                name = Console.ReadLine().Trim();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                Console.WriteLine("Ошибка! Название не может быть пустым.");
            }

            // Ввод цены
            decimal price;
            while (true)
            {
                Console.Write("Введите цену товара: ");
                if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
                {
                    break;
                }
                Console.WriteLine("Ошибка! Введите корректную положительную цену.");
            }

            // Ввод количества
            int quantity;
            while (true)
            {
                Console.Write("Введите количество товара: ");
                if (int.TryParse(Console.ReadLine(), out quantity) && quantity >= 0)
                {
                    break;
                }
                Console.WriteLine("Ошибка! Введите корректное неотрицательное количество.");
            }

            // Выбор категории
            ProductCategory category;
            while (true)
            {
                Console.WriteLine("Выберите категорию:");
                Console.WriteLine("1. Electronics");
                Console.WriteLine("2. Clothing");
                Console.WriteLine("3. Food");
                Console.WriteLine("4. Books");
                Console.WriteLine("5. Sports");
                Console.Write("Ваш выбор (1-5): ");

                string categoryChoice = Console.ReadLine();
                switch (categoryChoice)
                {
                    case "1":
                        category = ProductCategory.Electronics;
                        break;
                    case "2":
                        category = ProductCategory.Clothing;
                        break;
                    case "3":
                        category = ProductCategory.Food;
                        break;
                    case "4":
                        category = ProductCategory.Books;
                        break;
                    case "5":
                        category = ProductCategory.Sports;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор! Попробуйте снова.");
                        continue;
                }
                break;
            }

            // Добавление товара
            string code = GenerateProductCode();
            products.Add(new Product(code, name, price, quantity, category));
            Console.WriteLine($"Товар успешно добавлен! Код товара: {code}");
        }

        // Меню удаления товара
        static void DeleteProductMenu()
        {
            Console.WriteLine("\n=== УДАЛЕНИЕ ТОВАРА ===");

            if (products.Count == 0)
            {
                Console.WriteLine("Товаров для удаления нет.");
                return;
            }

            DisplayAllProducts();

            Console.Write("Введите код товара для удаления: ");
            string code = Console.ReadLine().Trim();

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                products.Remove(product);
                Console.WriteLine($"Товар с кодом {code} успешно удален.");
            }
            else
            {
                Console.WriteLine("Товар с таким кодом не найден.");
            }
        }

        // Меню заказа поставки
        static void OrderSupplyMenu()
        {
            Console.WriteLine("\n=== ЗАКАЗ ПОСТАВКИ ТОВАРА ===");

            if (products.Count == 0)
            {
                Console.WriteLine("Товаров нет.");
                return;
            }

            DisplayAllProducts();

            Console.Write("Введите код товара для заказа поставки: ");
            string code = Console.ReadLine().Trim();

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                int quantity;
                while (true)
                {
                    Console.Write("Введите количество для заказа: ");
                    if (int.TryParse(Console.ReadLine(), out quantity) && quantity > 0)
                    {
                        break;
                    }
                    Console.WriteLine("Ошибка! Введите корректное положительное количество.");
                }

                product.Quantity += quantity;
                Console.WriteLine($"Поставка успешно заказана! Новое количество: {product.Quantity}");
            }
            else
            {
                Console.WriteLine("Товар с таким кодом не найден.");
            }
        }

        // Меню продажи товара
        static void SellProductMenu()
        {
            Console.WriteLine("\n=== ПРОДАЖА ТОВАРА ===");

            if (products.Count == 0)
            {
                Console.WriteLine("Товаров нет.");
                return;
            }

            DisplayAllProducts();

            Console.Write("Введите код товара для продажи: ");
            string code = Console.ReadLine().Trim();

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                if (!product.InStock)
                {
                    Console.WriteLine("Товара нет в наличии!");
                    return;
                }

                int quantity;
                while (true)
                {
                    Console.Write($"Введите количество для продажи (доступно: {product.Quantity}): ");
                    if (int.TryParse(Console.ReadLine(), out quantity) && quantity > 0 && quantity <= product.Quantity)
                    {
                        break;
                    }
                    Console.WriteLine("Ошибка! Введите корректное количество.");
                }

                product.Quantity -= quantity;
                decimal total = product.Price * quantity;
                Console.WriteLine($"Продажа успешно завершена! Продано: {quantity} шт., Сумма: {total:C}");
                Console.WriteLine($"Остаток на складе: {product.Quantity} шт.");
            }
            else
            {
                Console.WriteLine("Товар с таким кодом не найден.");
            }
        }

        // Меню поиска товаров
        static void SearchProductsMenu()
        {
            Console.WriteLine("\n=== ПОИСК ТОВАРОВ ===");
            Console.WriteLine("1. Поиск по коду");
            Console.WriteLine("2. Поиск по названию");
            Console.WriteLine("3. Поиск по категории");
            Console.Write("Выберите тип поиска: ");

            string searchType = Console.ReadLine();

            switch (searchType)
            {
                case "1":
                    SearchByCode();
                    break;
                case "2":
                    SearchByName();
                    break;
                case "3":
                    SearchByCategory();
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }

        // Поиск по коду
        static void SearchByCode()
        {
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine().Trim();

            var product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                Console.WriteLine("\nНайденный товар:");
                Console.WriteLine(product);
            }
            else
            {
                Console.WriteLine("Товар с таким кодом не найден.");
            }
        }

        // Поиск по названию
        static void SearchByName()
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine().Trim().ToLower();

            var foundProducts = products.Where(p => p.Name.ToLower().Contains(name)).ToList();

            if (foundProducts.Count > 0)
            {
                Console.WriteLine($"\nНайдено товаров: {foundProducts.Count}");
                foreach (var product in foundProducts)
                {
                    Console.WriteLine(product);
                }
            }
            else
            {
                Console.WriteLine("Товары с таким названием не найдены.");
            }
        }

        // Поиск по категории
        static void SearchByCategory()
        {
            Console.WriteLine("Выберите категорию для поиска:");
            Console.WriteLine("1. Electronics");
            Console.WriteLine("2. Clothing");
            Console.WriteLine("3. Food");
            Console.WriteLine("4. Books");
            Console.WriteLine("5. Sports");
            Console.Write("Ваш выбор (1-5): ");

            string categoryChoice = Console.ReadLine();
            ProductCategory category;

            switch (categoryChoice)
            {
                case "1":
                    category = ProductCategory.Electronics;
                    break;
                case "2":
                    category = ProductCategory.Clothing;
                    break;
                case "3":
                    category = ProductCategory.Food;
                    break;
                case "4":
                    category = ProductCategory.Books;
                    break;
                case "5":
                    category = ProductCategory.Sports;
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    return;
            }

            var foundProducts = products.Where(p => p.Category == category).ToList();

            if (foundProducts.Count > 0)
            {
                Console.WriteLine($"\nНайдено товаров в категории {category}: {foundProducts.Count}");
                foreach (var product in foundProducts)
                {
                    Console.WriteLine(product);
                }
            }
            else
            {
                Console.WriteLine($"Товары в категории {category} не найдены.");
            }
        }
    }
}