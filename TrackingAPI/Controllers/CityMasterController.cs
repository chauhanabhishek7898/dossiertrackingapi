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
    public class CityMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CityMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = "DM_sp_CityMaster_SelectAll";
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
        [Route("GetAllActiveCities")]
        public JsonResult GetAllActiveCities()
        {
            string query = "DM_sp_CityMaster_SelectAll_Active";
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
        [Route("GetMainInterrelatedCities")]
        public JsonResult GetMainInterrelatedCities()
        {
            string query = "DM_sp_GetMainInterrelatedCities";
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
        [Route("CityMaster_SelectAll_ActiveLikeSearch/{vGeneric}")]
        public JsonResult CityMaster_SelectAll_ActiveLikeSearch(string vGeneric)
        {
            string query = "DM_sp_CityMaster_SelectAll_ActiveLikeSearch";
            if (vGeneric == "null") { vGeneric = null; }
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vGeneric", vGeneric);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(CityMaster CM)
        {
            try
            {
                string query = "DM_sp_CityMaster_Insert";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("vCityName", CM.vCityName);
                        myCommand.Parameters.AddWithValue("nStateId", CM.nStateId);
                        myCommand.Parameters.AddWithValue("btMainInterRelatedCity", CM.btMainInterRelatedCity);
                        myCommand.Parameters.AddWithValue("nInterRelatedCityId", CM.nInterRelatedCityId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Added Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        public JsonResult Put(CityMaster CM)
        {
            try
            {
                string query = "DM_sp_CityMaster_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nCityId", CM.nCityId);
                        myCommand.Parameters.AddWithValue("vCityName", CM.vCityName);
                        myCommand.Parameters.AddWithValue("nStateId", CM.nStateId);
                        myCommand.Parameters.AddWithValue("btActive", CM.btActive);
                        myCommand.Parameters.AddWithValue("btMainInterRelatedCity", CM.btMainInterRelatedCity);
                        myCommand.Parameters.AddWithValue("nInterRelatedCityId", CM.nInterRelatedCityId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetCityIdAgainstCityName/{vCityName}")]
        public JsonResult CheckExistsLicenseNo(string vCityName)
        {
            string query = "DM_sp_GetCityIdAgainstCityName";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vCityName", vCityName);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            if (table.Rows.Count == 0) { return new JsonResult("We don't provide service to this City.."); }
            return new JsonResult(table);
        }

    }
}
