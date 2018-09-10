using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace trx2junit
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("first arg must be the trx-file");
                Environment.Exit(1);
            }

            try
            {
                await RunAsync(args);
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(ex.Message);
                Console.ResetColor();
                Environment.Exit(2);
            }

            if (Debugger.IsAttached)
                Console.ReadKey();
        }
        //---------------------------------------------------------------------
        private static async Task RunAsync(string[] args)
        {
            Thread.CurrentThread.CurrentCulture   = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            string trxFile   = args[0];
            string jUnitFile = args.Length == 1
                ? Path.ChangeExtension(args[0], "xml")
                : Path.Combine(Path.GetDirectoryName(args[0]), "junit.xml");

            Console.WriteLine($"Converting {args.Length} trx-file(s) to JUnit-xml at '{jUnitFile}'");
            DateTime start = DateTime.Now;

            var utf8 = new UTF8Encoding(false);

            using (TextWriter output = new StreamWriter(jUnitFile, false, utf8))
            {
                var converter = new Trx2JunitConverter();
                var streams = args.Select(File.OpenRead).ToArray();
                try{
                    await converter.Convert(streams, output);
                }finally{
                    foreach(var stream in streams){
                        stream?.Dispose();
                    }
                }
            }

            Console.WriteLine($"done in {(DateTime.Now - start).TotalSeconds} seconds. bye.");
        }
    }
}
