namespace Buyzia.Services.Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;

    internal class ImagesHelper
    {
        // Resize a Bitmap  
        public static byte[] ResizeImageByLongestSide(string url, int longestSide, ImageFormat format)
        {
            byte[] imgArray = GetImageFromLink(url);
            var width = longestSide;
            var height = longestSide;

            byte[] resizedImgArray;
            Image startBitmap;
            Image resizedImage;
            using (MemoryStream StartMemoryStream = new MemoryStream(), NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(imgArray, 0, imgArray.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                startBitmap = new Bitmap(StartMemoryStream);
                resizedImage = ScaleImage(startBitmap, width, height);
            }

            resizedImgArray = ImageToByte(resizedImage, format);

            return resizedImgArray;
        }

        /// <summary>
        /// This method uses Internet connection to get the image as byte array. It cam be very easy modified to return
        /// it in MemoryStream
        /// </summary>
        /// <param name="pictureLink">Actual url of the image</param>
        /// <returns>
        /// Image as byte array
        /// </returns>
        private static byte[] GetImageFromLink(string pictureLink)
        {
            byte[] imageData = new byte[0];
            try
            {
                WebRequest req = WebRequest.Create(pictureLink);
                WebResponse response = req.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[1024];
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        while (bytesRead > 0)
                        {
                            memStream.Write(buffer, 0, bytesRead);
                            bytesRead = stream.Read(buffer, 0, buffer.Length);
                        }
                        imageData = memStream.ToArray();

                        //MemoryStream stream1 = new MemoryStream(imageData);
                        //Image objImage = Image.FromStream(stream1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting the image causes a problem - image url : " + pictureLink);
                Console.WriteLine("EX type : " + ex.GetType());
                Console.WriteLine("EX message : " + ex.Message);
            }

            return imageData;
        }

        private static byte[] ImageToByte(Image img, ImageFormat format)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, format);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        private static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
    }
}
