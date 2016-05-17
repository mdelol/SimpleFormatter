using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using CodeFormatter.Rules;

namespace CodeFormatter
{
    class Formatter
    {
        private readonly string _directoryPath;
        private readonly string _extension;

        [ImportMany(typeof(IRule))]
        private List<IRule> Rules { get; set; }

        private readonly List<ILineRule> _lineRules; 
        private readonly List<IFileRule> _fileRules;


        public Formatter(string directoryPath, string extension)
        {
            _directoryPath = directoryPath;
            _extension = extension;

            ImportRules();

            _fileRules = Rules.OfType<IFileRule>().Where(x => x.ApplicableTo.Contains(_extension)).ToList();
            _lineRules = Rules.OfType<ILineRule>().Where(x => x.ApplicableTo.Contains(_extension)).ToList();
        }

        private void ImportRules()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (Program).Assembly));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public void Format()
        {
            HandleDirectory(_directoryPath);
        }

        private void HandleDirectory(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            var filteredFiles = files.Where(x => x.EndsWith(_extension)).ToList();
            foreach (var s in filteredFiles)
            {
                FormatFile(s);
            }

            foreach (var directory in Directory.GetDirectories(directoryPath))
            {
                HandleDirectory(directory);
            }
        }

        private void FormatFile(string file)
        {
            var readAllLines = File.ReadAllLines(file).ToList();

            var fileName = Path.GetFileName(file);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var rule in _lineRules)
            {
                readAllLines = readAllLines.Select(x =>
                {
                    if (!rule.Matches(x)) return x;
                    Log($"{fileName}: applying {rule} to '{x.Trim()}'");
                    return rule.Apply(x);
                }).ToList();
            }

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var rule in _fileRules)
            {
                if (!rule.Matches(readAllLines)) continue;
                Log($"{fileName}: applying {rule}");
                readAllLines = rule.Apply(readAllLines);
            }

            File.Delete(file);
            File.WriteAllLines(file, readAllLines);
        }

        private static void Log(string str)
        {
            Console.WriteLine(str);
        }
    }
}