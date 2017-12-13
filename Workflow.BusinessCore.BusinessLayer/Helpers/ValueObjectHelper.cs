using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Helpers
{
    /// <summary>
    /// Permet une représentation arborescence de ValueObject.
    /// </summary>
    public class TreeValueObject
    {
        public TreeValueObject Parent { get; set; }
        public List<TreeValueObject> Children { get; set; }
        public ValueObject Node { get; set; }
        public string TreeValue { get; set; }
        public int Level { get; set; }

        public TreeValueObject()
        {
            Parent = null;
            Children = new List<TreeValueObject>();
            Node = null;
            TreeValue = null;
        }

        public static IEnumerable<TreeValueObject> GetNodesFromLevel(TreeValueObject root, int level)
        {
            List<TreeValueObject> res = new List<TreeValueObject>();

            if (root.Level == level)
                res.Add(root);
            if (root.Level < level)
                foreach (TreeValueObject child in root.Children)
                    res.AddRange(GetNodesFromLevel(child, level));

            return res;
        }
    }


    public static class ValueObjectHelper
    {
        /// <summary>
        /// Champ privé afin d'optimiser les performances. Initialisé lors de la création du processus.
        /// </summary>
        private static Dictionary<Tuple<double, int>, double> _dicFormatToPower = new Dictionary<Tuple<double, int>, double>();


        /// <summary>
        /// Donne la précision multiplicative d'un format numérique.
        /// </summary>
        /// <param name="format">Format numérique</param>
        /// <returns>Précision "multiplicative"</returns>
        public static double GetPrecisionAbs(string format)
        {
            double coef = StringHelper.GetCoefficient(format);
            int prec = StringHelper.GetPrecision(format);

            if (double.IsNaN(coef))
                throw new ConfigurationException($"GetPrecisionAbs : numerical format is wrong ({format})!");
            if (coef == 0)
                throw new ConfigurationException($"GetPrecisionAbs : numerical format is wrong ({format}), coefficient can't be zero!");

            return GetPrecisionAbs(coef, prec);
        }

        /// <summary>
        /// Donne la précision "multiplicative" selon un coefficient et une precision simple données.
        /// </summary>
        /// <param name="coefficient">Coefficient multiplicateur</param>
        /// <param name="precisionSimple">Précision simple, issu d'un format numérique</param>
        /// <returns>Précision "multiplicative"</returns>
        public static double GetPrecisionAbs(double coefficient, int precisionSimple)
        {
            if (double.IsNaN(coefficient))
                throw new ConfigurationException($"GetPrecisionAbs : invalid coefficient ({coefficient})!");
            if (coefficient == 0)
                throw new ConfigurationException($"GetPrecisionAbs : coefficient can't be zero!");

            return Math.Pow(10, -1 * precisionSimple) / coefficient;
        }

        /// <summary>
        /// Vérifie l'égalité de 2 doubles, sur une précision de 10E-x. (si la différence est inférieure à 10E-x, ils sont vus comme égaux, sinon ils sont vus comme inégaux).
        /// La précision 10E-x dépend du format numérique donné en paramètre.
        /// </summary>
        /// <param name="a">Premier double</param>
        /// <param name="b">Second double</param>
        /// <param name="format">format numérique</param>
        /// <returns>True : vus comme égaux à 10E-x, False : vus comme inégaux</returns>
        /// <remarks>Méthode issue du TargetManager</remarks>
        public static bool AlmostEqual(double a, double b, string format)
        {
            double coef = StringHelper.GetCoefficient(format);
            int prec = StringHelper.GetPrecision(format);

            double power = 0;
            if (_dicFormatToPower.ContainsKey(new Tuple<double, int>(coef, prec)))
                power = _dicFormatToPower[new Tuple<double, int>(coef, prec)];
            else
            {
                power = GetPrecisionAbs(coef, prec);
                _dicFormatToPower.Add(new Tuple<double, int>(coef, prec), power);
            }

            return Math.Abs(a - b) <= power; // Math.Pow(10, -1 * ALMOST_EQUALS_ACCURACY);
        }

    }

}
