using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCollections
{
	public partial class Stack<T> where T : class
	{
		public Stack()
		{
			m_top = new Node(null, null);
		}

		public bool isEmpty { get { return size == 0; } }
		public ushort size { get { return m_size; } }

		public T Top()
		{
			if (isEmpty)
			{
				throw new ArgumentException("Try get top from empty stack");
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
		public void Push(T value)
		{
			if (m_size == ushort.MaxValue)
			{
				throw new OutOfMemoryException("Stack overflow");
			}

			++m_size;
			m_top = new Node(value, m_top);
		}

		public void Clear()
		{
			if (isEmpty)
			{
				return;
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

			public T value { get; private set; }
			public Node prev { get; private set; }
		}
	}
}
