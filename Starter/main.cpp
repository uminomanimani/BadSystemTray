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
     if (argParser(argc, argv) == -1)
         return 0;
     if(!fileExist(std::string(argv[2])))
     {
         std::endl(std::cerr << argv[2] << " does not exist.");
         return 0;
     }
     //run the python script asynchronically
     std::string app = "python.exe ";
     std::thread([=]()-> void { system((app + std::string(argv[2])).c_str()); }).detach();

    //run the CSharp Application asynchronically
    for(int i = std::stoi(argv[4]); i >= 0; --i)
    {
        std::string app = "SystemTrayApp.exe ";
        app += std::to_string(i);
        std::thread([=]()-> void{ system(app.c_str()); }).detach();
        Sleep(100);
    }
    while(true)
    {
        Sleep(50000);
    }
    return 0;
}