using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Threading.GettingStarted
{
    [TestFixture]
    public class ThreadPoolingFixture
    {
        [TestFixture]
        public class ViaTPL
        {
            void Go()
            {
                Console.WriteLine("Hello from the thread pool!");
            }

            [Test]
            public void Create_and_start_a_new_task()
            {
                Task.Factory.StartNew(Go);    
            }

            
            string DownloadString(string uri)
            {
                using (var wc = new System.Net.WebClient())
                    //return wc.DownloadString(uri);
                    return "content for uri: " + uri;
            }

            [Test]
            public void Create_and_start_a_new_task_using_generics_for_obtaining_back_a_result()
            {
                // Start the task executing:
                Task<string> task = Task.Factory.StartNew<string>
                  (() => DownloadString("http://www.google.com"));

                // We can do other work here and it will execute in parallel:
                Go();
                

                // When we need the task's return value, we query its Result property:
                // If it's still executing, the current thread will now block (wait)
                // until the task finishes:
                string result = task.Result;
                Console.WriteLine("Got: " + result);
            }
        }

        [TestFixture]
        public class WithoutTPL
        {
            [Test]
            public void Using_QueueUserWorkItem()
            {
                ThreadPool.QueueUserWorkItem(Go);
                ThreadPool.QueueUserWorkItem(Go, 123);

            }
            void Go(object data)   // data will be null with the first call.
            {   Console.WriteLine("Hello from the thread pool! " + data);   }


            [Test]
            public void Using_async_delegates_in_order_to_be_able_to_pass_typed_args_and_get_back_typed_results_too()
            // Furthermore, unhandled exceptions on asynchronous delegates are conveniently rethrown (marshalled back...) on the original thread 
            // (or more accurately, the thread that calls EndInvoke), and so they don’t need explicit handling.
            {
                Func<string, int> method = Work;
                IAsyncResult cookie = method.BeginInvoke("test", null, null);
                //
                // ... here's where we can do other work in parallel...
                //
                int result = method.EndInvoke(cookie);
                Console.WriteLine("String length is: " + result);
            }
            int Work(string s) { return s.Length; }
        }

      
    }
}
