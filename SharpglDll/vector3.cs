using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpglWrapper
{
   public class vector3
    {
        public double x, y, z;
        public vector3(string str)
        {
            var d = str.Split(' ');
            x = Convert.ToDouble(d[0]);
            y = Convert.ToDouble(d[1]);
            z = Convert.ToDouble(d[2]);
        }
        public vector3(string x,string y,string z)
        {
            this.x = Convert.ToDouble(x);
            this.y = Convert.ToDouble(y);
            this.z = Convert.ToDouble(z);
        }
        public vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool Equals(vector3 obj)
        {
            var a = this.ToDouble();
            var b = obj.ToDouble();
            //Array.Sort(a);
            //Array.Sort(b);
            return (a[0] == b[0] && a[1] == b[1] && a[2] == b[2]) ? true : false;
        }
        //public static implicit operator ascPoint(vector3 obj)
        //    => new ascPoint(obj.x, obj.y, obj.z);

        public override string ToString()
        {
            return x.ToString() + " " + y.ToString() + " " + z.ToString() + " 0 0 0";
        }
        public double[] ToDouble() { return new double[] { x, y, z }; }

        //向量的模
        public double GetDistance()
        {
            return (Math.Sqrt(x * x + y * y + z * z));
        }

        //向量的点乘
        //v1*rightVector =x1*x2+y1*y2+z1*z2
        public double DotProduct(vector3 rightVector)
        {
            return (x * rightVector.x + y * rightVector.y + z * rightVector.z);
        }

        //向量的夹角
        // Cos(θ) = A·B /(|A|*|B|)
        public double Cos(vector3 rightVector)
        {
            var cos = DotProduct(rightVector) / (GetDistance() + rightVector.GetDistance());
            return cos;
        }

        //x,y,z
        public (double degX, double degY, double zCos) GetProjectionDeg()
        {
            double degX = Math.Acos(x / z);
            double degY = Math.Acos(y / z);
            return (degX, degY, 0);
        }
        public static explicit operator double[](vector3 v) { return new double[] { v.x, v.y, v.z }; }
    }

}
