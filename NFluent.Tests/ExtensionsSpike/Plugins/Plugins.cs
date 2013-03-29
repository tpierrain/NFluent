namespace Spike.Plugins
{
    using System;
    using Spike.Core;

    public class IntFluent : IGenericFluent<int>
    {
        public int Sut { get; private set; }
        public IntFluent(int? sut) { Sut = sut ?? 0; }
    }

    public static class IntExt
    {
        public static void IsCoolNumber(this IGenericFluent<int> fluentInterface)
        {
            if (fluentInterface.Sut != 42) throw new Exception("Not cool, try 42!");
        }
    }

    public class StringFluent : IGenericFluent<string>
    {
        public string Sut { get; private set; }
        public StringFluent(string sut) { Sut = sut; }
    }

    public static class StringExt
    {
        public static void HasTheForce(this IGenericFluent<string> fluentInterface)
        {
            if (!fluentInterface.Sut.ToLower().Contains("force")) throw new Exception("damn, you're so common!");
        }
    }
}
