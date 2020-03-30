using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraAttribute
{
    public struct FOV
    {
        public double H, V, D;
    }
    public struct Depth
    {
        public double MinDepthDistance;
        public int SensorResolutionWidth, SensorResolutionHeight;
        public double FrameRate;
        public FOV FOV;
    }
    public struct RGB
    {
        public int SensorResolutionWidth, SensorResolutionHeight;
        public double FrameRate;
        public double pixelSize;
        public FOV FOV;
    }
    public struct Physical
    {
        public double Length, Depth, Height;
        public string Measure;
    }
}
