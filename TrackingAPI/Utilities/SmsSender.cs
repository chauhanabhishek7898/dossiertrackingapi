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

            var client = new RestClient("http://sms-alerts.servetel.in/api/v4/?api_key=Ad67d33a53f54aa86b37acb7552acd024&method=sms");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("message", otpMsg);
            request.AddParameter("to", mobileNumber);
            request.AddParameter("sender", "DROMES");
            IRestResponse response = (IRestResponse)client.Execute(request);
            if (response.IsSuccessful) { return otp; }
            else { return "TryAgain"; }
        }

        public static void SendSmsText(string smsText, string mobileNumber)
        {

            var client = new RestClient("http://sms-alerts.servetel.in/api/v4/?api_key=Ad67d33a53f54aa86b37acb7552acd024&method=sms");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("message", smsText);
            request.AddParameter("to", mobileNumber);
            request.AddParameter("sender", "DROMES");
            IRestResponse response = (IRestResponse)client.Execute(request);
        }
    }
}
