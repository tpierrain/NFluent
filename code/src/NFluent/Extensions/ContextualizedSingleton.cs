// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ContextualizedSingleton.cs" company="NFluent">
//   Copyright 2021 Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Helpers
{
    using System;
    using System.Collections.Generic;

    internal sealed class ContextualizedSingleton<T>
    {
        public T DefaultValue { get; set; }

        public T Value => !ThreadStaticStore<Stack<T>>.Exists(this) ? this.DefaultValue : this.MasterStack.Peek();

        private Stack<T> MasterStack => ThreadStaticStore<Stack<T>>.GetStack(this);

        public IDisposable ScopedCustomization(T newValue)
        {
            return new ScopedInstance(newValue, this);
        }

        private void ClearStack()
        {
            ThreadStaticStore<Stack<T>>.ClearStack(this);
        }

        public void Push(T value)
        {
            this.MasterStack.Push(value);
        }

        public void Pop()
        {
            var stack = this.MasterStack;
            stack.Pop();
            if (stack.Count == 0)
            {
                this.ClearStack();
            }
        }

        private sealed class ScopedInstance : IDisposable
        {
            private readonly ContextualizedSingleton<T> singleton;

            public ScopedInstance(T val, ContextualizedSingleton<T> singleton)
            {
                this.singleton = singleton;
                singleton.Push(val);
            }

            public void Dispose()
            {
                this.singleton.Pop();
            }
        }
    }
}