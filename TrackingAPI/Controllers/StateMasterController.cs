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
    public class StateMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public StateMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = "DM_sp_StateMaster_SelectAll";
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

        [Route("GetAllActiveStates")]
        public JsonResult GetAllActiveStates()
        {
            string query = "DM_sp_StateMaster_SelectAll_Active";
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
        public JsonResult Post(StateMaster SM)
        {
            try
            {
                string query = "DM_sp_StateMaster_Insert";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("vStateName", SM.vStateName);
                        myCommand.Parameters.AddWithValue("vStatePrefix", SM.vStatePrefix);
                        myCommand.Parameters.AddWithValue("nCountryId", SM.nCountryId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Added Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        public JsonResult Put(StateMaster SM)
        {
            try
            {
                string query = "DM_sp_StateMaster_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nStateId", SM.nStateId);
                        myCommand.Parameters.AddWithValue("vStateName", SM.vStateName);
                        myCommand.Parameters.AddWithValue("vStatePrefix", SM.vStatePrefix);
                        myCommand.Parameters.AddWithValue("nCountryId", SM.nCountryId);
                        myCommand.Parameters.AddWithValue("btActive", SM.btActive);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
