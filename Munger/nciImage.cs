
using NCI.CMS.Percussion.Manager.CMS;

namespace Munger
{
    class NciImage
    {
        const string gif = ".gif";
        const string jpg = ".jpg";
        const string png = ".png";
        const string bmp = ".bmp";

        public static string[] KnownExtensions = { gif, jpg, png, bmp };

        public string ContentType { get { return "nciImage"; } }

        private FieldSet _fieldSet = new FieldSet();
        public FieldSet FieldSet { get { return _fieldSet; } }

        public NciImage(ImageInfo info, string altText)
        {
            _fieldSet.Add("long_title", info.Title);
            _fieldSet.Add("img1", info.Data);
            _fieldSet.Add("img1_ext", info.Extension);
            _fieldSet.Add("img1_filename", info.FileName);
            _fieldSet.Add("img1_size", info.FileSize.ToString());
            _fieldSet.Add("img1_height", info.Height.ToString());
            _fieldSet.Add("img1_width", info.Width.ToString());
            _fieldSet.Add("img1_type", info.MimeType);
            _fieldSet.Add("img_alt", altText);
        }
    }
}
