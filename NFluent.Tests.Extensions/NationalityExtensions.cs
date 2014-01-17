// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="NationalityExtensions.cs" company="">
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
namespace NFluent.Tests.Extensions
{
    using NFluent.Extensibility;

    /// <summary>
    /// Extensions methods check
    /// </summary>
    public static class NationalityExtensions
    {
        public static ICheckLink<IStructCheck<Nationality>> IsEuropean(this IStructCheck<Nationality> check)
        {
            var structChecker = ExtensibilityHelper.ExtractStructChecker(check);

            return structChecker.ExecuteCheck(
                () =>
                {
                    if (!structChecker.Value.Equals(Nationality.English) && !structChecker.Value.Equals(Nationality.German) && !structChecker.Value.Equals(Nationality.Serbian) && !structChecker.Value.Equals(Nationality.French))
                    {
                        var message = FluentMessage.BuildMessage("The {0} is not part of Europe.").For("Nationality").On(structChecker.Value).ToString();
                        throw new FluentCheckException(message);
                    }        
                }, 
                FluentMessage.BuildMessage("The {0} is part of Europe whereas it must not.").For("Nationality").On(structChecker.Value).ToString());
        }

        public static ICheckLink<IStructCheck<Nationality>> IsOccidental(this IStructCheck<Nationality> check)
        {
            var structChecker = ExtensibilityHelper.ExtractStructChecker(check);

            return structChecker.ReturnValueForLinkage;
        }
    }
}
