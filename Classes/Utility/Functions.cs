using System;
using System.Collections.Generic;

namespace Utility
{
    class Functions
    {
        public static char IntToCheckerLetter(int n) 
        {
            int charNum = n + 65;
            if (charNum >= 65 && charNum <= 132) 
            {
                return (char) charNum;
            }
            else 
            {
                return '@';
            }
        }

        public static string Vector2ToCheckerCoordinate(Vector2 vector)
        {
            return IntToCheckerLetter(vector.x) + "" + (vector.y + 1);
        }

        public static void PrintList<T>(List<T> list)
        {
            foreach (T element in list)
            {
                Console.WriteLine(element.ToString());
            }
        }
    }
} 