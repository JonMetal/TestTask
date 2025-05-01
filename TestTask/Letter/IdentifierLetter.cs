using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Letter
{
    public static class IdentifierLetter
    {
        private readonly static string VowelLetters = "АЕЁИОУЫЭЮЯAEIOUY";
        private readonly static string ConsonantLetters = "БВГДЖЗЙКЛМНПРСТФХЦЧШЩBCDFGHJKLMNPQRSTVWXZ"; 

        public static LetterType? IdentifyLetter(char c)
        {
            char check = char.ToUpper(c);
            if(VowelLetters.Contains(check))
            {
                return LetterType.Vowel;
            }
            else if(ConsonantLetters.Contains(check))
            {
                return LetterType.Consonants;
            }
            return null;
        }
    }
}
