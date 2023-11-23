#include <iostream>
#include <cstdio>
#include <cstring>

using namespace std;

extern "C" int SomeFunction();

int main() {


    // Command to run FFmpeg (replace with your desired FFmpeg command)
    const char* ffmpegCommand = "-i \"D:\\kiten 2023-03-08 -.mp4\" -f rawvideo -pix_fmt bgr24 -";

    // Open a pipe to run FFmpeg as a child process
    FILE* ffmpegPipe = _popen(ffmpegCommand, "r");

    if (!ffmpegPipe) {
        std::cerr << "Error: Unable to open FFmpeg pipe." << std::endl;
        return 1;
    }

    // Buffer to store the output from FFmpeg
    char buffer[256];

    // Read and print the output from FFmpeg continuously
    while (fgets(buffer, sizeof(buffer), ffmpegPipe) != NULL) {
        std::cout << buffer;
    }

    // Close the FFmpeg process
    _pclose(ffmpegPipe);

    return 0;
}
