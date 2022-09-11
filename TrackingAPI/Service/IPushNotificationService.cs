using TrackingAPI.Controllers;
using TrackingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Service
{
   public interface IPushNotificationService
    {
         //Task SendNotification(MessagingService pushNotificationModel);
        Task SendNotificationToDrivers(OrderDetails orderDetails, List<string> deviceList);
        Task SendNotificationToDrivers(string msg, List<string> deviceList);
    }
}
