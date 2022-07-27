using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Utilities
{
    public class RandomCodeGenerator
    {
        public RandomCodeGenerator()
        {

        }
        public static string GetRandomCode(int length, CodeType codeType)
        {
            string sOTP = String.Empty;
            string[] saAllowedCharacters = null;
            if (codeType == CodeType.Otp)
            {
                saAllowedCharacters = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            }
            else if (codeType == CodeType.EmailVerification)
            {
                saAllowedCharacters = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", };
            }
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

        public enum CodeType
        {
            Otp,
            EmailVerification,
        }
    }
}
