#include "stdafx.h"

bool fileExist(std::string file)
{
    std::ifstream f(file);
    bool r = f.good();
    f.close();
    return r;
}

int argParser(int argc, char* argv[])
{
    //usage : Starter -p PyFlePath -n SystemTrayAppNum
    int r = -1;
    if (argc < 5 || std::string(argv[1]) != std::string("-p") || std::string(argv[3]) != std::string("-n"))
    {
        std::endl(std::cerr << "Usage: Starter -p [the path of the Python Script] -n [the number of Tray Applications]");
        return r;
    }
    try
    {
        r = std::stoi(argv[4]);
    }
    catch (const std::invalid_argument&)
    {
        r = -1;
    }
    return r >= 0 ? r : -1;
}

int main(int argc, char* argv[])
{
    std::thread([]()->void { system("python.exe VideoCapturer.py"); }).detach();
    std::thread([]()->void { system("SystemTrayApp.exe"); }).detach();
    while(true)
    {
        Sleep(50000);
    }
    return 0;
}