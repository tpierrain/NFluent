// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpHelper.cs" company="">
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
// </copyright>
// <summary>
//   Defines the HttpHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Helpers
{
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Helper class to provide http helper like stream reading.
    /// </summary>
    internal static class HttpHelper
    {
        public static string ReadResponseStream(HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string ReadGZipStream(HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (var reader = new StreamReader(gzipStream, Encoding.Default))
                    {
                        return reader.ReadToEnd();
                    }    
                }
            }
        }
    }
}
