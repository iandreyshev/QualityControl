using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCollection.Test
{
	class Program
	{
		public static void Main()
		{
			Stack<string> stack = new Stack<string>();
			string[] arr = { "0", "1", "2" };
			stack.Push(arr[0]);
			stack.Push(arr[1]);
			stack.Push(arr[2]);
			stack.Reverse();
		}
	}
}
