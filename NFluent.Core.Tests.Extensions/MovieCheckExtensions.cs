// -------------------------------------------------------------------------------------------------------------------
// <copyright file="MovieCheckExtensions.cs" company="">
//   Copyright 2013 Thomas Pierrain, Cyrille Dupuydauby
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace NFluent.Tests.Extensions
{
    using Extensibility;
    using NFluent.Extensions;

    public static class MovieCheckExtensions
    {
        public static ICheckLink<ICheck<Movie>> IsDirectedBy(this ICheck<Movie> check, string directorFullName)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (!checker.Value.Director.ToString().ToLower().Contains(directorFullName.ToLower()))
                        throw new FluentCheckException("Not!");
                },
                "Effectively...");
        }

        public static ICheckLink<ICheck<Movie>> IsAfGreatMovie(this ICheck<Movie> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (!checker.Value.Name.Contains("Eternal sunshine of the spotless mind"))
                        throw new FluentCheckException(string.Format("[{0}]Is not a great movie",
                            checker.Value.Name.ToStringProperlyFormated()));
                },
                string.Format("[{0}]Is a great movie!", checker.Value.Name.ToStringProperlyFormated()));
        }
    }
}