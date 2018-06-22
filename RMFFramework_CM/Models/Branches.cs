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
    public class Branches
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string BranchRemarks { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
        public int AddressID { get; set; }
        public int IsActive { get; set; }
        public int AreaID {get; set;}
        public int Status { get; set; }

        public DataTable getBranch(int Status, int IsActive)
        {
            DataTable _dt = new DataTable();
            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spBranch_get @Status={0},@BranchID={1}", Status,IsActive), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }
        }

        public DataTable insertBranch(Branches branch)
        {
            DataTable _dt = new DataTable();
            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spBranch_insert @BranchName={0}, @BranchRemarks={1}, @Latitude=N'{2}', @Longtitude=N'{3}', @AddressID={4}, @AreaID={5}", branch.BranchName, branch.BranchRemarks, branch.Latitude, branch.Longtitude, branch.AddressID,branch.AreaID), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }
        }

        public DataTable updateBranch(Branches branch)
        {
            DataTable _dt = new DataTable();
            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spBranch_update @BranchID={0},@BranchName={1}, @BranchRemarks={2}, @Latitude=N'{3}', @Longtitude=N'{4}', @AddressID={5}, @AreaID={6}",branch.BranchID, branch.BranchName, branch.BranchRemarks, branch.Latitude, branch.Longtitude, branch.AddressID,branch.AreaID), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }	
        }

        public DataTable deleteBranch(long branchId)
        {
            DataTable _dt = new DataTable();
            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spBranch_delete @BranchID={0}", branchId), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }
        }
    }
}