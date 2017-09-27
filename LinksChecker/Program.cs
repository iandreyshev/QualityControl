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
				CURLValidator validator = new CURLValidator();
				validator.StartFrom(args[0]);
				CreateReports(validator);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return EXIT_SUCCESS;
			}
			Console.WriteLine("Done.");
			return EXIT_FAILED;
		}
		static void InitParams(string[] args)
		{
			if (args.Length < MIN_ARGS_COUNT)
			{
				throw new ArgumentException("Invalid arguments count.\n" +
					"Use: LinksChecker.exe <first link>.");
			}

			if (args.Length >= MIN_ARGS_COUNT + 1 && args[1] == ON_LOG_ARG)
			{
				CLogger.isActive = true;
			}
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

		private const int MIN_ARGS_COUNT = 1;
		private const int EXIT_SUCCESS = 0;
		private const int EXIT_FAILED = 1;
		private const string ON_LOG_ARG = "-log";
		private const string ALL_URLS_FILE = "URLsList.txt";
		private const string BAD_URLS_FILE = "BrokenURLsList.txt";
	}
}
