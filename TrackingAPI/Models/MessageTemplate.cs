using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Models
{
    public class MessageTemplate
    {


        public static string SubGetOTPMsgToVerifyEmail => "OTP Verification from DCPL";
        public static string GetOTPMsgMailVerifyEmail(string otp)
        { return $"Your One Time Pin (OTP) to verify your EmailId w.r.t. DCPL APP is <strong>{otp}</strong>. It is valid for 1 minute."; }
        
        //Registered
        public static string GetOTPMsgSMSVerifyMobile(string otp)
        { return $"Your One Time Pin (OTP) to verify your Mobile No. w.r.t. to DCPL APP is {otp}. It is valid for 1 minute."; }

        //Registered
        public static string SubGetOTPMsg => "OTP Verification from DCPL";
        public static string GetOTPMsgMail(string otp)
        { return $"Your One Time Pin (OTP) to login to DCPL APP is <strong>{otp}</strong>. It is valid for 1 minute."; }
        public static string GetOTPMsgSMS(string otp)
        { return $"Your One Time Pin (OTP) to login to DCPL APP is {otp}. It is valid for 1 minute."; }

        // Change Password - Registered
        public static string SubGetOTPMsgCP => "OTP Verification from DCPL to Change your Login Password";
        public static string GetOTPMsgCPMail(string otp)
        { return $"Your One Time Pin (OTP) to Change your Login Password w.r.t. DCPL APP is <strong>{otp}</strong>. It is valid for 1 minute."; }
        public static string GetOTPMsgCPSMS(string otp)
        { return $"Your One Time Pin (OTP) to Change your Login Password w.r.t. DCPL APP is {otp}. It is valid for 1 minute."; }

        // Change EmailId
        public static string SubGetOTPMsgCE => "OTP Verification from DCPL to Verify your EmailId";
        public static string GetOTPMsgCEMail(string otp)
        { return $"Your One Time Pin (OTP) to verify your EmailId w.r.t. DCPL APP is <strong>{otp}</strong>. It is valid for 1 minute."; }


        // Change MobileNo - Registered
        public static string SubGetOTPMsgCM => "OTP Verification from DCPL to Verify your Mobile Number";
        public static string GetOTPMsgCMSMS(string otp)
        { return $"Your One Time Pin (OTP) to verify your Mobile Number w.r.t. DCPL APP is {otp}. It is valid for 1 minute."; }

        //Customer creation Message
        public static string SubLoginCreation => "Login Creation Successful.";
        public static string LoginCreationMail(string AutoRegId, string UserName, string UserRole)
        { return $"Dear <strong>{UserName}</strong>, <strong>{UserRole} Login</strong> has been Created Successfully with Registration Id: <strong>{AutoRegId}</strong>. Please use <strong>mobile APP</strong> to access the application."; }
        public static string LoginCreationSMS(string AutoRegId, string UserRole)
        { return $"{UserRole} Login Created Successfully with Registration Id: {AutoRegId}. Please use mobile APP to access the application."; }

    }
}
