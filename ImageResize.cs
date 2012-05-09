using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

    public static class ImageUtilities
    {

        private static Dictionary<string, ImageCodecInfo> encoders = null;

        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            get
            {
                if (encoders == null)
                {
                    encoders = new Dictionary<string, ImageCodecInfo>();
                }

                if (encoders.Count == 0)
                {
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }
                return encoders;
            }
        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }
            return result;
        }

        public static void SaveJpeg(string path, Image image, int quality)
        {
            if ((quality < 0) || (quality > 100))
            {
                string error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                throw new ArgumentOutOfRangeException(error);
            }

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            image.Save(path, jpegCodec, encoderParams);
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            string lookupKey = mimeType.ToLower();
            ImageCodecInfo foundCodec = null;
            if (Encoders.ContainsKey(lookupKey))
            {
                foundCodec = Encoders[lookupKey];
            }
            return foundCodec;
        }

        public static IEnumerable<Bitmap> SeperateGif(string gifPath)
        {

            using (var gifImage = Image.FromFile(gifPath))
            {
                var dimension = new FrameDimension(gifImage.FrameDimensionsList[0]); 
                var frameCount = gifImage.GetFrameCount(dimension); 
                for (var index = 0; index < frameCount; index++)
                {
                    gifImage.SelectActiveFrame(dimension, index); 
                    yield return (Bitmap)gifImage.Clone(); 
                }
            }
        }
    }