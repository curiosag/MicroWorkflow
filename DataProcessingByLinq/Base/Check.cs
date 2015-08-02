using System;

namespace DataProcessingByLinq.Base
{
	public static class Check
	{

		public static void True (bool value)
		{
		}

		public static void True (bool value, string msg)
		{
		}

		public static void True (bool value, string msg, params object[] args)
		{
		}

		public static void False (bool value)
		{
		}

		public static void False (bool value, string msg)
		{
		}

		public static void False (bool value, string msg, params object[] args)
		{
		}

		public static void Assigned (object value)
		{
		}

		public static void Assigned (params object[] values)
		{
		}

		public static void AssignedOne (params object[] values)
		{
		}

		public static void Assigned (object value, string msg)
		{
		}

		public static void Assigned (object value, string msg, params object[] args)
		{
		}

		public static void NotNullOrEmpty (String value)
		{
		}

				
		public static void Between (int value, int lowerBound, int upperBound)
		{
			Between (value, lowerBound, upperBound, null, null);
		}

		public static void Between (int value, int lowerBound, int upperBound, string msg)
		{
		}

		public static void Between (int value, int lowerBound, int upperBound, string msg,
		                            params object[] args)
		{
		}

		public static void Between<T> (T value, T lowerBound, T upperBound)
				where T : IComparable
		{
			Between (value, lowerBound, upperBound, null, null);
		}

		public static void Between<T> (T value, T lowerBound, T upperBound, string msg)
				where T : IComparable
		{
		}

		public static void Between<T> (T value, T lowerBound, T upperBound, string msg,
		                               params object[] args) where T : IComparable
		{
		}



		public static void BetweenOrEqual (int value, int lowerBound, int upperBound)
		{
		}

		public static void BetweenOrEqual (int value, int lowerBound, int upperBound, string msg)
		{
		}

		public static void BetweenOrEqual (int value, int lowerBound, int upperBound, string msg,
		                                   params object[] args)
		{
		}


		public static void BetweenOrEqual<T> (T value, T lowerBound, T upperBound)
				where T : IComparable
		{
		}


		public static void BetweenOrEqual<T> (T value, T lowerBound, T upperBound, string msg)
				where T : IComparable
		{
		}

		public static void BetweenOrEqual<T> (T value, T lowerBound, T upperBound, string msg,
		                                      params object[] args) where T : IComparable
		{
			
		}

		public static void GreaterOrEqual(int value, int compValue, string msg)
		{
		}
	
	}
}
