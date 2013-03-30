//namespace NFluent.Tests.ExtensionsSpike
//{
//    using System;
//    using NUnit.Framework;
//    using Spike.Core;
//    using Spike.Tests;

//    [TestFixture]
//    public class FluentAssertionRepositoryTests
//    {
//        [Test]
//        public void Repository()
//        {
//            FluentAssertionRepository repository = new FluentAssertionRepository();
//            Version v0 = new Version(0,0);
//            Version v1 = new Version(1,0);
//            Version v2 = new Version(2,0);
            
//            //var fluentAssertion = new ComparableFluentAssertion(v1);
//            Check.That(v1).IsBefore(v2);

//            //var fluentAssertion = repository.GetFluentAssertionForInstance<Version>(v1);
            
//            // TODO use NFluent assertion here instead (should implement IsNotNull)
//            //Assert.IsNotNull(fluentAssertion);
//            //Assert.IsTrue(fluentAssertion.Sut is IComparable);
//            //fluentAssertion.IsBefore(v2);
//        }
//    }

//    public class FluentAssertionRepository
//    {
//        public IFluentAssertion<B> GetFluentAssertionForInstance<T,B>(T sut) where T : B
//        {
//            return null; //return new ComparableFluentAssertion(sut as IComparable);
//        }
//    }
//}
