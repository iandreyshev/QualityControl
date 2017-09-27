using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using HtmlAgilityPack;
//using Freya.Types.Uri;

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
			CLogger.Write("Validate done.");
		}
		public void Reset()
		{
			passedURLs = new Dictionary<Uri, int>();
			m_queue = new Queue<Uri>();
			m_ignoredURLs = new HashSet<Uri>();
			m_endTime = DateTime.MinValue;
			m_brokenCount = 0;
		}

		private Uri queueTop { get { return m_queue.Dequeue(); } }
		private bool isQueueEmpty { get { return queueSize == 0; } }
		private uint queueSize { get { return (uint)m_queue.Count; } }
		private string timeReport { get { return "End time: " + m_endTime + "."; } }

		private Queue<Uri> m_queue;
		private HashSet<Uri> m_ignoredURLs;
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
			passedURLs.Add(m_startURL, 0);
			m_queue.Enqueue(m_startURL);

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
					List<string> pageURLCollection = GetURLsFromHTML(HTML);
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
		private List<string> GetURLsFromHTML(string html)
		{
			ListDelegate<string> add_if = (ref List<string> list, string item) =>
			{
				if (item != string.Empty) list.Add(item);
			};

			List<string> result = new List<string>();
			HtmlDocument doc = new HtmlDocument();
			doc.LoadHtml(html);
			HtmlNodeCollection HTMLNodes = doc.DocumentNode.SelectNodes("//*");
			if (HTMLNodes == null)
			{
				return result;
			}

			foreach (HtmlNode node in HTMLNodes)
			{
				add_if(ref result, GetAttributeValue(node, "href"));
				add_if(ref result, GetAttributeValue(node, "src"));
			}
	
			return result;
		}
		private string GetAttributeValue(HtmlNode node, string attribute)
		{
			if (node == null || node.Attributes[attribute] == null)
			{
				return string.Empty;
			}

			return node.Attributes[attribute].Value;
		}

		private void PushURLsCollection(Uri parent, List<string> URLCollection)
		{
			foreach (var URLName in URLCollection)
			{
				CLogger.Write("Try initialize as URL: " + URLName);
				PrepareURLName(URLName);
				Uri resultURL = null;

				if (Uri.IsWellFormedUriString(URLName, UriKind.Absolute))
				{
					resultURL = new Uri(URLName);
				}
				else if (Uri.IsWellFormedUriString(URLName, UriKind.Relative))
				{
					Uri relativeURL = new Uri(URLName, UriKind.Relative);
					resultURL = new Uri(m_startURL, relativeURL);
				}

				if (resultURL == null) { }
				else if (IsIgnored(resultURL))
				{
					CLogger.Write("Ignore URL: " + URLName);
					m_ignoredURLs.Add(resultURL);
				}
				else if (!IsPassed(resultURL))
				{
					CLogger.Write("Push URL: " + resultURL);
					passedURLs.Add(resultURL, 0);
					m_queue.Enqueue(resultURL);
				}
			};
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
		private bool IsPassed(Uri URL)
		{
			return
				passedURLs.ContainsKey(URL) ||
				m_ignoredURLs.Contains(URL);
		}
		private bool IsIgnored(Uri URL)
		{
			return
				URL == null ||
				m_ignoredURLs.Contains(URL) ||
				IsMailto(URL);
		}
		private bool IsMailto(Uri URL)
		{
			string[] parts = URL.ToString().Split(':');
			return parts[0].ToLower() == "mailto";
		}

		private string PrepareURLName(string URLName)
		{
			return URLName;
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
	delegate void ListDelegate<T>(ref List<T> list, T element);
}
