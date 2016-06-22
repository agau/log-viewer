using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogViewer.Extensions
{
	public static class TypeExtensions
	{
		#region [ string SafeParse ... methods ]

		public static int? SafeParseInt(this object str, int? defVal = null)
		{
			int result;

			return (null != str && int.TryParse(str.ToString(), out result)) ? result : defVal;
		}

		public static DateTime? SafeParseDateTime(this object str, DateTime? defVal = null)
		{
			DateTime result;

			// DateTime.TryParse truncate the precision of DateTime2(7)
			if (DateTime.TryParse(str.ToString(), out result))
				return Convert.ToDateTime(str);
			else
				return defVal;
		}

		public static bool? SafeParseBool(this object str, bool? defVal = null)
		{
			bool result;
			return bool.TryParse(str.ToString(), out result) ? result : defVal;
		}

		public static decimal? SafeParseDecimal(this object str, decimal? defVal = null)
		{
			decimal result;

			return (null != str && decimal.TryParse(str.ToString(), out result)) ? result : defVal;
		}

		public static string SafeToString(this object obj, string defVal = null)
		{
			return obj != null ? obj.ToString() : defVal;
		}

		public static string SafeToString(this DateTime? obj, string format, string defVal = null)
		{
			return obj != null ? obj.ToString() : defVal;
		}
		#endregion
	}
}