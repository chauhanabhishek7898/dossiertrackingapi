using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Models
{
    public class MessageTemplate
    {
        //1
        public static string SubGetOTPMsg => "OTP Verification from Drome";
        public static string GetOTPMsgMail(string otp)
        { return $"Your One Time Pin (OTP) to login to Drome APP is <strong>{otp}</strong>. It is valid for 1 minute."; }
        public static string GetOTPMsgSMS(string otp)
        { return $"Your One Time Pin (OTP) to login to DCPL APP is {otp}. It is valid for 1 minute."; }
    }
}
