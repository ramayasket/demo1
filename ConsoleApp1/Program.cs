using System.Text;

namespace ConsoleApp1
{
    ////
    //// Assuming input data is correct (non-null, only allowed characters) -- no checks.
    ////
    class MyZip
    {
        public static string Zip(string x)
        {
            StringBuilder buffer = new();
            int count = 1;

            for (int i = 1; i <= x.Length; i++)
            {
                if (i < x.Length && x[i] == x[i-1]) count++;

                else
                {
                    buffer.Append(x[i-1]);
                    
                    if (count > 1)
                        buffer.Append(count);

                    count = 1;
                }
            }

            return buffer.ToString();
        }

        public static string Unzip(string x)
        {
            StringBuilder buffer = new();

            for (int i = 0; i < x.Length; i++)
            {
                char c = x[i];

                int count = 1;

                int j = i+1;
                while (j < x.Length && char.IsDigit(x[j])) j++;

                if (j > i+1)
                {
                    count = int.Parse(x.Substring(i+1, j-i-1));
                    i = j-1;
                }

                buffer.Append(c, count);
            }

            return buffer.ToString();
        }
    }

    internal class Program
    {
        static void Main()
        {
            //
            // quick test
            //
            string zip = MyZip.Zip("aaabbycccdde");
            Console.WriteLine(zip);

            string unzip = MyZip.Unzip(zip);
            Console.WriteLine(unzip);

            //
            // irregular input
            //
            unzip = MyZip.Unzip("a0c1zzzzz");
            Console.WriteLine(unzip);
        }
    }
}

