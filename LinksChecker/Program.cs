using System;
using Utils;

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
					throw new ArgumentException("Invalid arguments count.\n" +
						"Use: LinksChecker.exe <first link>.");
				}

				CLinksChecker checker = new CLinksChecker();
				checker.Start(args[0]);
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
