using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace SharpglWrapper
{
    public class ascColor : vector3
    {
        public ascColor(string r, string g, string b) : base(r, g, b)
        { }
      
    }
    public class ascPoint : vector3
    {
        public ascPoint(string x, string y, string z) : base(x, y, z)
        { }
        public ascPoint(double x, double y, double z) : base(x, y, z)
        { }
    }
    public class asc
    {
        public List<ascColor> ascColor;
        public List<ascPoint> ascPoint;

        public double[] X { get => ascPoint.Select(s => s.x).ToArray(); }
        public double[] Y { get => ascPoint.Select(s => s.y).ToArray(); }
        public double[] Z { get => ascPoint.Select(s => s.z).ToArray(); }

        public (double minX, double maxX, double minY, double maxY, double minZ, double maxZ) BoundingBox
        { get; private set; }

        public asc(string ascFile)
        {
            ascPoint = new List<ascPoint>();
            ascColor = new List<ascColor>();
            using (StreamReader sr = new StreamReader(ascFile))
            {
                string line;
                while (!(line = sr.ReadLine()).Contains("nd"))
                {
                    var s = line.Split(' ');
                    ascPoint.Add(new ascPoint(s[0], s[1], s[2]));
                    ascColor.Add(new ascColor(s[3], s[4], s[5]));
                }
            }

            var x = this.X;
            var y = this.Y;
            var z = this.Z;
            BoundingBox = (x.Min(), x.Max(), y.Min(), y.Max(), z.Min(), z.Max());
        }
        public asc(List<ascPoint> points)
        {
            this.ascPoint = points.ToArray().ToList();
            ascColor[] ascColors = new ascColor[ascPoint.Count];
            this.ascColor = ascColors.ToList();
            var x = this.X;
            var y = this.Y;
            var z = this.Z;
            BoundingBox = (x.Min(), x.Max(), y.Min(), y.Max(), z.Min(), z.Max());
        }

        //保存asc文件
        public void save(string ascFilePath)
        {
            //if (File.Exists(ascFilePath))
            //{
            //    Console.WriteLine(ascFilePath + ":是否覆盖该文件：y/n");
            //    while (true)
            //    {
            //        var key = Console.ReadKey();
            //        if (key.Key == ConsoleKey.Y) break;
            //        else if (key.Key == ConsoleKey.N) goto end;
            //        Console.WriteLine("y/n?");
            //    }
            //}
            if (ascPoint.Count <= 0) return;
            using (StreamWriter sw = new StreamWriter(ascFilePath))
            {
                Console.WriteLine("正在写入stl文件：" + ascFilePath);
                foreach (var v in ascPoint)
                {
                    string str = v.ToString();
                    sw.WriteLine(str);
                }
                sw.WriteLine("End Clouds");
                Console.WriteLine("asc文件写入完成...");
            }
            //end:;

        }
    }
}
