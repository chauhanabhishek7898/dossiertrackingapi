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
    public class VehicleRateMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VehicleRateMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("VehicleRateMaster_SelectAll")]
        public JsonResult VehicleRateMaster_SelectAll()
        {
            string query = "DM_sp_VehicleRateMaster_SelectAll";
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

        [HttpGet]
        [Route("VehicleRateMaster_SelectAll_Active")]
        public JsonResult VehicleRateMaster_SelectAll_Active()
        {
            string query = "DM_sp_VehicleRateMaster_SelectAll_Active";
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

        [HttpPost]
        public JsonResult Post(VehicleRateMaster CM)
        {
            try
            {
                string query = "DM_sp_VehicleRateMaster_Insert";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nVId", CM.nVId);
                        myCommand.Parameters.AddWithValue("nCityId", CM.nCityId);
                        myCommand.Parameters.AddWithValue("nRatePerKM", CM.nRatePerKM);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Added Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        public JsonResult Put(VehicleRateMaster CM)
        {
            try
            {
                string query = "DM_sp_VehicleRateMaster_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nVRId", CM.nVRId); myCommand.Parameters.AddWithValue("nVId", CM.nVId);
                        myCommand.Parameters.AddWithValue("nCityId", CM.nCityId); myCommand.Parameters.AddWithValue("nRatePerKM", CM.nRatePerKM);
                        myCommand.Parameters.AddWithValue("btActive", CM.btActive);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

    }
}
