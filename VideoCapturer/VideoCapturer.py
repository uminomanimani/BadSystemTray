import numpy as np
import cv2
import socket
import matplotlib.pyplot as plt

addrs = dict()
server = socket.socket()
server.bind(("127.0.0.1", 10086))
server.listen()
i = 0
while i < 49:
    s, addr = server.accept()
    data = s.recv(1)
    data = list(data)
    print(data)
    print(addr)
    i = i + 1
    addrs[data[0]] = s
print("Handshake Over.")

def windowSlide(frame : np.ndarray, size : int, stride : int) -> list:
    slicedImg = list()
    posX = 0
    while(posX + size < frame.shape[0]):
        posY = 0
        while(posY + size < frame.shape[1]):
            img = frame[posX : posX + size, posY : posY + size]
            # img = img.flatten()
            slicedImg.append(img.tobytes())
            posY = posY + size + stride
        posX = posX + size + stride
    return slicedImg

def main():
    
    input()

    vc = cv2.VideoCapture("C:/Users/ma_li/Desktop/BadAppleSystemTray/BadAppleSystemTray/VideoCapturer/Bad Apple.mp4")
    b = vc.isOpened()
    fps = vc.get(cv2.CAP_PROP_FPS)
    idx = 0
    freq = 5
    while True:
        ret, img = vc.read()
        if img is None:
            break
        img = img[:, 180:1440-180, :]
        img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        l = windowSlide(img, 100 * 1080 // 1792, 182 * 1080 // 1792)
        for i in range(len(l)):
            addrs[i].send(l[i])
        del img
    
    vc.release()
    pass

main()

server.close()
