using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Workflow.Transverse.Environment;
using System.Text.RegularExpressions;

namespace Workflow.BusinessCore.BusinessLayer.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Retire la virgule en bout de chaine, même avec des saut de ligne...
        /// </summary>
        /// <param name="sb"></param>
        public static void SkipComma(this StringBuilder sb)
        {
            if (sb == null)
                return;

            char[] sauts = System.Environment.NewLine.ToCharArray();

            int end = sb.Length - 1;
            bool hasFound = false;
            while ((end > 0) && !hasFound)
            {
                if (sb[end] == ' ')
                {
                    end -= 1;
                    continue;
                }
                if ((sauts.Length > 0) && (end - sauts.Length >= 0))
                {
                    bool areEquals = true;
                    for (int i = end - sauts.Length + 1; (i <= end) && areEquals; i++)
                        areEquals = sauts[i - end + sauts.Length - 1] == sb[i];
                    if (areEquals)
                    {
                        end -= sauts.Length;
                        continue;
                    }
                }
                if (sb[end] == ',')
                    hasFound = true;
                else
                    return;
            }

            sb.Remove(end, 1);
        }

        /// <summary>
        /// Donne le champ Precision du format en entrée.
        /// </summary>
        /// <param name="format">Format descripteur</param>
        /// <remarks>Le format numérique est de la forme : Prefix|Coefficient|Precision|Suffix</remarks>
        /// <returns>Precision</returns>
        public static int GetPrecision(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return int.MinValue;

            string[] divFormat = format.Split(Constant.SEPARATOR_FORMAT);
            if (divFormat.Length != 4)
                return int.MinValue;

            string strPrecision = divFormat[2];
            int nPrecision = 0;
            Int32.TryParse(strPrecision, out nPrecision);
            return nPrecision;
        }

        /// <summary>
        /// Donnne le champ Coefficient du format en entrée.
        /// </summary>
        /// <param name="format">Format descripteur</param>
        /// <remarks>Le format numérique est de la forme : Prefix|Coefficient|Precision|Suffix</remarks>
        /// <returns>Coefficient</returns>
        public static double GetCoefficient(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return double.NaN;

            string[] divFormat = format.Split(Constant.SEPARATOR_FORMAT);
            if (divFormat.Length != 4)
                return double.NaN;

            string strCoefficient = divFormat[1];
            double coefficient = 1;
            Double.TryParse(strCoefficient, out coefficient);
            return Math.Abs(coefficient);
        }

        /// <summary>
        /// Donne le prefixe du format en entrée.
        /// </summary>
        /// <param name="format">Format descripteur</param>
        /// <remarks>Le format numérique est de la forme : Prefix|Coefficient|Precision|Suffix</remarks>
        /// <returns>Prefixe</returns>
        public static string GetPrefix(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return null;

            string[] divFormat = format.Split(Constant.SEPARATOR_FORMAT);
            if (divFormat.Length != 4)
                return null;

            return divFormat[0];
        }

        /// <summary>
        /// Donne le suffixe du format en entrée.
        /// </summary>
        /// <param name="format">Format descripteur</param>
        /// <remarks>Le format numérique est de la forme : Prefix|Coefficient|Precision|Suffix</remarks>
        /// <returns>Suffixe</returns>
        public static string GetSuffix(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                return null;

            string[] divFormat = format.Split(Constant.SEPARATOR_FORMAT);
            if (divFormat.Length != 4)
                return null;

            return divFormat[3];
        }

        /// <summary>
        /// Permet de vérifier le format d'un email.
        /// </summary>
        /// <param name="emailaddress"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                //MailAddress m = new MailAddress(emailaddress); // trop d'erreurs passent...
                //return true;
                return Regex.IsMatch(emailaddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }


    }
}
