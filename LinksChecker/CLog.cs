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
		public static void QueueSize(uint size)
		{
			Write("Current queue size: " + size);
		}
		public static void TakeURLFromQueue(object url)
		{
			Write("Take from queue URL: " + url);
		}
		public static void BoolMessage(string message, bool isTrue)
		{
			Write(message + ": " + isTrue);
		}
		public static void ResponceCode(int code)
		{
			Write("Responce code: " + code);
		}
		public static void AddToQueueCount(int count)
		{
			Write("Add to queue " + count + "URLs");
		}
	}
}
