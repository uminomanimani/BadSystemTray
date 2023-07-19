using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using OpenCvSharp;
using System.Collections.Concurrent;

namespace SystemTrayApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            InitMats(100 * 1080 / (282 * columnCount - 182), 182 * 1080 / (282 * columnCount - 182));
            for (int i = 0; i < iconsQueue.Length; ++i)
                iconsQueue[i] = new ConcurrentQueue<Icon> ();
        }
        private static int columnCount = 6;
        private ConcurrentQueue<Mat> matsQueue = new ConcurrentQueue<Mat>();
        private ConcurrentQueue<Icon>[] iconsQueue = new ConcurrentQueue<Icon>[columnCount * columnCount];

        int count = 0;
        Mutex countMutex = new Mutex ();

        Rect[] rects = new Rect[columnCount * columnCount];
        bool finished = false;
        Mutex mutex = new Mutex();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);
        
        private void InitMats(int size, int stride)
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
            //if (DisplayOneFrame()) { }
            //else
            //{
            //    timer.Stop();
            //    timer.Enabled = false;
            //}
            Display();
        }

        private async void SplitToBitmapsAsync()
        {
            await Task.Run(async () => 
            { 
                while (true)
                {
                    countMutex.WaitOne();
                    if (count >= 5)
                    {
                        countMutex.ReleaseMutex();
                        await Task.Delay(50);
                        continue;
                    }
                    countMutex.ReleaseMutex();

                    if (matsQueue.TryDequeue(out var frame))
                    {
                        countMutex.WaitOne();
                        ++count;
                        countMutex.ReleaseMutex();
                        //Rect中，x是列，y是行
                        Rect r = new Rect(180, 0, 1080, 1080);
                        using (Mat Gray = new Mat())
                        using (Mat Origin = new Mat(frame, r))
                        {
                            Cv2.CvtColor(Origin, Gray, ColorConversionCodes.RGB2GRAY);
                            for (int i = 0; i < rects.Length; ++i)
                            {
                                using (Mat tmp = new Mat(Gray, rects[i]))
                                using (Bitmap bitmap = new Bitmap(tmp.Cols, tmp.Rows, (int)tmp.Step(), PixelFormat.Format8bppIndexed, tmp.Data))
                                {
                                    // 获取调色板
                                    ColorPalette palette = bitmap.Palette;
                                    // 设置白色为灰色
                                    for (int m = 0; m < 256; m++)
                                    {
                                        palette.Entries[m] = Color.FromArgb(m, m, m);
                                    }
                                    // 应用调色板
                                    bitmap.Palette = palette;
                                    //Bitmap bitmapCopy = ((Bitmap)bitmap.Clone());
                                    // 将复制后的对象放入队列
                                    Icon icon = Icon.FromHandle(bitmap.GetHicon());
                                    iconsQueue[i].Enqueue((Icon)icon.Clone());

                                    DestroyIcon(icon.Handle);
                                    icon.Dispose();
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            });
        }

        private void Display()
        {
            for (int i = 0; i < notifyIcons.Length; ++i) 
            {
                if (iconsQueue[i].TryDequeue(out var icon))
                {
                    notifyIcons[i].Icon = icon;
                    DestroyIcon(icon.Handle);
                    icon.Dispose();
                }
                else
                {
                    mutex.WaitOne();
                    if (finished)
                    {
                        mutex.ReleaseMutex();
                        return;
                    }
                    mutex.ReleaseMutex();
                }
            }
            countMutex.WaitOne();
            --count;
            countMutex.ReleaseMutex();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            button.Enabled = false;
            ReadVideoFrameAsync();
            // Thread.Sleep(500);
            SplitToBitmapsAsync();
            timer.Enabled = true;
            timer.Start();
        }

        private async void ReadVideoFrameAsync()
        {
            await Task.Run(async () => 
            {
                string Path = @"Bad Apple.mp4";
                using (VideoCapture videoCapture = new VideoCapture(Path))
                {
                    if (!videoCapture.IsOpened())
                    {
                        MessageBox.Show("Oops!没能打开" + Path + "...");
                        Environment.Exit(0);
                    }
                    using (Mat CapturedFrame = new Mat())
                    {
                        int i = 0;
                        while (true)
                        {
                            if (matsQueue.Count >= 10)
                            {
                                await Task.Delay(50);
                                continue;
                            }
                            if (!videoCapture.Read(CapturedFrame))
                            {
                                mutex.WaitOne();
                                finished = true;
                                mutex.ReleaseMutex();
                                break;
                            }
                            if (i % 4 == 0) matsQueue.Enqueue(CapturedFrame.Clone());
                            i += 1;
                        }
                    }
                }
            }
            );
        }
    }
}
