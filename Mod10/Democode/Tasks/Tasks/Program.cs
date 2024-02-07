// controlling task execution
using System.Diagnostics;

Task[] tasks = new Task[3]
{
    Task.Run( () => Console.WriteLine("Task code finishing 1")),
    Task.Run( () => Console.WriteLine("Task code finishing 2")),
    Task.Run( () => Console.WriteLine("Task code finishing 3"))
};

Task.WaitAll(tasks);
Console.WriteLine("All done!");
Console.WriteLine();

// parallel vs. sequential operation
int start = 0;
int length = 5000000;
var stopwatch = new Stopwatch();
double[] array = new double[length];
Console.WriteLine("Computing " + length + " square roots:");
Console.ReadLine();

Console.WriteLine("Sequential Operation...");
stopwatch.Reset();
stopwatch.Start();
for (int i = start; i < length; i++)
{
    array[i] = Math.Log10(Math.Sqrt(i));
}
stopwatch.Stop();
Console.WriteLine("Base 10 log of square root of 1000000 is " + array[1000000]);
Console.WriteLine("Finished sequential operation...");
Console.WriteLine("Execution Time (ms) = " + stopwatch.ElapsedMilliseconds);
Console.WriteLine();
Console.ReadLine();

Console.WriteLine("Parallel Operation...");
stopwatch.Reset();
stopwatch.Start();
Parallel.For(start, length, i =>
{
    array[i] = Math.Log10(Math.Sqrt(i));
});
stopwatch.Stop();
Console.WriteLine("Base 10 log of square root of 1000000 is " + array[1000000]);
Console.WriteLine("Finished parallel operation...");
Console.WriteLine("Execution Time (ms) = " + stopwatch.ElapsedMilliseconds);
Console.WriteLine();
