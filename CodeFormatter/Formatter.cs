using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeFormatter.Rules;
using CodeFormatter.Rules.FileRules;
using CodeFormatter.Rules.LineRules;

namespace CodeFormatter
{
  class Formatter
  {
    private readonly string _dir;
    private readonly string _extension;
    private readonly List<IFileRule> _fileRules = new List<IFileRule> {new EmptyLineRule()};
    private readonly List<ILineRule> _lineRules = new List<ILineRule> {new FigureBraceRule(), new CommaRule()};

    public Formatter(string dir, string extension)
    {
      _dir = dir;
      _extension = extension;
    }

    public void Format()
    {
      HandleDir(_dir);
    }

    private void HandleDir(string dir)
    {
      var files = Directory.GetFiles(dir);
      var filteredFiles = files.Where(x => x.EndsWith(_extension)).ToList();
      foreach (var s in filteredFiles)
      {
        FormatFile(s);
      }

      Directory.GetDirectories(dir).ToList().ForEach(HandleDir);

    }

    private void FormatFile(string file)
    {
      var readAllLines = File.ReadAllLines(file).ToList();

      foreach (var rule in _lineRules.Where(x=>x.ApplicableTo.Contains(_extension)))
      {
        readAllLines = readAllLines.Select(x =>
        {
          if (rule.Matches(x))
          {
            Log($"{file.Split('/').Last()}: applying {rule} to '{x}'");
            return rule.Apply(x);
          }
          return x;
        }).ToList();
      }

      foreach (var rule in _fileRules.Where(x=>x.ApplicableTo.Contains(_extension)))
      {
        if (rule.Matches(readAllLines))
        {
          Log($"{file.Split('/').Last()}: applying {rule}");
          readAllLines = rule.Apply(readAllLines);
        }
      }

      File.Delete(file);
      File.WriteAllLines(file, readAllLines);
    }

    private void Log(string str)
    {
      Console.WriteLine(str);
    }
  }
}