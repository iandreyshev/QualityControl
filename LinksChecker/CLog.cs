using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLValidator
{
	static class CLogger
	{
		public static void Write(string message)
		{
			Console.WriteLine("> " + message);
		}
	}
}
