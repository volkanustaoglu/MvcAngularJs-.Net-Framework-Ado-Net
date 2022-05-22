using MvcProject.Models;
using MvcProject.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcProject.Methods
{
    public class LogManager
    {
        private static readonly object Lock = new object();
        private static volatile LogManager _instance;
        public static LogManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LogManager();
                        }
                    }
                }
                return _instance;
            }
        }
        

        public ProjectResult<List<Log>> CreateLog(Log entity)
        {
            ProjectResult<List<Log>> resut = new ProjectResult<List<Log>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[CreateLog]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@UserId", entity.UserId);
                        sqlCommand.Parameters.AddWithValue("@Controller", entity.Controller);
                        sqlCommand.Parameters.AddWithValue("@Action", entity.Action);
                        sqlCommand.Parameters.AddWithValue("@Role", entity.Role);
                        sqlCommand.Parameters.AddWithValue("@CreateDate", entity.CreateDate);
                        int effectedRow = sqlCommand.ExecuteNonQuery();

                        sqlCommand.Dispose();
                    }
                    ConnectionManager.Instance.Disposed(sqlConnection);
                }
            }
            catch (Exception ex)
            {
                ConnectionManager.Instance.Excep(ex, sqlConnection);
                resut.IsSuccess = false;
                resut.Message = ex.Message;
            }

            return resut;
        }

        public ProjectResult<List<Log>> GetAllEditorEditLogs()
        {
            ProjectResult<List<Log>> result = new ProjectResult<List<Log>>();
            List<Log> pages = new List<Log>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetAllEditorEditLogs]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                        {

                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    Log page = new Log();
                                    page.Id = Convert.ToInt32(rdr["Id"]);
                                    page.UserId = Convert.ToInt32(rdr["UserId"]);
                                    page.Controller = Convert.ToString(rdr["Controller"]);
                                    page.Action = Convert.ToString(rdr["Action"]);
                                    page.Role = Convert.ToString(rdr["Role"]);
                                    page.CreateDate = Convert.ToDateTime(rdr["CreateDate"]);
                                    page.Username = Convert.ToString(rdr["Username"]);
                                    pages.Add(page);
                                }
                            }
                            rdr.Close();
                        }
                        result.Data = pages;
                        sqlCommand.Dispose();
                    }
                    ConnectionManager.Instance.Disposed(sqlConnection);
                }
            }
            catch (Exception ex)
            {
                ConnectionManager.Instance.Excep(ex, sqlConnection);

                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;


        }
    }
   
}