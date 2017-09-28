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
				InitParams(args);
				CLogger.isActive = isLogActive;
				CURLValidator validator = new CURLValidator();
				validator.StartFrom(startURL);
				CreateReports(validator);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return EXIT_SUCCESS;
			}
			return EXIT_FAILED;
		}
		static void InitParams(string[] args)
		{
			if (args.Length < ARGS_COUNT)
			{
				throw new ArgumentException("Invalid arguments count.\n" +
					"Use: LinksChecker.exe <first link>.");
			}

			startURL = args[0];
			isLogActive = args.Length >= ARGS_COUNT + 1 && args[1] == LOG_ARG;
		}
		static void CreateReports(CURLValidator validator)
		{
			CLogger.Write("Create reports.");
			StreamWriter allURLsWritter = new StreamWriter(ALL_URLS_FILE);
			StreamWriter badURLsWritter = new StreamWriter(BAD_URLS_FILE);

			foreach (KeyValuePair<Uri, int> record in validator.passedURLs)
			{
				string fileRecord = record.Key + " " + record.Value;

				if (record.Value != CURLValidator.GOOD_RESPONCE_CODE)
				{
					badURLsWritter.WriteLine(fileRecord);
				}
				allURLsWritter.WriteLine(fileRecord);
			}

			allURLsWritter.WriteLine(validator.commonReport);
			badURLsWritter.WriteLine(validator.badReport);
			allURLsWritter.Close();
			badURLsWritter.Close();
		}

		private const int ARGS_COUNT = 1;
		private const int EXIT_SUCCESS = 0;
		private const int EXIT_FAILED = 1;
		private const string LOG_ARG = "-log";
		private const string ALL_URLS_FILE = "URLsList.txt";
		private const string BAD_URLS_FILE = "BrokenURLsList.txt";

		private static string startURL { get; set; } = string.Empty;
		private static bool isLogActive { get; set; } = false;
	}
}
