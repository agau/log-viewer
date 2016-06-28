using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LogViewer.Data
{
	public class SqlGeneral
	{
		#region [ Connection ]

		private static SqlConnection createConnection()
		{
			string sqlCn = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
			SqlConnection cn = new SqlConnection(sqlCn);
			cn.Open();
			return cn;
		}

		//private static SqlConnection createConnection(string connectionString)
		//{
		//	SqlConnection cn = new SqlConnection(connectionString);
		//	cn.Open();
		//	return cn;
		//}

		private static SqlConnection createConnection(string environment)
		{
			string sqlCn = System.Configuration.ConfigurationManager.ConnectionStrings[environment].ConnectionString;
			SqlConnection cn = new SqlConnection(sqlCn);
			cn.Open();
			return cn;
		}

		#endregion

		#region [ ExecuteReader ]

		/// <summary>
		/// Wraps the SqlCommand.ExecuteReader() method. 
		/// </summary>
		/// <param name="procedureNameOrSql">Stored procedure name or a Sql statement.</param>
		/// <param name="parameters">List of SqlParameter. Set to null if no parameters.</param>
		/// <param name="isStoredProcedure">True if the procedureNameOrSql is a stored procedure, false if it is a SQL statement.</param>
		/// <returns>Returns a SqlDataReader which MUST be wrapped in a using statement so that its SqlConnecion is closed as soon as the SqlDataReader is disposed.</returns>
		//public static SqlDataReader ExecuteReader(string procedureNameOrSql, List<SqlParameter> parameters, bool isStoredProcedure, string environment)
		//{
		//	return ExecuteReader(procedureNameOrSql, parameters, isStoredProcedure, environment);
		//}

		/// <summary>
		/// Wraps the SqlCommand.ExecuteReader() method.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="procedureNameOrSql">Stored procedure name or a Sql statement.</param>
		/// <param name="parameters">List of SqlParameter. Set to null if no parameters.</param>
		/// <param name="isStoredProcedure">True if the procedureNameOrSql is a stored procedure, false if it is a SQL statement.</param>
		/// <returns>Returns a SqlDataReader which MUST be wrapped in a using statement so that its SqlConnecion is closed as soon as the SqlDataReader is disposed.</returns>
		public static SqlDataReader ExecuteReader(string procedureNameOrSql, List<SqlParameter> parameters, bool isStoredProcedure, string environment)
		{
			//IMPORTANT: make sure you wrap the returned SqlDataReader in a using statement so that it is closed. (You do not need to close the SqlConnection object.)
			SqlConnection cn = (string.IsNullOrEmpty(environment) ? createConnection() : createConnection(environment));

			SqlCommand cmd = new SqlCommand(procedureNameOrSql, cn);

			if (isStoredProcedure)
				cmd.CommandType = CommandType.StoredProcedure;
			if (parameters != null)
				cmd.Parameters.AddRange(parameters.ToArray());
			
			return cmd.ExecuteReader(CommandBehavior.CloseConnection);
		}

		#endregion

		#region [ ExecuteScalar ]

		/// <summary>
		/// Wraps the SqlCommand.ExecuteScalar() method. 
		/// </summary>
		/// <param name="procedureNameOrSql">Stored procedure name or a Sql statement.</param>
		/// <param name="parameters">List of SqlParameter. Set to null if no parameters.</param>
		/// <param name="isStoredProcedure">True if the procedureNameOrSql is a stored procedure, false if it is a SQL statement.</param>
		/// <returns>Returns the first value of the first row of the Sql Statement.</returns>
		public static object ExecuteScalar(string procedureNameOrSql, List<SqlParameter> parameters, bool isStoredProcedure)
		{
			return ExecuteScalar(null, procedureNameOrSql, parameters, isStoredProcedure);
		}

		/// <summary>
		/// Wraps the SqlCommand.ExecuteScalar() method. 
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="procedureNameOrSql">Stored procedure name or a Sql statement.</param>
		/// <param name="parameters">List of SqlParameter. Set to null if no parameters.</param>
		/// <param name="isStoredProcedure">True if the procedureNameOrSql is a stored procedure, false if it is a SQL statement.</param>
		/// <returns>Returns the first value of the first row of the Sql Statement.</returns>
		public static object ExecuteScalar(string connectionString, string procedureNameOrSql, List<SqlParameter> parameters, bool isStoredProcedure)
		{
			object scalarValue;
			using (SqlConnection cn = (string.IsNullOrEmpty(connectionString) ? createConnection() : createConnection(connectionString)))
			{
				SqlCommand cmd = new SqlCommand(procedureNameOrSql, cn);

				if (isStoredProcedure)
					cmd.CommandType = CommandType.StoredProcedure;
				if (parameters != null)
					cmd.Parameters.AddRange(parameters.ToArray());

				scalarValue = cmd.ExecuteScalar();
			}

			return scalarValue;
		}

		public static object ExecuteScalar(string connectionString, string procedureNameOrSql, List<SqlParameter> parameters, bool isStoredProcedure, int timeout)
		{
			object scalarValue;
			using (SqlConnection cn = (string.IsNullOrEmpty(connectionString) ? createConnection() : createConnection(connectionString)))
			{
				SqlCommand cmd = new SqlCommand(procedureNameOrSql, cn);
				cmd.CommandTimeout = timeout;
				if (isStoredProcedure)
					cmd.CommandType = CommandType.StoredProcedure;
				if (parameters != null)
					cmd.Parameters.AddRange(parameters.ToArray());

				scalarValue = cmd.ExecuteScalar();
			}

			return scalarValue;
		}

		#endregion

		#region [ ExecuteNonQuery ]

		/// <summary>
		/// Wraps the SqlCommand.ExecuteNonQuery() method. 
		/// </summary>
		/// <param name="procedureNameOrSql">Stored procedure name or a Sql statement.</param>
		/// <param name="parameters">List of SqlParameter. Set to null if no parameters.</param>
		/// <param name="isStoredProcedure">True if the procedureNameOrSql is a stored procedure, false if it is a SQL statement</param>
		/// <returns>Returns the number of rows affected by ExecuteNonQuery().</returns>
		public static int ExecuteNonQuery(string procedureNameOrSql, List<SqlParameter> parameters, bool isStoredProcedure)
		{
			return ExecuteNonQuery(null, procedureNameOrSql, parameters, isStoredProcedure);
		}

		/// <summary>
		/// Wraps the SqlCommand.ExecuteNonQuery() method. 
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="procedureNameOrSql">Stored procedure name or a Sql statement.</param>
		/// <param name="parameters">List of SqlParameter. Set to null if no parameters.</param>
		/// <param name="isStoredProcedure">True if the procedureNameOrSql is a stored procedure, false if it is a SQL statement</param>
		/// <returns>Returns the number of rows affected by ExecuteNonQuery().</returns>
		public static int ExecuteNonQuery(string connectionString, string procedureNameOrSql, List<SqlParameter> parameters, bool isStoredProcedure)
		{
			int rowsAffected;
			using (SqlConnection cn = (string.IsNullOrEmpty(connectionString) ? createConnection() : createConnection(connectionString)))
			{
				SqlCommand cmd = new SqlCommand(procedureNameOrSql, cn);
				if (parameters != null)
					cmd.Parameters.AddRange(parameters.ToArray());
				if (isStoredProcedure)
					cmd.CommandType = CommandType.StoredProcedure;

				rowsAffected = cmd.ExecuteNonQuery();
			}

			return rowsAffected;
		}

		#endregion

		#region [ ParamBuilder ]

		public class ParamBuilder
		{
			private readonly List<SqlParameter> _parameters = new List<SqlParameter>();
			public List<SqlParameter> Parameters
			{
				get
				{
					return _parameters;
				}
			}

			public void AddParam(SqlDbType sqlDbType, string paramName, object paramVal)
			{
				SqlParameter p = new SqlParameter(paramName, sqlDbType);
				p.Value = paramVal ?? DBNull.Value;
				_parameters.Add(p);
			}

			public SqlParameter AddOutputParam(SqlDbType sqlDbType, string paramName)
			{
				SqlParameter p = new SqlParameter(paramName, sqlDbType);
				p.Direction = ParameterDirection.Output;
				_parameters.Add(p);
				return p;
			}
		}

		#endregion

		public static string CleanDynamicSql(string sql)
		{
			if (sql != null)
			{
				sql = sql.Replace("'", "''");
			}

			return sql;
		}
	}
}