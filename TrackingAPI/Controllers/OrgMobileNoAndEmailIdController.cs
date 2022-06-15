using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using TrackingAPI.Models;


namespace TrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrgMobileNoAndEmailIdController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrgMobileNoAndEmailIdController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = "DM_sp_OrgMobileNoAndEmailId_SelectAll";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(OrgMobileNoAndEmailId CM)
        {
            try
            {
                string query = "DM_sp_OrgMobileNoAndEmailId_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nID", CM.nID);
                        myCommand.Parameters.AddWithValue("vMobileNoOrg", CM.vMobileNoOrg);
                        myCommand.Parameters.AddWithValue("vEmailIdOrg", CM.vEmailIdOrg);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
