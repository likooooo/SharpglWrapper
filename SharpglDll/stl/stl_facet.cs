using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpglWrapper
{
    public class stl_facet
    {
        public vector3 vector;
        public double x { get => vector.x; }
        public double y { get => vector.y; }
        public double z { get => vector.z; }
        public stl_facet(string facetNormal, string pattern = "   facet normal ")
        {
            string data = facetNormal.Remove(0, pattern.Length);
            vector = new vector3(data);
        }
        public override string ToString()
        {
            return (vector.x + " " + vector.y + " " + vector.z);
        }
    }
}
