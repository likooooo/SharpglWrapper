using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpglWrapper
{
   public struct vector3
    {
        public double x, y, z;
        public vector3(string str)
        {
            var d = str.Split(' ');
            x = Convert.ToDouble(d[0]);
            y = Convert.ToDouble(d[1]);
            z = Convert.ToDouble(d[2]);
        }

        public bool Equals(vector3 obj)
        {
            var a = this.ToDouble();
            var b = obj.ToDouble();
            //Array.Sort(a);
            //Array.Sort(b);
            return (a[0] == b[0] && a[1] == b[1] && a[2] == b[2]) ? true : false;
        }


        public override string ToString()
        {
            return x.ToString() + " " + y.ToString() + " " + z.ToString() + " 0 0 0";
        }
        public double[] ToDouble() { return new double[] { x, y, z }; }


        public static explicit operator double[](vector3 v) { return new double[] { v.x, v.y, v.z }; }
    }

}
