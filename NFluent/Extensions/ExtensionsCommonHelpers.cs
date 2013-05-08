// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsCommonHelpers.cs" company="">
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
namespace NFluent.Extensions
{
    using System;
    using System.Collections;
    using System.Globalization;

    /// <summary>
    /// Common helper methods for the NFluent extension methods.
    /// </summary>
    public static class ExtensionsCommonHelpers
    {
        /// <summary>
        /// Returns a string that represents the current object. If the object is already a string, this method will surround it with brackets.
        /// </summary>
        /// <param name="theObject">The theObject.</param>
        /// <returns>A string that represents the current object. If the object is already a string, this method will surround it with brackets.</returns>
        public static string ToStringProperlyFormated(this object theObject)
        {
            if (theObject is string)
            {
                return string.Format(@"""{0}""", theObject);
            }
            
            if (theObject is DateTime)
            {
                return ToStringProperlyFormated((DateTime)theObject);
            }

            if (theObject is bool)
            {
                return ToStringProperlyFormated((bool)theObject);
            }

            if (theObject is IEnumerable)
            {
                return ((IEnumerable)theObject).ToEnumeratedString();
            }

            if (theObject == null)
            {
                return "null";
            }
            else
            {
                return theObject.ToString();    
            }
        }

        /// <summary>
        /// Returns a string that represents the current DateTime.         
        /// </summary>
        /// <param name="theDateTime">The DateTime.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        public static string ToStringProperlyFormated(this DateTime theDateTime)
        {
            // return a ISO-8601 Date format
            return string.Format(CultureInfo.InvariantCulture, "{0:o}, Kind = {1}", theDateTime, theDateTime.Kind);
        }

        /// <summary>
        /// Returns a string that represents the current Boolean.         
        /// </summary>
        /// <param name="theBoolean">The Boolean.</param>
        /// <returns>A string that represents the current object with current culture ignore.</returns>
        public static string ToStringProperlyFormated(this bool theBoolean)
        {
            // Ensure that boolean values are not localized 
            return theBoolean.ToString(CultureInfo.InvariantCulture);
        }
    }
}