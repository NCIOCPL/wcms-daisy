using System;
using System.IO;
using System.Net;
using System.Drawing;

namespace Munger
{
    class ImageInfo
    {
        /// <summary>
        /// Gets the height of the image
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the width of the image
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets a Base-64 Encoded string representing the data of the image
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        /// Gets the file name of the image
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the extension of the image
        /// </summary>
        public string Extension { get; private set; }

        /// <summary>
        /// Gets the path where the image is stored.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the mime type of the image
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// Gets image's title.
        /// </summary>
        public string Title
        {
            get
            {
                int index = FileName.LastIndexOf('.');
                if (index == -1)
                    return FileName;
                else
                    return FileName.Substring(0, index);
            }
        }

        /// <summary>
        /// Gets the file size of the image
        /// </summary>
        public long FileSize { get; private set; }

        private ImageInfo()
        {
        }

        public static ImageInfo DownloadImage(string host, string filePath)
        {
            ImageInfo rtnImage = new ImageInfo();

            string extension = getFileExtension(filePath);
            string tempFile = "temp_migrate" + extension;
            string url = host + filePath;


            WebClient wc = new WebClient();
            wc.DownloadFile(url, tempFile);            

            FileInfo file = new FileInfo(tempFile);
            rtnImage.Extension = extension;
            rtnImage.FileName = getFilename(filePath);
            rtnImage.MimeType = GetMimeType(extension);
            rtnImage.FileSize = file.Length;
            rtnImage.Data = Convert.ToBase64String(ReadBinaryFile(file));

            int pathEnd = filePath.LastIndexOf('/');
            if (pathEnd != -1)
                rtnImage.Path = filePath.Substring(0, pathEnd);
            else
                rtnImage.Path = string.Empty;


            using (Bitmap image = new Bitmap(tempFile, false))
            {
                rtnImage.Height = image.Height;
                rtnImage.Width = image.Width;
            }            
                        
            return rtnImage;
        }


        private static string GetMimeType(string ext)
        {
            string mimeType = "application/unknown";
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        /// <summary>
        ///     Reads binary data from the specified file
        /// </summary>
        /// <param name="filePath">
        ///     the path of the file from which to read binary data from; 
        ///     assumed not <code>null</code> or empty.
        /// </param>
        /// <returns>
        ///     the content of the specified file.
        /// </returns>
        public static byte[] ReadBinaryFile(FileInfo file)
        {
            byte[] binaryData = new byte[file.Length];

            using (FileStream inFile = file.OpenRead())
            {
                inFile.Read(binaryData, 0, binaryData.Length);
            }

            return binaryData;
        }

        /// <summary>
        ///     Gets the file name from the specified file path.
        /// </summary>
        /// <param name="filePath">
        ///     the specified file path.
        /// </param>
        /// <returns>
        ///     the file name of the specified file path.
        /// </returns>
        private static string getFilename(string filePath)
        {
            int index = filePath.LastIndexOf('/');
            if (index != -1)
                return filePath.Substring(index + 1);
            else
                return filePath;
        }

        /// <summary>
        ///     Gets the file name from the specified file path.
        /// </summary>
        /// <param name="filePath">
        ///     the specified file path.
        /// </param>
        /// <returns>
        ///     the file name of the specified file path.
        /// </returns>
        private static string getFileExtension(string filePath)
        {
            int index = filePath.LastIndexOf('.');
            if (index != -1)
                return filePath.Substring(index);
            else
                return string.Empty; // No extension
        }

    }
}
