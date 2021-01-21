using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConsoleKey key;
            do
            {
                key = Menu();
                Console.WriteLine();
                Console.Clear();
                await ExecuteMenu(key);
                Console.WriteLine("Press a key to continue");
                Console.ReadKey();
                Console.Clear();


            } while (key != ConsoleKey.E);

        }

        private static ConsoleKey Menu()
        {
            Console.WriteLine("1: CallerContinuesWhileTaskIsBeingExecutedAsync");
            Console.WriteLine("2: CallerAwaitsTaskToBeExecutedAsync");
            Console.WriteLine("3: MultipleTasksExecutionInefficientAsync");
            Console.WriteLine("4: MultipleTasksExecutionEfficientAsync");
            Console.WriteLine("5: MultipleTasksListExecutionAsync");
            Console.WriteLine("6: MultipleTasksExecutionEfficientWithTimeResultAsync");
            
            Console.WriteLine("e: Exit");

            return Console.ReadKey().Key;
        }

        private static async Task ExecuteMenu(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.NumPad1:
                    await CallerContinuesWhileTaskIsBeingExecutedAsync();
                    break;
                case ConsoleKey.NumPad2:
                    await CallerAwaitsTaskToBeExecutedAsync();
                    break;
                case ConsoleKey.NumPad3:
                    await MultipleTasksExecutionInefficientAsync();
                    break;
                case ConsoleKey.NumPad4:
                    await MultipleTasksExecutionEfficientAsync();
                    break;
                case ConsoleKey.NumPad5:
                    MultipleTasksListExecutionAsync();
                    break;
                case ConsoleKey.NumPad6:
                    await MultipleTasksExecutionEfficientWithTimeResultAsync();
                    break;
                default:
                    break;
            }
        }

        private static async Task CallerContinuesWhileTaskIsBeingExecutedAsync()
        {
            Console.WriteLine("Starting task (Caller)");
            var task = ExecuteTimeConsumingTaskAsync();
            Console.WriteLine("Continuing code execution (Caller)");
            int result = await task;
            Console.WriteLine($"result (Caller): {result}");
        }

        private static async Task CallerAwaitsTaskToBeExecutedAsync()
        {
            Console.WriteLine("Starting task (Caller)");
            var result = await ExecuteTimeConsumingTaskAsync();
            Console.WriteLine("Continuing code execution (Caller)");
            Console.WriteLine($"result (Caller): {result}");
        }

        private static async Task MultipleTasksExecutionInefficientAsync()
        {
            Console.WriteLine("Starting task1 (Caller)");
            var result1 = await ExecuteParrallelTask1Async();
            Console.WriteLine("Starting task2 (Caller)");
            var result2 = await ExecuteParrallelTask2Async();
            Console.WriteLine("Waiting tasks to be completed (Caller)");
            var result = result1 + result2;
            Console.WriteLine($"Result: {result}");
        }

        private static async Task MultipleTasksExecutionEfficientAsync()
        {
            Console.WriteLine("Starting task1 (Caller)");
            var task1 = ExecuteParrallelTask1Async();
            Console.WriteLine("Starting task2 (Caller)");
            var task2 = ExecuteParrallelTask2Async();
            Console.WriteLine("Waiting tasks to be completed (Caller)");
            var result = await task1 + await task2;
            Console.WriteLine($"Result: {result}");
        }

        private static void MultipleTasksListExecutionAsync()
        {
            var tasks = new List<Task>();
            Console.WriteLine("Adding task1 to the list (Caller)");
            tasks.Add(ExecuteTaskAsync(1500));
            Console.WriteLine("Adding task2 to the list (Caller)");
            tasks.Add(ExecuteTaskAsync(1000));
            Console.WriteLine("Waiting tasks to be completed (Caller)");
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Tasks completed (Caller)");
        }

        private static async Task MultipleTasksExecutionEfficientWithTimeResultAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Starting task1 and stopwatch (Caller)");
            stopwatch.Start();
            var task1 = ExecuteParrallelTaskAsync(1000);
            Console.WriteLine("Starting task2 (Caller)");
            var task2 = ExecuteParrallelTaskAsync(3000);
            Console.WriteLine($"Waiting tasks to be completed (Caller)");
            var result = await task1 + await task2;
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed {stopwatch.Elapsed}");
            Console.WriteLine($"Result: {result}");
        }

        private static async Task<int> ExecuteTimeConsumingTaskAsync()
        {
            await Task.Delay(1000);
            Console.WriteLine("Task ending (Task)");
            return 1 + 1;
        }

        private static async Task<int> ExecuteParrallelTask1Async()
        {
            Console.WriteLine("Parrallel Task Starting (Task1)");
            await Task.Delay(2000);
            Console.WriteLine("Parrallel Task ending (Task1)");
            return 1 + 1;
        }

        private static async Task<int> ExecuteParrallelTask2Async()
        {
            Console.WriteLine("Parrallel Task Starting (Task2)");
            await Task.Delay(1000);
            Console.WriteLine("Parrallel Task ending (Task2)");
            return 2 + 2;
        }
        private static async Task<int> ExecuteParrallelTaskAsync(int ms)
        {
            Console.WriteLine($"Parrallel Task Starting (Task {ms})");
            await Task.Delay(ms);
            Console.WriteLine($"Parrallel Task ending (Task {ms})");
            return 1 + 1;
        }

        private static async Task ExecuteTaskAsync(int ms)
        {
            await Task.Delay(ms);
            Console.WriteLine("Task has finished");
        }
    }

   
}
