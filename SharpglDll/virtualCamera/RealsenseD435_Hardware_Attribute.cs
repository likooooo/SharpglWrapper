using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraAttribute
{
    public class RealsenseD435_Hardware_Attribute
    {
        public readonly Physical Physical;
        public readonly RGB RGB;
        public readonly Depth Depth;

        public RealsenseD435_Hardware_Attribute()
        {
            Physical = new Physical()
            { Length = 90, Depth = 25, Height = 25 };

            var FOV = new FOV() { H = 69.4, V = 42.5, D = 77 };
            RGB = new RGB()
            {
                SensorResolutionWidth = 1920,
                SensorResolutionHeight = 1080,
                FrameRate = 30,
                FOV = new FOV() { H = 69.4, V = 42.5, D = 77 }
            };

            Depth = new Depth()
            {
                SensorResolutionWidth = 1280,
                SensorResolutionHeight = 720,
                FrameRate = 90,
                FOV = new FOV() { H = 87, V = 95, D = 58 }
            };
        }
    }
}
