using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Threading.GettingStarted
{
    [TestFixture]
    public class ThreadPoolingFixture
    {
        [TestFixture]
        public class ViaTPL
        {
            [Test]
            public void Test1()
            {
                Assert.Pass("ok");
            }
        }
        [TestFixture]
        public class WithoutTPL
        {
            [Test]
            public void Test2()
            {
                Assert.Fail("Ko");
            }
        }
    }
}
