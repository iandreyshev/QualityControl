using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace LinksCheckerApp
{
	class Program
	{
		static int Main(string[] args)
		{
			try
			{
				if (args.Length < ARGS_COUNT)
				{
					throw new ArgumentNullException("Invalid arguments count." +
						"Use: LinksChecker.exe <first link>.");
				}

				LinksChecker checker = new LinksChecker();
				checker.Start("vk.com");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return EXIT_SUCCESS;
			}

			return EXIT_FAILED;
		}

		private const int ARGS_COUNT = 1;
		private const int EXIT_SUCCESS = 0;
		private const int EXIT_FAILED = 1;
	}
}
