using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpglWrapper
{
    public class stl_trangle
    {
        public vector3 point0;
        public vector3 point1;
        public vector3 point2;
        public double[] X
        {
            get => new double[] { point0.x, point1.x, point2.x };
            //set =>
        }
        public double[] Y { get => new double[] { point0.y, point1.y, point2.y }; }
        public double[] Z { get => new double[] { point0.z, point1.z, point2.z }; }
        public stl_trangle(string tranglePoint0, string tranglePoint1, string tranglePoint2, string pattern = "         vertex ")
        {
            string data = tranglePoint0.Remove(0, pattern.Length);
            point0 = new vector3(data);
            data = tranglePoint1.Remove(0, pattern.Length);
            point1 = new vector3(data);
            data = tranglePoint2.Remove(0, pattern.Length);
            point2 = new vector3(data);
        }
        public stl_trangle(vector3 tranglePoint0, vector3 tranglePoint1, vector3 tranglePoint2)
        {
            point0 = tranglePoint0;
            point1 = tranglePoint1;
            point2 = tranglePoint2;
        }

        public override string ToString()
        {
            string line = point0.ToString() + Environment.NewLine;
            line += point1.ToString() + Environment.NewLine;
            line += point2.ToString();

            return line;
        }

        public double[] ToDouble()
        {
            double[] d = new double[9];
            d[0] = point0.x;
            d[1] = point0.y;
            d[2] = point0.z;
            d[3] = point1.x;
            d[4] = point1.y;
            d[5] = point1.z;
            d[6] = point2.x;
            d[7] = point2.y;
            d[8] = point2.z;
            return d;
        }
    }
}
