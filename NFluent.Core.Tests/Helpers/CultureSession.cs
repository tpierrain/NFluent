// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="CultureSession.cs" company="">
// //   Copyright 2016 Cyrille DUPUYDAUBY
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
using System;

namespace NFluent.Tests.Helpers
{
    using System.Globalization;
#if !NETCOREAPP1_0
    using System.Threading;
#endif
    public class CultureSession : IDisposable
    {
        private readonly CultureInfo savedCulture;
        private readonly CultureInfo savedUiCulture;

        public CultureSession(string culture)
        {
            this.savedCulture = CultureInfo.CurrentUICulture;
            this.savedUiCulture = CultureInfo.CurrentUICulture;
            var newCulture = new CultureInfo(culture);

            SetCulture(newCulture, newCulture);
        }

        private static void SetCulture(CultureInfo newCulture, CultureInfo newUiCulture)
        {
#if NETCOREAPP1_0
            CultureInfo.CurrentUICulture = newUiCulture;
            CultureInfo.CurrentCulture = newCulture;
#else
            Thread.CurrentThread.CurrentUICulture = newUiCulture;
            Thread.CurrentThread.CurrentCulture = newCulture;
#endif
        }

        public void Dispose()
        {
            SetCulture(this.savedCulture, this.savedUiCulture);
        }
    }
}
