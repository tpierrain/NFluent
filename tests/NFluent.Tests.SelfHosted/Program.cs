namespace NFluent.Tests.SelfHosted
{
    using System;

    using NFluent.Helpers;

    /// <summary>
    /// Performs some test to simulate unknown test framework
    /// </summary>
    public class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                BasicTest();
                ExceptionScanTest();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return 0;
        }

        public static void BasicTest()
        {
            Check.That("MsTest").IsNotEmpty();

            Check.ThatCode(() => Check.That("MsTest").IsEqualTo("great")).Throws<FluentCheckException>();
        }

        public static void ExceptionScanTest()
        {
            Check.That(ExceptionHelper.BuildException("Test")).IsInstanceOf<FluentCheckException>();
        }
    }
}