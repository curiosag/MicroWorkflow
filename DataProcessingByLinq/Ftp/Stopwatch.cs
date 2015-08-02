using System;

namespace DataProcessingByLinq.Ftp
{
	public class Stopwatch
	{
		// there is no stopwatch class in mono

		public Stopwatch ()
		{
		}

		public void Start(){}
		public void Stop(){}
		public void Restart(){}
		public double ElapsedMilliseconds(){
			return 0;
		}
	}
}

