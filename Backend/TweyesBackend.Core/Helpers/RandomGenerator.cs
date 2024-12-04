using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.Helpers
{
    public class RandomGenerator
    {
         // Instantiate random number generator.  
            private static readonly Random _random = new Random();

            // Generates a random number within a range.      
            public static int RandomNumber(int min, int max)
            {
                return _random.Next(min, max);
            }


            // Generates a random string with a given size.    
            public static string Generate(string prefix, int size, bool lowerCase = false)
            {
                var builder = new StringBuilder(16);

                // char is a single Unicode character  
                char offset = lowerCase ? 'a' : 'A';
                const int lettersOffset = 26; // A...Z or a..z: length=26  

                for (var i = 0; i < size; i++)
                {
                    var @char = (char)_random.Next(offset, offset + lettersOffset);
                    builder.Append(@char);
                }

                var id = lowerCase ? builder.ToString().ToLower() : builder.ToString();
                return $"{prefix}_{id}";
            }

        }
    }

