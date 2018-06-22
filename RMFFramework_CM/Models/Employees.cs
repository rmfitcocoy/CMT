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
    public class Employees
    {
        public int EmployeeID { get; set; }
        public string FirstName{get;set;}
        public string MiddleName{get;set;}
        public string LastName {get;set;}
        public string EmailAddress {get;set;}
        public string ContactNumber {get; set;}
        public int BranchID {get; set;}

        public DataTable insertEmployee(Employees emp)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spEmployee_insert @FirstName={0},@MiddleName={1},@LastName={2},@EmailAddress=N'{3}',@ContactNumber={4},@BranchID={5}", emp.FirstName, emp.MiddleName,emp.LastName,emp.EmailAddress,emp.ContactNumber,emp.BranchID), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }	
        }
        public DataTable updateEmployee(Employees emp)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spEmployee_update @EmployeeID={0}, @FirstName={1},@MiddleName={2},@LastName={3},@EmailAddress=N'{4}',@ContactNumber={5},@BranchID={6}",emp.EmployeeID, emp.FirstName, emp.MiddleName, emp.LastName, emp.EmailAddress, emp.ContactNumber, emp.BranchID), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }	
        }
        public DataTable deleteEmployee(int EmployeeID)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spEmployee_delete @EmployeeID={0}", EmployeeID), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }
        }
        public DataTable getEmployee(int Status , int EmployeeID)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spEmployee_get @Status={0}, @EmployeeID={1}",Status, EmployeeID), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }
        }
    
    }
}