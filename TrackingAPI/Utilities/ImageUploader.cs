using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Utilities
{
    public class ImageUploader
    {
        public ImageUploader()
        {

        }
        public static async Task<bool> SaveImage(IFormFile imageFile, int userId, string path = null)
        {
            bool status = false;
            try
            {
                if (path == null)
                {
                    path = Path.Combine("assets\\images\\" + userId + "", imageFile.FileName);
                }
                path = "wwwroot\\" + path;
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public static async Task<bool> SaveImageND(IFormFile imageFile, string MobileDOB, string path = null)
        {
            bool status = false;
            try
            {
                if (path == null)
                {
                    path = Path.Combine("assets\\images\\" + MobileDOB + "", imageFile.FileName);
                }
                path = "wwwroot\\" + path;
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
    }
}
