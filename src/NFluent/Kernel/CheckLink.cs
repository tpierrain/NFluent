// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="CheckLink.cs" company="">
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
namespace NFluent.Kernel
{
    using System.Diagnostics;

    internal class CheckLink<T> : ICheckLink<T> where T : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
    {
        private readonly IForkableCheck forkableCheck;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckLink{T}" /> class.
        /// </summary>
        /// <param name="previousCheck">The previous fluent check.</param>
        public CheckLink(IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense previousCheck)
        {
            this.forkableCheck = previousCheck as IForkableCheck;
        }

        /// <inheritdoc />
        public T And => this.forkableCheck.ForkInstance() as T;

        /// <inheritdoc />
        public override string ToString()
        {
            return "Success";
        }
    }
}