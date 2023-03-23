using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using OpenCvSharp;

namespace SystemTrayApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            InitMats();
            ReadVideoFrameCore();
        }
        private Queue<byte[]> imageData = new Queue<byte[]>();
        Mutex mutex = new Mutex();
        Rect[] rects = new Rect[49];
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private void InitMats(int size = 100 * 1080 / 1792, int stride = 182 * 1080 / 1792)
        {
            int i = 0;
            int PosX = 0;
            while(PosX + size < 1080)
            {
                int PosY = 0;
                while(PosY + size < 1080)
                {
                    rects[i] = new Rect(PosY, PosX, size, size);
                    ++i;
                    PosY = PosY + size + stride;
                }
                PosX = PosX + size + stride;
            }
        }

        private void ReadVideoFrameCore()
        {
            string Path = @"D:\Bad Apple.mp4";
            using (VideoCapture videoCapture = new VideoCapture(Path))
            {
                if(!videoCapture.IsOpened())
                {
                    MessageBox.Show("Oops!");
                    Environment.Exit(0);
                }
                Mat CapturedFrame = new Mat();
                while(videoCapture.Read(CapturedFrame))
                {
                    //Rect中，x是列，y是行
                    Rect r = new Rect(180, 0, 1080, 1080);
                    Mat Origin = new Mat(CapturedFrame, r);
                    Mat Gray = new Mat();
                    Cv2.CvtColor(Origin, Gray, ColorConversionCodes.BGR2GRAY);
                    int i = 0;
                    foreach(var rect in rects)
                    {
                        Mat tmp = new Mat(Gray, rect);
                        Bitmap bitmap = new Bitmap(tmp.Cols, tmp.Rows, (int)tmp.Step(), PixelFormat.Format8bppIndexed, tmp.Data);

                        Icon icon = Icon.FromHandle(bitmap.GetHicon());

                        notifyIcons[i].Icon = icon;
                        DestroyIcon(icon.Handle);

                        icon.Dispose();
                        bitmap.Dispose();
                        tmp.Dispose();

                        ++i;
                    }

                }
            }
        }
    }
}
