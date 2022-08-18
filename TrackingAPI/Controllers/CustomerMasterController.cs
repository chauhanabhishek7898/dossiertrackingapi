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
        public JsonResult Post(CustomerMasterClass RUS)
        {
            string query = "DM_sp_CustomerMaster_Insert";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            var JsonInput = JsonConvert.SerializeObject(RUS);
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("JSON_INPUT", JsonInput);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();

                    //Send Email and Sms
                    string MailSubject = MessageTemplate.SubLoginCreation;
                    string EmailText = MessageTemplate.LoginCreationMail(table.Rows[0]["AutoCId"].ToString(), table.Rows[0]["CustomerName"].ToString(), table.Rows[0]["UserRole"].ToString());
                    string SMSText = MessageTemplate.LoginCreationSMS(table.Rows[0]["AutoCId"].ToString(), table.Rows[0]["UserRole"].ToString());
                    MailSender.SendEmailText(MailSubject, EmailText, table.Rows[0]["CustomerEmailId"].ToString(), table.Rows[0]["vEmailIdOrg"].ToString());
                    SmsSender.SendSmsText(SMSText, table.Rows[0]["CombinedMobileNo"].ToString());
                }
            }
            return new JsonResult("Record Added Successfully !!");
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

        [HttpGet]
        [Route("GetCustomerDetailsForAdmin/{vGeneric}")]
        public JsonResult GetCustomerDetailsForAdmin(string vGeneric)
        {
            string query = "DM_sp_GetCustomerDetailsForAdmin";
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

        [HttpGet]
        [Route("GetCustomerSavedAddressForAdmin/{vGeneric}")]
        public JsonResult GetCustomerSavedAddressForAdmin(string vGeneric)
        {
            string query = "DM_sp_GetCustomerSavedAddressForAdmin";
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

        [HttpPut]
        [Route("ActivateRevokeRightsOfCustomer")]
        public JsonResult ActivateRevokeRightsOfCustomer(UserMaster CM)
        {
            try
            {
                string query = "DM_sp_ActivateRevokeRightsOfCustomer";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nUserId", CM.nUserId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }
    }

    public class CustomerMasterClass
    {
        public List<CustomerMaster> CustomerMaster { get; set; }
    }
}
