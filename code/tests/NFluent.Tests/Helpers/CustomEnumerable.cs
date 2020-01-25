using System;
using System.Collections.Generic;

namespace NFluent.Tests.Helpers
{
    using System.Collections;

    class CustomEnumerable<T> : IDisposable, IEnumerable<T>
    {
        private IEnumerable<T> source;

        public CustomEnumerable(IEnumerable<T> source)
        {
            this.source = source;
        }

        public void Dispose()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
