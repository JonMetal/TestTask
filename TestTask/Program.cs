using System;
using System.Collections.Generic;
using System.Linq;
using TestTask.Extensions;
using TestTask.Letter;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        static void Main(string[] args)
        {
            IReadOnlyStream inputStream1 = GetInputStream(args[0]);
            IReadOnlyStream inputStream2 = GetInputStream(args[1]);

            ICollection<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
            ICollection<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);
            RemoveCharStatsByType(singleLetterStats, LetterType.Vowel);
            RemoveCharStatsByType(doubleLetterStats, LetterType.Consonants);

            PrintStatistic(singleLetterStats);
            PrintStatistic(doubleLetterStats);

            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            return new ReadOnlyStream(fileFullPath);
        }

        private static void AddOrIncStatistic(ICollection<LetterStats> listLetterStats, char c, LetterType letterType)
        {
            LetterStats letterStats = listLetterStats.FirstOrDefault(ls => ls.Letter == c);
            if (letterStats == null)
            {
                listLetterStats.Add(new LetterStats(c, letterType));
            }
            else
            {
                letterStats.IncStatictic();
            }
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static ICollection<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            ICollection<LetterStats> result = new List<LetterStats>();
            stream.ResetPositionToStart();
            using(stream)
            {
                while (!stream.IsEof)
                {
                    char c = stream.ReadNextChar();
                    LetterType? letterType = IdentifierLetter.IdentifyLetter(c);
                    if(letterType == null)
                    {
                        continue;
                    }
                    else
                    {
                        AddOrIncStatistic(result, c, letterType.Value);
                    }                               
                }
            }
            return result;
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static ICollection<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            stream.ResetPositionToStart();
            ICollection<LetterStats> result = new List<LetterStats>();
            char lastChar = '\n';
            using(stream)
            {
                while (!stream.IsEof)
                {
                    char c = char.ToUpper(stream.ReadNextChar());
                    LetterType? letterType = IdentifierLetter.IdentifyLetter(c);
                    if(letterType != null)
                    {
                        if(c == lastChar)
                        {
                            AddOrIncStatistic(result, c, letterType.Value);
                        }
                    }
                    lastChar = c;
                }
            }
            return result;
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром letterType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="letterType">Тип букв для анализа</param>

        private static void RemoveCharStatsByType(ICollection<LetterStats> letters, LetterType letterType)
        {
            switch (letterType)
            {
                case LetterType.Consonants:
                    letters.CopyFrom(letters.Where(l => l.LetterType == LetterType.Vowel).ToList());
                    break;
                case LetterType.Vowel:
                    letters.CopyFrom(letters.Where(l => l.LetterType == LetterType.Consonants).ToList());
                    break;
            }
            
        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            int sum = 0;
            letters = letters.OrderBy(l => l.Letter);
            foreach(var letter in letters)
            {
                sum += letter.Count;
                Console.WriteLine($"{letter.Letter} : {letter.Count}");
            }
            Console.WriteLine($"ИТОГО: {sum}");
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        /// Метод перенесён в класс LetterStats
    }
}
