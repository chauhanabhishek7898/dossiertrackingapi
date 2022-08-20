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
    public class CorporateMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CorporateMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<JsonResult> Post(IFormFile AuthorizedSignatoryFile, IFormFile LogoFile)
        {
            try
            {
                string CorporateMasterJson = Request.Form["CorporateMaster"];
                CorporateMasterClass PMC = JsonConvert.DeserializeObject<CorporateMasterClass>(CorporateMasterJson);
                //string CurrentDt = DateTime.Now.ToString("ddMMyyyyHHmm");
                if (AuthorizedSignatoryFile != null)
                {
                    var vAuthorizedSignatoryFilePath = $"assets//CorporateMaster//PMC.CorporateMaster[0].vCPMobileNo//AuthorizedSignatoryFile//{AuthorizedSignatoryFile.FileName}";
                    var response = await ImageUploader.SaveImageND(AuthorizedSignatoryFile, PMC.CorporateMaster[0].vCPMobileNo, vAuthorizedSignatoryFilePath);
                    PMC.CorporateMaster[0].vAuthorizedSignatoryFilePath = vAuthorizedSignatoryFilePath;
                }
                if (LogoFile != null)
                {
                    var vLogoFilePath = $"assets//CorporateMaster//PMC.CorporateMaster[0].vCPMobileNo//LogoFile//{LogoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(LogoFile, PMC.CorporateMaster[0].vCPMobileNo, vLogoFilePath);
                    PMC.CorporateMaster[0].vLogoFilePath = vLogoFilePath;
                }
                string query = "DM_sp_CorporateMaster_Insert";

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
                        string MailSubject = MessageTemplate.SubER;
                        string EmailText = MessageTemplate.ERMail(table.Rows[0]["vContactPersonOwner"].ToString(), table.Rows[0]["vEType"].ToString(), table.Rows[0]["vEstablishmentName"].ToString(), table.Rows[0]["vEId"].ToString(), table.Rows[0]["vRoleName"].ToString(), table.Rows[0]["vMobileNo"].ToString());
                        string SMSText = MessageTemplate.ERSMS(table.Rows[0]["vContactPersonOwner"].ToString(), table.Rows[0]["vEType"].ToString(), table.Rows[0]["vEstablishmentName"].ToString(), table.Rows[0]["vEId"].ToString(), table.Rows[0]["vRoleName"].ToString(), table.Rows[0]["vMobileNo"].ToString());

                        MailSender.SendEmailText(MailSubject, EmailText, table.Rows[0]["vEmailId"].ToString(), table.Rows[0]["vEmailIdOrg"].ToString());
                        SmsSender.SendSmsText(SMSText, table.Rows[0]["CombinedMobileNo"].ToString());
                    }
                }
                return new JsonResult(table);
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        public async Task<JsonResult> Put(IFormFile AuthorizedSignatoryFile, IFormFile LogoFile)
        {
            try
            {
                string CorporateMasterJson = Request.Form["CorporateMaster"];
                CorporateMasterClass PMC = JsonConvert.DeserializeObject<CorporateMasterClass>(CorporateMasterJson);
                if (AuthorizedSignatoryFile != null)
                {
                    var vAuthorizedSignatoryFilePath = $"assets//CorporateMaster//PMC.CorporateMaster[0].vCPMobileNo//AuthorizedSignatoryFile//{AuthorizedSignatoryFile.FileName}";
                    var response = await ImageUploader.SaveImageND(AuthorizedSignatoryFile, PMC.CorporateMaster[0].vCPMobileNo, vAuthorizedSignatoryFilePath);
                    PMC.CorporateMaster[0].vAuthorizedSignatoryFilePath = vAuthorizedSignatoryFilePath;
                }
                if (LogoFile != null)
                {
                    var vLogoFilePath = $"assets//CorporateMaster//PMC.CorporateMaster[0].vCPMobileNo//LogoFile//{LogoFile.FileName}";
                    var response = await ImageUploader.SaveImageND(LogoFile, PMC.CorporateMaster[0].vCPMobileNo, vLogoFilePath);
                    PMC.CorporateMaster[0].vLogoFilePath = vLogoFilePath;
                }
                string query = "DM_sp_CorporateMaster_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                var JsonInput = JsonConvert.SerializeObject(PMC);
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("JSON_INPUT", JsonInput);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

    }

    public class CorporateMasterClass
    {
        public List<CorporateMaster> CorporateMaster { get; set; }
    }
}
