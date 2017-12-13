using System.Globalization;

namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// DoubleExtension class.
    ///</summary>
    ///<remarks>
    /// This class permit to contain extension functions for double objects.
    /// </remarks>
    public static class DoubleExtension
    {
        /// <summary>
        /// This function permits to convert a string value to double.
        /// </summary>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string theValue)
        {
            double retNum;
            var result = double.TryParse(theValue, out retNum);
            return result ? retNum : 0;
        }

        /// <summary>
        /// This funciton permits to format a double using specific culture currency settings.
        /// </summary>
        /// <param name="value">The double to be formatted.</param>
        /// <param name="cultureName">The string representation for the culture to be used, for instance "en-US" for US English.</param>
        /// <returns>The double formatted based on specific culture currency settings.</returns>
        public static string ToSpecificCurrencyString(this double value, string cultureName)
        {
            CultureInfo currentCulture = new CultureInfo(cultureName);
            return (string.Format(currentCulture, "{0:C}", value));
        }

        /// <summary>
        /// This function permits to display a double value with a precision.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="precision">Precision</param>
        /// <returns>String</returns>
        public static string DisplayDouble(this double value, int precision)
        {
            return value.ToString("N" + precision);
        }

    }
}