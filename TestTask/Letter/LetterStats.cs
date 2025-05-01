using System.Xml.Schema;

namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public class LetterStats
    {
        public char Letter { get; private set; }

        public int Count { get; private set; } = 1;

        public LetterType? LetterType { get; private set; }

        public void IncStatictic() { Count++; }

        public LetterStats(char letter, LetterType? type)
        {
            Letter = letter;
            LetterType = type;
        }
    }
}
