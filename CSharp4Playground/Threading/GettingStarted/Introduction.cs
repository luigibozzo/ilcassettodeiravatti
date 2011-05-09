using System;
using System.Threading;
using NUnit.Framework;

namespace Threading
{
    [TestFixture]
    public class Introduction
    {
        [Test]
        public void StartAThreadAndDoSomeWorkInParallel()
        {
            Thread t = new Thread(WriteY); // Kick off a new thread
            t.Start(); // running WriteY()
            Assert.IsTrue(t.IsAlive);
            
            // Simultaneously, do something on the main thread.
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
                Console.Write("x");
            }
            Thread.Sleep(40*100 + 1000);
            Assert.IsFalse(t.IsAlive);
        }

        private static void WriteY()
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(50);
                Console.Write("YY");
            }
        }

        [Test]
        public void CallTheSameMethodFromMainAndChildThread()
        {
            new Thread(WriteY).Start();
            WriteY();
        }

        [Test]
        public void SharedResourceCouldPreventThreadSafety()    
        {
            // even if unpredictable, often will print "Doing" twice
            done = false;
            new Thread(CheckDoSet).Start();
            CheckDoSet();

            Thread.Sleep(500);
            Console.WriteLine();

            // in this case, often will print "Doing" once
            done = false;
            new Thread(CheckSetDo).Start();
            CheckSetDo();
        }
        private static bool done;
        private static void CheckDoSet()
        {
            if (!done)
            {
                Console.WriteLine("Doing into the CheckDoSet!");
                done = true;
            }
        }

        private static void CheckSetDo()
        {
            if (!done)
            {
                done = true;
                Console.WriteLine("Doing into the CheckSetDo");
            }
        }


        [Test]
        public void ThreadSafetyThroughExclusiveLocks()
        {
            new Thread(AquireLockAndDo).Start();
            AquireLockAndDo();
        }
        static readonly object locker = new object();
        private static void AquireLockAndDo()
        {
            lock (locker)
            {
                if (!done) { Console.WriteLine("Done"); done = true; }
            }
        }

        [Test]
        public void WaitForAThreadCompletionUsingJoin()
        {
            var thread = new Thread(WriteY);
            thread.Start();
            thread.Join();
            Console.WriteLine("Thread ended");
        }
    }
}