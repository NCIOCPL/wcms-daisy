using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using FileManipulation;

namespace SerializeImageUploads
{
    public class ImageInputs
    {
        public InputInfo[] Files { get; set; }

        public void Process(string outputFile)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("list");
            doc.AppendChild(root);

            foreach (InputInfo item in Files)
            {
                CreateEntry(doc.DocumentElement, item);
            }

            doc.Save(outputFile);
        }

        public void CreateEntry(XmlNode root, InputInfo imageEntry)
        {
            XmlElement itemNode = root.OwnerDocument.CreateElement("item");
            ImageInfo file = ImageInfo.LoadImage(imageEntry.file);

            itemNode.AppendChild(CreateField(itemNode, "mig_id", imageEntry.mig_id.ToString()));
            itemNode.AppendChild(CreateField(itemNode, "contenttype", "gloImage"));
            itemNode.AppendChild(CreateField(itemNode, "folder", imageEntry.folder));
            itemNode.AppendChild(CreateField(itemNode, "community", imageEntry.community));

            itemNode.AppendChild(CreateField(itemNode, "long_title",imageEntry.title));
            itemNode.AppendChild(CreateField(itemNode, "img1", file.Data));
            itemNode.AppendChild(CreateField(itemNode, "img1_ext", file.Extension));
            itemNode.AppendChild(CreateField(itemNode, "img1_filename", file.FileName));
            itemNode.AppendChild(CreateField(itemNode, "img1_size", file.FileSize.ToString()));
            itemNode.AppendChild(CreateField(itemNode, "img1_height", file.Height.ToString()));
            itemNode.AppendChild(CreateField(itemNode, "img1_width", file.Width.ToString()));
            itemNode.AppendChild(CreateField(itemNode, "img1_type", file.MimeType));

            root.AppendChild(itemNode);
        }

        public XmlElement CreateField(XmlNode itemBase, string fieldName, string value)
        {
            XmlElement field = itemBase.OwnerDocument.CreateElement(fieldName);
            XmlText fieldValue = itemBase.OwnerDocument.CreateTextNode(value);
            field.AppendChild(fieldValue);

            return field;
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
    }
}
