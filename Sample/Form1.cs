using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using PointcloudWrapper;
namespace Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SharpglWrapper.SharpglWrapper sharpgl = new SharpglWrapper.SharpglWrapper(this.openGLControl1);
            var arryX0 = OpenFile("绝缘子三维扫描系统_Htuple_x.txt");
            var arryY0 = OpenFile("绝缘子三维扫描系统_Htuple_y.txt");
            var arryZ0 = OpenFile("绝缘子三维扫描系统_Htuple_z.txt");
            sharpgl.SetDraw(new PointcloudF(arryX0, arryY0, arryZ0));
            sharpgl.AddEventMove();
            sharpgl.AddEventZoom();
         }
        public static float[] OpenFile(string path)
        {
            List<float> arry;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(System.IO.File.OpenRead(path)))
            {
                int lineCount = Convert.ToInt32(sr.ReadLine());
                arry = new List<float>();
                string temp;
                for (int i = 0; i < lineCount; i++)
                {
                    try
                    {
                        temp = sr.ReadLine();
                        if (temp == null)
                            break;
                        temp = (temp.Split(' ')).Last();
                        arry.Add(float.Parse(temp));
                    }
                    catch { }
                }
            }
            return arry.ToArray();
        }
    }
}
