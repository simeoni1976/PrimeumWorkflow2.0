using System;
using System.Linq;
using System.Reflection;

namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// MethodExtension class.
    ///</summary>
    ///<remarks>
    /// This class permit to contain extension functions for specifics types.
    /// </remarks>
    public static class MethodExtension
	{
		/// <summary>
		/// This function permits to convert a string to a datetime format.
		/// </summary>
		/// <param name="s">Value</param>
		/// <returns>DatteTime</returns>
		public static DateTime? ToDateTime(this string s)
		{
			DateTime dtr;
			var tryDtr = DateTime.TryParse(s, out dtr);
			return (tryDtr) ? dtr : new DateTime?();
		}

		/// <summary>
		/// This function permit to map a model object to a querystring
		/// </summary>
		/// <param name="obj">Object</param>
		/// <returns>String</returns>
		public static string AsQueryString(this object obj)
		{
			var properties = from p in obj.GetType().GetProperties()
							 where p.GetValue(obj, null) != null
							 select p.Name + "=" + p.GetValue(obj, null).ToString();

			return String.Join("&", properties.ToArray());
		}
	}
}