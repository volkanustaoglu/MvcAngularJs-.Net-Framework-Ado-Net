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
    public class ExhibitionManager
    {
        private static readonly object Lock = new object();
        private static volatile ExhibitionManager _instance;
        public static ExhibitionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ExhibitionManager();
                        }
                    }
                }
                return _instance;
            }

        }
        public ProjectResult<List<Exhibition>> GetAllExhibitions()
        {
            ProjectResult<List<Exhibition>> result = new ProjectResult<List<Exhibition>>();
            List<Exhibition> exhibitions = new List<Exhibition>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetAllExhibitons]";

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
                                    Exhibition exhibition = new Exhibition();
                                    exhibition.Id = Convert.ToInt32(rdr["Id"]);
                                    exhibition.Name = Convert.ToString(rdr["Name"]);
                                    exhibition.Description = Convert.ToString(rdr["Description"]);
                                    exhibition.GalleryId = Convert.ToInt32(rdr["GalleryId"]);
                                    exhibition.UserId = Convert.ToInt32(rdr["UserId"]);
                                    exhibitions.Add(exhibition);
                                }
                            }
                            rdr.Close();
                        }
                        result.Data = exhibitions;
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

        public bool DeleteExhibition(int Id)
        {
            bool result = false;
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[DeleteExhibition]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);


                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", Id);

                        int effectedRow = sqlCommand.ExecuteNonQuery();
                        result = effectedRow > 0;
                        sqlConnection.Close();
                    }
                    ConnectionManager.Instance.Disposed(sqlConnection);
                }
            }
            catch (Exception ex)
            {
                ConnectionManager.Instance.Excep(ex, sqlConnection);
            }

            return result;
        }


        public ProjectResult<List<Exhibition>> CreateExhibition(Exhibition newEx)
        {
            ProjectResult<List<Exhibition>> resut = new ProjectResult<List<Exhibition>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[CreateExhibition]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Name", newEx.Name);
                        sqlCommand.Parameters.AddWithValue("@Description", newEx.Description);
                        sqlCommand.Parameters.AddWithValue("@GalleryId", newEx.GalleryId);
                        sqlCommand.Parameters.AddWithValue("@UserId", newEx.UserId);
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

        public ProjectResult<List<Exhibition>> GetExhibition(int Id)
        {
            List<Exhibition> exhibitions = new List<Exhibition>();
            ProjectResult<List<Exhibition>> result = new ProjectResult<List<Exhibition>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetExhibition]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", Id);

                        using (SqlDataReader read = sqlCommand.ExecuteReader())
                        {
                            if (read.HasRows)
                            {
                                while (read.Read())
                                {
                                    Exhibition exhibition = new Exhibition();
                                    exhibition.Id = Convert.ToInt32(read["Id"]);
                                    exhibition.Name = Convert.ToString(read["Name"]);
                                    exhibition.Description = Convert.ToString(read["Description"]);
                                    exhibition.GalleryId = Convert.ToInt32(read["GalleryId"]);
                                    exhibition.UserId = Convert.ToInt32(read["UserId"]);
                                    exhibitions.Add(exhibition);
                                }
                            }
                            read.Close();
                        }
                        result.Data = exhibitions;
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

        public ProjectResult<List<Exhibition>> UpdateExhibition(Exhibition entity)
        {
            ProjectResult<List<Exhibition>> result = new ProjectResult<List<Exhibition>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[UpdateExhibition]";
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);
                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Name", entity.Name);
                        sqlCommand.Parameters.AddWithValue("@Description", entity.Description);
                        sqlCommand.Parameters.AddWithValue("@GalleryId", entity.GalleryId);
                        sqlCommand.Parameters.AddWithValue("@UserId", entity.UserId);

                        sqlCommand.ExecuteNonQuery();

                        sqlCommand.Dispose();

                    }
                    sqlConnection.Dispose();
                    sqlConnection.Close();
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
        public ProjectResult<List<Exhibition>> GetAllExhibitionsForGalleryId(int Id)
        {
            List<Exhibition> exhibitions = new List<Exhibition>();
            ProjectResult<List<Exhibition>> result = new ProjectResult<List<Exhibition>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetAllExhibitionsForGalleryId]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@GalleryId", Id);

                        using (SqlDataReader read = sqlCommand.ExecuteReader())
                        {
                            if (read.HasRows)
                            {
                                while (read.Read())
                                {
                                    Exhibition exhibition = new Exhibition();
                                    exhibition.Id = Convert.ToInt32(read["Id"]);
                                    exhibition.Name = Convert.ToString(read["Name"]);
                                    exhibition.Description = Convert.ToString(read["Description"]);
                                    exhibition.GalleryId = Convert.ToInt32(read["GalleryId"]);
                                    exhibition.UserId = Convert.ToInt32(read["UserId"]);
                                    exhibitions.Add(exhibition);
                                }
                            }
                            read.Close();
                        }
                        result.Data = exhibitions;
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