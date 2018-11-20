using System;
using Xunit;
using NFluent;

namespace NFluent.Tests.Core.xUnit
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Check.ThatCode(() =>
                Check
                    .That(new Exception())
                    .IsSameReferenceAs(new Exception())).Throws<FluentCheckException>();
        }
    }
}
