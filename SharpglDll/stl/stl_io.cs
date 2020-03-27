using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SharpglWrapper
{
    public class stl_io
    {
        public string solidName { get; private set; }
        public Dictionary<vector3, int> pointsMap { get; private set; }
        public stl_trangle[] stl_trangle { get; private set; }
        public stl_facet[] stl_facet { get; private set; }
        public double[] points;
        public stl_io(string filepath, string ascFilepath, string facetFilePath)
        {
            string line;
            var stl_trangle = new List<stl_trangle>();
            var stl_facet = new List<stl_facet>();

            using (StreamReader sr = new StreamReader(filepath, Encoding.Default))
            {
                line = sr.ReadLine();
                solidName = line.Remove(0, 6);
                while ((line = sr.ReadLine()) != "endsolid")
                {
                    stl_facet facet = new stl_facet(line);
                    sr.ReadLine();

                    line = sr.ReadLine();
                    line = line.Remove(0, 16);
                    var p0 = new vector3(line);

                    line = sr.ReadLine();
                    line = line.Remove(0, 16);
                    var p1 = new vector3(line);

                    line = sr.ReadLine();
                    line = line.Remove(0, 16);
                    var p2 = new vector3(line);

                    stl_trangle.Add(new stl_trangle(p0, p1, p2));
                    stl_facet.Add(facet);
                    sr.ReadLine();
                    sr.ReadLine();
                }
            }
            using (StreamWriter sw = new StreamWriter(ascFilepath))
            {
                for (int i = 0; i < stl_trangle.Count; i++)
                {
                    //setRandomPoints(stl_trangle[i]);
                    sw.WriteLine(stl_trangle[i].ToString());
                }
            }
            using (StreamWriter sw = new StreamWriter(facetFilePath))
            {
                for (int i = 0; i < stl_facet.Count; i++)
                {
                    sw.WriteLine(stl_facet[i].ToString());
                }
            }

            this.stl_trangle = stl_trangle.ToArray();
            this.stl_facet = stl_facet.ToArray();
        }

        public stl_io(string filepath, string ascFilepath)
        {
            string line;
            var stl_trangle = new List<stl_trangle>();
            var stl_facet = new List<stl_facet>();

            using (StreamReader sr = new StreamReader(filepath, Encoding.Default))
            {
                line = sr.ReadLine();
                solidName = line.Remove(0, 6);

                while ((line = sr.ReadLine()) != "endsolid")
                {
                    line = sr.ReadLine();
                    stl_facet facet = new stl_facet(line);
                    sr.ReadLine();

                    line = line.Remove(0, 16);
                    var p0 = new vector3(line);

                    line = sr.ReadLine();
                    line = line.Remove(0, 16);
                    var p1 = new vector3(line);

                    line = sr.ReadLine();
                    line = line.Remove(0, 16);
                    var p2 = new vector3(line);

                    stl_trangle.Add(new stl_trangle(p0, p1, p2));
                    stl_facet.Add(facet);
                    sr.ReadLine();
                    sr.ReadLine();
                }
            }
            using (StreamWriter sw = new StreamWriter(ascFilepath))
            {
                for (int i = 0; i < stl_trangle.Count; i++)
                {
                    setRandomPoints(stl_trangle[i]);
                    sw.WriteLine(stl_trangle[i].ToString());
                }
            }

            this.stl_trangle = stl_trangle.ToArray();
            this.stl_facet = stl_facet.ToArray();
        }
        public stl_io(string filepath)
        {
            string line;
            pointsMap = new Dictionary<vector3, int>();
            var stl_trangle = new List<stl_trangle>();
            var stl_facet = new List<stl_facet>();
            var points = new List<double>();
            using (StreamReader sr = new StreamReader(filepath, Encoding.Default))
            {
                line = sr.ReadLine();
                solidName = line.Remove(0, 6);

                while ((line = sr.ReadLine()) != "endsolid")
                {
                    stl_facet facet = new stl_facet(line);
                    sr.ReadLine();

                    line = sr.ReadLine();
                    line = line.Remove(0, 16);
                    var p0 = new vector3(line);
                    if (pointsMap.ContainsKey(p0))
                        pointsMap[p0]++;
                    else
                        pointsMap.Add(p0, 1);
                
                    line = sr.ReadLine();
                    line = line.Remove(0, 16);
                    var p1  = new vector3(line);
                    if (pointsMap.ContainsKey(p1))
                        pointsMap[p1]++;
                    else
                        pointsMap.Add(p1, 1);
              
                    line = sr.ReadLine();
                    line = line.Remove(0, 16);
                    var   p2 = new vector3(line);
                    if (pointsMap.ContainsKey(p2))
                        pointsMap[p2]++;
                    else
                        pointsMap.Add(p2, 1);
                 
                    sr.ReadLine();
                    sr.ReadLine();


                    stl_trangle.Add(new stl_trangle(p0, p1, p2));
                    stl_facet.Add(facet);
                }
            }

            this.stl_trangle = stl_trangle.ToArray();
            this.stl_facet = stl_facet.ToArray();
            this.points = points.ToArray();
            var p = stl_trangle.Select(s => s.ToDouble()).ToArray();
        }


        public void stl2asc_addRandomErro(string ascFilePath)
        {
            Random r = new Random();
            var points = pointsMap.Keys.ToArray();
            double scala;
            int pointcount = points.Length;
            //0.8～1.2倍的坐标误差
            for (int i = 0; i < pointcount; i++)
            {
                scala = (1.0d * r.Next(8, 12)) / 10.0;
                points[i].x *= scala;
                points[i].y *= scala;
                points[i].z *= scala;
            }
            //增加随机点      

            stl2asc(ascFilePath, points);
        }

        void setRandomPoints(stl_trangle trangle)
        {
            Random r = new Random();
            double minX, maxX, minY, maxY, minZ, maxZ;
            minX = trangle.X.Min();
            minY = trangle.Y.Min();
            minZ = trangle.Z.Min();
            maxX = trangle.X.Max();
            maxY = trangle.Y.Max();
            maxZ = trangle.Z.Max();

            double scala = r.NextDouble();
            trangle.point0.x = minX + (maxX - minX) * scala;
            scala = r.NextDouble();
            trangle.point0.y = minY + (maxY - minY) * scala;
            scala = r.NextDouble();
            trangle.point0.z = minZ + (maxZ - minZ) * scala;


            scala = r.NextDouble();
            trangle.point1.x = minX + (maxX - minX) * scala;
            scala = r.NextDouble();
            trangle.point1.y = minY + (maxY - minY) * scala;
            scala = r.NextDouble();
            trangle.point1.z = minZ + (maxZ - minZ) * scala;

            scala = r.NextDouble();
            trangle.point2.x = minX + (maxX - minX) * scala;
            scala = r.NextDouble();
            trangle.point2.y = minY + (maxY - minY) * scala;
            scala = r.NextDouble();
            trangle.point2.z = minZ + (maxZ - minZ) * scala;
        }

        //保存asc文件
        void stl2asc(string ascFilePath, IEnumerable<vector3> points)
        {
            if (File.Exists(ascFilePath))
            {
                Console.WriteLine(ascFilePath + ":是否覆盖该文件：y/n");
                while (true)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Y) break;
                    else if (key.Key == ConsoleKey.N) goto end;
                    Console.WriteLine("y/n?");
                }
            }

            using (StreamWriter sw = new StreamWriter(ascFilePath))
            {
                Console.WriteLine("正在写入stl文件：" + ascFilePath);
                foreach (var v in points)
                {
                    string str = v.ToString();
                    sw.WriteLine(str);
                }
                sw.WriteLine("End Clouds：" + solidName);
                Console.WriteLine("asc文件写入完成...");
            }
            end:;

        }

        public void stl2asc(string ascFilePath) { stl2asc(ascFilePath, this.pointsMap.Keys); }
    }

}
