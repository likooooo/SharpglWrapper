using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using PointcloudWrapper;
using SharpglWrapper;
namespace Sample
{
    public partial class Form1 : Form
    {
        List<SharpglPointcloud> pointArry;


        private int currentPage;
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                if (value >= pointArry.Count)
                    value = 0;
                else if (value < 0)
                    value = pointArry.Count - 1;
                currentPage = value;
                sharpgl.SetDraw(pointArry[currentPage]);
            }
        }
        SharpglWrapper.SharpglWrapper sharpgl;


        public Form1()
        {
            InitializeComponent();
            sharpgl = new SharpglWrapper.SharpglWrapper(this.openGLControl1);
            sharpgl.AddEventMove();
            sharpgl.AddEventZoom();
            pointArry = new List<SharpglPointcloud>();
            for (int i = 0; i < 3; i++)
            {
                pointArry.Add(
                    new SharpglPointcloud(
                         OpenFile("X" + i.ToString() + ".txt"),
                          OpenFile("Y" + i.ToString() + ".txt"),
                           OpenFile("Z" + i.ToString() + ".txt"))
                    );
                pointArry[i].IsRenderColor = true;
            }
            currentPage = -1;
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


        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            this.Text = "当前点云ID：" + CurrentPage.ToString();
        }
    }
}
