using GeneralUI;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

using System;

namespace DrawRectangleWithElementBufferObj
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSetting = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "LearnOpenTK"
            };

            using (var window = new RootWindow(GameWindowSettings.Default, nativeWindowSetting))
            {
                window.Run();
            }
        }
    }
}
