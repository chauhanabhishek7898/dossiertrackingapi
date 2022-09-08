﻿using Microsoft.AspNetCore.Mvc;
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
    public class UserMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetMobileNoAndEmailIdOfUser/{nUserId}")]
        public JsonResult GetMobileNoAndEmailIdOfUser(int nUserId)
        {
            string query = "DM_sp_GetMobileNoAndEmailIdOfUser";
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
        [Route("GetUserDetailsByUserId/{nUserId}/{vDeviceId}")]
        public JsonResult GetUserDetailsByUserId(int nUserId, string vDeviceId)
        {
            string query = "DM_sp_GetUserDetailsByUserId";
            if (vDeviceId == "null") { vDeviceId = null; }
            DataTable table = new DataTable(); string token = string.Empty;
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nUserId", nUserId); myCommand.Parameters.AddWithValue("vDeviceId", vDeviceId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                    if (table.Rows.Count > 0)
                    {
                        token = JwtTokenGenerater.GetJsonWebToken(nUserId.ToString(), _configuration);
                    }
                }
            }
            return new JsonResult(new
            {
                data = table,
                jwtToken = token
            }
           );
        }

        //CheckExistsMobileNo
        [HttpGet]
        [Route("CheckExistsMobileNo/{vMobileNo}")]
        public JsonResult CheckExistsMobileNo(string vMobileNo)
        {
            string query = "DM_sp_CheckExistsMobileNoOfUser";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vMobileNo", vMobileNo);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            if (table.Rows.Count > 0) { return new JsonResult("Mobile No. already registered."); }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetUserDetailsUsingUNandPW/{vUserName}/{vPassword}/{vDeviceId}")]
        public JsonResult GetUserDetailsUsingUNandPW(string vUserName, string vPassword, string vDeviceId)
        {
            string query = "DM_sp_GetUserDetailsUsingUNandPW";
            if (vDeviceId == "null") { vDeviceId = null; }
            string token = string.Empty; DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vUserName", vUserName);
                    myCommand.Parameters.AddWithValue("vPassword", vPassword);
                    myCommand.Parameters.AddWithValue("vDeviceId", vDeviceId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                    if (table.Rows.Count > 0)
                    {
                        token = JwtTokenGenerater.GetJsonWebToken(vUserName, _configuration);
                    }
                }
            }
            return new JsonResult(new
            {
                data = table,
                jwtToken = token
            });
        }

        [HttpGet]
        [Route("CheckUsersOldPassword/{vPassword}/{nUserId}")]
        public JsonResult CheckUsersOldPassword(string vPassword, int nUserId)
        {
            string query = "DM_sp_CheckUsersOldPassword";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vPassword", vPassword);
                    myCommand.Parameters.AddWithValue("nUserId", nUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            if (table.Rows.Count == 0) { return new JsonResult("Old Password do not match. Please try again."); }
            return new JsonResult(table);
        }

        [HttpPut]
        [Route("UserMobileNo_Update")]
        public JsonResult UserMobileNo_Update(UserMaster CM)
        {
            try
            {
                string query = "DM_sp_UserMobileNo_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nUserId", CM.nUserId);
                        myCommand.Parameters.AddWithValue("vMobileNo", CM.vMobileNo);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Mobile No. Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("UserEmailId_Update")]
        public JsonResult UserEmailId_Update(UserMaster CM)
        {
            try
            {
                string query = "DM_sp_UserEmailId_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nUserId", CM.nUserId);
                        myCommand.Parameters.AddWithValue("vEmailId", CM.vEmailId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("EmailId Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("UserChangePassword_Update")]
        public JsonResult UserChangePassword_Update(UserMaster CM)
        {
            try
            {
                string query = "DM_sp_UserChangePassword_Update";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nUserId", CM.nUserId);
                        myCommand.Parameters.AddWithValue("vPassword", CM.vPassword);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("UserMasterForgetPassword")]
        public JsonResult UserMasterForgetPassword(UserMaster CM)
        {
            try
            {
                string query = "DM_sp_UserMasterForgetPassword";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nRoleId", CM.nRoleId);
                        myCommand.Parameters.AddWithValue("vMobileNoOrEmailId", CM.vMobileNoOrEmailId);
                        myCommand.Parameters.AddWithValue("vPassword", CM.vPassword);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetProfileDetails/{nUserId}")]
        public JsonResult GetProfileDetails(int nUserId)
        {
            string query = "DM_sp_GetProfileDetails";
            DataSet DS = new DataSet(); DataTable TAB1 = new DataTable(); DataTable TAB2 = new DataTable(); TAB1.TableName = "TAB1"; TAB2.TableName = "TAB2";
            DS.Tables.Add(TAB1); DS.Tables.Add(TAB2);
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nUserId", nUserId);
                    myReader = myCommand.ExecuteReader(); DS.Load(myReader, LoadOption.OverwriteChanges, TAB1, TAB2); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(DS);
        }

        [HttpPost]
        [Route("AdminSignUp_Insert")]
        public JsonResult AdminSignUp_Insert(UserMaster CM)
        {
            try
            {
                string query = "DM_sp_AdminSignUp_Insert";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("vFullName", CM.vFullName);
                        myCommand.Parameters.AddWithValue("vPassword", CM.vPassword);
                        myCommand.Parameters.AddWithValue("vMobileNo", CM.vMobileNo);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Added Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetUsersDetails_Report")]
        public JsonResult GetUsersDetails_Report()
        {
            string query = "DM_sp_GetUsersDetails_Report";
            string token = string.Empty; DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut]
        [Route("ActivateRevokeRightsOfUser")]
        public JsonResult ActivateRevokeRightsOfUser(UserMaster CM)
        {
            try
            {
                string query = "DM_sp_ActivateRevokeRightsOfUser";
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
                return new JsonResult("Rights Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
