using System;

namespace DataProcessingByLinq.Base
{
	public static class ArgumentCheck
	{
		private const StringComparison cDefaultComparison = StringComparison.Ordinal;


		public static void NotEquals<T>(T value, T notEqualValue, string parameterName)
		{
		}


		public static void NotEquals(string value, string notEqualValue, string parameterName)
		{
		}


		public static void NotEquals(string value, string notEqualValue, string parameterName, StringComparison comparison)
		{
		}


		public static void InRange<T>(T value, T minimum, T maximum, string parameterName) where T : IComparable
		{
		}

		public static void NotNullOrEmpty(string value, string parameterName)
		{
		}

		public static void Assigned(object value, string parameterName)
		{
		}

		public static void Is<T>(object value, string parameterName)
		{
		}

		public static void Is(Type expectedType, object value, string parameterName)
		{
		}

		public static void GreaterOrEqual<T>(T value, T compValue, string parameterName)
			where T : IComparable
		{
		}

	
		public static void Greater<T>(T value, T compValue, string parameterName)
			where T : IComparable
		{
		}
	}
}

