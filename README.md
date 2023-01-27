# 一个在系统托盘图标阵列中播放Bad Apple!!的项目

- [一个在系统托盘图标阵列中播放Bad Apple!!的项目](#一个在系统托盘图标阵列中播放bad-apple的项目)
  - [简介](#简介)
  - [要求](#要求)
  - [原理](#原理)
  - [理想效果](#理想效果)
  - [现状](#现状)
  - [写在最后](#写在最后)


## 简介

[中文维基](https://zh.m.wikipedia.org/zh-hans/Bad_Apple!!)

[哔哩哔哩](https://www.bilibili.com/video/BV1xx411c79H)

[YouTube](https://youtu.be/FtutLA63Cp8)

[有示波器的地方，Bad apple可能会迟到，但从未缺席！](https://www.bilibili.com/video/BV1Et411W743)

## 要求

- Windows（11）
- Visual Studio（2022）
- .NET Framework（4.8）
- Python 3
- C++ 11
- C# 8.0
- [速效救心丸](https://item.jkcsjd.com/3810395.html)
- [米诺地尔生发酊](https://item.yiyaojd.com/100009773041.html)
- [《活着》](http://product.dangdang.com/1612701486.html)余华，作家出版社


## 原理

通过OpenCV逐帧读取视频，转换成灰度图并切割后通过socket发送至WinForm应用，其包含 $7 \times 7 = 49$ 个notifyIcon控件：

```csharp
//Main.Designer.cs
private System.Windows.Forms.NotifyIcon[] notifyIcons = new System.Windows.Forms.NotifyIcon[49];
```

WinForm通过适当的格式转换将ICO格式的图片传给```notifyIcon```控件。

## 理想效果

<div align="center"><img src="./image/expected.png" alt=""></div>

理想是丰满的，现实是骨感的。

## 现状

算不上流畅播放，但是已经达到了惊人的 $3$ 帧每秒，毕竟一帧能看，两帧流畅，三帧电竞（~~逃~~）。此外，由于写代码的时间比上写readme的时间高达惊人的 $1:9$ ，目前还有以下问题没有解决（~~毕竟代码可以丑可以菜，readme一定要显得用心~~）：

```csharp
//Main.cs
private async void setNotifyIconAsync()
{
    await Task.Run(() =>
    {
        int counter = 0;
        while (true)
        {
            //System.Collections.Generic.Queue<T>是非线程安全的，因此访问它之前要上锁
            mutex.WaitOne();
            if (imageData.Count > 0)
            {
                var imgBytes = imageData.Dequeue();
                var icon = bitmapToIcon(toGrayBitmap(imgBytes, 60, 60));
                notifyIcons[counter].Icon = icon;
                //要及时释放句柄，不然会内存泄漏，被Windows干掉
                DestroyIcon(icon.Handle);
                counter = counter >= 48 ? 0 : counter + 1;
            }
            //释放锁
            mutex.ReleaseMutex();
        }
    });
}
```

考虑到TCP按序发送以及队列先进先出的特性，这里定义了一个循环自增的```counter```变量来确定收到的图片交给哪个```notifyIcon```控件。但实际运行起来会发生画面偏移的情况：

<div align="center"><img src="./image/real.png" alt=""></div>

## 写在最后

目前还没想到对策，随缘改吧，毕竟太菜。另外关注[@嘉然](https://space.bilibili.com/672328094)，顿顿解馋！关注[@永雏塔菲](https://space.bilibili.com/1265680561)喵，关注永雏塔菲谢谢喵！
