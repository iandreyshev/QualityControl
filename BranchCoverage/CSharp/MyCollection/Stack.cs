using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCollection
{
	public partial class Stack<T> where T : class
	{
		public Stack()
		{
			m_top = null;
		}

		public bool isEmpty { get { return size == 0; } }
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
			if (m_size == ushort.MaxValue)
			{
				throw new OutOfMemoryException("Stack overflow");
			}

			++m_size;
			m_top = new Node(value, m_top);
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

			for (int i = result.Length - 1; i > 0; --i)
			{
				result[i] = top.value;
				top = top.prev;
			}

			return result;
		}
		public List<T> ToList()
		{
			return new List<T>(ToArray());
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

		private Node m_top;
		private ushort m_size;
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