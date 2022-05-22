using MvcProject.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcProject.Methods
{
    public sealed class ConnectionManager : IDisposable
    {
        private static readonly object Lock = new object();
        private static volatile ConnectionManager _instance;

        private static SqlConnection sqlConnection;
        private static SqlCommand sqlCommand;
        public static ConnectionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConnectionManager();
                        }
                    }
                }
                return _instance;
            }

        }

        public ConnectionManager()
        {


        }
        //Main
        public void Excep(Exception ex, SqlConnection sqlcon)
        {
            if (sqlcon != null)
            {
                sqlcon.Close();
            }
            WebLogger.GeneralLogger.Error(ex);
        }

        //Main
        public void cmdOperations()
        {
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.Clear();
        }


        public SqlCommand command(string proc, SqlConnection ww)
        {
            sqlCommand = new SqlCommand(proc, ww);
            return sqlCommand;
        }

        public void sqlConnect(SqlConnection connect)
        {
            if (connect.State == ConnectionState.Broken)
                connect.Close();

            if (connect.State == ConnectionState.Closed)
                connect.Open();
        }

        public void Disposed(SqlConnection connect)
        {
            connect.Dispose();
            connect.Close();
        }

        ///

        //Main
        public static SqlConnection GetSqlConnection()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString());
            if (sqlConnection.State == ConnectionState.Broken)
                sqlConnection.Close();

            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            return sqlConnection;
        }

        //Main
        public void Dispose()
        {

            sqlConnection.Dispose();
            sqlConnection.Close();
        }

        //Main
        public static SqlCommand command(string proc)
        {
            sqlCommand = new SqlCommand(proc, sqlConnection);
            return sqlCommand;
        }


    }
}