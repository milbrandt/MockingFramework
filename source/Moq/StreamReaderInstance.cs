using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoqSut
{
    public static class StreamReaderInstance
    {
        public static void Main()
        {
            using (var sr = new StreamReader(@"Resources\TextFile1.txt"))
            {
                var text = ReadText(sr);
                PrintText(text);
            }

            Console.ReadKey();
        }

        public static IList<string> ReadText(StreamReader sr)
        {
            if (sr == null)
            {
                throw new ArgumentNullException(nameof(sr));
            }

            var text = new List<string>();

            try
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    text.Add(line);
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
