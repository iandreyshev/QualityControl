using System;
using System.Collections;
using System.Collections.Generic;

namespace MyCollection
{
	public partial class Stack<T> where T : class
	{
		public Stack()
		{
		}
		public Stack(ushort size)
		{
			while (m_size < size)
			{
				Add(null);
			}
		}
		public Stack(T[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("Initialize from null array");
			}

			foreach (T element in array)
			{
				Push(element);
			}
		}

		public bool isEmpty { get { return m_size == 0; } }
		public ushort size { get { return m_size; } }

		public T Top()
		{
			if (isEmpty)
			{
				throw new InvalidOperationException("Try get top from empty stack");
			}

			return m_top.value;
		}
		public void Pop()
		{
			if (isEmpty)
			{
				throw new InvalidOperationException("Try pop from empty stack");
			}

			--m_size;
			m_top = m_top.prev;
		}
		public T Dequeue()
		{
			if (isEmpty)
			{
				throw new InvalidOperationException("Can not dequeue from empty stack");
			}

			T result = Top();
			Pop();
			return result;
		}
		public void Push(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("Push null is not allowed");
			}
			else if (m_size == ushort.MaxValue)
			{
				throw new OutOfMemoryException("Stack overflow");
			}

			Add(value);
		}

		public void Reverse()
		{
			if (isEmpty)
			{
				return;
			}

			Node top = m_top;
			m_top = null;
			m_size = 0;

			while (top != null)
			{
				Push(top.value);
				top = top.prev;
			}
		}
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("Array is null");
			}
			else if (index < 0)
			{
				throw new ArgumentException("Array index con not be less than zero");
			}

			int maxIndex = array.Length - 1;
			Node top = m_top;

			while (index <= maxIndex && top != null)
			{
				array[index] = top.value;
				++index;
				top = top.prev;
			}
		}

		public IEnumerator GetEnumerator()
		{
			Node current = m_top;

			while (current != null)
			{
				yield return current.value;
				current = current.prev;
			}

			yield break;
		}

		public bool Contains(T element)
		{
			Node node = m_top;

			while (node != null)
			{
				if (node.value == element)
				{
					return true;
				}
				node = node.prev;
			}

			return false;
		}
		public bool Contains(Predicate<T> predicate)
		{
			Node node = m_top;

			while (node != null)
			{
				if (predicate(node.value))
				{
					return true;
				}
				node = node.prev;
			}

			return false;
		}

		public T[] ToArray()
		{
			T[] result = new T[m_size];
			Node top = m_top;

			for (int i = 0; i < result.Length; ++i)
			{
				result[i] = top.value;
				top = top.prev;
			}

			return result;
		}
		public List<T> ToList()
		{
			List<T> result = new List<T>();
			Node top = m_top;

			while (top != null)
			{
				result.Add(top.value);
				top = top.prev;
			}

			return result;
		}

		public void Clear()
		{
			if (isEmpty)
			{
				return;
			}

			while (!isEmpty)
			{
				Pop();
			}
		}

		private Node m_top = null;
		private ushort m_size = 0;

		private void Add(T value)
		{
			++m_size;
			m_top = new Node(value, m_top);
		}
	}

	public partial class Stack<T> where T : class
	{
		private class Node
		{
			public Node(T value, Node prev)
			{
				this.value = value;
				this.prev = prev;
			}

			public T value { get; set; }
			public Node prev { get; set; }
		}
	}
}