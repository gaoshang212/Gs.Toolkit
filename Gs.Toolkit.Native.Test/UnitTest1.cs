using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gs.Toolkit.Native.Test
{
    [TestClass]
    public class UnitTest1
    {
        [NativeFuncton("test")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int Test(int p_sleep);

        [TestMethod]
        public void DelegateFunction()
        {
            int input = 0;
            int result = -1;
            using (var native = NativeFactory.Create(@"../../libtest.dll"))
            {
                using (var native1 = NativeFactory.Create(@"../../libtest.dll"))
                {
                    var test = native1.GetFunction<UnitTest1.Test>();
                    result = test(input);
                }
            }

            Assert.AreEqual(input, result);

        }

        [TestMethod]
        public void AutoFunction()
        {
            int input = 0;
            int result = -1;
            using (var native = NativeFactory.Create(@"../../libtest.dll"))
            {
                result = native.Invoke<int>("test", input);
            }

            Assert.AreEqual(input, result);

        }

        [TestMethod]
        public void DynamicFunction()
        {
            int input = 0;
            int result = -1;
            using (dynamic native = NativeFactory.Create(@"../../libtest.dll"))
            {
                result = native.test<int>(input);
            }

            Assert.AreEqual(input, result);

        }

        [TestMethod]
        public void CreateTest()
        {
            string input = "";

            try
            {
                using (dynamic native = NativeFactory.Create(input))
                {
                    //result = native.test<int>(input);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
