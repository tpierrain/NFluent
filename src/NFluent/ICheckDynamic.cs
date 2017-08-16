// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ICheckDynamic.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Rui CARVALHO
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
#if PORTABLE || NETSTANDARD1_3 || DOTNET_45
    /// <summary>
    /// interface for dynamic related checks
    /// </summary>
    public interface ICheckDynamic
    {
        /// <summary>
        /// Checks if the given dynamic is null
        /// </summary>
        /// <returns></returns>
        ICheckDynamic IsNotNull();
    }
#endif
}