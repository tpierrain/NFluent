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
                AssumptionTest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            return 0;
        }

        private static void AssumptionTest()
        {
            Check.ThatCode(() => Assuming.That(2).IsEqualTo(3)).Throws<FluentCheckException>();
            Check.ThatCode(() => Assuming.That(2).IsEqualTo(3)).IsaFailingAssumption();
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