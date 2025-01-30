// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IsEquivalentShould.cs" company="NFluent">
//   Copyright 2025 Cyrille DUPUYDAUBY
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

namespace NFluent.NetCore3.Tests.ReportedIssues;

using NUnit.Framework;

public class IsEquivalentShould
{


    internal class MyCustomObject(string Property1, string Property2, NestedObject NestedObject)
    {
        public string Property1 { get; init; } = Property1;
        public string Property2 { get; init; } = Property2;
        public NestedObject NestedObject { get; init; } = NestedObject;
    }

    internal class NestedObject(string Property1, string Property2)
    {
        public string Property1 { get; init; } = Property1;
        public string Property2 { get; init; } = Property2;
    } 
}