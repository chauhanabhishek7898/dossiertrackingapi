using TrackingAPI.Controllers;
using TrackingAPI.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace TrackingAPI.Service
{
    //https://firebase.google.com/docs/cloud-messaging/android/device-group
    public class PushNotificationService : IPushNotificationService
    {
        private readonly IConfiguration _configuration;

        public PushNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task SendNotificationToDrivers(DataTable orderDetails,List<String> deviceList)
        {
            var anony_object = new
            {
                registration_ids= deviceList,
                //to = "cLvlh6eYRDyI6ikojiCaOR:APA91bH-1uAyo9IPICESywQnnCgkQs-VImexpfs1-fiKTTJ_sEhnM6HgfLyegC6HCHxOm91vouPh1IVoedIwB1H1Ddr8IKApCkhOydDTklq36vNc4B3GgrxZTS9PGrjBoru4W0ZS7dmH",
                collapse_key = "type_a",
                notification = new
                {
                    body = orderDetails.Rows[0]["CEmail"],
                    title = "Title of Your Notification"
                },
                data = new
                {
                    body = orderDetails.Rows[0]["CEmail"],
                    title = "Title of Your Notification in Title",
                    //key_1 = "Value for key_1",
                    //key_2 = "Value for key_2"
                }
            };
            var message = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_configuration["Firebase:baseurl"]),
                Headers =
                            {
                                { HttpRequestHeader.ContentType.ToString(), "application/json" }
                               // { HttpRequestHeader.Authorization.ToString(), "key=AAAAIf5D3Og:APA91bHznPagFn5ixyXmPs8nBATllGy_dlR__kSsbBJUVfRhmwc8Tfb5yU4ggi-idombjzZBKNPTdeRlP33maVECmcaKGoCfBOmfag76qzyCY9zYvRD_C33dBjjahkB8CAbb30AARLHX" }
                            },
                Content = new StringContent(JsonConvert.SerializeObject(anony_object), Encoding.UTF8, "application/json")
            };
            string serverKey =  _configuration["Firebase:serverKey"].ToString();
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={serverKey}");
            await client.SendAsync(message); ;
        }

        public async Task SendNotificationToDrivers(string msg, List<String> deviceList)
        {
            var anony_object = new
            {
                registration_ids = deviceList,
                //to = "cLvlh6eYRDyI6ikojiCaOR:APA91bH-1uAyo9IPICESywQnnCgkQs-VImexpfs1-fiKTTJ_sEhnM6HgfLyegC6HCHxOm91vouPh1IVoedIwB1H1Ddr8IKApCkhOydDTklq36vNc4B3GgrxZTS9PGrjBoru4W0ZS7dmH",
                collapse_key = "type_a",
                notification = new
                {
                    body = msg,
                    title = "Title of Your Notification"
                },
                data = new
                {
                    body = msg,
                    title = "Title of Your Notification in Title",
                    //key_1 = "Value for key_1",
                    //key_2 = "Value for key_2"
                }
            };
            var message = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_configuration["Firebase:baseurl"]),
                Headers =
                            {
                                { HttpRequestHeader.ContentType.ToString(), "application/json" }
                               // { HttpRequestHeader.Authorization.ToString(), "key=AAAAIf5D3Og:APA91bHznPagFn5ixyXmPs8nBATllGy_dlR__kSsbBJUVfRhmwc8Tfb5yU4ggi-idombjzZBKNPTdeRlP33maVECmcaKGoCfBOmfag76qzyCY9zYvRD_C33dBjjahkB8CAbb30AARLHX" }
                            },
                Content = new StringContent(JsonConvert.SerializeObject(anony_object), Encoding.UTF8, "application/json")
            };
            string serverKey = _configuration["Firebase:serverKey"].ToString();
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={serverKey}");
            await client.SendAsync(message); ;
        }

    }



}

public class PushNotificationModel
    { 
      public string To { get; set; }
      public Notificatin notification { get; set; }
      
      public Data data { get; set; }
    }

    public class Notificatin 
    { 
      public string Body { get; set; }
      public string Title { get; set; }
    }

    public class Data 
    {
        public string Body { get; set; }
        public string Title { get; set; }
    }

