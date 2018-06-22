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
    public class Regions
    {
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionRemarks { get; set; }

        public DataTable insertRegion(Regions region)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spRegion_insert @RegionName={0},@RegionRemarks={1}", region.RegionName, region.RegionRemarks), _conn);
                _da.Fill(_dt);
                _conn.Close();

                return _dt;
            }
        }
        public DataTable updateRegion(Regions region)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spRegion_update @RegionID=N'{0}',@RegionName=N'{1}',@RegionRemarks=N'{2}'", region.RegionID, region.RegionName, region.RegionRemarks), _conn);
                _da.Fill(_dt);
                _conn.Close();

                return _dt;
            }
        }
        public DataTable deleteRegion(int regionID)
        {

            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spRegion_delete @RegionID=N'{0}'", regionID), _conn);
                _da.Fill(_dt);
                _conn.Close();

                return _dt;
            }
        }
        public DataTable getRegion(int Status, int RegionID)
        {
            DataTable _dt = new DataTable();

            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spRegion_get @Status=N'{0}',@RegionID=N'{1}'", Status, RegionID), _conn);
                _da.Fill(_dt);
                _conn.Close();

                return _dt;
            }
        }
    }
}