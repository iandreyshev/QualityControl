using System;
using System.Collections.Generic;
using System.IO;

namespace URLValidator
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

				CURLValidator validator = new CURLValidator();
				validator.StartFrom(args[0]);
				CreateReports(validator);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return EXIT_SUCCESS;
			}

			return EXIT_FAILED;
		}
		static void CreateReports(CURLValidator validator)
		{
			StreamWriter allURLsWritter = new StreamWriter(ALL_URLS_FILE);
			StreamWriter badURLsWritter = new StreamWriter(BAD_URLS_FILE);

			foreach (KeyValuePair<Uri, int> record in validator.passedLinks)
			{
				string fileRecord = record.Key + " " + record.Value;

				if (record.Value != CURLValidator.GOOD_RESPONCE_CODE)
				{
					badURLsWritter.WriteLine(fileRecord);
				}

				allURLsWritter.WriteLine(fileRecord);
			}

			allURLsWritter.WriteLine(validator.endTime);
			badURLsWritter.WriteLine(validator.endTime);
		}

		private const int ARGS_COUNT = 0;
		private const int EXIT_SUCCESS = 0;
		private const int EXIT_FAILED = 1;
		private const string ALL_URLS_FILE = "URLsList.txt";
		private const string BAD_URLS_FILE = "BadURLsList.txt";
	}
}
