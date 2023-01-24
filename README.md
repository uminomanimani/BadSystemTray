<h1>一个在系统托盘图标阵列中播放Bad Apple!!的项目</h1>

<h1>简介</h1>

[中文维基](https://zh.m.wikipedia.org/zh-hans/Bad_Apple!!)

[哔哩哔哩](https://www.bilibili.com/video/BV1xx411c79H)

[YouTube](https://youtu.be/FtutLA63Cp8)

[示波器](https://www.bilibili.com/video/BV1Et411W743)


<h2>原理</h2>

通过OpenCV逐帧读取视频，转换成灰度图并切割后通过socket分发至 $7 \times 7 = 49$ 个WinForm应用里，WinForm通过适当的格式转换将ICO格式的图片传给```notifyIcon```控件。

<h2>要求</h2>

- Windows（11）
- Visual Studio（2022）
- .NET Framework（4.8）
- Python 3
- C++ 11
- C# 8.0
- 速效救心丸
- 米诺地尔生发酊
- 《活着》余华，作家出版社

<h2>测试效果</h2>

算不上流畅播放，但是已经达到了惊人的 $1$ 帧每秒，毕竟一帧能看，两帧流畅，三帧电竞（~~逃~~）。

<h2>现状</h2>

由于写代码的时间比上写readme的时间高达惊人的 $1:9$ ，目前还有以下问题没有解决（~~毕竟代码可以丑可以菜，readme一定要显得用心~~）：

```csharp
//Main.cs
private async void setNotifyIconAsync()
{
    await Task.Run(() => {
        byte[] oldImage = new byte[3600];
        while(true)
        {
            //imageData是一个队列，是线程不安全的，因此在异步方法中访问之前需要上锁
            mutex.WaitOne();
            if (imageData.Count > 0)
            {
                var img = imageData.Dequeue();
                if(bytesEqual(ref oldImage, ref img))
                {
                    notifyIcon.Icon = bitmapToIcon(ToGrayBitmap(img, 60, 60));
                    oldImage = img;
                }
            }
            //释放锁
            mutex.ReleaseMutex();
        }
    });
}
```

其中，
```csharp
notifyIcon.Icon = bitmapToIcon(ToGrayBitmap(img, 60, 60));
```
将消息队列中的字节数组转换为```Bitmap```，再转换为```Icon```，最后赋值给```notifyIcon```的```Icon```属性字段。但是快速地刷新该属性字段会导致文件资源管理器慢响应、不响应甚至崩溃。

<h2>写在最后</h2>

随缘写吧，毕竟太菜。另外关注[@嘉然](https://space.bilibili.com/672328094)，顿顿解馋！关注[@永雏塔菲](https://space.bilibili.com/1265680561)喵，关注永雏塔菲谢谢喵！
