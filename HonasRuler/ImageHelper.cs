using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonasRuler
{
    public static class ImageHelper
    {
        public static Image RotateImage(Image img, RotateFlipType rotatetype)
        {
            var bmp = new Bitmap(img);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.DrawImage(img, 0, 0, img.Width, img.Height);
            }

            bmp.RotateFlip(rotatetype);
            return bmp;
        }

    }
}
