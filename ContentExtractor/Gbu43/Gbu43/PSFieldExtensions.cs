using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NCI.CMS.Percussion.Manager.PercussionWebSvc;

namespace Gbu43
{
    public static class PSFieldExtensions
    {
        public static string GetFirstValue(this PSField field)
        {
            if (field.PSFieldValue != null && field.PSFieldValue.Length == 1)
            {
                return field.PSFieldValue[0].RawData;
            }
            else
            {
                return null;
            }
        }

        public static long GetFirstValueAsLong(this PSField field)
        {
            long l = -1;
            if (field.PSFieldValue != null && field.PSFieldValue.Length == 1)
            {
                try
                {
                    l = long.Parse(field.PSFieldValue[0].RawData);
                }
                catch { }
            }

            return l;
        }

        public static bool SaveFieldAsFile(this PSField field, string path)
        {
            // Get the value of the binary field, "item"
            string data = field.GetFirstValue();

            if (String.IsNullOrEmpty(data))
            {
                Console.WriteLine("Cannot save file, " + path + ", contents are null");
                return false;
            }

            // Assuming found the "item" field and it is not empty.
            // Retrieves the binary data from the "item" field.
            try
            {
                File.WriteAllBytes(path, Convert.FromBase64String(data));
                return true;
            } catch (Exception ex) {
                Console.WriteLine("Cannot save file, " + path + ", exception occurred: " + ex.ToString());
            }

            return false;
        }
    }
}
