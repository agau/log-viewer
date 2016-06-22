using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogViewer.Models
{
	public class ResnetLog
	{
		public int? Id { get; set; }

		public string Date { get; set; }
		public string Thread { get; set; }
		public string Level { get; set; }
		public string Logger { get; set; }
		public string Message { get; set; }

		public string Exception { get; set; }

		public string Host { get; set; }
		public string RemoteHost { get; set; }
		public string Username { get; set; }
		public string Browser { get; set; }

		public string RequestUrl { get; set; }

		public string UniqueId { get; set; }
	}
}