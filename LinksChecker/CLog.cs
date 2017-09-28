using System;
using System.IO;

namespace URLValidator
{
	static class CLogger
	{
		public static bool isActive { get; set; } = false;

		public static void Write(string message)
		{
			if (isActive)
			{
				Console.WriteLine("> " + message);
			}
		}
		public static void ErrWrite(string message)
		{
			if (isActive)
			{
				StreamWriter writter = new StreamWriter("err.txt", true);
				writter.WriteLine(message);
				writter.Close();
			}
		}
		private static void BreakLine()
		{
			if (isActive)
			{
				Console.WriteLine(string.Empty);
			}
		}
	}
}
