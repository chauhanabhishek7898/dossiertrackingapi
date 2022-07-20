using TrackingAPI.Models;
using RestSharp;

namespace TrackingAPI.Utilities
{
    public class SmsSender
    {
        public SmsSender()
        {

        }

        public static string SendSms(string otp, string mobileNumber)
        {
            string otpMsg = MessageTemplate.GetOTPMsgSMS(otp);

            var client = new RestClient("http://sms.indiasms.com/SMSApi/send");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("userid", "dossierorcui6");
            request.AddParameter("password", "ind123");
            request.AddParameter("senderid", "DOSIER");
            request.AddParameter("sendMethod", "quick");
            request.AddParameter("mobile", mobileNumber);
            request.AddParameter("msg", otpMsg);
            request.AddParameter("msgType", "text");
            var response = client.Execute(request);
            if (response.IsSuccessful) { return otp; }
            else { return "TryAgain"; }
        }

        public static void SendSmsText(string smsText, string mobileNumber)
        {

            var client = new RestClient("http://sms.indiasms.com/SMSApi/send");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("userid", "dossierorcui6");
            request.AddParameter("password", "ind123");
            request.AddParameter("senderid", "DOSIER");
            request.AddParameter("sendMethod", "quick");
            request.AddParameter("mobile", mobileNumber);
            request.AddParameter("msg", smsText);
            request.AddParameter("msgType", "text");
            request.AddParameter("msgType", "text");
            var response = client.Execute(request);

        }
    }
}
