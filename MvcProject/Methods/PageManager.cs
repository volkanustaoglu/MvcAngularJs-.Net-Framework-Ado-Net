

using MvcProject.Models;
using MvcProject.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace MvcProject.Methods
{
    public class PageManager
    {
        private static readonly object Lock = new object();
        private static volatile PageManager _instance;
        private static Random random = new Random();
        public static PageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PageManager();
                        }
                    }
                }
                return _instance;
            }

        }
        public ProjectResult<List<Page>> GetAllPages()
        {
            ProjectResult<List<Page>> result = new ProjectResult<List<Page>>();
            List<Page> pages = new List<Page>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetAllPages]";

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
                                    Page page = new Page();
                                    page.Id = Convert.ToInt32(rdr["Id"]);
                                    page.PageId = Convert.ToString(rdr["PageId"]);
                                    page.UserId = Convert.ToInt32(rdr["UserId"]);
                                    page.ArtistName = Convert.ToString(rdr["ArtistName"]);
                                    page.Title = Convert.ToString(rdr["Title"]);
                                    page.Technique = Convert.ToString(rdr["Technique"]);
                                    page.Size = Convert.ToString(rdr["Size"]);
                                    page.DateYear = Convert.ToString(rdr["DateYear"]);
                                    page.Signature = Convert.ToString(rdr["Signature"]);
                                    page.PageKey = Convert.ToString(rdr["PageKey"]);
                                    page.Img = Convert.ToString(rdr["Img"]);
                                    page.Note = Convert.ToString(rdr["Note"]);
                                    page.ExhibitionId = Convert.ToInt32(rdr["ExhibitionId"]);
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

        public bool DeletePage(int Id)
        {
            bool result = false;
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[DeletePage]";

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


        public ProjectResult<List<Page>> CreatePage(Page newPage)
        {
            ProjectResult<List<Page>> resut = new ProjectResult<List<Page>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[CreatePage]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@PageId", newPage.PageId);
                        sqlCommand.Parameters.AddWithValue("@PageKey", newPage.PageKey);
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

        public ProjectResult<List<Page>> GetPage(int Id)
        {
            List<Page> pages = new List<Page>();
            ProjectResult<List<Page>> result = new ProjectResult<List<Page>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetPage]";

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
                                    Page page = new Page();
                                    page.Id = Convert.ToInt32(read["Id"]);
                                    page.PageId = Convert.ToString(read["PageId"]);
                                    page.UserId = Convert.ToInt32(read["UserId"]);
                                    page.ArtistName = Convert.ToString(read["ArtistName"]);
                                    page.Title = Convert.ToString(read["Title"]);
                                    page.Technique = Convert.ToString(read["Technique"]);
                                    page.Size = Convert.ToString(read["Size"]);
                                    page.DateYear = Convert.ToString(read["DateYear"]);
                                    page.Signature = Convert.ToString(read["Signature"]);
                                    page.PageKey = Convert.ToString(read["PageKey"]);
                                    page.Img = Convert.ToString(read["Img"]);
                                    page.Note = Convert.ToString(read["Note"]);
                                    page.ExhibitionId = Convert.ToInt32(read["ExhibitionId"]);
                                    pages.Add(page);

                                    pages.Add(page);
                                }
                            }
                            read.Close();
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

        public ProjectResult<List<Page>> UpdatePage(Page entity)
        {
            ProjectResult<List<Page>> result = new ProjectResult<List<Page>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[UpdatePage]";
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);
                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", entity.Id);
                        sqlCommand.Parameters.AddWithValue("@PageId", entity.PageId);
                        sqlCommand.Parameters.AddWithValue("@UserId", entity.UserId);
                        sqlCommand.Parameters.AddWithValue("@Img", entity.Img);
                        sqlCommand.Parameters.AddWithValue("@ArtistName", entity.ArtistName);
                        sqlCommand.Parameters.AddWithValue("@Title", entity.Title);
                        sqlCommand.Parameters.AddWithValue("@Technique", entity.Technique);
                        sqlCommand.Parameters.AddWithValue("@Size", entity.Size);
                        sqlCommand.Parameters.AddWithValue("@DateYear", entity.DateYear);
                        sqlCommand.Parameters.AddWithValue("@Signature", entity.Signature);
                        sqlCommand.Parameters.AddWithValue("@Note", entity.Note);
                        sqlCommand.Parameters.AddWithValue("@ExhibitionId", entity.ExhibitionId);

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

        public string RandomString(int length)

        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)

                .Select(s => s[random.Next(s.Length)]).ToArray());

        }

        public ProjectResult<List<Page>> GetPagesByUserId(Page findPage)
        {
            List<Page> pages = new List<Page>();
            ProjectResult<List<Page>> result = new ProjectResult<List<Page>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetPagesByUserId]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@UserId", findPage.UserId);

                        using (SqlDataReader read = sqlCommand.ExecuteReader())
                        {
                            if (read.HasRows)
                            {
                                while (read.Read())
                                {
                                    Page page = new Page();
                                    page.Id = Convert.ToInt32(read["Id"]);
                                    page.UserId = Convert.ToInt32(read["UserId"]);
                                    page.Img = read["Img"].ToString();
                                    page.ArtistName = Convert.ToString(read["ArtistName"]);
                                    page.Title = Convert.ToString(read["Title"]);
                                    page.Technique = Convert.ToString(read["Technique"]);
                                    page.Size = Convert.ToString(read["Size"]);
                                    page.DateYear = Convert.ToString(read["DateYear"]);
                                    page.Signature = Convert.ToString(read["Signature"]);
                                    page.PageKey = read["PageKey"].ToString();
                                    page.PageId = Convert.ToString(read["PageId"]);
                                    page.Note = Convert.ToString(read["Note"]);
                                    page.ExhibitionId = Convert.ToInt32(read["ExhibitionId"]);

                                    pages.Add(page);
                                }
                            }
                            read.Close();
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

        public ProjectResult<List<Page>> UpdatePageByUser(Page entity)
        {
            ProjectResult<List<Page>> result = new ProjectResult<List<Page>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[UpdatePageByUser]";
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);
                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", entity.Id);
                        sqlCommand.Parameters.AddWithValue("@Img", entity.Img);
                        sqlCommand.Parameters.AddWithValue("@ArtistName", entity.ArtistName);
                        sqlCommand.Parameters.AddWithValue("@Title", entity.Title);
                        sqlCommand.Parameters.AddWithValue("@Technique", entity.Technique);
                        sqlCommand.Parameters.AddWithValue("@Size", entity.Size);
                        sqlCommand.Parameters.AddWithValue("@DateYear", entity.DateYear);
                        sqlCommand.Parameters.AddWithValue("@Signature", entity.Signature);
                        sqlCommand.Parameters.AddWithValue("@Note", entity.Note);
                        sqlCommand.Parameters.AddWithValue("@ExhibitionId", entity.ExhibitionId);

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

        public ProjectResult<List<Page>> GetPagesByPageKey(string id)
        {
            List<Page> pages = new List<Page>();
            ProjectResult<List<Page>> result = new ProjectResult<List<Page>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetPageByPageKey]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@PageKey", id);

                        using (SqlDataReader read = sqlCommand.ExecuteReader())
                        {
                            if (read.HasRows)
                            {
                                while (read.Read())
                                {
                                    Page page = new Page();
                                    page.Id = Convert.ToInt32(read["Id"]);
                                    page.UserId = Convert.ToInt32(read["UserId"]);
                                    page.Img = read["Img"].ToString();
                                    page.ArtistName = Convert.ToString(read["ArtistName"]);
                                    page.Title = Convert.ToString(read["Title"]);
                                    page.Technique = Convert.ToString(read["Technique"]);
                                    page.Size = Convert.ToString(read["Size"]);
                                    page.DateYear = Convert.ToString(read["DateYear"]);
                                    page.Signature = Convert.ToString(read["Signature"]);
                                    page.Note = Convert.ToString(read["Note"]);
                                    page.PageKey = read["PageKey"].ToString();
                                    page.PageId = Convert.ToString(read["PageId"]);
                                    page.ExhibitionId = Convert.ToInt32(read["ExhibitionId"]);

                                    pages.Add(page);
                                }
                            }
                            read.Close();
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

        public ProjectResult<List<Page>> GetAllPagesWithUsername()
        {
            ProjectResult<List<Page>> result = new ProjectResult<List<Page>>();
            List<Page> pages = new List<Page>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetAllPagesWithUsername]";

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
                                    Page page = new Page();
                                    page.Id = Convert.ToInt32(rdr["Id"]);
                                    page.PageId = Convert.ToString(rdr["PageId"]);
                                    page.Username = Convert.ToString(rdr["Username"]);
                                    page.ArtistName = Convert.ToString(rdr["ArtistName"]);
                                    page.Title = Convert.ToString(rdr["Title"]);
                                    page.Technique = Convert.ToString(rdr["Technique"]);
                                    page.Size = Convert.ToString(rdr["Size"]);
                                    page.DateYear = Convert.ToString(rdr["DateYear"]);
                                    page.Signature = Convert.ToString(rdr["Signature"]);
                                    page.PageKey = Convert.ToString(rdr["PageKey"]);
                                    page.Img = Convert.ToString(rdr["Img"]);
                                    page.Note = Convert.ToString(rdr["Note"]);
                                    page.UserId = Convert.ToInt32(rdr["UserId"]);
                                    page.ExhibitionId = Convert.ToInt32(rdr["ExhibitionId"]);
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
        public ProjectResult<List<Page>> GetPageWithUsername(int Id)
        {
            List<Page> pages = new List<Page>();
            ProjectResult<List<Page>> result = new ProjectResult<List<Page>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetPageWithUsername]";

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
                                    Page page = new Page();
                                    page.Id = Convert.ToInt32(read["Id"]);
                                    page.PageId = Convert.ToString(read["PageId"]);
                                    page.UserId = Convert.ToInt32(read["UserId"]);
                                    page.ArtistName = Convert.ToString(read["ArtistName"]);
                                    page.Title = Convert.ToString(read["Title"]);
                                    page.Technique = Convert.ToString(read["Technique"]);
                                    page.Size = Convert.ToString(read["Size"]);
                                    page.DateYear = Convert.ToString(read["DateYear"]);
                                    page.Signature = Convert.ToString(read["Signature"]);
                                    page.PageKey = Convert.ToString(read["PageKey"]);
                                    page.Img = Convert.ToString(read["Img"]);
                                    page.Note = Convert.ToString(read["Note"]);
                                    page.Username = Convert.ToString(read["Username"]);
                                    page.ExhibitionId = Convert.ToInt32(read["ExhibitionId"]);
                                    pages.Add(page);

                                    pages.Add(page);
                                }
                            }
                            read.Close();
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

        public ProjectResult<List<Page>> GetPageWithExhibition(int Id)
        {
            List<Page> pages = new List<Page>();
            ProjectResult<List<Page>> result = new ProjectResult<List<Page>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetPageWithExhibition]";

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
                                    Page page = new Page();
                                    page.Id = Convert.ToInt32(read["Id"]);
                                    page.PageId = Convert.ToString(read["PageId"]);
                                    page.UserId = Convert.ToInt32(read["UserId"]);
                                    page.ArtistName = Convert.ToString(read["ArtistName"]);
                                    page.Title = Convert.ToString(read["Title"]);
                                    page.Technique = Convert.ToString(read["Technique"]);
                                    page.Size = Convert.ToString(read["Size"]);
                                    page.DateYear = Convert.ToString(read["DateYear"]);
                                    page.Signature = Convert.ToString(read["Signature"]);
                                    page.PageKey = Convert.ToString(read["PageKey"]);
                                    page.Img = Convert.ToString(read["Img"]);
                                    page.Note = Convert.ToString(read["Note"]);
                                    page.ExhibitionId = Convert.ToInt32(read["ExhibitionId"]);
                                    page.ExhibitionName = Convert.ToString(read["ExhibitionName"]);

                                    pages.Add(page);
                                }
                            }
                            read.Close();
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