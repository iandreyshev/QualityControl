using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Utils;

namespace LinksCheckerApp
{
	sealed class CLinksChecker
	{
		public CLinksChecker()
		{
			Reset();
		}

		public Dictionary<string, int> passedLinks { get; private set; }
		public DateTime endTime { get; private set; } = DateTime.MinValue;

		public void Start(string startPage)
		{
			Reset();
			InitStartPage(startPage);
			ProcessQueue();
			endTime = DateTime.Now;
		}
		public void Reset()
		{
			passedLinks = new Dictionary<string, int>();
			m_queue = new Queue<StrPair>();
		}

		private Queue<StrPair> m_queue;
		private Uri m_startURL;

		private void InitStartPage(string link)
		{
			try
			{
				m_startURL = new Uri(link, UriKind.Absolute);
			}
			catch (UriFormatException)
			{
				throw new ArgumentException("Invalid format of start page link.");
			}
			catch (ArgumentNullException)
			{
				throw new ArgumentException("Start page link is null.");
			}
		}
		private void ProcessQueue()
		{
			string pageHTML;
			m_queue.Enqueue(new StrPair("", m_startURL.ToString()));

			while (m_queue.Count != 0)
			{	
				StrPair queueTop = m_queue.Dequeue();
				Uri newURL = CreateURLFromQueueElement(queueTop);

				if (passedLinks.ContainsKey(newURL.ToString()))
				{
					continue;
				}

				int URLStatus = GetHTMLByLink(newURL, out pageHTML);

				if (IsGoodStatus(URLStatus) && IsCurrentDomain(newURL))
				{
					GetURLsFromHTML(pageHTML).ForEach((link) =>
					{
						m_queue.Enqueue(new StrPair(newURL.ToString(), link));
					});
				}

				passedLinks.Add(newURL.ToString(), URLStatus);
			}
		}
		private int GetHTMLByLink(Uri link, out string html)
		{
			HttpWebResponse responce = null;
			html = "";

			try
			{
				HttpWebRequest request = WebRequest.Create(link) as HttpWebRequest;
				responce = request.GetResponse() as HttpWebResponse;
			}
			catch (WebException e)
			{
				return (int)(e.Response as HttpWebResponse).StatusCode;
			}

			Stream stream = responce.GetResponseStream();
			html = (new StreamReader(stream)).ReadToEnd();

			return (int)responce.StatusCode;
		}
		private List<string> GetURLsFromHTML(string pageHTML)
		{
			List<string> links = new List<string>();
			return links;
		}
		private Uri CreateURLFromQueueElement(StrPair pair)
		{
			return new Uri(pair.second);
		}

		private bool IsGoodStatus(int responceStatus)
		{
			return true;
		}
		private bool IsCurrentDomain(Uri link)
		{
			if (!link.IsAbsoluteUri)
			{
				throw new ArgumentException("CLinksChecker.IsCurrentDomain: " +
					"Check the address can only be absolute");
			}

			return link.Host == m_startURL.Host;
		}
	}

	enum BadLink
	{
		NOT_FOUND = 404,
		FORBIDDEN = 403,
	}
}
