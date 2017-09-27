using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace URLValidator
{
	using QueueNode = KeyValuePair<Uri, Uri>;

	sealed class CURLValidator
	{
		public const int GOOD_RESPONCE_CODE = 200;

		public Dictionary<Uri, int> passedLinks { get; private set; }
		public DateTime endTime { get; private set; } = DateTime.MinValue;

		public void StartFrom(string startURL)
		{
			Reset();
			SetStartPage(startURL);
			EnterProcess();
			endTime = DateTime.Now;
		}
		public void Reset()
		{
			passedLinks = new Dictionary<Uri, int>();
			m_queue = new Queue<Uri>();
		}

		private Uri queueTop { get { return m_queue.Dequeue(); } }
		private bool isQueueEmpty { get { return queueSize == 0; } }
		private uint queueSize { get { return (uint)m_queue.Count; } }

		private Queue<Uri> m_queue;
		private Uri m_startURL;

		private void SetStartPage(string link)
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
		private void EnterProcess()
		{
			m_queue.Enqueue(m_startURL);

			while (!isQueueEmpty)
			{
				CLogger.QueueSize(queueSize);
				Uri currentUrl = queueTop;
				CLogger.TakeURLFromQueue(currentUrl);

				string html;
				int status = GetHTMLFromURL(currentUrl, out html);

				if (IsStatusValid(status))
				{
					CLogger.Write("Responce code is valid");
					var pageURLs = GetURLsFromPage(html);
					PushURLsToQueue(currentUrl, pageURLs);
				}
			}
		}
		private int GetHTMLFromURL(Uri link, out string html)
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
			CLogger.ResponceCode((int)responce.StatusCode);

			return (int)responce.StatusCode;
		}
		private List<string> GetURLsFromPage(string html)
		{
			List<string> result = new List<string>();
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(html);
			HtmlNodeCollection aNodes = doc.DocumentNode.SelectNodes("//a[@href]");

			foreach (HtmlNode node in aNodes)
			{
				result.Add(node.Attributes["href"].Value);
			}
	
			return result;
		}

		private void PushURLsToQueue(Uri parent, List<string> URLCollection)
		{
			foreach (var newURLName in URLCollection)
			{
				CLogger.Write("Try push to queue");
				Uri newURL = new Uri(newURLName);
				bool isSuccess = false;

				if (newURL.IsAbsoluteUri)
				{
					CLogger.Write("URL is absolute formatted");
					isSuccess = IsAbsoluteURLValid(newURL);
					CLogger.BoolMessage("Is URL form instance host", isSuccess);
				}
				else
				{
				}
			}
		}

		private bool IsAbsoluteURLValid(Uri url)
		{
			if (!url.IsAbsoluteUri)
			{
				throw new ArgumentException("CLinksChecker.IsAbsoluteURLValid: " +
					"Check the url can only be absolute.");
			}

			return url.Host == m_startURL.Host;
		}
		private bool IsStatusValid(int pageStatus)
		{
			return pageStatus == GOOD_RESPONCE_CODE;
		}
	}
}
