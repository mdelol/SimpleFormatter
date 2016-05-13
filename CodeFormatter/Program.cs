using System;
using CodeFormatter.Rules.LineRules;

namespace CodeFormatter
{
  class Program
  {
    static void Main(string[] args)
    {
       var dir = args[0];
      var format = args[1];
      Console.WriteLine($"formatting files with .{format} extension in {dir}");
      new Formatter(dir, format).Format();
      Console.WriteLine("done");
    }
  }
}


