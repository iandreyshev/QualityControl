using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
