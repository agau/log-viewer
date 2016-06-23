using LogViewer.Data;
using LogViewer.Extensions;
using LogViewer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LogViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();			
        }
		
		[HttpGet]
		public JsonResult LogResults(string environment, int rowCount, string logger, string level, bool useDate, string dateFrom, string dateTo, string uniqueId)
		{
			List<ResnetLog> logs = new List<ResnetLog>();
			bool isSuccess = false;
			string message = string.Empty;
			string stacktrace = string.Empty;
			string env = String.Empty;
			DateTime? fromDate = DateTime.Parse(dateFrom);
			DateTime? toDate = DateTime.Parse(dateTo);
			Guid logId = Guid.Empty;
			Guid.TryParse(uniqueId, out logId);
			string loggerName = logger == "All" ? null : logger;
			string logLevel = level == "All" ? null : level;

			try
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SELECT TOP(@RowsReturned) [Id],[Date],[Level],[Logger],[Message],[Exception],[RemoteHost],[Username],[Browser],[RequestUrl],[UniqueId]");
				sb.Append(" FROM [Resnet].[dbo].[resnet_log] with(NOLOCK)");
				sb.Append(" WHERE (@Logger IS NULL OR Logger = @Logger)");
				sb.Append(" AND (@Level IS NULL OR Level = @Level)");
				sb.Append(" AND (@DateFrom IS NULL OR Date > @DateFrom)");
				sb.Append(" AND (@DateTo IS NULL OR Date < @DateTo)");
				sb.Append(" AND (@UniqueId IS NULL OR Uniqueid = @UniqueId)");
				sb.Append(" order by Id desc");

				SqlGeneral.ParamBuilder p = new SqlGeneral.ParamBuilder();
				p.AddParam(SqlDbType.Int, "@RowsReturned", rowCount);
				p.Parameters[0].Value = rowCount;

				if(!String.IsNullOrEmpty(loggerName))
				{
					p.AddParam(SqlDbType.VarChar, "@Logger", loggerName);
					p.Parameters[1].Value = loggerName;
				}
				else
				{
					p.AddParam(SqlDbType.VarChar, "@Logger", DBNull.Value);
					p.Parameters[1].Value = DBNull.Value;
				}

				if (!String.IsNullOrEmpty(logLevel))
				{
					p.AddParam(SqlDbType.VarChar, "@Level", logLevel);
					p.Parameters[2].Value = logLevel;
				}
				else
				{
					p.AddParam(SqlDbType.VarChar, "@Level", DBNull.Value);
					p.Parameters[2].Value = DBNull.Value;
				}									

				if (logId == Guid.Empty)
				{
					p.AddParam(SqlDbType.UniqueIdentifier, "@UniqueId", DBNull.Value);
					p.Parameters[3].Value = DBNull.Value;
				}
				else
				{
					p.AddParam(SqlDbType.UniqueIdentifier, "@UniqueId", logId);
					p.Parameters[3].Value = logId;
				}

				if (useDate)
				{
					p.AddParam(SqlDbType.DateTime2, "@DateFrom", fromDate);
					p.Parameters[4].Value = dateFrom;
					p.AddParam(SqlDbType.DateTime2, "@DateTo", toDate);
					p.Parameters[5].Value = dateTo;
				}
				else
				{
					p.AddParam(SqlDbType.DateTime2, "@DateFrom", DBNull.Value);
					p.Parameters[4].Value = DBNull.Value;
					p.AddParam(SqlDbType.DateTime2, "@DateTo", DBNull.Value);
					p.Parameters[5].Value = DBNull.Value;
				}

				if (environment == "DEV")
					env = "DevResnetLog";
				else if (environment == "QA")
					env = "QaResnetLog";
				else if (environment == "UAT")
					env = "UatResnetLog";
				else if (environment == "PROD")
					env = "ProdResnetLog";

				using (SqlDataReader dr = SqlGeneral.ExecuteReader(sb.ToString(), p.Parameters, false, env))
				{
					string messageUniqueId = string.Empty;
					//int i = 0;
					while (dr.Read())
					{
						ResnetLog log = new ResnetLog();
						log.Id = dr.GetValue(0).SafeParseInt();
						log.Date = dr.GetValue(1).SafeParseDateTime().ToString();
						log.Level = dr.GetValue(2).SafeToString();
						log.Logger = dr.GetValue(3).SafeToString();
						log.Message = dr.GetValue(4).SafeToString();
						log.Exception = dr.GetValue(5).SafeToString();
						log.RemoteHost = dr.GetValue(6).SafeToString();
						log.Username = dr.GetValue(7).SafeToString();
						log.Browser = dr.GetValue(8).SafeToString();
						log.RequestUrl = dr.GetValue(9).SafeToString();
						log.UniqueId = dr.GetValue(10).SafeToString();
						logs.Add(log);
					}
				}

				isSuccess = true;

			}
			catch (Exception ex)
			{
				isSuccess = false;
				message = ex.Message;
				stacktrace = ex.StackTrace;
			}
		
			return Json(new { Result = isSuccess, Message = message, Stacktrace = stacktrace, data = logs }, JsonRequestBehavior.AllowGet);
		}
			
		
	}
}