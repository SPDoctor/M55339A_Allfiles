namespace FourthCoffee.LogProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var logLocator = new LogLocator("..\\..\\..\\..\\..\\..\\Data\\Logs\\");
            var logCombiner = new LogCombiner(logLocator);

            logCombiner.CombineLogs("..\\..\\..\\..\\..\\..\\Data\\Logs\\CombinedLog.txt");

        }
    }
}
