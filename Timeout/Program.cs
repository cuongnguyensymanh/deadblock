using System;
using System.Threading;
using System.Threading.Tasks;

namespace Timeout
{
    class Program
    {
        static void Main(string[] args)
        {
            TransferManager transfer = new TransferManager();
            Account acc1 = new Account(6);
            Account acc2 = new Account(4);
            transfer.DoDoubleTransfer(acc1, acc2);

            Console.ReadLine();
        }
    }

    internal class TransferManager
    {
        public void DoDoubleTransfer(Account acc1, Account acc2)
        {
            Console.WriteLine("Starting...");
            var task1 = Transfer(acc1, acc2, 500);
            var task2 = Transfer(acc2, acc1, 600);
            Task.WaitAll(task1, task2);
            Console.WriteLine("Finished...");
        }

        private Task Transfer(Account acc1, Account acc2, int sum)
        {
            var task = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        bool entered = Monitor.TryEnter(acc1, TimeSpan.FromMilliseconds(100));
                        if (!entered) continue;
                        entered = Monitor.TryEnter(acc2, TimeSpan.FromMilliseconds(100));
                        if (!entered) continue;

                    //do operation
                    Console.WriteLine($"Finished transferring sum {sum}");
                        break;
                    }
                    finally
                    {
                        if (Monitor.IsEntered(acc1)) Monitor.Exit(acc1);
                        if (Monitor.IsEntered(acc2)) Monitor.Exit(acc2);
                        Thread.Sleep(200);
                    }
                }
            });
            return task;
        }
    }
}
