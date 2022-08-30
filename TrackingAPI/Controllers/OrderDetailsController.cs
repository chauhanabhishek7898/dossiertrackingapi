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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public OrderDetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public JsonResult Post(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_O1_OrderDetails_Insert";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("dtDated", CM.dtDated);
                        myCommand.Parameters.AddWithValue("vTime", CM.vTime);
                        myCommand.Parameters.AddWithValue("nCustomerUserId", CM.nCustomerUserId);
                        myCommand.Parameters.AddWithValue("vCity", CM.vCity);
                        myCommand.Parameters.AddWithValue("nR1KMs", CM.nR1KMs);
                        myCommand.Parameters.AddWithValue("nR2KMs", CM.nR2KMs);
                        myCommand.Parameters.AddWithValue("nR3KMs", CM.nR3KMs);
                        myCommand.Parameters.AddWithValue("vSource", CM.vSource);
                        myCommand.Parameters.AddWithValue("vSourceLat", CM.vSourceLat);
                        myCommand.Parameters.AddWithValue("vSourceLong", CM.vSourceLong);
                        myCommand.Parameters.AddWithValue("vSourceAddress", CM.vSourceAddress);
                        myCommand.Parameters.AddWithValue("vD1", CM.vD1);
                        myCommand.Parameters.AddWithValue("vD1Lat", CM.vD1Lat);
                        myCommand.Parameters.AddWithValue("vD1Long", CM.vD1Long);
                        myCommand.Parameters.AddWithValue("vD1Address", CM.vD1Address);
                        myCommand.Parameters.AddWithValue("vD2", CM.vD2);
                        myCommand.Parameters.AddWithValue("vD2Lat", CM.vD2Lat);
                        myCommand.Parameters.AddWithValue("vD2Long", CM.vD2Long);
                        myCommand.Parameters.AddWithValue("vD2Address", CM.vD2Address);
                        myCommand.Parameters.AddWithValue("vD3", CM.vD3);
                        myCommand.Parameters.AddWithValue("vD3Lat", CM.vD3Lat);
                        myCommand.Parameters.AddWithValue("vD3Long", CM.vD3Long);
                        myCommand.Parameters.AddWithValue("vD3Address", CM.vD3Address);

                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Added Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetOrderDetails_Pending/{nCityId}/{dtDated}")]
        public JsonResult GetOrderDetails_Pending(int nCityId, string dtDated)
        {
            string query = "DM_sp_O2_GetOrderDetails_Pending";
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
        [Route("CancelOrderByCustomerOrAdmin")]
        public JsonResult CancelOrderByCustomerOrAdmin(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_O3_CancelOrderByCustomerOrAdmin";
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
                return new JsonResult("Record Cancelled Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("PickOrderByDriver")]
        public JsonResult PickOrderByDriver(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_O4_PickOrderByDriver";
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
                return new JsonResult("Order Picked by Driver Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("CancelOrderByDriver")]
        public JsonResult CancelOrderByDriver(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_O5_CancelOrderByDriver";
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
                return new JsonResult("Order Cancelled by Driver Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetOrderInProgessToUpdateOrCancel_ByDriver/{nDriverUserId}")]
        public JsonResult GetOrderInProgessToUpdateOrCancel_ByDriver(int nDriverUserId)
        {
            string query = "DM_sp_O6_GetOrderInProgessToUpdateOrCancel_ByDriver";
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
        [Route("UpdateOrder_PickUpPointReachedTimeByDriver")]
        public JsonResult UpdateOrder_PickUpPointReachedTimeByDriver(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_O7_UpdateOrder_PickUpPointReachedTimeByDriver";
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
        [Route("UpdateOrder_PickUpPointStartTimeByDriver")]
        public JsonResult UpdateOrder_PickUpPointStartTimeByDriver(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_O8_UpdateOrder_PickUpPointStartTimeByDriver";
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
        [Route("UpdateOrder_DestinationReachedEndTimeByDriver")]
        public JsonResult UpdateOrder_DestinationReachedEndTimeByDriver(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_O9_UpdateOrder_DestinationReachedEndTimeByDriver";
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
        [Route("GetOrderDetailsForCustomers/{nCustomerUserId}/{FromDt}/{ToDt}/{vGeneric}")]
        public JsonResult GetOrderDetailsForCustomers(int nCustomerUserId, string FromDt, string ToDt, string vGeneric)
        {
            string query = "DM_sp_O91_GetOrderDetailsForCustomers";
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
        [Route("GetOrderDetailsForAdmin/{FromDt}/{ToDt}/{vGeneric}")]
        public JsonResult GetOrderDetailsForAdmin(string FromDt, string ToDt, string vGeneric)
        {
            string query = "DM_sp_O92_GetOrderDetailsForAdmin";
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

        [HttpGet]
        [Route("GetSavedaddresses/{@nLoggedInUserId}")]
        public JsonResult GetSAvedAddresses(int @nLoggedInUserId)
        {
            string query = "DM_sp_GetSavedAddresses";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@nLoggedInUserId", @nLoggedInUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetTop5SourceAddresses/{@nLoggedInUserId}")]
        public JsonResult GetTop5SourceAddresses(int @nLoggedInUserId)
        {
            string query = "DM_sp_GetTop5SourceAddresses";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@nLoggedInUserId", @nLoggedInUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetTop5DestinationAddresses/{@nLoggedInUserId}")]
        public JsonResult GetTop5DestinationAddresses(int @nLoggedInUserId)
        {
            string query = "DM_sp_GetTop5DestinationAddresses";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@nLoggedInUserId", @nLoggedInUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetUnPaidStatusofCustomer/{@nLoggedInUserId}")]
        public JsonResult GetUnPaidStatusofCustomer(int @nLoggedInUserId)
        {
            string query = "DM_sp_GetUnPaidStatusofCustomer";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@nLoggedInUserId", @nLoggedInUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

    }
}