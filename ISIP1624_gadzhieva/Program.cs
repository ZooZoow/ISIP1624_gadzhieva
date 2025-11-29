using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TextAnalyzer
{
    class TextAnalysis
    {
        public string TextPreview { get; set; }
        public int TotalChars { get; set; }
        public int WordCount { get; set; }
        public string ShortestWord { get; set; }
        public int SentenceCount { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public string LongestWord { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; }

        public TextAnalysis()
        {
            TextPreview = "";
            ShortestWord = "";
            LongestWord = "";
            LetterFrequency = new Dictionary<char, int>();
        }
    }

    class Program
    {
        private static List<TextAnalysis> history = new List<TextAnalysis>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("ДОБРО ПОЖАЛОВАТЬ В АНАЛИЗАТОР ТЕКСТА!");

            bool continueRunning = true;

            while (continueRunning)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                if (choice == null) choice = "";
                choice = choice.Trim();

                switch (choice)
                {
                    case "1":
                        AnalyzeNewText();
                        break;
                    case "2":
                        ShowHistory();
                        break;
                    case "3":
                        continueRunning = false;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор! Пожалуйста, выберите 1, 2 или 3.");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\nМЕНЮ:");
            Console.WriteLine("1 - Анализ нового текста");
            Console.WriteLine("2 - Просмотр истории");
            Console.WriteLine("3 - Выход");
            Console.Write("Выберите действие (1-3): ");
        }

        static string GetTextFromUser()
        {
            while (true)
            {
                Console.WriteLine("\nВведите текст (минимум 100 символов):");
                string text = Console.ReadLine();

                if (text == null) text = "";
                text = text.Trim();

                if (text.Length >= 100)
                {
                    return text;
                }
                else
                {
                    Console.WriteLine("Текст слишком короткий! Введено {0} символов. Нужно минимум 100.", text.Length);
                }
            }
        }

        static string[] ExtractWords(string text)
        {
            List<string> wordsList = new List<string>();
            string currentWord = "";

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (char.IsLetterOrDigit(c))
                {
                    currentWord += c;
                }
                else
                {
                    if (currentWord.Length > 0)
                    {
                        wordsList.Add(currentWord.ToLower());
                        currentWord = "";
                    }
                }
            }

            if (currentWord.Length > 0)
            {
                wordsList.Add(currentWord.ToLower());
            }

            return wordsList.ToArray();
        }

        static int CountWords(string text)
        {
            string[] words = ExtractWords(text);
            return words.Length;
        }

        static string FindShortestWord(string text)
        {
            string[] words = ExtractWords(text);

            if (words.Length == 0) return "";

            string shortest = words[0];
            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length < shortest.Length)
                {
                    shortest = words[i];
                }
            }
            return shortest;
        }

        static int CountSentences(string text)
        {
            int count = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '.' || c == '!' || c == '?')
                {
                    count++;
                    while (i + 1 < text.Length && (text[i + 1] == '.' || text[i + 1] == '!' || text[i + 1] == '?'))
                    {
                        i++;
                    }
                }
            }
            return count;
        }

        static void CountVowelsAndConsonants(string text, out int vowels, out int consonants)
        {
            vowels = 0;
            consonants = 0;

            string vowelLetters = "аеёиоуыэюяaeiou";
            string consonantLetters = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz";

            for (int i = 0; i < text.Length; i++)
            {
                char c = char.ToLower(text[i]);

                if (vowelLetters.IndexOf(c) != -1)
                {
                    vowels++;
                }
                else if (consonantLetters.IndexOf(c) != -1)
                {
                    consonants++;
                }
            }
        }

        static string FindLongestWord(string text)
        {
            string[] words = ExtractWords(text);

            if (words.Length == 0) return "";

            string longest = words[0];
            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length > longest.Length)
                {
                    longest = words[i];
                }
            }
            return longest;
        }

        static Dictionary<char, int> CalculateLetterFrequency(string text)
        {
            Dictionary<char, int> frequency = new Dictionary<char, int>();

            for (int i = 0; i < text.Length; i++)
            {
                char c = char.ToLower(text[i]);

                if (char.IsLetter(c))
                {
                    if (frequency.ContainsKey(c))
                        frequency[c]++;
                    else
                        frequency[c] = 1;
                }
            }

            return frequency;
        }

        static TextAnalysis AnalyzeText(string text)
        {
            TextAnalysis analysis = new TextAnalysis();

            analysis.TextPreview = text.Length > 50 ? text.Substring(0, 50) + "..." : text;
            analysis.TotalChars = text.Length;
            analysis.WordCount = CountWords(text);
            analysis.ShortestWord = FindShortestWord(text);
            analysis.SentenceCount = CountSentences(text);
            analysis.LongestWord = FindLongestWord(text);

            int vowels, consonants;
            CountVowelsAndConsonants(text, out vowels, out consonants);
            analysis.VowelCount = vowels;
            analysis.ConsonantCount = consonants;

            analysis.LetterFrequency = CalculateLetterFrequency(text);

            return analysis;
        }

        static void DisplayStats(TextAnalysis stats)
        {
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("СТАТИСТИКА ТЕКСТА");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("Предпросмотр текста: {0}", stats.TextPreview);
            Console.WriteLine("Общее количество символов: {0}", stats.TotalChars);
            Console.WriteLine("Количество слов: {0}", stats.WordCount);
            Console.WriteLine("Самое короткое слово: '{0}'", stats.ShortestWord);
            Console.WriteLine("Количество предложений: {0}", stats.SentenceCount);
            Console.WriteLine("Гласные буквы: {0}", stats.VowelCount);
            Console.WriteLine("Согласные буквы: {0}", stats.ConsonantCount);
            Console.WriteLine("Самое длинное слово: '{0}'", stats.LongestWord);

            Console.WriteLine("\nЧастота букв:");
            if (stats.LetterFrequency.Count > 0)
            {
                List<char> letters = new List<char>(stats.LetterFrequency.Keys);
                letters.Sort();

                foreach (char letter in letters)
                {
                    Console.WriteLine("  {0}: {1}", letter, stats.LetterFrequency[letter]);
                }
            }
            else
            {
                Console.WriteLine("  Нет букв для анализа");
            }
            Console.WriteLine(new string('=', 50));
        }

        static void AnalyzeNewText()
        {
            string text = GetTextFromUser();
            TextAnalysis stats = AnalyzeText(text);
            history.Add(stats);
            DisplayStats(stats);

            while (true)
            {
                Console.Write("\nХотите проанализировать другой текст? (да/нет): ");
                string choice = Console.ReadLine();

                if (choice == null) choice = "";
                choice = choice.Trim().ToLower();

                if (choice == "да" || choice == "д")
                {
                    AnalyzeNewText();
                    return;
                }
                else if (choice == "нет" || choice == "н")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Пожалуйста, введите 'да' или 'нет'");
                }
            }
        }

        static void ShowHistory()
        {
            if (history.Count == 0)
            {
                Console.WriteLine("\nИстория пуста!");
                return;
            }

            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("ИСТОРИЯ АНАЛИЗА ТЕКСТОВ");
            Console.WriteLine(new string('=', 50));

            for (int i = 0; i < history.Count; i++)
            {
                TextAnalysis stats = history[i];
                Console.WriteLine("\nТекст #{0}:", i + 1);
                Console.WriteLine("  Предпросмотр: {0}", stats.TextPreview);
                Console.WriteLine("  Символов: {0}, Слов: {1}", stats.TotalChars, stats.WordCount);
                Console.WriteLine("  Предложений: {0}", stats.SentenceCount);
                Console.WriteLine("  Самое длинное слово: '{0}'", stats.LongestWord);
            }
        }
    }
}