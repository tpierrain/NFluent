// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FluentDynamicCheck.cs" company="">
// //   Copyright 2017 Cyrille DUPUYDAUBY
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
    using NFluent.Extensibility;
    using NFluent.Kernel;

#if PORTABLE || NETSTANDARD1_3 || DOTNET_45
    /// <summary>
    /// Provides fluent check methods to be executed on a given value.
    /// </summary>
    public class FluentDynamicCheck: IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense, IForkableCheck
    {
        private readonly dynamic value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public FluentDynamicCheck(dynamic value)
        {
            this.value = value;
        }

        /// <summary>
        /// Checks if the given dynamic is null.
        /// </summary>
        public DynamicCheckLink IsNotNull()
        {
            if (this.value != null)
            {
                return new DynamicCheckLink(this);
            }

            var message = FluentMessage.BuildMessage("The {0} is null whereas it must not.").For("dynamic").On(this.value);
            throw new FluentCheckException(message.ToString());
        }

        /// <summary>
        /// Checks if the given dynamic has the expected reference.
        /// </summary>
        /// <param name="expected">Expected reference.</param>
        public DynamicCheckLink IsSameReferenceAs(dynamic expected)
        {
            if (object.ReferenceEquals(this.value, expected))
            {
                return new DynamicCheckLink(this);
            }

            var message = FluentMessage.BuildMessage("The {0} is not the expected reference.").For("dynamic").Expected(expected).And.On(this.value);
            throw new FluentCheckException(message.ToString());
        }

        /// <summary>
        /// Checks if the given dynamic has the expected value.
        /// </summary>
        /// <param name="expected">The expected value. Comparison is done using <see cref="object.Equals(object, object)"/></param>
        public DynamicCheckLink IsEqualTo(dynamic expected)
        {
            if (object.Equals(this.value, expected))
            {
                return new DynamicCheckLink(this);
            }
            var message = FluentMessage.BuildMessage("The {0} is not equal to the {1}.").For("dynamic").Expected(expected).And.On(this.value);
            throw new FluentCheckException(message.ToString());
        }


        /// <inheritdoc cref="IForkableCheck.ForkInstance"/>
        public object ForkInstance()
        {
            return new FluentDynamicCheck(this.value);
        }
    }
#endif
}