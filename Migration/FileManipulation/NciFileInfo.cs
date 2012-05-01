using System;
using System.IO;
using System.Net;
using System.Drawing;

namespace FileManipulation
{
    public class NciFileInfo
    {
        const string pdf = ".pdf";
        const string doc = ".doc";
        const string docx = ".docx";
        const string xls = ".xls";
        const string xlsx = ".xlsx";
        const string ppt = ".ppt";
        const string pptx = ".pptx";
        const string mp3 = ".mp3";
        const string mov = ".mov";
        const string exe = ".exe";
        const string ics = ".ics";
        const string zip = ".zip";
        const string gz = ".gz";
        const string tgz = ".tgz";
        const string fil = ".fil";


        public static string[] KnownExtensions = { pdf, doc, docx, xls, xlsx, ppt, pptx, mp3, mov, exe, ics, zip, gz, tgz, fil };

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

        private NciFileInfo()
        {
        }

        public static NciFileInfo DownloadImage(string host, string filePath)
        {
            NciFileInfo rtnFile = new NciFileInfo();

            string extension = getFileExtension(filePath);
            string tempFile = "temp_migrate" + extension;
            string url = "http://" + host + filePath;


            WebClient wc = new WebClient();
            wc.DownloadFile(url, tempFile);            

            FileInfo file = new FileInfo(tempFile);
            rtnFile.Extension = extension;
            rtnFile.FileName = getFilename(filePath);
            rtnFile.MimeType = GetMimeType(extension);
            rtnFile.FileSize = file.Length;
            rtnFile.Data = Convert.ToBase64String(ReadBinaryFile(file));

            int pathEnd = filePath.LastIndexOf(rtnFile.FileName);
            if (pathEnd > 0)
                rtnFile.Path = filePath.Substring(0, pathEnd - 1);
            else
                rtnFile.Path = string.Empty;
                        
            return rtnFile;
        }


        private static string GetMimeType(string ext)
        {
            string mimeType = "application/unknown";

            switch (ext)
            {
                case pdf: mimeType = "application/pdf"; break;
                case doc: mimeType = "application/msword"; break;
                case docx: mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; break;
                case xls: mimeType = "application/vnd.ms-excel"; break;
                case xlsx: mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; break;
                case ppt: mimeType = "application/vnd.ms-powerpoint"; break;
                case pptx: mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation"; break;
                case mp3: mimeType = "audio/mpeg"; break;
                case mov: mimeType = "video/quicktime"; break;
                case exe: mimeType = "application/octet-stream"; break;
                case ics: mimeType = "application/octet-stream"; break;
                case zip: mimeType = "application/zip"; break;
                case gz: mimeType = "application/x-gzip"; break;
                case tgz: mimeType = "application/x-compressed"; break;
                case fil: mimeType = "application/octet-stream"; break;
            }

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
