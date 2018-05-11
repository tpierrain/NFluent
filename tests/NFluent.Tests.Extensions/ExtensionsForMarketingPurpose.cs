 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="ExtensionsForMarketingPurpose.cs" company="">
 //   Copyright 2013 Thomas PIERRAIN
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

//ReSharper disable once CheckNamespace
namespace NFluent.Tests
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// For documentation and marketing purpose only.
    /// </summary>
    public static class ExtensionsForMarketingPurpose
    {
        /// <summary>
        /// For documentation and marketing purpose only.
        /// </summary>
        /// <param name="tddCheck">you don't give a...</param>
        /// <param name="nfluentForMarketing">don't give a ...</param>
        /// <returns>hey: don't give a...</returns>
        public static ICheck<TddForMarketing> With(this ICheck<TddForMarketing> tddCheck, NFluentForMarketing nfluentForMarketing)
        {
            return null;
        }

        /// <summary>
        /// For documentation and marketing purpose only.
        /// </summary>
        /// <typeparam name="T">don't give a ...</typeparam>
        /// <param name="tddCheck">The TDD check.</param>
        // ReSharper disable once UnusedTypeParameter
        public static void IsAnInstanceOf<T>(this ICheck<TddForMarketing> tddCheck)
        {
        }
    }

    #region fake methods to make the NFluentMotto  test compile

    /// <summary>
    /// Fake class only for marketing and documentation purpose.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class TddForMarketing
    {
    }

    /// <summary>
    /// Fake class only for marketing and documentation purpose.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class NFluentForMarketing
    {
    }

    /// <summary>
    /// Fake class only for marketing and documentation purpose.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public class Awesomeness
    {
    }

    #endregion
}