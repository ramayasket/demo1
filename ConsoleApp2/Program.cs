namespace ConsoleApp2
{
    public static class Server
    {
        static int count;
        static readonly ReaderWriterLockSlim @lock = new ();

        public static int GetCount()
        {
            @lock.EnterReadLock();

            int value = count;

            @lock.ExitReadLock();

            return value;
        }

        public static void AddToCount(int value)
        {
            @lock.EnterWriteLock();
            
            try { checked { count += value; } }

            finally { @lock.ExitWriteLock(); }
        }
    }

    internal class Program
    {
        static void Main()
        {
            Server.AddToCount(852);

            var x = Server.GetCount();
            Console.WriteLine(x);
        }
    }
}
