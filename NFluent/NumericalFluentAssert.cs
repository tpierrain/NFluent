// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="NumericalFluentAssert.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
    using System;

    public class NumericalFluentAssert<T> : INumericalFluentAssert
    {
        private T sut;

        public NumericalFluentAssert(T sut)
        {
            this.sut = sut;
        }

        /// <summary>
        /// Verifies that the actual value is equal to zero.
        /// </summary>
        /// <exception cref="FluentAssertionException">The value is not equal to zero.</exception>
        public void IsZero()
        {
            bool res = InternalIsZero(this.sut);

            if (!res)
            {
                throw new FluentAssertionException(String.Format("[{0}] is not equal to zero.", this.sut));
            }
        }

        /// <summary>
        /// Verifies that the actual value is NOT equal to zero.
        /// </summary>
        /// <exception cref="FluentAssertionException">The value is equal to zero.</exception>
        public void IsNotZero()
        {
            bool res = InternalIsZero(this.sut);

            if (res)
            {
                throw new FluentAssertionException(String.Format("[{0}] is equal to zero.", this.sut));
            }
        }

        /// <summary>
        /// Verifies that the actual value is strictly positive.
        /// </summary>
        /// <exception cref="FluentAssertionException">The value is not strictly positive.</exception>
        public void IsPositive()
        {
            if (Convert.ToInt32(this.sut) <= 0)
            {
                throw new FluentAssertionException(String.Format("[{0}] is not a strictly positive value.", this.sut));
            }
        }

        /// <summary>
        /// Checks whether a given value is equal to zero.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is equal to zero; false otherwise.</returns>
        private static bool InternalIsZero<T>(T value)
        {
            return Convert.ToInt64(value) == 0;
        }

        #region IFluentAssert members

        public void IsInstanceOf(Type expectedType)
        {
            IsInstanceHelper.IsInstanceOf(this.sut, expectedType);
        }

        public void IsNotInstanceOf(Type expectedType)
        {
            IsInstanceHelper.IsNotInstanceOf(this.sut, expectedType);
        }

        #endregion

    }
}