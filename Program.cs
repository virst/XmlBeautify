#if SERI
using Serilog;
using System.Diagnostics;
using System.Globalization;
#endif
using System.Xml.Linq;

namespace XmlBeautify;

internal class Program
{
#if SERI
    static Program()
    {
        var lc = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(formatProvider: new CultureInfo("ru-RU"));
        Log.Logger = lc.CreateLogger();
    }
#endif

    static void Main(string[] args)
    {
        WriteLine("Begin {0} files.", args.Length);
        int n = 0;

        foreach (var fn in args)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                FileInfo fi = new FileInfo(fn);
                WriteLine("fn {0}.", fi.FullName);
                var fn2 = Path.Combine(fi.DirectoryName, Path.GetFileNameWithoutExtension(fn) + "_bt" + Path.GetExtension(fn));

                var xml = File.ReadAllText(fn);
                xml = BeautifyXml(xml);
                File.WriteAllText(fn2, xml);
                stopWatch.Stop();
                WriteLine("fn+ {0}. {1} ms.", fn2, stopWatch.ElapsedMilliseconds);
                n++;

            }
            catch (Exception ex)
            {
                WriteLine("Ex: {0}", ex.Message);
            }
        }

        WriteLine("Done {0} files.", n);
    }

    static string BeautifyXml(string xml)
    {
        XDocument doc = XDocument.Parse(xml);
        return doc.ToString();
    }

#if !SERI
    static void WriteLine(string? value)
    {
        Console.WriteLine(value);
    }

    static void WriteLine(string format, params object?[]? arg)
    {
        Console.WriteLine(format, arg);
    }
#else
    static void WriteLine(string? value)
    {
        Log.Information(value);
    }

    static void WriteLine(string format, params object?[]? arg)
    {
        Log.Information(format, arg);
    }
#endif
}
