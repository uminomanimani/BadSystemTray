import numpy as np
import cv2
import socket
import matplotlib.pyplot as plt

server = socket.socket()
server.bind(("127.0.0.1", 10086))
server.listen()

s, addr = server.accept()
print(addr)

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
    
    print("input anything to start:", end="")
    input()

    vc = cv2.VideoCapture("./Bad Apple.mp4")
    b = vc.isOpened()

    # fps = vc.get(cv2.CAP_PROP_FPS)
    c = 0
    while True:
        ret, img = vc.read()
        if img is None:
            break
        img = img[:, 180:1440-180, :]
        img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        l = windowSlide(img, 100 * 1080 // 1792, 182 * 1080 // 1792)
        for i in range(len(l)):
            s.send(i.to_bytes())
            s.send(l[i])
        print(f"\t发送了第{c}帧。")
        c = c + 1
        del img
    
    vc.release()
    pass

main()

server.close()
