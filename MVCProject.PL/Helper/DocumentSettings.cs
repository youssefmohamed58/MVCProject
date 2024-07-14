using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MVCProject.PL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            //1. Get Located Folder Path 
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            //2. Get File Name and Make it Unique 
            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            //3. Get File Path[Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);

            //4. Save File As Streams
            using var FS = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(FS);

            //5. Return File Name
            return FileName;
        }


        public static void DeleteFile(string folderName, string fileName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
