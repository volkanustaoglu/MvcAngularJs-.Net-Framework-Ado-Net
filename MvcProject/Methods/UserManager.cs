using MvcProject.Models;
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
    public class UserManager
    {
        private static readonly object Lock = new object();
        private static volatile UserManager _instance;
        public static UserManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserManager();
                        }
                    }
                }
                return _instance;
            }

        }
        public ProjectResult<List<User>> GetAllUsers()
        {
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            List<User> users = new List<User>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetAllUsers]";

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
                                    User user = new User();
                                    user.Id = Convert.ToInt32(rdr["Id"]);
                                    user.Username = Convert.ToString(rdr["Username"]);
                                    user.Email = Convert.ToString(rdr["Email"]);
                                    user.Role = Convert.ToString(rdr["Role"]);
                                    user.NameSurname = Convert.ToString(rdr["NameSurname"]);
                                    user.Address = Convert.ToString(rdr["Address"]);
                                    user.IdentityNumber = Convert.ToString(rdr["IdentityNumber"]);
                                    user.ConfirmEmail = Convert.ToBoolean(rdr["ConfirmEmail"]);
                                    user.ConfirmPhone = Convert.ToBoolean(rdr["ConfirmPhone"]);
                                    user.DialCode = Convert.ToString(rdr["DialCode"]);
                                    user.DialCodeIso = Convert.ToString(rdr["DialCodeIso"]);
                                    user.PhoneNumber = Convert.ToString(rdr["PhoneNumber"]);
                                    user.TaxAdministration = Convert.ToString(rdr["TaxAdministration"]);
                                    user.TaxNumber = Convert.ToString(rdr["TaxNumber"]);
                                    users.Add(user);
                                }
                            }
                            rdr.Close();
                        }
                        result.Data = users;
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

        public bool DeleteUser(User findUser)
        {
            bool result = false;
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[DeleteUser]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);


                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", findUser.Id);

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


        public ProjectResult<List<User>> CreateUser(User entity)
        {
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[RepeatRecordUser]";
            string proc1 = "[dbo].[CreateUser]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);
                    using (SqlCommand repeatRecord = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        repeatRecord.Parameters.AddWithValue("@Username", entity.Username);
                        repeatRecord.Parameters.AddWithValue("@Email", entity.Email);
                        int results = Convert.ToInt32(repeatRecord.ExecuteScalar());
                        repeatRecord.Dispose();

                        if (results == 0)
                        {
                            using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc1, sqlConnection))
                            {
                                ConnectionManager.Instance.cmdOperations();

                                sqlCommand.Parameters.AddWithValue("@Username", entity.Username);
                                sqlCommand.Parameters.AddWithValue("@Email", entity.Email);
                                sqlCommand.Parameters.AddWithValue("@Password", entity.Password);
                                sqlCommand.Parameters.AddWithValue("@NameSurname", entity.NameSurname);
                                sqlCommand.Parameters.AddWithValue("@Address", entity.Address);
                                sqlCommand.Parameters.AddWithValue("@IdentityNumber", entity.IdentityNumber);
                                sqlCommand.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber);
                                sqlCommand.Parameters.AddWithValue("@DialCode", entity.DialCode);
                                sqlCommand.Parameters.AddWithValue("@DialCodeIso", entity.DialCodeIso);
                                sqlCommand.Parameters.AddWithValue("@TaxAdministration", entity.TaxAdministration);
                                sqlCommand.Parameters.AddWithValue("@TaxNumber", entity.TaxNumber);
                                sqlCommand.Parameters.AddWithValue("@UserKey", entity.UserKey);
                                sqlCommand.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;

                                int effectedRow = sqlCommand.ExecuteNonQuery();

                                sqlCommand.Dispose();
                                result.ReturnId = Convert.ToInt32(sqlCommand.Parameters["@ReturnId"].Value);
                            }
                        }
                        else
                        {
                             result.IsSuccess = false;
                            result.Message = " Bu Kullanıcı Adı Veya Email Adresi Sistemimizde Kayıtlıdır.";

                        }
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

        public ProjectResult<List<User>> GetUser(int id)
        {
            List<User> users = new List<User>();
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetUser]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader read = sqlCommand.ExecuteReader())
                        {
                            if (read.HasRows)
                            {
                                while (read.Read())
                                {
                                    User user = new User();
                                    user.Id = Convert.ToInt32(read["Id"]);
                                    user.Username = read["Username"].ToString();
                                    user.Email = read["Email"].ToString();
                                    user.Role = read["Role"].ToString();
                                    user.NameSurname = Convert.ToString(read["NameSurname"]);
                                    user.Address = Convert.ToString(read["Address"]);
                                    user.IdentityNumber = Convert.ToString(read["IdentityNumber"]);
                                    user.ConfirmEmail = Convert.ToBoolean(read["ConfirmEmail"]);
                                    user.ConfirmPhone = Convert.ToBoolean(read["ConfirmPhone"]);
                                    user.DialCode = Convert.ToString(read["DialCode"]);
                                    user.DialCodeIso = Convert.ToString(read["DialCodeIso"]);
                                    user.PhoneNumber = Convert.ToString(read["PhoneNumber"]);
                                    user.TaxAdministration = Convert.ToString(read["TaxAdministration"]);
                                    user.TaxNumber = Convert.ToString(read["TaxNumber"]);
                                    user.UserKey = Convert.ToString(read["UserKey"]);

                                    users.Add(user);
                                }
                            }
                            read.Close();
                        }
                        result.Data = users;
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

        public ProjectResult<List<User>> UpdateUser(User entity)
        {
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[UpdateUser]";
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);
                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", entity.Id);
                        sqlCommand.Parameters.AddWithValue("@Username", entity.Username);
                        sqlCommand.Parameters.AddWithValue("@Email", entity.Email);
                        sqlCommand.Parameters.AddWithValue("@Role", entity.Role);
                        sqlCommand.Parameters.AddWithValue("@Password", entity.Password);
                        sqlCommand.Parameters.AddWithValue("@NameSurname", entity.NameSurname);
                        sqlCommand.Parameters.AddWithValue("@Address", entity.Address);
                        sqlCommand.Parameters.AddWithValue("@IdentityNumber", entity.IdentityNumber);
                        sqlCommand.Parameters.AddWithValue("@ConfirmEmail", entity.ConfirmEmail);
                        sqlCommand.Parameters.AddWithValue("@ConfirmPhone", entity.ConfirmPhone);
                        sqlCommand.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber);
                        sqlCommand.Parameters.AddWithValue("@DialCode", entity.DialCode);
                        sqlCommand.Parameters.AddWithValue("@DialCodeIso", entity.DialCodeIso);
                        sqlCommand.Parameters.AddWithValue("@TaxAdministration", entity.TaxAdministration);
                        sqlCommand.Parameters.AddWithValue("@TaxNumber", entity.TaxNumber);
                        //sqlCommand.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        sqlCommand.ExecuteNonQuery();

                        sqlCommand.Dispose();
                        //result.ReturnId = Convert.ToInt32(sqlCommand.Parameters["@ReturnId"].Value);

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

        public ProjectResult<List<User>> GetUserByName(string username)
        {
            List<User> users = new List<User>();
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetUserByName]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Username", username);


                        using ( SqlDataReader read = sqlCommand.ExecuteReader())
                        {
                            if (read.HasRows) 
                            {
                                while (read.Read())
                                {
                                    User user = new User();
                                    user.Id = Convert.ToInt32(read["Id"]);
                                    user.Username = read["Username"].ToString();
                                    user.Email = read["Email"].ToString();
                                    user.Role = read["Role"].ToString();
                                    user.NameSurname = Convert.ToString(read["NameSurname"]);
                                    user.Address = Convert.ToString(read["Address"]);
                                    user.IdentityNumber = Convert.ToString(read["IdentityNumber"]);
                                    user.ConfirmEmail = Convert.ToBoolean(read["ConfirmEmail"]);
                                    user.ConfirmPhone = Convert.ToBoolean(read["ConfirmPhone"]);
                                    user.DialCode = Convert.ToString(read["DialCode"]);
                                    user.DialCodeIso = Convert.ToString(read["DialCodeIso"]);
                                    user.PhoneNumber = Convert.ToString(read["PhoneNumber"]);
                                    user.TaxAdministration = Convert.ToString(read["TaxAdministration"]);
                                    user.TaxNumber = Convert.ToString(read["TaxNumber"]);

                                    users.Add(user);
                                }
                            }
                            read.Close();
                        }
                        result.Data = users;
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
        public ProjectResult<List<User>> UpdateUserEmailConfirm(string userId, string token)
        {
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[UpdateUserEmailConfirm]";
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);
                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", userId);
                        sqlCommand.Parameters.AddWithValue("@UserKey", token);
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

        public ProjectResult<List<User>> GetAllUsersRoleGallery()
        {
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            List<User> users = new List<User>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetAllUsersRoleGallery]";

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
                                    User user = new User();
                                    user.Id = Convert.ToInt32(rdr["Id"]);
                                    user.Username = Convert.ToString(rdr["Username"]);
                                    user.Email = Convert.ToString(rdr["Email"]);
                                    user.Role = Convert.ToString(rdr["Role"]);
                                    user.NameSurname = Convert.ToString(rdr["NameSurname"]);
                                    user.Address = Convert.ToString(rdr["Address"]);
                                    user.IdentityNumber = Convert.ToString(rdr["IdentityNumber"]);
                                    user.ConfirmEmail = Convert.ToBoolean(rdr["ConfirmEmail"]);
                                    user.ConfirmPhone = Convert.ToBoolean(rdr["ConfirmPhone"]);
                                    user.DialCode = Convert.ToString(rdr["DialCode"]);
                                    user.DialCodeIso = Convert.ToString(rdr["DialCodeIso"]);
                                    user.PhoneNumber = Convert.ToString(rdr["PhoneNumber"]);
                                    user.TaxAdministration = Convert.ToString(rdr["TaxAdministration"]);
                                    user.TaxNumber = Convert.ToString(rdr["TaxNumber"]);
                                    users.Add(user);
                                }
                            }
                            rdr.Close();
                        }
                        result.Data = users;
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