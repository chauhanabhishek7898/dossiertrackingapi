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
    public class CustomerMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CustomerMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        //public JsonResult Post(CustomerMasterClass RUS)
        //{
        //    string query = "DM_sp_CustomerMaster_Insert";
        //    DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
        //    var JsonInput = JsonConvert.SerializeObject(RUS);
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.CommandType = CommandType.StoredProcedure;
        //            myCommand.Parameters.AddWithValue("JSON_INPUT", JsonInput);
        //            myReader = myCommand.ExecuteReader(); myReader.Close(); myCon.Close();
        //        }
        //    }
        //    return new JsonResult("Record Added Successfully !!");
        //}
        public async Task<JsonResult> Post(IFormFile AadharNoFile)
        {
            try
            {
                string CustomerMasterJson = Request.Form["CustomerMaster"];
                CustomerMasterClass RUS = JsonConvert.DeserializeObject<CustomerMasterClass>(CustomerMasterJson);
                if (AadharNoFile != null)
                {
                    var vAadhaarNoFilePath = $"assets//CustomerMaster//PMC.CustomerMaster[0].vMobileNo//AadharNoFile//{AadharNoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(AadharNoFile, RUS.CustomerMaster[0].vMobileNo, vAadhaarNoFilePath);
                    RUS.CustomerMaster[0].vAadhaarNoFilePath = vAadhaarNoFilePath;
                }
                string query = "DM_sp_CustomerMaster_Insert";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                var JsonInput = JsonConvert.SerializeObject(RUS);
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("JSON_INPUT", JsonInput);
                        myReader = myCommand.ExecuteReader(); myReader.Close(); myCon.Close();
                    }
                }
                return new JsonResult("Record Added Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        public async Task<JsonResult> Put(IFormFile AadharNoFile)
        {
            try
            {
                string CustomerMasterJson = Request.Form["CustomerMaster"];
                CustomerMasterClass PMC = JsonConvert.DeserializeObject<CustomerMasterClass>(CustomerMasterJson);
                 if (AadharNoFile != null)
                {
                    var vAadhaarNoFilePath = $"assets//CustomerMaster//PMC.CustomerMaster[0].vMobileNo//AadharNoFile//{AadharNoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(AadharNoFile, PMC.CustomerMaster[0].vMobileNo, vAadhaarNoFilePath);
                    PMC.CustomerMaster[0].vAadhaarNoFilePath = vAadhaarNoFilePath;
                }
                 string query = "DM_sp_CustomerMaster_Update";

                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                var JsonInput = JsonConvert.SerializeObject(PMC);
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("JSON_INPUT", JsonInput);
                        myReader = myCommand.ExecuteReader(); myReader.Close(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetCustomerMasterByUserId/{nUserId}")]
        public JsonResult GetCustomerMasterByUserId(int nUserId)
        {
            string query = "DM_sp_GetCustomerMasterByUserId";
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

    public class CustomerMasterClass
    {
        public List<CustomerMaster> CustomerMaster { get; set; }
    }
}
