using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SUT
{
    public static class MockWithoutInterface
    {
        public static void Main()
        {
            using (var sr = new MyStreamReader(@"Resources\TextFile1.txt"))
            {
                var text = ReadText(sr);
                PrintText(text);
            }

            Console.ReadKey();
        }

        public static IList<string> ReadText(IStreamReader sr)
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

    public interface IStreamReader
    {
        bool EndOfStream { get; }

        string ReadLine();
    }

    public class MyStreamReader : IStreamReader, IDisposable
    {
        private bool disposed = false;
        private readonly StreamReader sr;

        public MyStreamReader(string filename) => sr = new StreamReader(filename);

        public bool EndOfStream => sr.EndOfStream;

        public string ReadLine() => sr.ReadLine();

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                sr.Dispose();
            }

            disposed = true;
        }
    }
}
