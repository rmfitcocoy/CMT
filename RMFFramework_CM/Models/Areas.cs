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
    public class Areas
    {
        public long CreatedBy { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string Description { get; set; }
        public int RegionCode { get; set; }
        public int IsActive { get; set; }

        public DataTable getArea(int isActive)
        {
            DataTable _dt = new DataTable();
            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spArea_get @IsActive={0}", isActive), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }
        }

        public DataTable insertArea(Areas area)
        {
            DataTable _dt = new DataTable();
            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spArea_insert @CreatedBy={0}, @AreaName=N'{1}', @Description=N'{2}', @RegionCode={3}, @IsActive={4}", area.CreatedBy, area.AreaName, area.Description, area.RegionCode, area.IsActive), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }
        }

        public DataTable updateArea(Areas area)
        {
            DataTable _dt = new DataTable();
            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spArea_update @AreaName=N'{0}', @Description=N'{1}', IsActive={2}, @RegionCode={3}", area.AreaName, area.Description, area.IsActive, area.RegionCode), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }
        }

        public DataTable deleteArea(int areaId, long createdBy)
        {
            DataTable _dt = new DataTable();
            using (SqlConnection _conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                _conn.Open();
                SqlDataAdapter _da = new SqlDataAdapter(String.Format("exec spArea_delete @AreaID={0}, @CreatedBy={1}", areaId, createdBy), _conn);
                _da.Fill(_dt);
                _conn.Close();
                return _dt;
            }
        }

    }
}