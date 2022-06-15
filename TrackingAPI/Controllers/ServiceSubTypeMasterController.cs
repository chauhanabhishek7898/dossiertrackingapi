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
    public class ServiceSubTypeMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ServiceSubTypeMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ServiceSubTypeMaster_SelectAll")]
        public JsonResult ServiceSubTypeMaster_SelectAll()
        {
            string query = "DM_sp_ServiceSubTypeMaster_SelectAll";
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
        [Route("ServiceSubTypeMaster_SelectAll_Active")]
        public JsonResult ServiceSubTypeMaster_SelectAll_Active()
        {
            string query = "DM_sp_ServiceSubTypeMaster_SelectAll_Active";
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
        public JsonResult Post(ServiceSubTypeMaster CM)
        {
            try
            {
                string query = "DM_sp_ServiceSubTypeMaster_Insert";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("vServiceSubType", CM.vServiceSubType);
                        myCommand.Parameters.AddWithValue("nSTId", CM.nSTId);
                        myCommand.Parameters.AddWithValue("nVId", CM.nVId);
                        myCommand.Parameters.AddWithValue("nCityId", CM.nCityId);
                        myCommand.Parameters.AddWithValue("nFromKM", CM.nFromKM);
                        myCommand.Parameters.AddWithValue("nToKM", CM.nToKM);
                        myCommand.Parameters.AddWithValue("nRate", CM.nRate);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Added Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        public JsonResult Put(ServiceSubTypeMaster CM)
        {
            try
            {
                string query = "DM_sp_ServiceSubTypeMaster_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nSSTId", CM.nSSTId);
                        myCommand.Parameters.AddWithValue("vServiceSubType", CM.vServiceSubType);
                        myCommand.Parameters.AddWithValue("nSTId", CM.nSTId);
                        myCommand.Parameters.AddWithValue("nVId", CM.nVId);
                        myCommand.Parameters.AddWithValue("nCityId", CM.nCityId);
                        myCommand.Parameters.AddWithValue("nFromKM", CM.nFromKM);
                        myCommand.Parameters.AddWithValue("nToKM", CM.nToKM);
                        myCommand.Parameters.AddWithValue("nRate", CM.nRate);
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
