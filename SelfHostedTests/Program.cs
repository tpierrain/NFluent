using System;

namespace SelfHostedTests
{
    using System.Reflection;
    using NUnit.Common;
    using NUnitLite;

    public class Program
    {
        public static int Main(string[] args)
        {
            return new AutoRun(typeof(Program).GetTypeInfo().Assembly)
                .Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
        }
    }
}