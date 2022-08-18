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
    public class DriverMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DriverMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<JsonResult> Post(IFormFile LicenseFile, IFormFile AadharNoFile, IFormFile PANNoFile, IFormFile VehicleRegFile, IFormFile VehicleInsuranceFile, IFormFile DriverPhotoFile)
        {
            try
            {
                string DriverMasterJson = Request.Form["DriverMaster"];
                DriverMasterClass PMC = JsonConvert.DeserializeObject<DriverMasterClass>(DriverMasterJson);
                //string CurrentDt = DateTime.Now.ToString("ddMMyyyyHHmm");
                if (LicenseFile != null)
                {
                    var vLicenseNoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//LicenseFile//{LicenseFile.FileName}";
                    var response = await ImageUploader.SaveImageND(LicenseFile,PMC.DriverMaster[0].vMobileNo, vLicenseNoFilePath);
                    PMC.DriverMaster[0].vLicenseNoFilePath = vLicenseNoFilePath;
                }
                if (AadharNoFile != null)
                {
                    var vAadhaarNoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//AadharNoFile//{AadharNoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(AadharNoFile, PMC.DriverMaster[0].vMobileNo, vAadhaarNoFilePath);
                    PMC.DriverMaster[0].vAadhaarNoFilePath = vAadhaarNoFilePath;
                }
                if (PANNoFile != null)
                {
                    var vPANNoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//PANNoFile//{PANNoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(PANNoFile, PMC.DriverMaster[0].vMobileNo, vPANNoFilePath);
                    PMC.DriverMaster[0].vPANNoFilePath = vPANNoFilePath;
                }
                if (VehicleRegFile != null)
                {
                    var vVehicleRegistrationNoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//VehicleRegFile//{VehicleRegFile.FileName}";
                    var response = await ImageUploader.SaveImageND(VehicleRegFile, PMC.DriverMaster[0].vMobileNo, vVehicleRegistrationNoFilePath);
                    PMC.DriverMaster[0].vVehicleRegistrationNoFilePath = vVehicleRegistrationNoFilePath;
                }
                if (VehicleInsuranceFile != null)
                {
                    var vVehicleInsuranceFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//VehicleInsuranceFile//{VehicleInsuranceFile.FileName}";
                    var response = await ImageUploader.SaveImageND(VehicleInsuranceFile, PMC.DriverMaster[0].vMobileNo, vVehicleInsuranceFilePath);
                    PMC.DriverMaster[0].vVehicleInsuranceFilePath = vVehicleInsuranceFilePath;
                }
                if (DriverPhotoFile != null)
                {
                    var vPhotoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//DriverPhotoFile//{DriverPhotoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(DriverPhotoFile, PMC.DriverMaster[0].vMobileNo, vPhotoFilePath);
                    PMC.DriverMaster[0].vPhotoFilePath = vPhotoFilePath;
                }
                string query = "DM_sp_DriverMaster_Insert";

                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                var JsonInput = JsonConvert.SerializeObject(PMC);
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("JSON_INPUT", JsonInput);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();

                        //Send Email and Sms
                        string MailSubject = MessageTemplate.SubLoginCreationDriver;
                        string EmailText = MessageTemplate.LoginCreationDriverMail(table.Rows[0]["AutoDriverId"].ToString(), table.Rows[0]["DriverName"].ToString(), table.Rows[0]["UserRole"].ToString());
                        string SMSText = MessageTemplate.LoginCreationDriverSMS(table.Rows[0]["AutoDriverId"].ToString(), table.Rows[0]["UserRole"].ToString());
                        MailSender.SendEmailText(MailSubject, EmailText, table.Rows[0]["DriverEmailId"].ToString(), table.Rows[0]["vEmailIdOrg"].ToString());
                        SmsSender.SendSmsText(SMSText, table.Rows[0]["CombinedMobileNo"].ToString());
                    }
                }
                return new JsonResult(table);
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        public async Task<JsonResult> Put(IFormFile LicenseFile, IFormFile AadharNoFile, IFormFile PANNoFile, IFormFile VehicleRegFile, IFormFile VehicleInsuranceFile, IFormFile DriverPhotoFile)
        {
            try
            {
                string DriverMasterJson = Request.Form["DriverMaster"];
                DriverMasterClass PMC = JsonConvert.DeserializeObject<DriverMasterClass>(DriverMasterJson);
                if (LicenseFile != null)
                {
                    var vLicenseNoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//LicenseFile//{LicenseFile.FileName}";
                    var response = await ImageUploader.SaveImageND(LicenseFile, PMC.DriverMaster[0].vMobileNo, vLicenseNoFilePath);
                    PMC.DriverMaster[0].vLicenseNoFilePath = vLicenseNoFilePath;
                }
                if (AadharNoFile != null)
                {
                    var vAadhaarNoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//AadharNoFile//{AadharNoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(AadharNoFile, PMC.DriverMaster[0].vMobileNo, vAadhaarNoFilePath);
                    PMC.DriverMaster[0].vAadhaarNoFilePath = vAadhaarNoFilePath;
                }
                if (PANNoFile != null)
                {
                    var vPANNoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//PANNoFile//{PANNoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(PANNoFile, PMC.DriverMaster[0].vMobileNo, vPANNoFilePath);
                    PMC.DriverMaster[0].vPANNoFilePath = vPANNoFilePath;
                }
                if (VehicleRegFile != null)
                {
                    var vVehicleRegistrationNoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//VehicleRegFile//{VehicleRegFile.FileName}";
                    var response = await ImageUploader.SaveImageND(VehicleRegFile, PMC.DriverMaster[0].vMobileNo, vVehicleRegistrationNoFilePath);
                    PMC.DriverMaster[0].vVehicleRegistrationNoFilePath = vVehicleRegistrationNoFilePath;
                }
                if (VehicleInsuranceFile != null)
                {
                    var vVehicleInsuranceFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//VehicleInsuranceFile//{VehicleInsuranceFile.FileName}";
                    var response = await ImageUploader.SaveImageND(VehicleInsuranceFile, PMC.DriverMaster[0].vMobileNo, vVehicleInsuranceFilePath);
                    PMC.DriverMaster[0].vVehicleInsuranceFilePath = vVehicleInsuranceFilePath;
                }
                if (DriverPhotoFile != null)
                {
                    var vPhotoFilePath = $"assets//DriverMaster//PMC.DriverMaster[0].vMobileNo//DriverPhotoFile//{DriverPhotoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(DriverPhotoFile, PMC.DriverMaster[0].vMobileNo, vPhotoFilePath);
                    PMC.DriverMaster[0].vPhotoFilePath = vPhotoFilePath;
                }
                string query = "DM_sp_DriverMaster_Update";

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
        [Route("DriverMaster_SelectAll/{vGeneric}")]
        public JsonResult DriverMaster_SelectAll(string vGeneric)
        {
            string query = "DM_sp_DriverMaster_SelectAll";
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
        [Route("GetDrivers_ForApproval/{vGeneric}")]
        public JsonResult GetDrivers_ForApproval(string vGeneric)
        {
            string query = "DM_sp_GetDrivers_ForApproval";
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
        [Route("GetDriverOnDutyStatus/{nDriverUserId}")]
        public JsonResult GetDriverOnDutyStatus(int nDriverUserId)
        {
            string query = "DM_sp_GetDriverOnDutyStatus";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nDriverUserId", nDriverUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetDriver_CurrentLocation/{nDriverUserId}")]
        public JsonResult GetDriver_CurrentLocation(int nDriverUserId)
        {
            string query = "DM_sp_GetDriver_CurrentLocation";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nDriverUserId", nDriverUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut]
        [Route("UpdateDriver_CurrentLocation")]
        public JsonResult UpdateDriver_CurrentLocation(DriverMaster CM)
        {
            try
            {
                string query = "DM_sp_UpdateDriver_CurrentLocation";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nDriverUserId", CM.nUserId);
                        myCommand.Parameters.AddWithValue("vDiriverCurrentLat", CM.vDiriverCurrentLat);
                        myCommand.Parameters.AddWithValue("vDiriverCurrentLong", CM.vDiriverCurrentLong);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("UpdateDriver_OnDutyStatus")]
        public JsonResult UpdateDriver_OnDutyStatus(DriverMaster CM)
        {
            try
            {
                string query = "DM_sp_UpdateDriver_OnDutyStatus";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nDriverUserId", CM.nUserId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetTrackingDetailsForDrivers/{nDriverUserId}/{FromDt}/{ToDt}/{vGeneric}")]
        public JsonResult GetTrackingDetailsForDrivers(int nDriverUserId,string FromDt,string ToDt, string vGeneric)
        {
            string query = "DM_sp_GetTrackingDetailsForDrivers";
            if (vGeneric == "null") { vGeneric = null; }
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nDriverUserId", nDriverUserId);
                    myCommand.Parameters.AddWithValue("FromDt", FromDt); myCommand.Parameters.AddWithValue("ToDt", ToDt);
                    myCommand.Parameters.AddWithValue("vGeneric", vGeneric);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut]
        [Route("ApproveDrivers")]
        public JsonResult ApproveDrivers(DriverMaster CM)
        {
            try
            {
                string query = "DM_sp_ApproveDrivers";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nUserId", CM.nUserId);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();

                        //Send Email and Sms
                        string MailSubject = MessageTemplate.SubDriverApproved;
                        string EmailText = MessageTemplate.DriverApprovedMail(table.Rows[0]["AutoDriverId"].ToString(), table.Rows[0]["DriverName"].ToString(), table.Rows[0]["UserRole"].ToString());
                        string SMSText = MessageTemplate.DriverApprovedSMS(table.Rows[0]["AutoDriverId"].ToString(), table.Rows[0]["UserRole"].ToString());
                        MailSender.SendEmailText(MailSubject, EmailText, table.Rows[0]["CustomerEmailId"].ToString(), table.Rows[0]["vEmailIdOrg"].ToString());
                        SmsSender.SendSmsText(SMSText, table.Rows[0]["CombinedMobileNo"].ToString());
                    }
                }
                return new JsonResult("Driver Approved Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }
    }

    public class DriverMasterClass
    {
        public List<DriverMaster> DriverMaster { get; set; }
    }
}
