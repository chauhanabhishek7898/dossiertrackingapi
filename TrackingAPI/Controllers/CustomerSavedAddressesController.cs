using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using TrackingAPI.Models;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TrackingAPI.Utilities;

namespace TrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSavedAddressesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CustomerSavedAddressesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(CustomerSavedAddresses CM)
        {
            try
            {
                string query = "DM_sp_CustomerSavedAddresses_Insert";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nUserId", CM.nUserId);
                        myCommand.Parameters.AddWithValue("nCityId", CM.nCityId);
                        myCommand.Parameters.AddWithValue("vAddress", CM.vAddress);
                        myCommand.Parameters.AddWithValue("vLat", CM.vLat);
                        myCommand.Parameters.AddWithValue("vLong", CM.vLong);
                        myCommand.Parameters.AddWithValue("btByDefault", CM.btByDefault);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Added Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        public JsonResult Put(CustomerSavedAddresses CM)
        {
            try
            {
                string query = "DM_sp_CustomerSavedAddresses_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nOtherAdId", CM.nOtherAdId);
                        myCommand.Parameters.AddWithValue("nCityId", CM.nCityId);
                        myCommand.Parameters.AddWithValue("vAddress", CM.vAddress);
                        myCommand.Parameters.AddWithValue("vLat", CM.vLat);
                        myCommand.Parameters.AddWithValue("vLong", CM.vLong);
                        myCommand.Parameters.AddWithValue("btActive", CM.btActive);
                        myCommand.Parameters.AddWithValue("btByDefault", CM.btByDefault);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetCustomerSavedAddressesByUserId/{nUserId}")]
        public JsonResult GetCustomerSavedAddressesByUserId(int nUserId)
        {
            string query = "DM_sp_GetCustomerSavedAddressesByUserId";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nUserId", nUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetCustomerSavedAddressesByUserId_Active/{nUserId}")]
        public JsonResult GetCustomerSavedAddressesByUserId_Active(int nUserId)
        {
            string query = "DM_sp_GetCustomerSavedAddressesByUserId_Active";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nUserId", nUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
