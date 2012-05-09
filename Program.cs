using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResizeImage
{
    class Program
    {
        static void Main(string[] args)
        {
            string gifPath = @"C:\Users\tiu\Pictures\92PCHK_e.gif";
            string imagePath = @"C:\Users\tiu\Pictures\m.jpg";
            Image img = Image.FromFile(imagePath);
            Image thumb=ImageUtilities.ResizeImage(img, 300, 220);
            ImageUtilities.SaveJpeg(@"C:\Users\tiu\Pictures\t.jpg", thumb, 100);
            int i = 0;
            foreach (var image in ImageUtilities.SeperateGif(gifPath))
            {
                
                string filename = @"C:\Users\tiu\Pictures\"+i.ToString()+".jpg";
                ImageUtilities.SaveJpeg(filename, image, 100);
                i++;
            }
            Console.ReadKey();
        }
    }
}
