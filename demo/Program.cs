using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Accord.Video.FFMPEG;

class Program
{
    static void Main(string[] args)
    {
        char[][] points;
        points = new char[10][];
        points[0] = new char[] { '@', '▌', };
        points[1] = new char[] { 'Ñ', '¥', 'Æ' };
        points[2] = new char[] { '¢', 'Ö', '$', '#', '%', '&' };
        points[3] = new char[] { 'É', 'Ñ', 'Ä', };
        points[4] = new char[] { 'Å', 'Ü', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        points[5] = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        points[6] = new char[] { 'à', 'á', 'â', 'a', 'ä', 'å', 'æ', 'ç', 'è', 'é', 'ê', 'ë', 'ì', 'í', 'î', 'ï', 'd', 'ñ', 'ò', 'ó', 'ô', 'o', 'ö', '÷', 'o', 'ù', 'ú', 'û', 'ü', 'y',
                                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
        points[7] = new char[] { '·', ',', '-', ':', ';' };
        points[8] = new char[] { ' ' };
        points[9] = new char[] { ' ' };

        while (true)
        {
            using (var vFReader = new VideoFileReader())
            {
                vFReader.Open("src/tonton_video_0.mp4");
                for (int i = 0; i < vFReader.FrameCount; i++)
                {
                    Bitmap bmpBaseOriginal = vFReader.ReadVideoFrame();
                    Print(bmpBaseOriginal, points);
                    System.Threading.Thread.Sleep(100);
                    Console.SetCursorPosition(0, 0);
                }
                vFReader.Close();
            }
        }
    }

    static void Print(Bitmap image, char[][] point,int width = 50, int height = 32)
    {
        image = ResizeImage(image, width, height);
        Random r = new Random();
        for (int i = 0; i < image.Height; i++)
        {
            string s = string.Empty;
            for (int j = 0; j < image.Width; j++)
            {
                Color pixel = image.GetPixel(j, i);
                int gray = (int)((0.3f * pixel.R + 0.59f * pixel.G + 0.11f * pixel.B) / 25.5f);
                int a = r.Next(0, point[gray].Length);
                s += point[gray][a].ToString();
            }
            Console.WriteLine(s);
        }
    }

    static Bitmap ResizeImage(Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }
}

