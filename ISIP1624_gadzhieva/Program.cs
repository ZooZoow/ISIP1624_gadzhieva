using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagement
{
    // Абстрактный класс Person
    public abstract class Person
    {
        private string name;
        private int age;
        private string contactInfo;
        private static int nextId = 1;

        public int Id { get; private set; }

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Имя не может быть пустым");
                name = value;
            }
        }

        public int Age
        {
            get => age;
            set
            {
                if (value < 16 || value > 100)
                    throw new ArgumentException("Возраст должен быть от 16 до 100 лет");
                age = value;
            }
        }

        public string ContactInfo
        {
            get => contactInfo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Контактная информация не может быть пустой");
                contactInfo = value;
            }
        }

        protected Person(string name, int age, string contactInfo)
        {
            Id = nextId++;
            Name = name;
            Age = age;
            ContactInfo = contactInfo;
        }

        public abstract string GetRole();

        public virtual string GetInfo()
        {
            return $"ID: {Id}, Имя: {Name}, Возраст: {Age}, Контакты: {ContactInfo}, Роль: {GetRole()}";
        }
    }

    // Интерфейс для отображения информации
    public interface IDisplayable
    {
        string DisplayInfo();
    }

    // Класс Student
    public class Student : Person, IDisplayable
    {
        private List<Course> courses;

        public string Major { get; set; }
        public int Year { get; set; }
        public IReadOnlyList<Course> Courses => courses.AsReadOnly();

        public Student(string name, int age, string contactInfo, string major, int year)
            : base(name, age, contactInfo)
        {
            Major = major;
            Year = year;
            courses = new List<Course>();
        }

        public override string GetRole()
        {
            return "Студент";
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Специальность: {Major}, Курс: {Year}";
        }

        public string DisplayInfo()
        {
            return GetInfo();
        }

        public void EnrollInCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!courses.Contains(course))
            {
                courses.Add(course);
                course.AddStudent(this);
            }
        }

        public void DropCourse(Course course)
        {
            if (course != null && courses.Contains(course))
            {
                courses.Remove(course);
                course.RemoveStudent(this);
            }
        }

        public string GetCoursesInfo()
        {
            if (courses.Count == 0)
                return "Студент не записан ни на один курс";

            return string.Join("\n", courses.Select(c => c.Name));
        }
    }

    // Класс Teacher
    public class Teacher : Person, IDisplayable
    {
        private List<Course> courses;

        public string Department { get; set; }
        public string Specialization { get; set; }
        public decimal Salary { get; set; }
        public IReadOnlyList<Course> Courses => courses.AsReadOnly();

        public Teacher(string name, int age, string contactInfo, string department, string specialization, decimal salary)
            : base(name, age, contactInfo)
        {
            Department = department;
            Specialization = specialization;
            Salary = salary;
            courses = new List<Course>();
        }

        public override string GetRole()
        {
            return "Преподаватель";
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Кафедра: {Department}, Специализация: {Specialization}, Зарплата: {Salary:C}";
        }

        public string DisplayInfo()
        {
            return GetInfo();
        }

        public void AssignToCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!courses.Contains(course))
            {
                courses.Add(course);
                course.AssignTeacher(this);
            }
        }

        public void RemoveFromCourse(Course course)
        {
            if (course != null && courses.Contains(course))
            {
                courses.Remove(course);
                course.RemoveTeacher();
            }
        }
    }

    // Класс Course
    public class Course : IDisplayable
    {
        private static int nextId = 1;
        private string name;
        private string description;
        private List<Student> students;

        public int Id { get; private set; }
        public string Code { get; set; }
        public int Credits { get; set; }
        public int MaxStudents { get; set; }
        public Teacher Teacher { get; private set; }
        public IReadOnlyList<Student> Students => students.AsReadOnly();

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название курса не может быть пустым");
                name = value;
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Описание курса не может быть пустым");
                description = value;
            }
        }

        public Course(string name, string code, string description, int credits, int maxStudents)
        {
            Id = nextId++;
            Name = name;
            Code = code;
            Description = description;
            Credits = credits;
            MaxStudents = maxStudents;
            students = new List<Student>();
        }

        public void AssignTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new ArgumentNullException(nameof(teacher));

            Teacher = teacher;
        }

        public void RemoveTeacher()
        {
            Teacher = null;
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (students.Count >= MaxStudents)
                throw new InvalidOperationException("Курс заполнен. Невозможно добавить больше студентов");

            if (!students.Contains(student))
            {
                students.Add(student);
            }
        }

        public void RemoveStudent(Student student)
        {
            if (student != null && students.Contains(student))
            {
                students.Remove(student);
            }
        }

        public string DisplayInfo()
        {
            string teacherInfo = Teacher != null ? Teacher.Name : "Не назначен";
            return $"Курс: {Name} ({Code})\n" +
                   $"Описание: {Description}\n" +
                   $"Кредиты: {Credits}, Макс. студентов: {MaxStudents}\n" +
                   $"Преподаватель: {teacherInfo}\n" +
                   $"Записанных студентов: {students.Count}";
        }

        public string GetStudentsInfo()
        {
            if (students.Count == 0)
                return "На курс пока никто не записан";

            return string.Join("\n", students.Select(s => s.Name));
        }
    }

    // Сервис управления университетом
    public class UniversityService
    {
        private List<Student> students;
        private List<Teacher> teachers;
        private List<Course> courses;

        public IReadOnlyList<Student> Students => students.AsReadOnly();
        public IReadOnlyList<Teacher> Teachers => teachers.AsReadOnly();
        public IReadOnlyList<Course> Courses => courses.AsReadOnly();

        public UniversityService()
        {
            students = new List<Student>();
            teachers = new List<Teacher>();
            courses = new List<Course>();
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            // Тестовые преподаватели
            var teacher1 = new Teacher("Иван Петров", 45, "ipetrov@university.ru", "Компьютерные науки", "Программирование", 80000);
            var teacher2 = new Teacher("Мария Сидорова", 38, "msidorova@university.ru", "Математика", "Высшая математика", 75000);

            teachers.Add(teacher1);
            teachers.Add(teacher2);

            // Тестовые студенты
            var student1 = new Student("Алексей Иванов", 20, "aivanov@student.ru", "Компьютерные науки", 2);
            var student2 = new Student("Екатерина Смирнова", 19, "esmirnova@student.ru", "Математика", 1);
            var student3 = new Student("Дмитрий Козлов", 21, "dkozlov@student.ru", "Компьютерные науки", 3);

            students.Add(student1);
            students.Add(student2);
            students.Add(student3);

            // Тестовые курсы
            var course1 = new Course("Программирование на C#", "CS101", "Основы программирования на C#", 4, 30);
            var course2 = new Course("Высшая математика", "MATH201", "Продвинутая математика", 5, 25);
            var course3 = new Course("Алгоритмы и структуры данных", "CS202", "Изучение основных алгоритмов", 4, 20);

            courses.Add(course1);
            courses.Add(course2);
            courses.Add(course3);

            // Назначение преподавателей
            course1.AssignTeacher(teacher1);
            course2.AssignTeacher(teacher2);
            course3.AssignTeacher(teacher1);

            // Запись студентов на курсы
            student1.EnrollInCourse(course1);
            student1.EnrollInCourse(course3);
            student2.EnrollInCourse(course2);
            student3.EnrollInCourse(course1);
            student3.EnrollInCourse(course2);
            student3.EnrollInCourse(course3);
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            students.Add(student);
        }

        public void AddTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new ArgumentNullException(nameof(teacher));

            teachers.Add(teacher);
        }

        public void AddCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            courses.Add(course);
        }

        public Student FindStudentById(int id)
        {
            return students.FirstOrDefault(s => s.Id == id);
        }

        public Teacher FindTeacherById(int id)
        {
            return teachers.FirstOrDefault(t => t.Id == id);
        }

        public Course FindCourseById(int id)
        {
            return courses.FirstOrDefault(c => c.Id == id);
        }

        public void EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = FindStudentById(studentId);
            var course = FindCourseById(courseId);

            if (student == null || course == null)
                throw new ArgumentException("Студент или курс не найден");

            student.EnrollInCourse(course);
        }

        public void DisplayAllPeople()
        {
            Console.WriteLine("\n=== ВСЕ ЛЮДИ В УНИВЕРСИТЕТЕ ===");

            var allPeople = students.Cast<Person>().Concat(teachers.Cast<Person>())
                            .OrderBy(p => p.Id);

            foreach (var person in allPeople)
            {
                Console.WriteLine(person.GetInfo());
            }
        }

        public void DisplayAllStudents()
        {
            Console.WriteLine("\n=== ВСЕ СТУДЕНТЫ ===");
            foreach (var student in students)
            {
                Console.WriteLine(student.DisplayInfo());
            }
        }

        public void DisplayAllTeachers()
        {
            Console.WriteLine("\n=== ВСЕ ПРЕПОДАВАТЕЛИ ===");
            foreach (var teacher in teachers)
            {
                Console.WriteLine(teacher.DisplayInfo());
            }
        }

        public void DisplayAllCourses()
        {
            Console.WriteLine("\n=== ВСЕ КУРСЫ ===");
            foreach (var course in courses)
            {
                Console.WriteLine(course.DisplayInfo());
                Console.WriteLine("---");
            }
        }
    }

    // Главная программа
    class Program
    {
        private static UniversityService universityService = new UniversityService();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Добро пожаловать в систему управления университетом!");

            bool isRunning = true;
            while (isRunning)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageStudents();
                        break;
                    case "2":
                        ManageTeachers();
                        break;
                    case "3":
                        ManageCourses();
                        break;
                    case "4":
                        DisplayAllInformation();
                        break;
                    case "5":
                        isRunning = false;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите действие от 1 до 5.");
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

        static void DisplayMainMenu()
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Управление студентами");
            Console.WriteLine("2. Управление преподавателями");
            Console.WriteLine("3. Управление курсами");
            Console.WriteLine("4. Просмотр всей информации");
            Console.WriteLine("5. Выйти из программы");
            Console.Write("Выберите действие (1-5): ");
        }

        static void ManageStudents()
        {
            Console.WriteLine("\n=== УПРАВЛЕНИЕ СТУДЕНТАМИ ===");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Просмотреть всех студентов");
            Console.WriteLine("3. Записать студента на курс");
            Console.WriteLine("4. Просмотреть курсы студента");
            Console.Write("Выберите действие (1-4): ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddNewStudent();
                    break;
                case "2":
                    universityService.DisplayAllStudents();
                    break;
                case "3":
                    EnrollStudentInCourse();
                    break;
                case "4":
                    ViewStudentCourses();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        static void AddNewStudent()
        {
            try
            {
                Console.Write("Введите имя студента: ");
                string name = Console.ReadLine();

                Console.Write("Введите возраст: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Введите контактную информацию: ");
                string contact = Console.ReadLine();

                Console.Write("Введите специальность: ");
                string major = Console.ReadLine();

                Console.Write("Введите курс: ");
                int year = int.Parse(Console.ReadLine());

                var student = new Student(name, age, contact, major, year);
                universityService.AddStudent(student);
                Console.WriteLine("Студент успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void EnrollStudentInCourse()
        {
            try
            {
                universityService.DisplayAllStudents();
                Console.Write("Введите ID студента: ");
                int studentId = int.Parse(Console.ReadLine());

                universityService.DisplayAllCourses();
                Console.Write("Введите ID курса: ");
                int courseId = int.Parse(Console.ReadLine());

                universityService.EnrollStudentInCourse(studentId, courseId);
                Console.WriteLine("Студент успешно записан на курс!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ViewStudentCourses()
        {
            universityService.DisplayAllStudents();
            Console.Write("Введите ID студента: ");

            if (int.TryParse(Console.ReadLine(), out int studentId))
            {
                var student = universityService.FindStudentById(studentId);
                if (student != null)
                {
                    Console.WriteLine($"\nКурсы студента {student.Name}:");
                    Console.WriteLine(student.GetCoursesInfo());
                }
                else
                {
                    Console.WriteLine("Студент не найден.");
                }
            }
        }

        static void ManageTeachers()
        {
            Console.WriteLine("\n=== УПРАВЛЕНИЕ ПРЕПОДАВАТЕЛЯМИ ===");
            Console.WriteLine("1. Добавить преподавателя");
            Console.WriteLine("2. Просмотреть всех преподавателей");
            Console.Write("Выберите действие (1-2): ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddNewTeacher();
                    break;
                case "2":
                    universityService.DisplayAllTeachers();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        static void AddNewTeacher()
        {
            try
            {
                Console.Write("Введите имя преподавателя: ");
                string name = Console.ReadLine();

                Console.Write("Введите возраст: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Введите контактную информацию: ");
                string contact = Console.ReadLine();

                Console.Write("Введите кафедру: ");
                string department = Console.ReadLine();

                Console.Write("Введите специализацию: ");
                string specialization = Console.ReadLine();

                Console.Write("Введите зарплату: ");
                decimal salary = decimal.Parse(Console.ReadLine());

                var teacher = new Teacher(name, age, contact, department, specialization, salary);
                universityService.AddTeacher(teacher);
                Console.WriteLine("Преподаватель успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ManageCourses()
        {
            Console.WriteLine("\n=== УПРАВЛЕНИЕ КУРСАМИ ===");
            Console.WriteLine("1. Добавить курс");
            Console.WriteLine("2. Просмотреть все курсы");
            Console.WriteLine("3. Просмотреть студентов на курсе");
            Console.Write("Выберите действие (1-3): ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddNewCourse();
                    break;
                case "2":
                    universityService.DisplayAllCourses();
                    break;
                case "3":
                    ViewCourseStudents();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }

        static void AddNewCourse()
        {
            try
            {
                Console.Write("Введите название курса: ");
                string name = Console.ReadLine();

                Console.Write("Введите код курса: ");
                string code = Console.ReadLine();

                Console.Write("Введите описание курса: ");
                string description = Console.ReadLine();

                Console.Write("Введите количество кредитов: ");
                int credits = int.Parse(Console.ReadLine());

                Console.Write("Введите максимальное количество студентов: ");
                int maxStudents = int.Parse(Console.ReadLine());

                var course = new Course(name, code, description, credits, maxStudents);
                universityService.AddCourse(course);
                Console.WriteLine("Курс успешно добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ViewCourseStudents()
        {
            universityService.DisplayAllCourses();
            Console.Write("Введите ID курса: ");

            if (int.TryParse(Console.ReadLine(), out int courseId))
            {
                var course = universityService.FindCourseById(courseId);
                if (course != null)
                {
                    Console.WriteLine($"\nСтуденты на курсе {course.Name}:");
                    Console.WriteLine(course.GetStudentsInfo());
                }
                else
                {
                    Console.WriteLine("Курс не найден.");
                }
            }
        }

        static void DisplayAllInformation()
        {
            Console.WriteLine("\n=== ПОЛНАЯ ИНФОРМАЦИЯ ОБ УНИВЕРСИТЕТЕ ===");
            universityService.DisplayAllPeople();
            universityService.DisplayAllCourses();
        }
    }
}