using System;
using TheLang2.Syntax;

namespace TheLang2
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var file = new Parser("Ideas\\array_list.tl").ParseFile();
            Console.ReadLine();
        }
    }
}