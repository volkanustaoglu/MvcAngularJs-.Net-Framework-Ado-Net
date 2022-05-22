using MvcProject.Models;
using MvcProject.Tool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MvcProject.Methods
{
    public class SecurityManager
    {
        private static readonly object Lock = new object();
        private static volatile SecurityManager _instance;
        public static SecurityManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SecurityManager();
                        }
                    }
                }
                return _instance;
            }

        }

        public ProjectResult<List<User>> GetLoginUser(User findUser)
        {
            List<User> users = new List<User>();
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetLoginUser]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Username", findUser.Username);
                        sqlCommand.Parameters.AddWithValue("@Password", findUser.Password);

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

        public ProjectResult<List<User>> GetUserByEmail(User entity)
        {
            List<User> users = new List<User>();
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[GetUserByEmail]";

            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);

                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Email", entity.Email);

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

        public ProjectResult<List<User>> UpdateUserPasswordByUserIdToken(User entity)
        {
            ProjectResult<List<User>> result = new ProjectResult<List<User>>();
            SqlConnection sqlConnection = null;
            string proc = "[dbo].[UpdateUserPasswordByUserIdToken]";
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ToString()))
                {
                    ConnectionManager.Instance.sqlConnect(sqlConnection);
                    using (SqlCommand sqlCommand = ConnectionManager.Instance.command(proc, sqlConnection))
                    {
                        ConnectionManager.Instance.cmdOperations();

                        sqlCommand.Parameters.AddWithValue("@Id", entity.Id);
                        sqlCommand.Parameters.AddWithValue("@UserKey", entity.UserKey);
                        sqlCommand.Parameters.AddWithValue("@Password", entity.Password);
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

        public string GetPasswordHash(User entity)
        {
            var salt = "%-%0%%1881s";
            // Create an MD5 hash of the supplied password using the supplied salt as well.
            string sourceText = salt + entity.Password;
            ASCIIEncoding asciiEnc = new ASCIIEncoding();
            string hash = null;
            byte[] byteSourceText = asciiEnc.GetBytes(sourceText);
            MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();
            byte[] byteHash = md5Hash.ComputeHash(byteSourceText);
            foreach (byte b in byteHash)
            {
                hash += b.ToString("x2");
            }

            // Return the hashed password
            entity.Password = hash;

            return entity.Password;

        }

        private string XMLPOST(string PostAddress, string xmlData)
        {
            try
            {
                var res = "";
                byte[] bytes = Encoding.UTF8.GetBytes(xmlData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PostAddress);

                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "text/xml";
                request.Timeout = 300000000;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }

                // This sample only checks whether we get an "OK" HTTP status code back.
                // If you must process the XML-based response, you need to read that from
                // the response stream.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        string message = String.Format(
                        "POST failed. Received HTTP {0}",
                        response.StatusCode);
                        throw new ApplicationException(message);
                    }

                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader rdr = new StreamReader(responseStream))
                    {
                        res = rdr.ReadToEnd();
                    }
                    return res;
                }
            }
            catch
            {

                return "-1";
            }

        }

        public void SendPhoneMessage(string numberPhone, string sendMessageText)
        {
            String testXml = "<request>";
            testXml += "<authentication>";
            testXml += "<username>5363516644</username>";
            testXml += "<password></password>";
            testXml += "<key></key>";
            testXml += "<hash></hash>";
            testXml += "</authentication>";
            testXml += "<order>";
            testXml += "<sender>APITEST</sender>";
            testXml += "<sendDateTime></sendDateTime>";
            testXml += "<message>";
            testXml += "<text>"+sendMessageText+" </text>";
            testXml += "<receipents>";
            testXml += "<number>" + numberPhone + "</number>";
            testXml += "</receipents>";
            testXml += "</message>";
            testXml += "</order>";
            testXml += "</request>";

            this.XMLPOST("http://api.iletimerkezi.com/v1/send-sms", testXml);
        }




    }
}