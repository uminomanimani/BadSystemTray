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

- Windows 10（Windows 11改变了系统托盘隐藏区的排放方式，  $49$ 个图标不按照 $7 \times 7$ 来摆放了…）
- Visual Studio（2022）
- .NET Framework（4.8）
- C# 8.0
- [速效救心丸](https://item.jkcsjd.com/3810395.html)
- [米诺地尔生发酊](https://item.yiyaojd.com/100009773041.html)
- [《活着》](http://product.dangdang.com/1612701486.html)余华，作家出版社


## 原理

通过OpenCV逐帧读取视频，转换成灰度图并切割分发至 $7 \times 7 = 49$ 个notifyIcon控件：

```csharp
//Main.Designer.cs
private System.Windows.Forms.NotifyIcon[] notifyIcons = new System.Windows.Forms.NotifyIcon[49];
```

## 理想效果

<div align="center"><img src="./image/expected.png" alt=""></div>

理想是丰满的，现实是骨感的。

## 现状

还没有解决的问题：
- OpenCV转换后的灰度图，本来应该是白色的地方变成了黄色；
- Windows在系统托盘隐藏区域的行为比较迷惑，```NotifyIcon```的排列比较随机。

## 写在最后

目前还没想到对策，随缘改吧，毕竟太菜。
