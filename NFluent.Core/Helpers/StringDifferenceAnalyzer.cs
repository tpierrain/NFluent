using System;
using System.Collections.Generic;

namespace NFluent.Helpers
{
    using Extensions;

    internal static class StringDifferenceAnalyzer
    {
        public static IList<StringDifference> Analyze(string actual, string expected)
        {
            if (actual != expected)
            {
                var stringDifference = new StringDifference();
                for (var i = 0;
                    i < Math.Min(actual.Length, expected.Length);
                    i++)
                {
                    if (actual[i] != expected[i])
                    {
                        if (StringExtensions.CompareChar(actual[i], expected[i], true))
                        {
                            if (stringDifference.Type != StringDifference.DifferenceMode.CaseDifference)
                            {
                                stringDifference.Type = StringDifference.DifferenceMode.CaseDifference;
                                stringDifference.Position = i;
                            }
                        }
                        else
                        {
                            stringDifference.Type = StringDifference.DifferenceMode.General;
                            stringDifference.Position = i;
                            break;
                        }
                    }
                }
                var result = new List<StringDifference> { stringDifference };
                return result;
            }
            return null;
        }
    }

    internal class StringDifference
    {
        public enum DifferenceMode
        {
            General,
            CaseDifference
        }

        public DifferenceMode Type;
        public int Position;
    }
}
