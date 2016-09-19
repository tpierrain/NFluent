using System;

namespace NFluent.Tests.Helpers
{
    using System.Globalization;
    using System.Threading;

    public class CultureSession : IDisposable
    {
        private readonly CultureInfo savedCulture;
        private readonly CultureInfo savedUiCulture;

        public CultureSession()
        {
        }

        public CultureSession(string culture)
        {
            this.savedCulture = CultureInfo.CurrentUICulture;
            this.savedUiCulture = CultureInfo.CurrentUICulture;
            var newCulture = new CultureInfo(culture);

            SetCulture(newCulture, newCulture);
        }

        private static void SetCulture(CultureInfo newCulture, CultureInfo newUICulture)
        {
#if CORE
            CultureInfo.CurrentUICulture = newCulture;
            CultureInfo.CurrentCulture = newCulture;
#else
            Thread.CurrentThread.CurrentUICulture = newCulture;
            Thread.CurrentThread.CurrentCulture = newUICulture;
#endif
        }

        public void Dispose()
        {
            SetCulture(this.savedCulture, this.savedUiCulture);
        }
    }
}
