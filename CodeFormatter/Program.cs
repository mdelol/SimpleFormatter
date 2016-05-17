using System;

namespace CodeFormatter
{
    //@TODO add analysis without fixing problems
    class Program
    {
        static void Main(string[] args)
        {
            var path = args[0];
            var extension = args[1];
            Console.WriteLine($"formatting files with .{extension} extension in {path}");
            new Formatter(path, extension).Format();
            Console.WriteLine("done");
        }
    }
}


