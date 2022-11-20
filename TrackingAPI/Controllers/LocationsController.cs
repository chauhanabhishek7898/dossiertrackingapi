using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using TrackingAPI.Hubs;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace TrackingAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private static IHubContext<TrackingHub> HubContext;
        private readonly IConfiguration _configuration;
        public static IHubContext<TrackingHub> getHubContext
        {
            get
            {
                return HubContext;
            }
        }

        public LocationsController(IHubContext<TrackingHub> hubcontext, IConfiguration configuration)
        {
            HubContext = hubcontext;
            _configuration = configuration;
        }
        [HttpGet]

        public string Get()
        {
            return "hello";
        }
        //[HttpPost]
        //public async  Task Post()
        //{
        //    try
        //    {
        //        var rawRequestBody = await Request.GetRawBodyAsync();
        //        //JsonSerializerOptions options = new JsonSerializerOptions()
        //        //{
        //        //    ReferenceHandler = ReferenceHandler.IgnoreCycles,
        //        //    WriteIndented = true
        //        //};
        //        var data = System.Text.Json.JsonSerializer.Deserialize<Root>(rawRequestBody);
        //        if (data.location.Count > 0)
        //        {
        //            var userId = data.user_id;
        //           // int count = data.location.Count;
        //            var lat = data.location[0].coords.latitude;
        //            var longt = data.location[0].coords.longitude;
        //            var speed= data.location[0].coords.speed;
        //            var time = data.location[0].timestamp;
        //            //string query = "DM_sp_GetUserIdsForDriver";
        //            string query = "DM_sp_UpdateDriver_CurrentLocation";
        //            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
        //            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //            {
        //                //myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //                //{
        //                //    myCommand.CommandType = CommandType.StoredProcedure;
        //                //    myCommand.Parameters.AddWithValue("nDriverUserId", userId);
        //                //    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
        //                //}
        //                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //                {
        //                    myCommand.CommandType = CommandType.StoredProcedure;
        //                    myCommand.Parameters.AddWithValue("nDriverUserId", userId);
        //                    myCommand.Parameters.AddWithValue("vDiriverCurrentLat", lat.ToString());
        //                    myCommand.Parameters.AddWithValue("vDiriverCurrentLong", longt.ToString());
        //                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
        //                }
        //            }
        //            await HubContext.Clients.All.SendAsync("CurrentLocationHub", userId, "", JsonConvert.SerializeObject(new {
        //            lat= lat,
        //            lng=longt,
        //            speed= speed,
        //            time=time
        //            }));
        //        }
        //    }
        //    catch (Exception ex)
        //    { 

        //    }
        //    //var res = json;
        //}

        //[HttpPost]
        //public async Task Post()
        //{
        //    try
        //    {
        //        var rawRequestBody = await Request.GetRawBodyAsync();
        //        //JsonSerializerOptions options = new JsonSerializerOptions()
        //        //{
        //        //    ReferenceHandler = ReferenceHandler.IgnoreCycles,
        //        //    WriteIndented = true
        //        //};
        //        var data = System.Text.Json.JsonSerializer.Deserialize<Root>(rawRequestBody);
        //        if (data.location!=null)
        //        {
        //            var userId = data.user_id;
        //            // int count = data.location.Count;
        //            var lat = data.location.coords.latitude;
        //            var longt = data.location.coords.longitude;
        //            var speed = data.location.coords.speed;
        //            var time = data.location.timestamp;
        //            //string query = "DM_sp_GetUserIdsForDriver";
        //            string query = "DM_sp_UpdateDriver_CurrentLocation";
        //            DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
        //            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //            {
        //                //myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //                //{
        //                //    myCommand.CommandType = CommandType.StoredProcedure;
        //                //    myCommand.Parameters.AddWithValue("nDriverUserId", userId);
        //                //    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
        //                //}
        //                myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //                {
        //                    myCommand.CommandType = CommandType.StoredProcedure;
        //                    myCommand.Parameters.AddWithValue("nDriverUserId", 25);
        //                    myCommand.Parameters.AddWithValue("vDiriverCurrentLat", lat.ToString());
        //                    myCommand.Parameters.AddWithValue("vDiriverCurrentLong", longt.ToString());
        //                    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
        //                }
        //            }
        //            await HubContext.Clients.All.SendAsync("CurrentLocationHub", userId, "", JsonConvert.SerializeObject(new
        //            {
        //                lat = lat,
        //                lng = longt,
        //                speed = speed,
        //                time = time
        //            }));
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    //var res = json;
        //}
        [HttpPost]
        public async Task LocationUpdate(DriverLocation driverLocation)
        {
            try
            {
                //var rawRequestBody = await Request.GetRawBodyAsync();
                //JsonSerializerOptions options = new JsonSerializerOptions()
                //{
                //    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                //    WriteIndented = true
                //};
               // var data = System.Text.Json.JsonSerializer.Deserialize<Root>(rawRequestBody);
                if (driverLocation != null)
                {
                    var userId = driverLocation.userId;
                    // int count = data.location.Count;
                    var lat = driverLocation.lat;
                    var longt = driverLocation.lng;
                   
                    //string query = "DM_sp_GetUserIdsForDriver";
                    string query = "DM_sp_UpdateDriver_CurrentLocation";
                    DataTable table = new DataTable(); string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon"); SqlDataReader myReader;
                    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                    {
                        //myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                        //{
                        //    myCommand.CommandType = CommandType.StoredProcedure;
                        //    myCommand.Parameters.AddWithValue("nDriverUserId", userId);
                        //    myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                        //}
                        myCon.Open(); using (SqlCommand myCommand = new SqlCommand(query, myCon))
                        {
                            myCommand.CommandType = CommandType.StoredProcedure;
                            myCommand.Parameters.AddWithValue("nDriverUserId", userId);
                            myCommand.Parameters.AddWithValue("vDiriverCurrentLat", lat.ToString());
                            myCommand.Parameters.AddWithValue("vDiriverCurrentLong", longt.ToString());
                            myReader = myCommand.ExecuteReader(); table.Load(myReader); myReader.Close(); myCon.Close();
                        }
                    }
                    await HubContext.Clients.All.SendAsync("CurrentLocationHub", userId, "", JsonConvert.SerializeObject(new
                    {
                        lat = lat,
                        lng = longt
                        
                    }));
                }
            }
            catch (Exception ex)
            {

            }
            //var res = json;
        }

    }



    static class BodyTransformation
    {

       public static async Task<string> GetRawBodyAsync(
       this HttpRequest request,
       Encoding encoding = null)
        {
            if (!request.Body.CanSeek)
            {               
                request.EnableBuffering();
            }

            request.Body.Position = 0;

            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);

            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            request.Body.Position = 0;

            return body;
        }
    }
    public class Activity
    {
       // public string type { get; set; }
       // public int confidence { get; set; }
    }

    public class Battery
    {
        //public bool is_charging { get; set; }
        //public int level { get; set; }
    }

    public class Coords
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
      //  public int accuracy { get; set; }
        public double speed { get; set; }
       // public int speed_accuracy { get; set; }
       // public int heading { get; set; }
       // public int heading_accuracy { get; set; }
        public int altitude { get; set; }
        public int altitude_accuracy { get; set; }
    }

    public class Extras
    {
    }

    public class Location
    {
       // public string @event { get; set; }
        public bool is_moving { get; set; }
        public string uuid { get; set; }
        public DateTime timestamp { get; set; }
        public double odometer { get; set; }
        public Coords coords { get; set; }
        public Activity activity { get; set; }
        public Battery battery { get; set; }
        public Extras extras { get; set; }
    }

    public class Root
    {
       // public List<Location> location { get; set; }
        public Location location { get; set; }
        public int user_id { get; set; }
        public string device_id { get; set; }
    }
    public class DriverLocation
    {
        // public List<Location> location { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string userId { get; set; }
    }

}
