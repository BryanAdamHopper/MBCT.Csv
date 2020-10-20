using System;

namespace MBCT.Csv
{
    /// <summary>
    /// Configuration class used with CsvReader(V2).
    /// </summary>
    public class CsvReaderV2Config
    {
        /// <summary>
        /// Error message when a csv is corrupt.
        /// </summary>
        public const string ErrorCorruptCsv = "CSV text is corrupt";

        /// <summary>
        /// ApplyQualifier
        /// </summary>
        public enum ApplyQualifier : byte
        {
            #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Always,
            OnlyWhenNeeded,
            Never
            #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        /// <summary>
        /// CsvDelimiter
        /// </summary>
        public enum CsvDelimiter : byte
        {
            #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Comma,
            SemiColon,
            Tab,
            Space
            #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        /// <summary>
        /// CsvQualifier
        /// </summary>
        public enum CsvQualifier : byte
        {
            #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            DoubleQuote,
            SingleQuote
            #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        /// <summary>
        /// Indicate if data should always be wrapped or only when needed. Default is ApplyWrapper.Always.
        /// </summary>
        public ApplyQualifier QualifierWhen { get; set; } = ApplyQualifier.Always;
        /// <summary>
        /// CSV delimiter. Default is comma.
        /// </summary>
        public CsvDelimiter Delimiter { get; set; } = CsvDelimiter.Comma;
        /// <summary>
        /// Char used to wrap data that needs it.
        /// </summary>
        public CsvQualifier Qualifier { get; set; } = CsvQualifier.DoubleQuote;

        /// <summary>
        /// Represents a new line. Default is Environment.NewLine.
        /// </summary>
        public string[] NewLine { get; } = { Environment.NewLine };

        /// <summary>
        /// Data should be wrapped if any of these chars are present.
        /// </summary>
        public string[] WrapIf { get; } = {
            $"{(char)9}" /*Tab*/,
            $"{(char)32}" /*Space*/, 
            $"{(char)34}" /*DoubleQuote*/,
            $"{(char)39}" /*SingleQuote*/,
            $"{(char)44}" /*Comma*/,
            $"{(char)59}" /*SemiColon*/,
            Environment.NewLine /*NewLine*/
        };

        /// <summary>
        /// Number of rows to display back to datatable. -1 for all rows. 0 for just headers. (Default is -1)
        /// </summary>
        public int DisplayRows { get; set; } = -1;

        /// <summary>
        /// String to DataTable: is there a header row? DataTable to String: Should the header row be included in the output to csv string. (Default is true)
        /// </summary>
        public bool IncludeHeaders { get; set; } = true;

        /// <summary>
        /// Eliminate false positives for rows that are in fact empty and should be ignored. This could be a row that is not null, but contains empty strings or only whitespace.
        /// </summary>
        public bool IgnoreFalsePositiveEmptyRows { get; set; } = true;

        /// <summary>
        /// Returns a string representation of the currently set delimiter.
        /// </summary>
        /// <returns></returns>
        public char GetDelimiter()
        {
            switch (Delimiter)
            {
                //case CsvDelimiter.Comma:
                //    {
                //        return (char)44;
                //    }
                case CsvDelimiter.SemiColon:
                    {
                        return (char)59;
                    }
                case CsvDelimiter.Space:
                    {
                        return (char)32;
                    }
                case CsvDelimiter.Tab:
                    {
                        return (char)9;
                    }
                default: /* CsvDelimiter.Comma */
                    {
                        return (char)44;
                    }
            }
        }

        /// <summary>
        /// Returns a string representation of the currently set qualifier.
        /// </summary>
        /// <returns></returns>
        public char GetQualifier()
        {
            switch (Qualifier)
            {
                //case CsvQualifier.DoubleQuote:
                //    {
                //        return (char)34;
                //    }
                case CsvQualifier.SingleQuote:
                    {
                        return (char)39;
                    }
                default: /* CsvQualifier.DoubleQuote */
                    {
                        return (char)34;
                    }
            }
        }

        /// <summary>
        /// Returns an ApplyQualifier enum value based on provided string. Defaults to 'Always' if conversion is not possible.
        /// </summary>
        /// <param name="text"></param>
        public ApplyQualifier ConvertQualifier(string text)
        {
            var value = ApplyQualifier.Always;

            switch (text)
            {
                //case "Always":
                //    {
                //        value = ApplyQualifier.Always;
                //        break;
                //    }
                case "OnlyWhenNeeded":
                    {
                        value = ApplyQualifier.OnlyWhenNeeded;
                        break;
                    }
                case "Never":
                    {
                        value = ApplyQualifier.Never;
                        break;
                    }
                default: /* ApplyQualifier.Always */
                    {
                        value = ApplyQualifier.Always;
                        break;
                    }
            }

            return value;
        }

        /// <summary>
        /// Set QualifierWhen from a string value. Defaults to 'Always' if conversion is not possible.
        /// </summary>
        /// <param name="text"></param>
        public void SetQualifierWhen(string text)
        {
            QualifierWhen = ConvertQualifier(text);
        }

    }
}
