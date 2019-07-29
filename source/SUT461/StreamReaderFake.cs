using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SUT461
{
    public static class StreamReaderFake
    {
        public static void Main()
        {
            var text = ReadText(@"Resources\TextFile1.txt");
            PrintText(text);

            Console.ReadKey();
        }

        public static IList<string> ReadText(string filename)
        {
            var text = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        text.Add(line);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return text;
        }

        public static void PrintText(IList<string> text)
        {
            text.ToList().ForEach(l => Console.WriteLine(l));

            Console.WriteLine();
            Console.WriteLine("Der Text hat " + text?.Count + " Zeilen");
        }
    }
}
