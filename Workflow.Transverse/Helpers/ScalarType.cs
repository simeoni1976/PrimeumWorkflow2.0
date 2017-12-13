using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// ScalarType class.
    /// </summary>
    /// <remarks>
    /// This class permits to define a type.
    /// </remarks>
    [NotMapped]
    public class ScalarType : IEquatable<ScalarType>
    {
        private string _value;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ScalarType() { }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ScalarType(object value)
        {
            _value = value.ToString();
        }

        /// <summary>
        /// Value property.
        /// </summary>
        /// <value>
        /// Gets or sets the Value value.
        /// </value>
        public string Value
        {
            get { return _value; }
            set { this._value = value; }
        }

        /// <summary>
        /// This method formats the value to a String format.
        /// </summary>
        public override string ToString() { return _value; }

        // User-defined conversion from ScalarType to string
        public static implicit operator string(ScalarType d)
        {
            return d.ToString();
        }
        
        //public static implicit operator ScalarType(double d)
        //{
        //    return ScalarType.Parse(d);
        //}
        
        public static implicit operator float(ScalarType d)
        {
            return float.Parse(d);
        }
        
        public static implicit operator int(ScalarType d)
        {
            return int.Parse(d);
        }

        public static implicit operator DateTime(ScalarType d)
        {
            return DateTime.Parse(d);
        }

        //  User-defined conversion from value to ScalarType
        public static implicit operator ScalarType(string d)
        {
            return new ScalarType(d);
        }

        public static implicit operator ScalarType(double d)
        {
            return new ScalarType(d);
        }

        public static implicit operator ScalarType(float d)
        {
            return new ScalarType(d);
        }

        public static implicit operator ScalarType(int d)
        {
            return new ScalarType(d);
        }

        public static implicit operator ScalarType(DateTime d)
        {
            return new ScalarType(d);
        }

        /// <summary>
        /// Equals function.
        /// </summary>
        public bool Equals(ScalarType other)
        {
            return other._value.ToString().Equals(this.ToString());
        }
    }
}
