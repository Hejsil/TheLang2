using System;

namespace TheLang2
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var compiler = new Compiler();
            compiler.Compile("Ideas\\Include\\test1.tl");
            Console.ReadLine();
        }
    }
}