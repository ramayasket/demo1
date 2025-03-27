using System.Globalization;
using Kw.Common;

#pragma warning disable CA2208
#pragma warning disable CA1069

// ReSharper disable NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
// ReSharper disable UnusedMember.Local

namespace ConsoleApp3
{
    internal class Program
    {
        enum Level
        {
            INFO = 1,
            INFORMATION = 1,
            WARN = 2,
            WARNING = 2,
            ERROR = 3,
            DEBUG = 4,
        }

        class Incident
        {
            public DateOnly Date;
            public TimeOnly Time;
            public Level Level;
            public string Caller;
            public string Message;
        }

        abstract class InputFormat
        {
            public abstract Incident Consume(string input);
        }

        class Format1 : InputFormat
        {
            public override Incident Consume(string input)
            {
                Incident i = new();

                string[] elements = input.Split(" ");

                if (elements.Length < 4)
                    throw new ArgumentException();

                i.Date = DateOnly.ParseExact(elements[0], "dd.MM.yyyy");
                i.Time = TimeOnly.ParseExact(elements[1], "HH:mm:ss.fff");
                i.Level = Enum.Parse<Level>(elements[2]);
                i.Caller = "";
                i.Message = string.Join(" ", elements.Skip(3));

                return i;
            }
        }
        
        class Format2 : InputFormat
        {
            public override Incident Consume(string input)
            {
                Incident i = new();

                string[] elements = input.Split("|");

                if (elements.Length < 5)
                    throw new ArgumentException();

                DateTime timestamp = DateTime.ParseExact(elements[0], "yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture);

                i.Date = DateOnly.FromDateTime(timestamp);
                i.Time = TimeOnly.FromDateTime(timestamp);
                i.Level = Enum.Parse<Level>(elements[1]);
                i.Caller = elements[3];
                i.Message = string.Join("|", elements.Skip(4));

                return i;
            }
        }
        
        static void Main()
        {
            using Stream input1 = File.Open("input1.log", FileMode.Open);
            using Stream input2 = File.Open("input2.log", FileMode.Open);

            Pump(input1, new Format1());
            Pump(input2, new Format2());
        }

        static void Pump(Stream input, InputFormat format)
        {
            using Stream output = File.Open("output.log", FileMode.Append);
            using Stream error = File.Open("problems.txt", FileMode.Append);

            using StreamReader reader = new(input);
            using StreamWriter writer = new(output);
            using StreamWriter errorWriter = new(error);

            while (reader.ReadLine() is { } s)
                try
                {
                    Incident i = format.Consume(s);

                    s = $"{i.Date:dd-MM-yyyy}\t{i.Time:HH:mm:ss.ffff}\t{i.Level}\t{i.Caller.InCase("", "DEFAULT")}\t{i.Message}";

                    writer.WriteLine(s);
                }
                catch
                {
                    errorWriter.WriteLine(s);
                }
        }
    }
}
