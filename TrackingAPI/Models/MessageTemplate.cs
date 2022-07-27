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
        
        public static string GetOTPMsgSMSVerifyMobile(string otp)
        { return $"Your One Time Pin (OTP) to verify your Mobile No. w.r.t. to DCPL APP is {otp}. It is valid for 1 minute."; }

        //
        public static string SubGetOTPMsg => "OTP Verification from DCPL";
        public static string GetOTPMsgMail(string otp)
        { return $"Your One Time Pin (OTP) to login to DCPL APP is <strong>{otp}</strong>. It is valid for 1 minute."; }
        public static string GetOTPMsgSMS(string otp)
        { return $"Your One Time Pin (OTP) to login to DCPL APP is {otp}. It is valid for 1 minute."; }

        // Change Password
        public static string SubGetOTPMsgCP => "OTP Verification from DCPL to Change your Login Password";
        public static string GetOTPMsgCPMail(string otp)
        { return $"Your One Time Pin (OTP) to Change your Login Password w.r.t. DCPL APP is <strong>{otp}</strong>. It is valid for 1 minute."; }
        public static string GetOTPMsgCPSMS(string otp)
        { return $"Your One Time Pin (OTP) to Change your Login Password w.r.t. DCPL APP is {otp}. It is valid for 1 minute."; }

        // Change EmailId
        public static string SubGetOTPMsgCE => "OTP Verification from DCPL to Verify your EmailId";
        public static string GetOTPMsgCEMail(string otp)
        { return $"Your One Time Pin (OTP) to verify your EmailId w.r.t. DCPL APP is <strong>{otp}</strong>. It is valid for 1 minute."; }


        // Change MobileNo
        public static string SubGetOTPMsgCM => "OTP Verification from DCPL to Verify your Mobile Number";
        public static string GetOTPMsgCMSMS(string otp)
        { return $"Your One Time Pin (OTP) to verify your Mobile Number w.r.t. DCPL APP is {otp}. It is valid for 1 minute."; }

    }
}
