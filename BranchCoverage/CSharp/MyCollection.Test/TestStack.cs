using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyCollection.Test
{
	[TestClass]
	public class TestStack
	{
		[TestMethod]
		public void create_str_stack()
		{
			Stack<string> strStack = new Stack<string>();
			Assert.IsNotNull(strStack);
		}
		[TestMethod]
		public void new_stack_is_empty()
		{
			Stack<string> stack = new Stack<string>();
			Assert.AreEqual(stack.size, 0);
		}
		[TestMethod]
		public void stack_size_incremented_after_adding_element()
		{
			Stack<string> stack = new Stack<string>();
			string element = "string";
			stack.Push(element);
			Assert.AreEqual(stack.size, 1);
		}
		[TestMethod]
		public void stack_top_dont_spoils_element()
		{
			Stack<string> stack = new Stack<string>();
			string element = "string";
			stack.Push(element);
			Assert.AreEqual(element, stack.Top());
		}
		[TestMethod]
		public void stack_dequeue_dont_spoils_element()
		{
			Stack<string> stack = new Stack<string>();
			string element = "string";
			stack.Push(element);
			Assert.AreEqual(element, stack.Dequeue());
		}
		[TestMethod]
		public void dequeue_method_return_element_and_pop_his_from_stack()
		{
			Stack<string> stack = new Stack<string>();
			string firstElement = "string1";
			string secondElement = "string2";
			stack.Push(firstElement);
			stack.Push(secondElement);
			Assert.AreEqual(secondElement, stack.Dequeue());
			Assert.AreEqual(stack.size, 1);
			Assert.AreEqual(firstElement, stack.Top());
		}
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void get_top_from_empty_stack_throw_exception()
		{
			Stack<string> stack = new Stack<string>();
			stack.Top();
		}
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void dequeue_from_empty_stack_throw_exception()
		{
			Stack<string> stack = new Stack<string>();
			stack.Dequeue();
		}
		[TestMethod]
		public void clear_empty_stack()
		{
			Stack<string> stack = new Stack<string>();
			stack.Clear();
			Assert.AreEqual(stack.size, 0);
		}
		[TestMethod]
		public void clear_stack_with_elements()
		{
			Stack<string> stack = new Stack<string>();
			stack.Push("");
			stack.Push("");
			stack.Push("");
			Assert.AreEqual(stack.size, 3);
			stack.Clear();
			Assert.AreEqual(stack.size, 0);
		}
		[TestMethod]
		public void reverse_empty_stack()
		{
			Stack<string> stack = new Stack<string>();
			stack.Reverse();
		}
		[TestMethod]
		public void reverse_stack_with_elements()
		{
			Stack<string> stack = new Stack<string>();
			string[] arr = { "0", "1", "2" };
			stack.Push(arr[0]);
			stack.Push(arr[1]);
			stack.Push(arr[2]);
			stack.Reverse();
			Assert.AreEqual(stack.Dequeue(), arr[0]);
			Assert.AreEqual(stack.Dequeue(), arr[1]);
			Assert.AreEqual(stack.Dequeue(), arr[2]);
			Assert.IsTrue(stack.isEmpty);
		}
	}
}
