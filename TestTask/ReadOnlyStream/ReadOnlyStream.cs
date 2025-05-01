using System;
using System.Text;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private Stream _localStream;
        private StreamReader _streamReader;
        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            IsEof = true;
            _localStream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
            _streamReader = new StreamReader(_localStream, Encoding.UTF8);
        }
                
        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof { get; private set; }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {  
            if(IsEof == true)
            {
                CloseStream();
                throw new IOException("Reading a file after its end");
            }
            if(_streamReader.Peek() == -1)
            {
                IsEof = true;
                return '\0';
            }
            char c = (char)_streamReader.Read();
            return c;
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                IsEof = true;
                return;
            }

            _localStream.Position = 0;
            IsEof = false;
        }

        public void CloseStream()
        {
            _localStream.Close();
            _streamReader.Close();
        }

        public void Dispose()
        {
            _localStream.Dispose();
            _streamReader.Dispose();
        }
    }
}
