using System;
using System.Threading;

namespace Deadlock
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass MC = new MyClass();

            //Make threads.
            Thread th1 = new Thread(() => MC.transferFromAToB(100));
            Thread th2 = new Thread(() => MC.transferFromBToA(100));

            //Start threads.
            th1.Start();
            th2.Start();

            Console.ReadLine();
        }
    }
    class MyClass
    {
        private int a;
        private int b;

        //lock objects.
        private Object aLock = new Object();
        private Object bLock = new Object();

        public MyClass()
        {
            a = 1000;
            b = 1000;
        }

        #region Deadlock Code


        public void transferFromAToB(int amount)
        {
            lock (aLock)
            {
                //Simulate computing time to get the value;
                Thread.Sleep(100);
                a = a - amount;

                lock (bLock)
                {
                    //Simulate computing time to get the value;
                    Thread.Sleep(100);
                    b = b + amount;
                }
            }

            Console.WriteLine(amount + " was transfered from A to B.");
        }

        public void transferFromBToA(int amount)
        {
            lock (bLock)
            {
                //Simulate computing time to get the value;
                Thread.Sleep(100);
                b = b - amount;

                lock (aLock)
                {
                    //Simulate computing time to get the value;
                    Thread.Sleep(100);
                    a = a + amount;
                }
            }

            Console.WriteLine(amount + " was transfered from B to A.");
        }


        #endregion

        #region Resolved deadlock Code
        /*
        public void transferFromAToB(int amount)
        {
            lock (aLock)
            {
                //Simulate computing time to get the value;
                Thread.Sleep(100);
                a = a - amount;
              
                lock (bLock)
                {
                    //Simulate computing time to get the value;
                    Thread.Sleep(100);
                    b = b + amount;
                }
            }
            Console.WriteLine(amount + " was transfered from A to B.");
        }
        public void transferFromBToA(int amount)
        {
            lock (aLock)
            {
                //Simulate computing time to get the value;
                Thread.Sleep(100);
                a = a + amount;          
                lock (bLock)
                {
                    //Simulate computing time to get the value;
                    Thread.Sleep(100);
                    b = b - amount;
                }
            }
            Console.WriteLine(amount + " was transfered from B to A.");
        }
        */
        #endregion
    }
}
