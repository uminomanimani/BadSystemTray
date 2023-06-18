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
        }
        private Queue<Mat> MatsQueue = new Queue<Mat>();
        Mutex QueueMutex = new Mutex();
        Rect[] rects = new Rect[49];
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private void InitMats(int size = 100 * 1080 / 1792, int stride = 182 * 1080 / 1792)
        {
            int i = 0;
            int PosX = 0;
            while (PosX + size < 1080)
            {
                int PosY = 0;
                while (PosY + size < 1080)
                {
                    rects[i] = new Rect(PosY, PosX, size, size);
                    ++i;
                    PosY = PosY + size + stride;
                }
                PosX = PosX + size + stride;
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            DisplayOneFrame();
        }

        private void DisplayOneFrame()
        {
            Mat Frame;
            while (true)
            {
                QueueMutex.WaitOne();
                if (MatsQueue.Count == 0)
                {
                    QueueMutex.ReleaseMutex();
                    continue;
                }
                Frame = MatsQueue.Dequeue();
                QueueMutex.ReleaseMutex();
                break;
            }

            //Rect中，x是列，y是行
            Rect r = new Rect(180, 0, 1080, 1080);
            Mat Origin = new Mat(Frame, r);
            Mat Gray = new Mat();
            Cv2.CvtColor(Origin, Gray, ColorConversionCodes.RGB2GRAY);
            for (int i = 0; i < rects.Length; ++i)
            {
                Mat tmp = new Mat(Gray, rects[i]);
                Bitmap bitmap = new Bitmap(tmp.Cols, tmp.Rows, (int)tmp.Step(), PixelFormat.Format8bppIndexed, tmp.Data);
                // 获取调色板
                ColorPalette palette = bitmap.Palette;
                // 设置白色为灰色
                for (int m = 0; m < 256; m++)
                {
                    palette.Entries[m] = Color.FromArgb(m, m, m);
                }
                // 应用调色板
                bitmap.Palette = palette;

                Icon icon = Icon.FromHandle(bitmap.GetHicon());

                notifyIcons[i].Icon = icon;
                DestroyIcon(icon.Handle);

                icon.Dispose();
                bitmap.Dispose();
                tmp.Dispose();
            }

            Origin.Dispose();
            Gray.Dispose();
            Frame.Dispose();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            ReadVideoFrameAsync();
            Thread.Sleep(500);
            timer.Enabled = true;
            timer.Start();
        }

        private async void ReadVideoFrameAsync()
        {
            await Task.Run(() => { ReadVideoFrameCore(); });
        }

        private void ReadVideoFrameCore()
        {
            string Path = @"Bad Apple.mp4";
            using (VideoCapture videoCapture = new VideoCapture(Path))
            {
                if (!videoCapture.IsOpened())
                {
                    MessageBox.Show("Oops!没能打开" + Path + "...");
                    Environment.Exit(0);
                }
                Mat CapturedFrame = new Mat();
                int i = 0;
                while (true)
                {
                    QueueMutex.WaitOne();
                    if (MatsQueue.Count >= 100)
                    {
                        QueueMutex.ReleaseMutex();
                        Thread.Sleep(100);
                        continue;
                    }
                    if (!videoCapture.Read(CapturedFrame))
                    {
                        QueueMutex.ReleaseMutex();
                        break;
                    }
                    if (i % 4 == 0) MatsQueue.Enqueue(CapturedFrame.Clone());
                    QueueMutex.ReleaseMutex();
                    ++i;
                }
                CapturedFrame.Dispose();
            }
        }
    }
}
