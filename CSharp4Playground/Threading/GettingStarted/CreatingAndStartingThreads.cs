using System;
using System.Threading;
using NUnit.Framework;

namespace Threading.GettingStarted
{
    [TestFixture]
    public class CreatingAndStartingThreads
    {
        private static void Go()
        {
            Console.WriteLine("hello!");
        }

        [Test]
        public void Create_using_thread_start()
        {
            Thread t = new Thread(Go);
            t.Start(); // Run Go() on the new thread.
            Go(); // Simultaneously run Go() in the main thread.
        }

        [Test]
        public void Thread_start_can_be_infered()
        {
            Thread t = new Thread(Go); // No need to explicitly use ThreadStart  
            t.Start();
            Go();
        }

        [Test]
        public void Create_using_lambda()
        {
            Thread t = new Thread(() => Console.WriteLine("Hello!"));
            t.Start();
        }
    }


    [TestFixture]
    public class PassingDataToAThread
    {
        public static void Print(string msg)
        {
            Console.WriteLine(msg);
        }

        [Test]
        public void Using_a_lambda_that_calls_the_desired_method_with_the_desired_arguments()
        {
            Thread t = new Thread(() => Print("Hello from t!"));
            t.Start();
        }

        [Test]
        public void Wrap_the_entire_implementation_inside_a_multistatement_lambda()
        {
            new Thread(() =>
                           {
                               Console.WriteLine("I'm running on another thread!");
                               Console.WriteLine("This is so easy!");
                           }).Start();
        }

        [Test]
        public void Pay_attention_to_not_capture_variables_using_lambda()
        {
            for (int i = 0; i < 10; i++)
                new Thread(() => Console.Write(i)).Start(); // the output is non-deterministic (i.e. 0223557799)
        }
    }

    [TestFixture]
    public class NamingThreads
    {
        private static void Go()
        {
            Console.WriteLine("Hello from " + Thread.CurrentThread.Name);
        }

        [Test]
        public void Threads_can_be_named()
        {
            Thread.CurrentThread.Name = "main";
            Thread worker = new Thread(Go);
            worker.Name = "worker";
            worker.Start();
            Go();
        }
    }

    [TestFixture]
    public class ForegroundAndBackgroundAndPriority
    {
        [Test]
        public void A_thread_can_be_moved_background()
        {
            Thread worker = new Thread(() => Console.WriteLine("hello"));
            worker.IsBackground = true;
            worker.Start();
        }

        [Test]
        public void
            A_thread_priority_can_be_set_even_if_the_thread_priority_is_throttled_by_the_applications_process_priority()
        {
            Thread worker = new Thread(() => Console.WriteLine("hallo"));
            worker.Priority = ThreadPriority.Highest;
            worker.Start();
        }
    }

    [TestFixture]
    public class ExceptionHandling
    {
        [Test]
        public void Remember_to_handle_exceptions_on_all_thread_entry_methods_just_as_you_do_on_your_main_thread()
        {
            new Thread(Go).Start();
        }

        private static void Go()
        {
            try
            {
                // ...
                throw null; // The NullReferenceException will get caught below
                // ...
            }
            catch (Exception ex)
            {
                // Typically log the exception, and/or signal another thread
                // that we've come unstuck
                // ...
                Console.WriteLine("Exception caught");
            }
        }
    }
}