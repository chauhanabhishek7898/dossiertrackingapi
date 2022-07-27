using TrackingAPI.Models;
using TrackingAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UtilityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetCurrentDBDate")]
        public JsonResult GetCurrentDBDate()
        {
            string query = "AB_sp_GetCurrentDBDate";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetCurrentDBDateDDMMMYYYY")]
        public JsonResult GetCurrentDBDateDDMMMYYYY()
        {
            string query = "AB_sp_GetCurrentDBDateDMY";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetCurrentDBTime")]
        public JsonResult GetCurrentDBTime()
        {
            string query = "AB_sp_GetCurrentDBTime";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        // For Login through mobile
        [HttpGet]
        [Route("SendOtp/{mobileNumber}")]
        public JsonResult SendOtp(string mobileNumber)
        {
            string otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
            if (mobileNumber != "null") { SmsSender.SendSmsText(MessageTemplate.GetOTPMsgSMS(otp), mobileNumber); }
            return new JsonResult(otp);
        }

        //For Mobile Verification
        [HttpGet]
        [Route("GetOTPMsgSMSVerifyMobile/{mobileNumber}")]
        public JsonResult SendOtpToVerifyMobileNo(string mobileNumber)
        {
            string otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
            if (mobileNumber != "null") { SmsSender.SendSmsText(MessageTemplate.GetOTPMsgSMSVerifyMobile(otp), mobileNumber); }
            return new JsonResult(otp);
        }

        // For Login through Email
        [HttpGet]
        [Route("SendOtpToEmail/{email}")]
        public JsonResult SendOtpToEmail(string email)
        {
            string otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
            MailSender.SendEmailWithOtp(MessageTemplate.SubGetOTPMsg, MessageTemplate.GetOTPMsgMail(otp), email);
            return new JsonResult(otp);
        }

        //For Email Verification
        [HttpGet]
        [Route("GetOTPMsgMailVerifyEmail/{email}")]
        public JsonResult GetOTPMsgMailVerifyEmail(string email)
        {
            string otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
            MailSender.SendEmailWithOtp(MessageTemplate.SubGetOTPMsg, MessageTemplate.GetOTPMsgMailVerifyEmail(otp), email);
            return new JsonResult(otp);
        }

        // For Login through mobile and Email
        [HttpGet]
        [Route("SendOtpToMobileAndEmail")]
        public JsonResult SendOtpToMobileAndEmail(string m, string e)
        {
            string otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
            string response = SmsSender.SendSms(otp, m);

            if (e != "null") { MailSender.SendEmailWithOtp(MessageTemplate.SubGetOTPMsg, MessageTemplate.GetOTPMsgMail(otp), e); }
            return new JsonResult(response);
        }

        //Change Mobile No.
        [HttpGet]
        [Route("SendOtpToMobleToChangeMobileNo/{mobileNumber}")]
        public JsonResult SendOtpToMobleToChangeMobileNo(string mobileNumber)
        {
            string otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
            if (mobileNumber != "null") { SmsSender.SendSmsText(MessageTemplate.GetOTPMsgCMSMS(otp), mobileNumber); }
            return new JsonResult(otp);
        }

        //Change EmailId
        [HttpGet]
        [Route("SendOtpToEmailToChangeEmailId/{email}")]
        public JsonResult SendOtpToEmailToChangeEmailId(string email)
        {
            string otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
            MailSender.SendEmailWithOtp(MessageTemplate.SubGetOTPMsgCE, MessageTemplate.GetOTPMsgCEMail(otp), email);
            return new JsonResult(otp);
        }

        // Change Login Password
        [HttpGet]
        [Route("SendOtpToMobileAndEmailToChangePW")]
        public JsonResult SendOtpToMobileAndEmailToChangePW(string m, string e)
        {
            string otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
            if (m != "null") { SmsSender.SendSmsText(MessageTemplate.GetOTPMsgCPSMS(otp), m); ; }
            if (e != "null") { MailSender.SendEmailWithOtp(MessageTemplate.SubGetOTPMsgCP, MessageTemplate.GetOTPMsgCPMail(otp), e); }
            return new JsonResult(otp);
        }

        [HttpGet]
        [Route("SendOtpToMobileOrEmailByUNorMobileNo/{vUserNameORMobileNo}/{isOTPOnMobile}/{isOTPOnEmail}")]
        public JsonResult SendOtpToMobileOrEmailByUNorMobileNo(string vUserNameORMobileNo, bool isOTPOnMobile, bool isOTPOnEmail)
        {
            string query = "DM_sp_GetMobileNoAndEmailIdByUNorMobileNo";
            string otp;
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vUserNameORMobileNo", vUserNameORMobileNo);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            if (table.Rows.Count > 0)
            {
                if (isOTPOnEmail)
                {
                    if (table.Rows[0]["vEmailId"].ToString().Trim() == "" || table.Rows[0]["vEmailId"].ToString() == DBNull.Value.ToString())
                    {
                        return new JsonResult(new
                        {
                            statusCode = 0,
                            statusMsg = "Email Id not found. Please add your Email Id to  receive communications from DCPL APP. Inconvenience caused is deeply regretted."
                        });
                    }
                }
                if (isOTPOnMobile)
                {
                    if (table.Rows[0]["vMobileNo"].ToString().Trim() == "" || table.Rows[0]["vMobileNo"].ToString() == DBNull.Value.ToString())
                    {
                        return new JsonResult(new
                        {
                            statusCode = 0,
                            statusMsg = "Mobile Number not found. Please add your Mobile Number to receive communications from DCPL APP. Inconvenience caused is deeply regretted."
                        });
                    }
                }
                string emailId = table.Rows[0]["vEmailId"].ToString();
                string mobileNumber = table.Rows[0]["vMobileNo"].ToString();
                otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
                if (isOTPOnMobile) { SmsSender.SendSms(otp, mobileNumber); }
                if (isOTPOnEmail) { MailSender.SendEmailWithOtp(MessageTemplate.SubGetOTPMsg, MessageTemplate.GetOTPMsgMail(otp), emailId); }
                var result = new
                {
                    statusCode = 1,
                    userData = table,
                    otp = otp
                };
                return new JsonResult(result);
            }
            else
            {
                //"Not_Found"
                return new JsonResult(new
                {
                    statusCode = 0,
                    statusMsg = "User Id OR Mobile No. not Found."
                }); ;
            }
        }

        [HttpGet]
        [Route("SendOtpToMobileOrEmailByUNorMobileNoFP/{vUserNameORMobileNo}/{isOTPOnMobile}/{isOTPOnEmail}")]
        public JsonResult SendOtpToMobileOrEmailByUNorMobileNoFP(string vUserNameORMobileNo, bool isOTPOnMobile, bool isOTPOnEmail)
        {
            string query = "DM_sp_GetMobileNoAndEmailIdByUNorMobileNo";
            string otp;
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vUserNameORMobileNo", vUserNameORMobileNo);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            if (table.Rows.Count > 0)
            {
                if (isOTPOnEmail)
                {
                    if (table.Rows[0]["vEmailId"].ToString().Trim() == "" || table.Rows[0]["vEmailId"].ToString() == DBNull.Value.ToString())
                    {
                        return new JsonResult(new
                        {
                            statusCode = 0,
                            statusMsg = "Email Id not found. Please add your Email Id to  receive communications from DCPL APP. Inconvenience caused is deeply regretted."
                        });
                    }
                }
                if (isOTPOnMobile)
                {
                    if (table.Rows[0]["vMobileNo"].ToString().Trim() == "" || table.Rows[0]["vMobileNo"].ToString() == DBNull.Value.ToString())
                    {
                        return new JsonResult(new
                        {
                            statusCode = 0,
                            statusMsg = "Mobile Number not found. Please add your Mobile Number to receive communications from DCPL APP. Inconvenience caused is deeply regretted."
                        });
                    }
                }
                string emailId = table.Rows[0]["vEmailId"].ToString();
                string mobileNumber = table.Rows[0]["vMobileNo"].ToString();
                otp = RandomCodeGenerator.GetRandomCode(4, RandomCodeGenerator.CodeType.Otp);
                if (isOTPOnMobile) { SmsSender.SendSmsText(MessageTemplate.GetOTPMsgCPSMS(otp), mobileNumber); }
                if (isOTPOnEmail) { MailSender.SendEmailWithOtp(MessageTemplate.SubGetOTPMsgCP, MessageTemplate.GetOTPMsgCPMail(otp), emailId); }
                var result = new
                {
                    statusCode = 1,
                    userData = table,
                    otp = otp
                };
                return new JsonResult(result);
            }
            else
            {
                //"Not_Found"
                return new JsonResult(new
                {
                    statusCode = 0,
                    statusMsg = "User Id OR Mobile No. not Found."
                }); ;
            }
        }
    }
}
