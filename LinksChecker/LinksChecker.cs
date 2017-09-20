using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksCheckerApp
{
	class LinksChecker
	{
		public LinksChecker()
		{
			m_queue = new Queue<string>();
			m_passedLinks = new Dictionary<string, bool>();
		}

		public void Start(string startPage)
		{
			Cleanup();
			Console.WriteLine(DateTime.Now);
			InitStartPage(startPage);

			while (m_queue.Count != 0)
			{

			}

			Console.WriteLine(DateTime.Now);
		}
		public void Cleanup()
		{
			m_queue.Clear();
			m_passedLinks.Clear();
		}

		private Link m_startPage;
		private Queue<string> m_queue;
		private Dictionary<string, bool> m_passedLinks;

		private void InitStartPage(string link)
		{
			m_startPage = ParseLink(link);
		}
		private Link ParseLink(string link)
		{
			return new Link();
		}
	}

	struct Link
	{
	}
}
