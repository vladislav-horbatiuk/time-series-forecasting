using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BSWork.BS_Main
{
    public static class ImgUtils
    {
        public static Image SetImgOpacity(Image imgPic, float imgOpac)
        {

            Bitmap bmpPic = new Bitmap(imgPic.Width, imgPic.Height);

            Graphics gfxPic = Graphics.FromImage(bmpPic);

            ColorMatrix cmxPic = new ColorMatrix();

            cmxPic.Matrix33 = imgOpac;

            ImageAttributes iaPic = new ImageAttributes();

            iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            gfxPic.DrawImage(imgPic, new Rectangle(0, 0, bmpPic.Width, bmpPic.Height), 0, 0, imgPic.Width, imgPic.Height, GraphicsUnit.Pixel, iaPic);

            gfxPic.Dispose();

            return bmpPic;
        }
    }
}
