using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace URLValidator
{
	sealed class CURLValidator
	{
		public const int GOOD_RESPONCE_CODE = 200;
		public const int NOT_FOUND = 404;

		public Dictionary<Uri, int> passedURLs { get; private set; }
		public string commonReport
		{
			get
			{
				if (passedURLs.Count == 0)
				{
					return "Validator has not been used.";
				}
				return "Checked " + passedURLs.Count + " URLs. " + timeReport;
			}
		}
		public string badReport
		{
			get
			{
				if (passedURLs.Count == 0)
				{
					return "Validator has not been used.";
				}
				else if (m_brokenCount == 0)
				{
					return "Broken URLs not found:) " + timeReport;
				}
				return "Found " + m_brokenCount + " broken URLs. " + timeReport;
			}
		}

		public void StartFrom(string startURL)
		{
			Reset();
			SetStartPage(startURL);
			EnterProcess();
			CalcResults();
		}
		public void Reset()
		{
			passedURLs = new Dictionary<Uri, int>();
			m_queue = new Queue<Uri>();
			m_endTime = DateTime.MinValue;
			m_brokenCount = 0;
		}

		private Uri queueTop { get { return m_queue.Dequeue(); } }
		private bool isQueueEmpty { get { return queueSize == 0; } }
		private uint queueSize { get { return (uint)m_queue.Count; } }
		private string timeReport { get { return "End time: " + m_endTime + "."; } }

		private Queue<Uri> m_queue;
		private Uri m_startURL;
		private DateTime m_endTime;
		private int m_brokenCount;

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
			Push(m_startURL);

			while (!isQueueEmpty)
			{
				CLogger.Write("Queue size: " + queueSize);
				CLogger.Write("Passed list size: " + passedURLs.Count);
				Uri currentURL = queueTop;
				CLogger.Write("Work with URL: " + currentURL);

				string HTML;
				int responseCode = GetHTMLFromURL(currentURL, out HTML);
				CLogger.Write("Response code: " + responseCode);
				passedURLs[currentURL] = responseCode;
				CLogger.Write("Edit code complete");

				if (IsResponseCodeValid(responseCode) &&
					IsURLFromStartDomain(currentURL))
				{
					CLogger.Write("Response code valid");
					List<string> pageURLCollection = GetURLsFromPage(HTML);
					CLogger.Write("Take " + pageURLCollection.Count + " new URLs");
					PushURLsCollection(currentURL, pageURLCollection);
				}
				CLogger.Write(string.Empty);
			}
		}

		private int GetHTMLFromURL(Uri URL, out string HTML)
		{
			HttpWebResponse response = null;
			HTML = string.Empty;

			if (IsMailto(URL))
			{
				return NOT_FOUND;
			}

			try
			{
				HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
				response = request.GetResponse() as HttpWebResponse;
				CLogger.Write("Get response complete");
			}
			catch (WebException e)
			{
				HttpWebResponse badResponse = e.Response as HttpWebResponse;
				return (badResponse == null) ? NOT_FOUND : (int)badResponse.StatusCode;
			}

			Stream stream = response.GetResponseStream();
			CLogger.Write("Create stream complete");

			StreamReader reader = new StreamReader(stream);
			HTML = (reader).ReadToEnd();
			CLogger.Write("Read stream complete");

			return (int)response.StatusCode;
		}
		private List<string> GetURLsFromPage(string html)
		{
			List<string> result = new List<string>();
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(html);
			HtmlNodeCollection aNodes = doc.DocumentNode.SelectNodes("//a[@href]");

			if (aNodes == null)
			{
				return result;
			}

			foreach (HtmlNode node in aNodes)
			{
				result.Add(node.Attributes["href"].Value);
			}
	
			return result;
		}

		private void PushURLsCollection(Uri parent, List<string> URLCollection)
		{
			foreach (var URLName in URLCollection)
			{
				CLogger.Write("Try initialize as URL: " + URLName);
				Uri resultURL = null;

				if (Uri.IsWellFormedUriString(URLName, UriKind.Absolute))
				{
					CLogger.Write("Absolute");
					resultURL = new Uri(URLName);
				}
				else if (Uri.IsWellFormedUriString(URLName, UriKind.Relative))
				{
					CLogger.Write("Relative");
					Uri relativeURL = new Uri(URLName);
					resultURL = new Uri(m_startURL, relativeURL);
				}
				else
				{
					CLogger.Write("Undefined URL: " + URLName);
				}

				Push(resultURL);
			};
		}
		private void Push(Uri newURL)
		{
			if (newURL == null || passedURLs.ContainsKey(newURL))
			{
				return;
			}

			CLogger.Write("Push: " + newURL);
			passedURLs.Add(newURL, -1);
			m_queue.Enqueue(newURL);
		}

		private bool IsURLFromStartDomain(Uri URL)
		{
			if (!URL.IsAbsoluteUri)
			{
				throw new ArgumentException(
					"Not abslute argument in IsURLFromStartDomain function");
			}

			return URL.Host == m_startURL.Host;
		}
		private bool IsResponseCodeValid(int pageStatus)
		{
			return pageStatus == GOOD_RESPONCE_CODE;
		}
		private bool IsMailto(Uri URL)
		{
			string[] parts = URL.ToString().Split(':');
			return parts[0].ToLower() == "mailto";
		}

		private void CalcResults()
		{
			m_endTime = DateTime.Now;
			foreach (var URLRecord in passedURLs)
			{
				if (URLRecord.Value != GOOD_RESPONCE_CODE)
				{
					m_brokenCount++;
				}
			}
		}
	}

	delegate void BoolStrDelegate(bool isTrue, string str);
}
