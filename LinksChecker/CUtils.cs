using System;
using System.Net;

namespace Utils
{
	public class Pair<TKey, UValue>
	{
		public static Pair<TKey, UValue> Create(TKey first, UValue second)
		{
			Pair<TKey, UValue> newPair = new Pair<TKey, UValue>();
			newPair.first = first;
			newPair.second = second;
			return newPair;
		}

		public TKey first { get; set; }
		public UValue second { get; set; }
	}
	public sealed class StrPair : Pair<string, string>
	{
		public StrPair(string first, string second)
		{
			this.first = first;
			this.second = second;
		}
	}

	static class CUtils
	{
		public static bool IsLinkAbsolute(string link)
		{
			link = ClearArgument(link);
			if (Uri.CheckSchemeName(link) &&
				Uri.CheckHostName(link) != UriHostNameType.Unknown)
			{
				return true;
			}

			return false;
		}
		public static string ClearArgument(string argument)
		{
			if (argument == null)
			{
				throw new ArgumentNullException("Clear null argument.");
			}

			int start = 0;
			int end = argument.Length - 1;

			for (; start <= end && argument[start] == ' '; ++start) ;
			for (; end > start && argument[end] == ' '; --end) ;

			return argument.Substring(start, end - start + 1);
		}
	}
}
