using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SharpglWrapper
{
    public class stl
    {
        string solidName;
        public List<stl_trangleVector> pointsArry { get; private set; }
        public List<stl_facetVector> facetArry { get; private set; }

        public double[] X { get => pointsArry.Select(s => s.x).ToArray(); }
        public double[] Y { get => pointsArry.Select(s => s.y).ToArray(); }
        public double[] Z { get => pointsArry.Select(s => s.z).ToArray(); }

        public (double minX, double maxX, double minY, double maxY, double minZ, double maxZ) BoundingBox
        { get; private set; }

        private stl() { }
        public stl(string stlfilepath)
        {
            string line;
        
            facetArry = new List<stl_facetVector>();
            pointsArry = new List<stl_trangleVector>();
            using (StreamReader sr = new StreamReader(stlfilepath, Encoding.Default))
            {
                line = sr.ReadLine();
                solidName = line.Remove(0, 6);

                while ((line = sr.ReadLine()) != "endsolid")
                {
                    stl_facetVector facet = new stl_facetVector(line);
                    sr.ReadLine();

                    line = sr.ReadLine();
                    var p0 = new stl_trangleVector(line);
                    //if (pointsMap.ContainsKey(p0))
                    //    pointsMap[p0]++;
                    //else
                    //    pointsMap.Add(p0, 1);

                    line = sr.ReadLine();
                    var p1 = new stl_trangleVector(line);
                    //if (pointsMap.ContainsKey(p1))
                    //    pointsMap[p1]++;
                    //else
                    //    pointsMap.Add(p1, 1);

                    line = sr.ReadLine();
                    var p2 = new stl_trangleVector(line);
                    //if (pointsMap.ContainsKey(p2))
                    //    pointsMap[p2]++;
                    //else
                    //    pointsMap.Add(p2, 1);

                    sr.ReadLine();
                    sr.ReadLine();


                    pointsArry.Add(p0);
                    pointsArry.Add(p1);
                    pointsArry.Add(p2);
                    facetArry.Add(facet);
                }
            }

            var x = this.X;
            var y = this.Y;
            var z = this.Z;
            BoundingBox = (x.Min(), x.Max(), y.Min(), y.Max(), z.Min(), z.Max());
        }


        public stl MoveModel2Zero()
        {
            stl a = new stl();
            a.facetArry = null;
            return a;
        }

        public string[] stl_trangleVector2AscString()
        {
            var line = pointsArry.Select(s => s.ToString()).ToArray();
            return line;
        }

    }
}
