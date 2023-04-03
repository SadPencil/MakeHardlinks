using Mono.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MakeHardlinks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // these variables will be set when the command line is parsed
            int verbosity = 0;
            bool shouldShowHelp = false;
            bool fallback = false;
            var allowedExt = new List<string>();
            var disallowedExt = new List<string>();
            // these are the available options, note that they set the variables
            var options = new OptionSet {
                { "allow=", "only make hardlinks for files with given extensions; copy other files, e.g., --allow=.exe --allow=.png", n => allowedExt.Add (n.ToUpperInvariant()) },
                { "disallow=", "copy and don't make hardlinks for files with given extensions, e.g., --disallow=.ini --disallow=.conf", n=> disallowedExt.Add (n.ToUpperInvariant()) },
                { "fallback", "copy files if making hardlinks fails", f => fallback = f != null },
                { "v|verbose", "print filepath when processing every file", v => { if (v != null) ++verbosity; } },
                { "h|help", "show this message and exit", h => shouldShowHelp = h != null },
            };

            string srcFolder;
            string dstFolder;
            try
            {
                // parse the command line
                var extra = options.Parse(args);

                if (shouldShowHelp)
                {
                    Console.WriteLine("Help:");
                    Console.WriteLine("[arguments] <src_dir> <dest_dir>");
                    foreach (var option in options)
                    {
                        Console.WriteLine($"{option.Prototype}\t\t{option.Description}");
                    }
                    return;

                }

                srcFolder = extra[0];
                dstFolder = extra[1];

                // extensions must start with dot
                foreach (string ext in allowedExt.Concat(disallowedExt))
                {
                    if (!ext.StartsWith(".")) throw new Exception("Extensions must start with a dot.");
                }

                if (allowedExt.Count == 0) allowedExt = null;
                if (disallowedExt.Count == 0) disallowedExt = null;
            }
            catch (Exception e)
            {
                // output some error message
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `--help' for more information.");
                return;
            }

            try
            {
                MyIO.CreateHardLinksOfFiles(srcFolder, dstFolder, true, fallback, disallowedExt, allowedExt,
                    (src, dst) => { if (verbosity >= 1) Console.WriteLine($"Make a hard link from {src} to {dst}."); },
                    (src, dst) => { if (verbosity >= 1) Console.WriteLine($"Copy a file from {src} to {dst}."); },
                    (src, dst) => { if (verbosity >= 1) Console.WriteLine($"Create a directory at {dst}."); }
                );
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}
