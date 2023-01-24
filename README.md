<h1>一个在系统托盘图标阵列中播放Bad Apple!!的项目</h1>

目前还有以下问题没有解决：

```csharp
//Main.cs
private async void setNotifyIconAsync()
{
    await Task.Run(() => {
        byte[] oldImage = new byte[3600];
        while(true)
        {
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
            mutex.ReleaseMutex();
        }
    });
}
```

其中，
```csharp
notifyIcon.Icon = bitmapToIcon(ToGrayBitmap(img, 60, 60));
```
将消息队列中的字节数组转换为Bitmap，再转换为Icon，最后赋值给notifyIcon的Icon属性字段。但是快速地刷新该属性字段会导致文件资源管理器不响应甚至崩溃。

就这样吧，随缘解决，太菜了…
