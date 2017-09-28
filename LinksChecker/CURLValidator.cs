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
			CLogger.Write("Validate done.");
		}
		public void Reset()
		{
			passedURLs = new Dictionary<Uri, int>();
			m_queue = new Queue<Uri>();
			m_ignoredURLs = new HashSet<Uri>();
			m_ignoredNames = new HashSet<string>();
			m_endTime = DateTime.MinValue;
			m_brokenCount = 0;
			m_invalidSchemes = new HashSet<string>();
			foreach (string scheme in IGNORED_SCHEMES)
				m_invalidSchemes.Add(scheme);
		}

		private readonly string[] IGNORED_SCHEMES = { "mailto", "file", "tel" };

		private Uri queueTop { get { return m_queue.Dequeue(); } }
		private bool isQueueEmpty { get { return queueSize == 0; } }
		private uint queueSize { get { return (uint)m_queue.Count; } }
		private string timeReport { get { return "End time: " + m_endTime + "."; } }

		private Queue<Uri> m_queue;
		private HashSet<Uri> m_ignoredURLs;
		private HashSet<string> m_ignoredNames;
		private HashSet<string> m_invalidSchemes;
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
				passedURLs[currentURL] = responseCode;
				CLogger.Write("Response code: " + responseCode);

				if (IsResponseCodeValid(responseCode) &&
					IsURLFromStartDomain(currentURL))
				{
					List<string> pageURLs = GetURLsFromHTML(HTML);
					CLogger.Write("Take " + pageURLs.Count + " URLs names");
					PushURLNames(currentURL, pageURLs);
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
			}
			catch (WebException e)
			{
				response = e.Response as HttpWebResponse;
				return (response == null) ? NOT_FOUND : (int)response.StatusCode;
			}

			Stream stream = response.GetResponseStream();
			HTML = (new StreamReader(stream)).ReadToEnd();

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

		private void PushURLNames(Uri parent, List<string> URLNamesCollection)
		{
			foreach (var URLName in URLNamesCollection)
			{
				if (IsIgnored(URLName))
				{
					continue;
				}

				Uri result;

				if (!ParseToURL(URLName, parent, out result))
				{
					CLogger.ErrWrite("Can not parse URL name: " + URLName);
					m_ignoredNames.Add(URLName);
				}
				else if (IsIgnored(result))
				{
					CLogger.Write("Ignore URL: " + result.ToString());
					m_ignoredURLs.Add(result);
				}
				else if (!IsPassed(result))
				{
					CLogger.Write("Push URL: " + result);
					passedURLs.Add(result, 0);
					m_queue.Enqueue(result);
				}
			};
		}
		private bool ParseToURL(string URLName, Uri parent, out Uri result)
		{
			CLogger.Write("Parse URL name: " + URLName);
			result = null;

			try
			{
				if (Uri.IsWellFormedUriString(URLName, UriKind.Absolute))
				{
					result = new Uri(URLName, UriKind.Absolute);
				}
				else if (Uri.IsWellFormedUriString(URLName, UriKind.Relative))
				{
					Uri relativeURL = new Uri(URLName, UriKind.Relative);
					Uri newURL = new Uri(parent, relativeURL);
					result = newURL;
				}
			}
			catch (Exception)
			{
				return false;
			}

			return result != null;
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
				IsInvalidScheme(URL);
		}
		private bool IsIgnored(string URLName)
		{
			return m_ignoredNames.Contains(URLName);
		}
		private bool IsInvalidScheme(Uri URL)
		{
			return m_invalidSchemes.Contains(URL.Scheme.ToLower());
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

	delegate void ListDelegate<T>(ref List<T> list, T element);
}
