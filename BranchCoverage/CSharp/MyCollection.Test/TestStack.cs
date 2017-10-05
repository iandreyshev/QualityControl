using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SysCol = System.Collections.Generic;

namespace MyCollection.Test.Stack
{
	[TestClass]
	public class Constructor
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
		public void create_stack_with_start_size_0()
		{
			ushort size = 0;

			Stack<string> stack = new Stack<string>(size);
			Assert.AreEqual(stack.size, size);
		}
		[TestMethod]
		public void create_stack_with_max_start_size()
		{
			ushort size = ushort.MaxValue;

			Stack<string> stack = new Stack<string>(size);
			Assert.AreEqual(stack.size, size);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void create_stack_from_null_array()
		{
			CustomClass[] array = null;
			Stack<CustomClass> stack = new Stack<CustomClass>(array);
		}
		[TestMethod]
		public void create_stack_from_empty_array()
		{
			CustomClass[] array = new CustomClass[0];
			Stack<CustomClass> stack = new Stack<CustomClass>(array);
			Assert.AreEqual(stack.size, array.Length);
		}
		[TestMethod]
		public void create_stack_from_array_with_max_elements_count()
		{
			CustomClass[] array = new CustomClass[ushort.MaxValue];
			for (int i = 0; i < array.Length; ++i)
			{
				array[i] = new CustomClass();
			}
			Stack<CustomClass> stack = new Stack<CustomClass>(array);
			Assert.AreEqual(stack.size, array.Length);
		}
	}
	[TestClass]
	public class Push
	{
		[TestMethod]
		public void stack_size_incremented_after_adding_element()
		{
			Stack<string> stack = new Stack<string>();
			string element = "string";
			stack.Push(element);
			Assert.AreEqual(stack.size, 1);
		}
		[TestMethod]
		[ExpectedException(typeof(OutOfMemoryException))]
		public void stack_overflow_after_ushort_max_elements_count()
		{
			Stack<CustomClass> stack = new Stack<CustomClass>();

			try
			{
				for (int i = 0; i < ushort.MaxValue; ++i)
				{
					stack.Push(new CustomClass());
				}
			}
			catch (Exception)
			{
				Assert.Fail();
			}

			stack.Push(new CustomClass());
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void push_null_not_allowed()
		{
			Stack<CustomClass> stack = new Stack<CustomClass>();
			stack.Push(null);
		}
	}
	[TestClass]
	public class Pop
	{
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void throw_exception_if_pop_from_empty_stack()
		{
			Stack<CustomClass> stack = new Stack<CustomClass>();
			stack.Pop();
		}
	}
	[TestClass]
	public class Top
	{
		[TestMethod]
		public void stack_top_dont_spoils_element()
		{
			Stack<string> stack = new Stack<string>();
			string element = "string";
			stack.Push(element);
			Assert.AreEqual(element, stack.Top());
		}
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void get_top_from_empty_stack_throw_exception()
		{
			Stack<string> stack = new Stack<string>();
			stack.Top();
		}
	}
	[TestClass]
	public class Dequeue
	{
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
		public void dequeue_from_empty_stack_throw_exception()
		{
			Stack<string> stack = new Stack<string>();
			stack.Dequeue();
		}
	}
	[TestClass]
	public class Clear
	{
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
	}
	[TestClass]
	public class Reverse
	{
		[TestMethod]
		public void reverse_empty_stack()
		{
			Stack<string> stack = new Stack<string>();
			stack.Reverse();
		}
		[TestMethod]
		public void reverse_stack_with_three_elements()
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
	[TestClass]
	public class Contains
	{
		[TestMethod]
		public void contains_method_return_false_if_stack_empty()
		{
			Stack<string> stack = new Stack<string>();
			bool isCcontainStrByValue = stack.Contains("string");
			bool isContainStrByPredicate = stack.Contains((string e) => { return true; });
			Assert.IsFalse(isCcontainStrByValue);
			Assert.IsFalse(isContainStrByPredicate);
		}
		[TestMethod]
		public void contains_method_return_true_if_stack_contain_element()
		{
			Stack<string> stack = new Stack<string>();
			string notValidElement1 = "1";
			string notValidELement2 = "2";
			string validElement = "string";

			stack.Push(notValidElement1);
			stack.Push(validElement);
			stack.Push(notValidELement2);

			bool isCcontainStrByValue = stack.Contains(validElement);
			bool isContainStrByPredicate = stack.Contains(
				(string e) => { return e == validElement; });

			Assert.IsTrue(isCcontainStrByValue);
			Assert.IsTrue(isContainStrByPredicate);
		}
	}
	[TestClass]
	public class Convert
	{
		[TestMethod]
		public void empty_stack_to_array()
		{
			Stack<CustomClass> stack = new Stack<CustomClass>();
			CustomClass[] arrFromStack = stack.ToArray();

			Assert.AreEqual(arrFromStack.Length, 0);
		}
		[TestMethod]
		public void not_empty_stack_to_array()
		{
			string[] notEmptyArray = { "0", "1", "2" };
			Stack<string> stack = new Stack<string>();
			stack.Push(notEmptyArray[2]);
			stack.Push(notEmptyArray[1]);
			stack.Push(notEmptyArray[0]);
			string[] arrFromStack = stack.ToArray();

			Assert.IsTrue(Utils.Compare(arrFromStack, notEmptyArray));
		}
		[TestMethod]
		public void empty_stack_to_list()
		{
			Stack<CustomClass> stack = new Stack<CustomClass>();
			SysCol.List<CustomClass> listFromStack = stack.ToList();

			Assert.AreEqual(listFromStack.Count, 0);
		}
		[TestMethod]
		public void not_empty_stack_to_list()
		{
			SysCol.List<string> notEmptyList = new SysCol.List<string>();
			notEmptyList.Add("0");
			notEmptyList.Add("1");
			notEmptyList.Add("2");

			Stack<string> stack = new Stack<string>();
			stack.Push(notEmptyList[2]);
			stack.Push(notEmptyList[1]);
			stack.Push(notEmptyList[0]);
			SysCol.List<string> listFromStack = stack.ToList();

			Assert.IsTrue(Utils.Compare(notEmptyList, listFromStack));
		}
	}
	[TestClass]
	public class ForeachOperator
	{
		[TestMethod]
		public void foeeach_operator_on_empty_stack()
		{
			Stack<string> stack = new Stack<string>();

			foreach(string str in stack)
			{
				Assert.Fail();
			}
		}
		[TestMethod]
		public void foeeach_operator_on_stack_with_elements()
		{
			string[] initArray = { "t", "s", "e", "T" };
			Stack<string> stack = new Stack<string>(initArray);
			string result = "";

			foreach (string element in stack)
			{
				result += element;
			}

			Assert.AreEqual("Test", result);
		}
	}
	[TestClass]
	public class CopyTo
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void copy_to_null_throw_exception()
		{
			Stack<CustomClass> stack = new Stack<CustomClass>();
			CustomClass[] array = null;
			stack.CopyTo(array, 0);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void copy_to_array_with_index_less_than_zero_throw_exception()
		{
			Stack<CustomClass> stack = new Stack<CustomClass>();
			CustomClass[] array = new CustomClass[0];
			stack.CopyTo(array, -1);
		}
		[TestMethod]
		public void copy_to_array_with_elements_from_first_element()
		{
			string[] result = { "T", "e", "s", "t" };
			string[] initArr = { "t", "s", "e", "T" };
			string[] copyToArr = { "", "", "", "" };

			Stack<string> stack = new Stack<string>(initArr);
			stack.CopyTo(copyToArr, 0);
			Assert.IsTrue(Utils.Compare(result, copyToArr));
		}
		[TestMethod]
		public void copy_to_array_with_elements_if_start_index_greather_than_capacity()
		{
			string[] initArr = { "t", "s", "e", "T" };
			string[] result = { };
			string[] copyToArr = { };

			Stack<string> stack = new Stack<string>(initArr);
			stack.CopyTo(copyToArr, ushort.MaxValue);
			Assert.IsTrue(Utils.Compare(result, copyToArr));
		}
	}

	class CustomClass
	{
	}

	class Utils
	{
		public static bool Compare<T>(SysCol.List<T> first, SysCol.List<T> second)
			where T : IComparable
		{
			return Compare<T>(first.ToArray(), second.ToArray());
		}
		public static bool Compare<T>(T[] first, T[] second) where T : IComparable
		{
			bool isEqual = true;

			if (first == null || second == null)
			{
				isEqual = (first == second);
			}
			else if (first.Length != second.Length)
			{
				isEqual = false;
			}
			else
			{
				for (int i = 0; i < first.Length; ++i)
				{
					if (first[i].CompareTo(second[i]) != 0)
					{
						isEqual = false;
						break;
					}
				}
			}

			return isEqual;
		}
	}
}
