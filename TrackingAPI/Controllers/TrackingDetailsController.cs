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
    public class TrackingDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TrackingDetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(TrackingDetails CM)
        {
            try
            {
                string query = "DM_sp_1_TrackingDetails_Insert";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("dtDated", CM.dtDated);
                        myCommand.Parameters.AddWithValue("vTime", CM.vTime);
                        myCommand.Parameters.AddWithValue("nCustomerUserId", CM.nCustomerUserId);
                        myCommand.Parameters.AddWithValue("nCityId", CM.nCityId);
                        myCommand.Parameters.AddWithValue("vFromLocation", CM.vFromLocation);
                        myCommand.Parameters.AddWithValue("vFromLocationLat", CM.vFromLocationLat);
                        myCommand.Parameters.AddWithValue("vFromLocationLong", CM.vFromLocationLong);
                        myCommand.Parameters.AddWithValue("vToLocation", CM.vToLocation);
                        myCommand.Parameters.AddWithValue("vToLocationLat", CM.vToLocationLat);
                        myCommand.Parameters.AddWithValue("vToLocationLong", CM.vToLocationLong);
                        myCommand.Parameters.AddWithValue("nKMs", CM.nKMs);
                        myCommand.Parameters.AddWithValue("nRate", CM.nRate);
                        myCommand.Parameters.AddWithValue("nTotalRate", CM.nTotalRate);
                        myCommand.Parameters.AddWithValue("vRemarks", CM.vRemarks);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Added Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetTrackingDetails_Pending/{nCityId}/{dtDated}")]
        public JsonResult GetTrackingDetails_Pending(int nCityId, string dtDated)
        {
            string query = "DM_sp_2_GetTrackingDetails_Pending";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nCityId", nCityId);
                    myCommand.Parameters.AddWithValue("dtDated", dtDated);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut]
        [Route("CancelTrackByCustomerOrAdmin")]
        public JsonResult CancelTrackByCustomerOrAdmin(TrackingDetails CM)
        {
            try
            {
                string query = "DM_sp_3_CancelTrackByCustomerOrAdmin";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        myCommand.Parameters.AddWithValue("vRemarks", CM.vRemarks);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("PickTrackByDriver")]
        public JsonResult PickTrackByDriver(TrackingDetails CM)
        {
            try
            {
                string query = "DM_sp_4_PickTrackByDriver";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("CancelTrackByDriver")]
        public JsonResult CancelTrackByDriver(TrackingDetails CM)
        {
            try
            {
                string query = "DM_sp_5_CancelTrackByDriver";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetTrackInProgessToUpdateOrCancel_ByDriver/{nDriverUserId}")]
        public JsonResult GetTrackInProgessToUpdateOrCancel_ByDriver(int nDriverUserId)
        {
            string query = "DM_sp_6_GetTrackInProgessToUpdateOrCancel_ByDriver";
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
        [Route("UpdateTrack_PickUpPointReachedTimeByDriver")]
        public JsonResult UpdateTrack_PickUpPointReachedTimeByDriver(TrackingDetails CM)
        {
            try
            {
                string query = "DM_sp_7_UpdateTrack_PickUpPointReachedTimeByDriver";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("UpdateTrack_PickUpPointStartTimeByDriver")]
        public JsonResult UpdateTrack_PickUpPointStartTimeByDriver(TrackingDetails CM)
        {
            try
            {
                string query = "DM_sp_8_UpdateTrack_PickUpPointStartTimeByDriver";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("UpdateTrack_DestinationReachedEndTimeByDriver")]
        public JsonResult UpdateTrack_DestinationReachedEndTimeByDriver(TrackingDetails CM)
        {
            try
            {
                string query = "DM_sp_9_UpdateTrack_DestinationReachedEndTimeByDriver";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Updated Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetTrackingDetailsForCustomers/{nDriverUserId}/{FromDt}/{ToDt}/{vGeneric}")]
        public JsonResult GetTrackingDetailsForCustomers(int nCustomerUserId, string FromDt, string ToDt, string vGeneric)
        {
            string query = "DM_sp_10_GetTrackingDetailsForCustomers";
            if (vGeneric == "null") { vGeneric = null; }
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nCustomerUserId", nCustomerUserId);
                    myCommand.Parameters.AddWithValue("FromDt", FromDt); myCommand.Parameters.AddWithValue("ToDt", ToDt);
                    myCommand.Parameters.AddWithValue("vGeneric", vGeneric);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetTrackingDetailsForAdmin/{FromDt}/{ToDt}/{vGeneric}")]
        public JsonResult GetTrackingDetailsForAdmin(string FromDt, string ToDt, string vGeneric)
        {
            string query = "DM_sp_11_GetTrackingDetailsForAdmin";
            if (vGeneric == "null") { vGeneric = null; }
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("FromDt", FromDt); myCommand.Parameters.AddWithValue("ToDt", ToDt);
                    myCommand.Parameters.AddWithValue("vGeneric", vGeneric);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
