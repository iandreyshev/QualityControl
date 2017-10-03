using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyCollections.Test
{
	[TestClass]
	public class TestStack
	{
		[TestMethod]
		public void create_empty_stack()
		{
			Stack<string> stack = new Stack<string>();
			Assert.AreEqual(stack.size, 0);
		}
	}
}
