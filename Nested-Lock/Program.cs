using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nested_Lock
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
                var lock1 = acc1.Id < acc2.Id ? acc1 : acc2;
                var lock2 = acc1.Id < acc2.Id ? acc2 : acc1;
                lock (lock1)
                {
                    Thread.Sleep(1000);
                    lock (lock2)
                    {
                        Console.WriteLine($"Finished transfering sum {sum}");
                    }
                }
            });
            return task;
        }
    }
}
