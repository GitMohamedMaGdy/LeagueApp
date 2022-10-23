
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueApp.API.Utilites
{
    public static class UploadFileHelper

    {
        private static Random random = new Random();
        public static string UploadFile(IFormFile file, string filePath,string fileName = null)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName).ToLower();
            var newFileName = (fileName == null ? fileNameWithoutExt + RandomString(4) : fileName ) + fileExtension;

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream($"{filePath}\\{newFileName}", FileMode.Create))
            {
                 file.CopyToAsync(stream);
            }
            return newFileName;
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetAppFilesPath(string AppFilesPath,string Folderpath)
        {
            var root = Path.Combine(AppFilesPath, Folderpath);
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            return root;
        }
        public static bool CheckFileExtension(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName).ToLower();
            return fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png";
        }
    }
}
