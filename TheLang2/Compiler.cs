using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLang2.AST;
using TheLang2.Syntax;

namespace TheLang2
{
    public class Compiler
    {
        public ASTProgram Tree { get; set; }

        private readonly ConcurrentQueue<string> _filesToParse = new ConcurrentQueue<string>();
        private readonly HashSet<string> _filesParsed = new HashSet<string>();

        public bool Compile(string filename)
        {
            AddFile(filename);
            return Parse();
        }

        public void AddFile(string filename) => _filesToParse.Enqueue(filename);

        private bool Parse()
        {
            var fileNodes = new List<ASTFile>();

            do
            {
                var tasks = new List<Task<ASTFile>>();

                while (_filesToParse.TryDequeue(out string fileName))
                {
                    var fullPath = Path.GetFullPath(fileName);
                    if (!_filesParsed.Add(fullPath))
                        continue;

                    tasks.Add(Task.Run(() => new Parser(fullPath, this).ParseFile()));
                }

                Task.WaitAll(tasks.ToArray());

                foreach (var task in tasks)
                {
                    if (task == null)
                        return false;

                    fileNodes.Add(task.Result);
                }
            } while (!_filesToParse.IsEmpty);

            Tree = new ASTProgram { Files = fileNodes };
            return true;
        }
    }
}