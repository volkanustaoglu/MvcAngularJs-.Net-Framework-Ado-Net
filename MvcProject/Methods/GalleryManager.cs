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
    public class GalleryManager
    {
        private static readonly object Lock = new object();
        private static volatile GalleryManager _instance;
        public static GalleryManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GalleryManager();
                        }
                    }
                }
                return _instance;
            }

        }

        public ProjectResult<List<Gallery>> GetAllGalleries()
        {
            ProjectResult<List<Gallery>> result = new ProjectResult<List<Gallery>>();
            List<Gallery> galleries = new List<Gallery>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetAllGalleries]";

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
                                    Gallery gallery = new Gallery();
                                    gallery.Id = Convert.ToInt32(rdr["Id"]);
                                    gallery.Name = Convert.ToString(rdr["Name"]);
                                    gallery.Img = Convert.ToString(rdr["Img"]);
                                    gallery.Input1 = Convert.ToString(rdr["Input1"]);
                                    gallery.Input2 = Convert.ToString(rdr["Input2"]);
                                    gallery.UserId = Convert.ToInt32(rdr["UserId"]);
                                    galleries.Add(gallery);
                                }
                            }
                            rdr.Close();
                        }
                        result.Data = galleries;
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

        public bool DeleteGallery(int Id)
        {
            bool result = false;
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[DeleteGallery]";

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

        public ProjectResult<List<Gallery>> CreateGallery(Gallery newGallery)
        {
            ProjectResult<List<Gallery>> resut = new ProjectResult<List<Gallery>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[CreateGallery]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Name", newGallery.Name);
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

        public ProjectResult<List<Gallery>> GetGallery(int Id)
        {
            List<Gallery> galleries = new List<Gallery>();
            ProjectResult<List<Gallery>> result = new ProjectResult<List<Gallery>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetGallery]";

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
                                    Gallery gallery = new Gallery();
                                    gallery.Id = Convert.ToInt32(read["Id"]);
                                    gallery.Name = Convert.ToString(read["Name"]);
                                    gallery.UserId = Convert.ToInt32(read["UserId"]);
                                    gallery.Img = Convert.ToString(read["Img"]);
                                    gallery.Input1 = Convert.ToString(read["Input1"]);
                                    gallery.Input2 = Convert.ToString(read["Input2"]);
                                    galleries.Add(gallery);
                                }
                            }
                            read.Close();
                        }
                        result.Data = galleries;
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

        public ProjectResult<List<Gallery>> UpdateGallery(Gallery entity)
        {
            ProjectResult<List<Gallery>> result = new ProjectResult<List<Gallery>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[UpdateGallery]";
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);
                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", entity.Id);
                        sqlCommand.Parameters.AddWithValue("@Name", entity.Name);
                        sqlCommand.Parameters.AddWithValue("@UserId", entity.UserId);
                        sqlCommand.Parameters.AddWithValue("@Img", entity.Img);
                        sqlCommand.Parameters.AddWithValue("@Input1", entity.Input1);
                        sqlCommand.Parameters.AddWithValue("@Input2", entity.Input2);

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

        public ProjectResult<List<Gallery>> GetAllGalleriesWithUsername()
        {
            ProjectResult<List<Gallery>> result = new ProjectResult<List<Gallery>>();
            List<Gallery> galleries = new List<Gallery>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetAllGalleriesWithUsername]";

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
                                    Gallery gallery = new Gallery();
                                    gallery.Id = Convert.ToInt32(rdr["Id"]);
                                    gallery.Name = Convert.ToString(rdr["Name"]);
                                    gallery.Username = Convert.ToString(rdr["Username"]);
                                    gallery.Img = Convert.ToString(rdr["Img"]);
                                    gallery.Input1 = Convert.ToString(rdr["Input1"]);
                                    gallery.Input2 = Convert.ToString(rdr["Input2"]);
                                    gallery.UserId = Convert.ToInt32(rdr["UserId"]);
                                    galleries.Add(gallery);
                                }
                            }
                            rdr.Close();
                        }
                        result.Data = galleries;
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

        public ProjectResult<List<Gallery>> GetGalleryWithUsername(int Id)
        {
            List<Gallery> galleries = new List<Gallery>();
            ProjectResult<List<Gallery>> result = new ProjectResult<List<Gallery>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetGalleryWithUsername]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", Id);

                        using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    Gallery gallery = new Gallery();
                                    gallery.Id = Convert.ToInt32(rdr["Id"]);
                                    gallery.Name = Convert.ToString(rdr["Name"]);
                                    gallery.Username = Convert.ToString(rdr["Username"]);
                                    gallery.Img = Convert.ToString(rdr["Img"]);
                                    gallery.Input1 = Convert.ToString(rdr["Input1"]);
                                    gallery.Input2 = Convert.ToString(rdr["Input2"]);
                                    gallery.UserId = Convert.ToInt32(rdr["UserId"]);
                                    galleries.Add(gallery);
                                }
                            }
                            rdr.Close();
                        }
                        result.Data = galleries;
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
        public ProjectResult<List<Gallery>> GetGalleryWithUsernameForUserId(int Id)
        {
            List<Gallery> galleries = new List<Gallery>();
            ProjectResult<List<Gallery>> result = new ProjectResult<List<Gallery>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetGalleryWithUsernameForUserId]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@UserId", Id);

                        using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                while (rdr.Read())
                                {
                                    Gallery gallery = new Gallery();
                                    gallery.Id = Convert.ToInt32(rdr["Id"]);
                                    gallery.Name = Convert.ToString(rdr["Name"]);
                                    gallery.Username = Convert.ToString(rdr["Username"]);
                                    gallery.Img = Convert.ToString(rdr["Img"]);
                                    gallery.Input1 = Convert.ToString(rdr["Input1"]);
                                    gallery.Input2 = Convert.ToString(rdr["Input2"]);
                                    gallery.UserId = Convert.ToInt32(rdr["UserId"]);
                                    galleries.Add(gallery);
                                }
                            }
                            rdr.Close();
                        }
                        result.Data = galleries;
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

        public ProjectResult<List<Gallery>> UpdateGalleryByUser(Gallery entity)
        {
            ProjectResult<List<Gallery>> result = new ProjectResult<List<Gallery>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[UpdateGalleryByUser]";
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);
                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", entity.Id);
                        sqlCommand.Parameters.AddWithValue("@Name", entity.Name);
                        sqlCommand.Parameters.AddWithValue("@Img", entity.Img);
                        sqlCommand.Parameters.AddWithValue("@Input1", entity.Input1);
                        sqlCommand.Parameters.AddWithValue("@Input2", entity.Input2);

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
        public ProjectResult<List<Gallery>> GetGalleryByName(string id)
        {
            List<Gallery> galleries = new List<Gallery>();
            ProjectResult<List<Gallery>> result = new ProjectResult<List<Gallery>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetGalleryByName]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Name", id);

                        using (SqlDataReader read = sqlCommand.ExecuteReader())
                        {
                            if (read.HasRows)
                            {
                                while (read.Read())
                                {
                                    Gallery gallery = new Gallery();
                                    gallery.Id = Convert.ToInt32(read["Id"]);
                                    gallery.Name = Convert.ToString(read["Name"]);
                                    gallery.UserId = Convert.ToInt32(read["UserId"]);
                                    gallery.Img = Convert.ToString(read["Img"]);
                                    gallery.Input1 = Convert.ToString(read["Input1"]);
                                    gallery.Input2 = Convert.ToString(read["Input2"]);
                                    galleries.Add(gallery);
                                }
                            }
                            read.Close();
                        }
                        result.Data = galleries;
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