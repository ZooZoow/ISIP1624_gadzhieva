using System;
using System.Collections.Generic;

namespace LibraryManagement
{
    public enum Genre
    {
        Fantasy,
        ScienceFiction,
        Mystery,
        Romance,
        Horror
    }

    public class Book
    {
        private static int nextId = 1;

        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public Book(string title, string author, Genre genre, int year, decimal price)
        {
            Id = nextId++;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Название: \"{Title}\", Автор: {Author}, Жанр: {Genre}, Год: {Year}, Цена: {Price:C}";
        }
    }

    public class BookService
    {
        private List<Book> books = new List<Book>();

        public BookService()
        {
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            books.Add(new Book("Властелин Колец", "Дж. Р. Р. Толкин", Genre.Fantasy, 1954, 1200m));
            books.Add(new Book("1984", "Джордж Оруэлл", Genre.ScienceFiction, 1949, 850m));
            books.Add(new Book("Убийство в Восточном экспрессе", "Агата Кристи", Genre.Mystery, 1934, 750m));
            books.Add(new Book("Гордость и предубеждение", "Джейн Остин", Genre.Romance, 1813, 680m));
            books.Add(new Book("Дракула", "Брэм Стокер", Genre.Horror, 1897, 920m));
        }

        public bool AddBook(string title, string author, Genre genre, int year, decimal price)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Ошибка: Название книги не может быть пустым.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("Ошибка: Автор не может быть пустым.");
                return false;
            }

            if (year < 1000 || year > DateTime.Now.Year)
            {
                Console.WriteLine("Ошибка: Неверный год издания.");
                return false;
            }

            if (price < 0)
            {
                Console.WriteLine("Ошибка: Цена не может быть отрицательной.");
                return false;
            }

            Book book = new Book(title.Trim(), author.Trim(), genre, year, price);
            books.Add(book);
            Console.WriteLine("Книга успешно добавлена! ID: " + book.Id);
            return true;
        }

        public bool RemoveBook(int id)
        {
            Book bookToRemove = null;
            foreach (Book book in books)
            {
                if (book.Id == id)
                {
                    bookToRemove = book;
                    break;
                }
            }

            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                Console.WriteLine("Книга с ID " + id + " успешно удалена.");
                return true;
            }
            else
            {
                Console.WriteLine("Книга с ID " + id + " не найдена.");
                return false;
            }
        }

        public List<Book> FindByTitle(string title)
        {
            List<Book> result = new List<Book>();
            foreach (Book book in books)
            {
                if (book.Title.ToLower().Contains(title.ToLower()))
                {
                    result.Add(book);
                }
            }
            return result;
        }

        public List<Book> FindByAuthor(string author)
        {
            List<Book> result = new List<Book>();
            foreach (Book book in books)
            {
                if (book.Author.ToLower().Contains(author.ToLower()))
                {
                    result.Add(book);
                }
            }
            return result;
        }

        public List<Book> FindByGenre(Genre genre)
        {
            List<Book> result = new List<Book>();
            foreach (Book book in books)
            {
                if (book.Genre == genre)
                {
                    result.Add(book);
                }
            }
            return result;
        }

        public List<Book> SortByTitle()
        {
            List<Book> sorted = new List<Book>(books);
            sorted.Sort((x, y) => x.Title.CompareTo(y.Title));
            return sorted;
        }

        public List<Book> SortByYear()
        {
            List<Book> sorted = new List<Book>(books);
            sorted.Sort((x, y) => x.Year.CompareTo(y.Year));
            return sorted;
        }

        public Book GetMostExpensiveBook()
        {
            if (books.Count == 0) return null;

            Book mostExpensive = books[0];
            foreach (Book book in books)
            {
                if (book.Price > mostExpensive.Price)
                {
                    mostExpensive = book;
                }
            }
            return mostExpensive;
        }

        public Book GetCheapestBook()
        {
            if (books.Count == 0) return null;

            Book cheapest = books[0];
            foreach (Book book in books)
            {
                if (book.Price < cheapest.Price)
                {
                    cheapest = book;
                }
            }
            return cheapest;
        }

        public void DisplayBooksByAuthor()
        {
            Dictionary<string, int> authorCount = new Dictionary<string, int>();

            foreach (Book book in books)
            {
                if (authorCount.ContainsKey(book.Author))
                {
                    authorCount[book.Author]++;
                }
                else
                {
                    authorCount[book.Author] = 1;
                }
            }

            Console.WriteLine("\nКоличество книг по авторам:");
            foreach (var pair in authorCount)
            {
                Console.WriteLine("Автор: " + pair.Key + ", Количество книг: " + pair.Value);
            }
        }

        public void DisplayAllBooks()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("В библиотеке нет книг.");
                return;
            }

            Console.WriteLine("\nВсе книги в библиотеке:");
            foreach (Book book in books)
            {
                Console.WriteLine(book);
            }
        }
    }

    class Program
    {
        private static BookService bookService = new BookService();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Добро пожаловать в систему управления библиотекой!");

            bool isRunning = true;
            while (isRunning)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewBook();
                        break;
                    case "2":
                        RemoveBook();
                        break;
                    case "3":
                        SearchBooks();
                        break;
                    case "4":
                        SortBooks();
                        break;
                    case "5":
                        ShowPriceExtremes();
                        break;
                    case "6":
                        bookService.DisplayBooksByAuthor();
                        break;
                    case "7":
                        bookService.DisplayAllBooks();
                        break;
                    case "8":
                        isRunning = false;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите действие от 1 до 8.");
                        break;
                }

                if (isRunning)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\n=== МЕНЮ УПРАВЛЕНИЯ БИБЛИОТЕКОЙ ===");
            Console.WriteLine("1. Добавить новую книгу");
            Console.WriteLine("2. Удалить книгу по ID");
            Console.WriteLine("3. Найти книги");
            Console.WriteLine("4. Сортировать книги");
            Console.WriteLine("5. Показать самую дорогую и дешёвую книгу");
            Console.WriteLine("6. Показать количество книг по авторам");
            Console.WriteLine("7. Показать все книги");
            Console.WriteLine("8. Выйти из программы");
            Console.Write("Выберите действие (1-8): ");
        }

        static void AddNewBook()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОЙ КНИГИ ===");

            Console.Write("Введите название книги: ");
            string title = Console.ReadLine();

            Console.Write("Введите автора: ");
            string author = Console.ReadLine();

            Console.WriteLine("Доступные жанры:");
            Console.WriteLine("1. Fantasy");
            Console.WriteLine("2. ScienceFiction");
            Console.WriteLine("3. Mystery");
            Console.WriteLine("4. Romance");
            Console.WriteLine("5. Horror");

            Console.Write("Выберите жанр (1-5): ");
            string genreInput = Console.ReadLine();
            Genre genre;
            switch (genreInput)
            {
                case "1": genre = Genre.Fantasy; break;
                case "2": genre = Genre.ScienceFiction; break;
                case "3": genre = Genre.Mystery; break;
                case "4": genre = Genre.Romance; break;
                case "5": genre = Genre.Horror; break;
                default:
                    Console.WriteLine("Неверный выбор жанра.");
                    return;
            }

            Console.Write("Введите год издания: ");
            string yearInput = Console.ReadLine();
            if (!int.TryParse(yearInput, out int year))
            {
                Console.WriteLine("Неверный формат года.");
                return;
            }

            Console.Write("Введите цену: ");
            string priceInput = Console.ReadLine();
            if (!decimal.TryParse(priceInput, out decimal price))
            {
                Console.WriteLine("Неверный формат цены.");
                return;
            }

            bookService.AddBook(title, author, genre, year, price);
        }

        static void RemoveBook()
        {
            Console.WriteLine("\n=== УДАЛЕНИЕ КНИГИ ===");
            Console.Write("Введите ID книги для удаления: ");

            string input = Console.ReadLine();
            if (int.TryParse(input, out int id))
            {
                bookService.RemoveBook(id);
            }
            else
            {
                Console.WriteLine("Неверный формат ID.");
            }
        }

        static void SearchBooks()
        {
            Console.WriteLine("\n=== ПОИСК КНИГ ===");
            Console.WriteLine("1. Поиск по названию");
            Console.WriteLine("2. Поиск по автору");
            Console.WriteLine("3. Поиск по жанру");
            Console.Write("Выберите тип поиска (1-3): ");

            string searchType = Console.ReadLine();
            List<Book> results = new List<Book>();

            switch (searchType)
            {
                case "1":
                    Console.Write("Введите название для поиска: ");
                    string title = Console.ReadLine();
                    results = bookService.FindByTitle(title);
                    break;
                case "2":
                    Console.Write("Введите автора для поиска: ");
                    string author = Console.ReadLine();
                    results = bookService.FindByAuthor(author);
                    break;
                case "3":
                    Console.WriteLine("Доступные жанры:");
                    Console.WriteLine("1. Fantasy");
                    Console.WriteLine("2. ScienceFiction");
                    Console.WriteLine("3. Mystery");
                    Console.WriteLine("4. Romance");
                    Console.WriteLine("5. Horror");
                    Console.Write("Выберите жанр (1-5): ");
                    string genreInput = Console.ReadLine();
                    Genre genre;
                    switch (genreInput)
                    {
                        case "1": genre = Genre.Fantasy; break;
                        case "2": genre = Genre.ScienceFiction; break;
                        case "3": genre = Genre.Mystery; break;
                        case "4": genre = Genre.Romance; break;
                        case "5": genre = Genre.Horror; break;
                        default:
                            Console.WriteLine("Неверный выбор жанра.");
                            return;
                    }
                    results = bookService.FindByGenre(genre);
                    break;
                default:
                    Console.WriteLine("Неверный выбор типа поиска.");
                    return;
            }

            DisplaySearchResults(results);
        }

        static void SortBooks()
        {
            Console.WriteLine("\n=== СОРТИРОВКА КНИГ ===");
            Console.WriteLine("1. Сортировка по названию");
            Console.WriteLine("2. Сортировка по году издания");
            Console.Write("Выберите тип сортировки (1-2): ");

            string sortType = Console.ReadLine();
            List<Book> sortedBooks = new List<Book>();

            switch (sortType)
            {
                case "1":
                    sortedBooks = bookService.SortByTitle();
                    Console.WriteLine("\nКниги отсортированы по названию:");
                    break;
                case "2":
                    sortedBooks = bookService.SortByYear();
                    Console.WriteLine("\nКниги отсортированы по году издания:");
                    break;
                default:
                    Console.WriteLine("Неверный выбор типа сортировки.");
                    return;
            }

            foreach (Book book in sortedBooks)
            {
                Console.WriteLine(book);
            }
        }

        static void ShowPriceExtremes()
        {
            Console.WriteLine("\n=== САМАЯ ДОРОГАЯ И ДЕШЁВАЯ КНИГИ ===");

            Book mostExpensive = bookService.GetMostExpensiveBook();
            Book cheapest = bookService.GetCheapestBook();

            if (mostExpensive != null)
            {
                Console.WriteLine("Самая дорогая книга: " + mostExpensive);
            }

            if (cheapest != null)
            {
                Console.WriteLine("Самая дешёвая книга: " + cheapest);
            }

            if (mostExpensive == null && cheapest == null)
            {
                Console.WriteLine("В библиотеке нет книг.");
            }
        }

        static void DisplaySearchResults(List<Book> results)
        {
            if (results.Count == 0)
            {
                Console.WriteLine("Книги по вашему запросу не найдены.");
                return;
            }

            Console.WriteLine("\nНайдено книг: " + results.Count);
            foreach (Book book in results)
            {
                Console.WriteLine(book);
            }
        }
    }
}