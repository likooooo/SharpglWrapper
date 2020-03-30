using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpglWrapper
{
    public class stl_facetVector : vector3
    {
        public stl_facetVector(string facetNormal, string pattern = "   facet normal ")
            :base(facetNormal.Remove(0, pattern.Length))
        {
     
        }
    }
    public class stl_trangleVector : vector3
    {
        public stl_trangleVector(string stl_trangleVector, string pattern = "         vertex ")
       : base(stl_trangleVector.Remove(0, pattern.Length))
        {

        }
    }
}
