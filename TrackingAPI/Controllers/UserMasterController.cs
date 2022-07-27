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
    public class UserMasterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetMobileNoAndEmailId/{nUserId}")]
        public JsonResult GetMobileNoAndEmailId(int nUserId)
        {
            string query = "DM_sp_GetMobileNoAndEmailId";
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
        [Route("GetUserDetailsByUserId/{nUserId}")]
        public JsonResult GetUserDetailsByUserId(int nUserId)
        {
            string query = "DM_sp_GetUserDetailsByUserId";
            DataTable table = new DataTable(); string token = string.Empty;
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nUserId", nUserId);
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
        [Route("CheckExistsMobileNo/{vMobileNo}/{nRoleId}")]
        public JsonResult CheckExistsMobileNo(string vMobileNo, int nRoleId)
        {
            string query = "DM_sp_CheckExistsMobileNo";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vMobileNo", vMobileNo);
                    myCommand.Parameters.AddWithValue("nRoleId", nRoleId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            if (table.Rows.Count > 0) { return new JsonResult("Mobile No. already registered."); }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetUserDetailsUsingUNandPWUsingUserName/{vUserNameOrMemberCode}/{vPassword}")]
        public JsonResult GetUserDetailsUsingUNandPWUsingUserName(string vUserNameOrMemberCode, string vPassword)
        {
            string query = "DM_sp_GetUserDetailsUsingUNandPWUsingUserName";
            string token = string.Empty; DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vUserNameOrMemberCode", vUserNameOrMemberCode);
                    myCommand.Parameters.AddWithValue("vPassword", vPassword);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                    if (table.Rows.Count > 0)
                    {
                        token = JwtTokenGenerater.GetJsonWebToken(vUserNameOrMemberCode, _configuration);
                    }
                }
            }
            return new JsonResult(new
            {
                data = table,
                jwtToken = token
            });
        }


    }
}
