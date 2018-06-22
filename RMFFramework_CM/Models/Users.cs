using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.ComponentModel.DataAnnotations;

namespace RMFFramework_CM.Models
{
    public class Users
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }

        public DataTable Login(string userName, string password)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec  spUser_login @UserName={0}, @Password=N'{1}'", userName, password), _conn);
                _da.Fill(_dt);
                _conn.Close();

                return _dt;
            }
        }

        public DataTable SignUp(int CreatedBy, Users pUser)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spUser_insert @CreatedBy={0}, @UserName={1}, @Password=N'{2}', @EmailAddress=N'{3}', @FirstName={4}, @MiddleName={5}, @LastName={6}, @RoleID={7}", CreatedBy, pUser.UserName, pUser.Password, pUser.EmailAddress, pUser.FirstName, pUser.MiddleName, pUser.LastName, pUser.Role), _conn);
                _da.Fill(_dt);
                _conn.Close();

                return _dt;
            }
            
        }
        public DataTable getUsers(long userId)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spUser_get @UserID=N'{0}'", userId), _conn);
                _da.Fill(_dt);
                _conn.Close();

                return _dt;
            }
        }
        public DataTable updateUsers(Users pUser)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spUser_update @UserID=N'{0}', @Password={1}, @FirstName={2},@MiddleName={3},@LastName={4},@Role={5}", pUser.UserID,pUser.Password,pUser.FirstName,pUser.MiddleName,pUser.LastName,pUser.Role), _conn);
                _da.Fill(_dt);
                _conn.Close();

                return _dt;
            }
        }
        public DataTable deleteUser(int UserID)
        {

            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spUser_delete @UserID=N'{0}'", UserID), _conn);
                _da.Fill(_dt);
                _conn.Close();

                return _dt;
            }
        }

 

    }
}