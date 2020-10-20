using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MBCT.Csv.Depricated
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class CsvExtensions
    {
        //private static string[] _wrapif = { " ", "\"", "\'", ",", Environment.NewLine };
        private static string _dq = $"{(char)34}"; //double-quote
        private static string _sq = $"{(char)39}"; //single-quote
        private static string[] _doubleupif = {$"{_dq}", $"{_sq}"};

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CsvSanitize(this string value)
        {
            foreach (string s in _doubleupif)
            {
                if (value.Contains(s))
                {
                    value = value.Replace(s, s + s);
                }
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CsvWrap(this string value)
        {
            return $"{_dq}{value}{_dq}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CsvSanitizeAndWrap(this string value)
        {
            return value.CsvSanitize().CsvWrap();
        }

    }
}
