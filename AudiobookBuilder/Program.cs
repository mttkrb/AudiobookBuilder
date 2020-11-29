using AudiobookBuilder.Objects;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace AudiobookBuilder
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var rootcmd = new RootCommand("Audiobookbuilder")
            {
                new Option<DirectoryInfo>("--d",()=>new DirectoryInfo(Directory.GetCurrentDirectory())  ,"directory"),
                new Option<bool>("--r","Create audiobooks recursivly")
            };


            rootcmd.Handler = CommandHandler.Create<DirectoryInfo, bool>((d, r) => {

                var folders = new List<DirectoryInfo>();

                if(r)
                {
                    folders.AddRange( d.EnumerateDirectories("*", SearchOption.AllDirectories));
                }
                else
                {
                    folders.Add(d);
                }

                Parallel.ForEach(folders, folder => {
                    Console.WriteLine($"Begin Convert {folder.Name}");
                    var result = AudioConverter.Convert(folder);
                    if(result ==null)
                    {
                        Console.WriteLine($"Nothing to do for folder: {folder.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"Convert Done for: {folder.Name} Created File {result.Name}");
                    }
                    

                });


            });
            return rootcmd.InvokeAsync(args).Result;

        }
    }
}
