using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using TrackingAPI.Models;
using TrackingAPI.Service;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TrackingAPI.Utilities;
using TrackingAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Razorpay.Api;

namespace TrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPushNotificationService _pushNotificationService;
        private static IHubContext<TrackingHub> HubContext;
        public static IHubContext<TrackingHub> getHubContext
        {
            get
            {
                return HubContext;
            }
        }
        //private static IHubContext<ChatHub> HubContext;
        //public static IHubContext<ChatHub> getHubContext
        //{
        //    get
        //    {
        //        return HubContext;
        //    }
        //}
        public OrderDetailsController(IConfiguration configuration, IPushNotificationService pushNotificationService, IHubContext<TrackingHub> hubcontext)
        {
            _configuration = configuration;
            _pushNotificationService = pushNotificationService;
            HubContext = hubcontext;

        }

        [HttpGet]
        [Route("GetRates/{vCity}/{nR1KMs}/{nR2KMs}/{nR3KMs}")]
        public JsonResult GetRates(string vCity, string nR1KMs, string nR2KMs, string nR3KMs)
        {
            string query = "DM_sp_GetRates";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("vCity", vCity);
                    myCommand.Parameters.AddWithValue("nR1KMs", nR1KMs);
                    myCommand.Parameters.AddWithValue("nR2KMs", nR2KMs);
                    myCommand.Parameters.AddWithValue("nR3KMs", nR3KMs);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_O1_OrderDetails_Insert";
                DataSet DS = new DataSet(); DataTable TAB1 = new DataTable(); DataTable TAB2 = new DataTable(); TAB1.TableName = "TAB1"; TAB2.TableName = "TAB2";
                DS.Tables.Add(TAB1); DS.Tables.Add(TAB2); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
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
                        myCommand.Parameters.AddWithValue("vCustomerName1", CM.vCustomerName1);
                        myCommand.Parameters.AddWithValue("vCustomerMobileNo1", CM.vCustomerMobileNo1);
                        myCommand.Parameters.AddWithValue("vCustomerName2", CM.vCustomerName2);
                        myCommand.Parameters.AddWithValue("vCustomerMobileNo2", CM.vCustomerMobileNo2);
                        myCommand.Parameters.AddWithValue("vCustomerName3", CM.vCustomerName3);
                        myCommand.Parameters.AddWithValue("vCustomerMobileNo3", CM.vCustomerMobileNo3);
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
                        myCommand.Parameters.AddWithValue("vItemType", CM.vItemType);

                        myReader = myCommand.ExecuteReader(); DS.Load(myReader, LoadOption.OverwriteChanges, TAB1, TAB2); myReader.Close(); myCon.Close();

                        //await HubContext.Clients.All.SendAsync("BroadcastMessage", PMC.ChatDetails[0].nUserId, JsonConvert.SerializeObject(PMC.ChatDetails));
                        //await HubContext.Clients.All.SendAsync("BroadcastMessage");
                        // Firebases
                        List<string> devicesId = new List<string>();
                        foreach (DataRow row in TAB2.Rows)
                        {
                            devicesId.Add(row["vDeviceId"].ToString());
                        }
                        if (devicesId.Count > 0)
                        {
                            _pushNotificationService.SendNotificationToDrivers(TAB1, devicesId);
                        }

                    }
                }
                return new JsonResult(DS);
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPost]
        [Route("UploadTrackImage")]
        public async Task<JsonResult> UploadTrackImage(IFormFile PhotoFile)
        {
            try
            {
                string OrderDetailsPhotosJson = Request.Form["OrderDetailsPhotos"];
                OrderDetailsPhotosClass PMC = JsonConvert.DeserializeObject<OrderDetailsPhotosClass>(OrderDetailsPhotosJson);
                string CurrentDt = DateTime.Now.ToString("ddMMyyyyHHmm");
                if (PhotoFile != null)
                {
                    var vPhotoFilePath = $"assets//OrderDetailsPhotos//{PMC.OrderDetailsPhotos[0].nTrackId}//{CurrentDt}//PhotoFile//{PhotoFile.FileName}";
                    var response = await ImageUploader.SaveImage(PhotoFile, PMC.OrderDetailsPhotos[0].nTrackId, vPhotoFilePath);
                    PMC.OrderDetailsPhotos[0].vPhotoFilePath = vPhotoFilePath;
                }
                string query = "DM_sp_OrderDetailsPhotoTrack_Insert";

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
                return new JsonResult(table);
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPost]
        [Route("UploadImage")]
        public async Task<JsonResult> UploadImage(IFormFile PhotoFile)
        {
            try
            {
                string OrderDetailsPhotosJson = Request.Form["OrderDetailsPhotos"];
                OrderDetailsPhotosClass PMC = JsonConvert.DeserializeObject<OrderDetailsPhotosClass>(OrderDetailsPhotosJson);
                string CurrentDt = DateTime.Now.ToString("ddMMyyyyHHmm");
                if (PhotoFile != null)
                {
                    var vPhotoFilePath = $"assets//OrderDetailsPhotos//{PMC.OrderDetailsPhotos[0].nTrackId}//{CurrentDt}//PhotoFile//{PhotoFile.FileName}";
                    var response = await ImageUploader.SaveImage(PhotoFile, PMC.OrderDetailsPhotos[0].nTrackId, vPhotoFilePath);
                    PMC.OrderDetailsPhotos[0].vPhotoFilePath = vPhotoFilePath;
                }
                string query = "DM_sp_OrderDetailsPhoto_Insert";

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
                return new JsonResult(table);
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        [Route("GetOrderDetails_Pending/{nCityId}/{dtDated}/{nDriverUserId}")]
        public JsonResult GetOrderDetails_Pending(int nCityId, string dtDated, int nDriverUserId)
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
                    myCommand.Parameters.AddWithValue("nDriverUserId", nDriverUserId);
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
                        myCommand.Parameters.AddWithValue("vCancellationReason", CM.vCancellationReason);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Record Cancelled Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("PickOrderByDriver")]
        public async Task<JsonResult> PickOrderByDriver(OrderPickUp CM)
        {
            try
            {
                string query = "DM_sp_O4_PickOrderByDriver";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                    }
                }
                await HubContext.Clients.All.SendAsync("BookingAccepted", CM.nLoggedInUserId, JsonConvert.SerializeObject(table),CM.latitude,CM.longitude);
                return new JsonResult(table);
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
                        myCommand.Parameters.AddWithValue("vCancellationReason", CM.vCancellationReason);
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
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                    }
                }
                return new JsonResult(table);
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("UpdateOrder_PickUpPointStartTimeByDriver")]
        public async Task<JsonResult> UpdateOrder_PickUpPointStartTimeByDriver(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_O8_UpdateOrder_PickUpPointStartTimeByDriver";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                    }
                }
                await HubContext.Clients.All.SendAsync("PickupStarted", CM.nLoggedInUserId, JsonConvert.SerializeObject(table));
                return new JsonResult(table);
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
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("nLoggedInUserId", CM.nLoggedInUserId);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                    }
                }
                return new JsonResult(table);
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
        [Route("GetOrderDetailsForDrivers/{nDriverUserId}/{FromDt}/{ToDt}/{vGeneric}")]
        public JsonResult GetOrderDetailsForDrivers(int nDriverUserId, string FromDt, string ToDt, string vGeneric)
        {
            string query = "DM_sp_O93_GetOrderDetailsForDrivers";
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
        [Route("GetSavedaddresses/{nLoggedInUserId}")]
        public JsonResult GetSAvedAddresses(int nLoggedInUserId)
        {
            string query = "DM_sp_GetSavedAddresses";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nLoggedInUserId", nLoggedInUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetTop5SourceAddresses/{nLoggedInUserId}")]
        public JsonResult GetTop5SourceAddresses(int nLoggedInUserId)
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
        [Route("GetTop5DestinationAddresses/{nLoggedInUserId}")]
        public JsonResult GetTop5DestinationAddresses(int nLoggedInUserId)
        {
            string query = "DM_sp_GetTop5DestinationAddresses";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nLoggedInUserId", nLoggedInUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetUnPaidStatusofCustomer/{nLoggedInUserId}")]
        public JsonResult GetUnPaidStatusofCustomer(int nLoggedInUserId)
        {
            string query = "DM_sp_GetUnPaidStatusofCustomer";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nLoggedInUserId", nLoggedInUserId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet]
        [Route("GetOrderImages/{nTrackId}")]
        public JsonResult GetOrderImages(int nTrackId)
        {
            string query = "DM_sp_O91_GetOrderImages";
            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("nTrackId", nTrackId);
                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        // RazorPay Methods Start

        //decimal GetOrderFinalAmount(int nTrackId)
        //{
        //    try
        //    {
        //        string query = "DM_sp_GetOrderFinalAmount";
        //        DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
        //        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //        {
        //            myCon.Open();
        //            using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //            {
        //                myCommand.CommandType = CommandType.StoredProcedure;
        //                myCommand.Parameters.AddWithValue("nTrackId", nTrackId);
        //                myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
        //            }
        //        }
        //        var OrderAmount = Convert.ToDecimal(table.Rows[0]["nGrandTotal"]); return OrderAmount;
        //    }
        //    catch (Exception ex) { throw ex; }
        //}

        string CreateRazorPayOrderId(double OrderAmount)
        {
            RazorpayClient client = new RazorpayClient("rzp_test_dCJynUA3J6jM1d", "LpzdNika3CUmoy3lxy4yazLM");
            //RazorpayClient client = new RazorpayClient("rzp_live_kvPoPHM5OoecBq", "dBAdleY0Hu1y4rMRuEqEz8pG");
            Order order = new Order();
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", OrderAmount * 100); options.Add("currency", "INR"); order = client.Order.Create(options);
            var orderId = order.Attributes["id"];
            return orderId;
        }

        [HttpPut]
        [Route("OrdersPayment")]
        public JsonResult OrdersPayment(OrderDetailsPhotosClass ED)
        {
            try
            {
                string query = "DM_sp_OrdersPayment";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                var orderId = CreateRazorPayOrderId(ED.OrderDetails[0].nGrandTotal);
                ED.OrderDetails[0].vOrderId = orderId;
                var JsonInput = JsonConvert.SerializeObject(ED);
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;

                        myCommand.Parameters.AddWithValue("nTrackId", ED.OrderDetails[0].nTrackId);
                        myCommand.Parameters.AddWithValue("vOrderId", orderId);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                    }
                }
                var result = new { orderId = orderId, table = table, amount = ED.OrderDetails[0].nGrandTotal, nTrackId = ED.OrderDetails[0].nTrackId };
                return new JsonResult(result);
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("OrderDetail_PaymentSuccess")]
        public JsonResult OrderDetail_PaymentSuccess(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_OrderDetail_PaymentSuccess";
                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("vPaymentReferenceId", CM.vPaymentReferenceId);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();

                        //Send Email and Sms
                        string MailSubject = MessageTemplate.SubOrderPaymentSuccess;
                        string EmailText = MessageTemplate.OrderPaymentSuccessEmail(table.Rows[0]["vOrderId"].ToString(), table.Rows[0]["nAmount"].ToString());
                        string SMSText = MessageTemplate.OrderPaymentSuccessSMS(table.Rows[0]["vOrderId"].ToString(), table.Rows[0]["nAmount"].ToString());
                        MailSender.SendEmailText(MailSubject, EmailText, table.Rows[0]["CEmail"].ToString(), table.Rows[0]["vEmailIdOrg"].ToString());
                        SmsSender.SendSmsText(SMSText, table.Rows[0]["MobileNos"].ToString());
                    }
                }
                return new JsonResult("Payment Processed Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("OrderDetail_PaymentFailure")]
        public JsonResult OrderDetail_PaymentFailure(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_OrderDetail_PaymentFailure";

                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); int retValue = 0;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myCommand.Parameters.AddWithValue("vPaymentReferenceId", CM.vPaymentReferenceId);
                        retValue = myCommand.ExecuteNonQuery(); myCon.Close();
                    }
                }
                return new JsonResult("Payment Failure !!");
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpPut]
        [Route("OrdersPaymentReceivedByDriver")]
        public JsonResult OrdersPaymentReceivedByDriver(OrderDetails CM)
        {
            try
            {
                string query = "DM_sp_OrdersPaymentReceivedByDriver";

                DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("nTrackId", CM.nTrackId);
                        myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();

                        //Send Email and Sms
                        string MailSubject = MessageTemplate.SubOrderPaymentSuccess;
                        string EmailText = MessageTemplate.OrderPaymentSuccessEmail(table.Rows[0]["vOrderId"].ToString(), table.Rows[0]["nAmount"].ToString());
                        string SMSText = MessageTemplate.OrderPaymentSuccessSMS(table.Rows[0]["vOrderId"].ToString(), table.Rows[0]["nAmount"].ToString());
                        MailSender.SendEmailText(MailSubject, EmailText, table.Rows[0]["CEmail"].ToString(), table.Rows[0]["vEmailIdOrg"].ToString());
                        SmsSender.SendSmsText(SMSText, table.Rows[0]["MobileNos"].ToString());
                    }
                }
                return new JsonResult("Payment Processed Successfully !!");
            }
            catch (Exception ex) { throw ex; }
        }

        // RazorPay Methods End

    }

    public class OrderDetailsPhotosClass
    {
        public List<OrderDetails> OrderDetails { get; set; }
        public List<OrderDetailsPhotos> OrderDetailsPhotos { get; set; }
    }
    public class OrderPickUp
    { 
        public int nTrackId { get; set; }
        public int nLoggedInUserId { get; set; }
        public string latitude { get; set; }
        public string longitude { get;set; }



    }
}