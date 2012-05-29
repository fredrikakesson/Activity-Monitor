using System.Drawing;
using System.Drawing.Imaging;
using BitmapToIcon;

namespace ActivityMonitor2.Doman.Ikongenerator
{
    public class Ikongenerator : IIkongenerator
    {
        public Icon SkapaTrayikon(float procent, bool aktivFärgschema)
        {
            Color bakgrund;
            Color förgrund;

            if (aktivFärgschema)
            {
                bakgrund = Color.DodgerBlue;
                förgrund = Color.Gold;
            }
            else
            {
                bakgrund = Color.LightSlateGray;
                förgrund = Color.LightGray;
            }

            var bmp = new Bitmap(16, 16, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillEllipse(new SolidBrush(bakgrund), 0, 0, 16, 16);
                g.FillPie(new SolidBrush(förgrund), new Rectangle(0, 0, 16, 16), 270, (360 * procent));
            }
            return Converter.BitmapToIcon(bmp);
        }
    }
}