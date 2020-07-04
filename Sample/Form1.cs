using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using SharpglWrapper.Viewer;
using SharpglWrapper;
using SharpGL;

namespace Sample
{
    public partial class Form1 : Form
    {
        SharpglWindow Window, Window1;

        public Form1()
        {
            InitializeComponent();
            //this.openGLControl1.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl1_OpenGLDraw);
            //(int x, int y, int z)[] gg = { (1, 1, 1), (2, 2, 2), (3, 3, 3) };
            //var v = gg.GetType().GetProperties().Select
            //    (s => (
            //             (((int, int, int))s.GetValue(gg)).GetType().GetProperties().Select
            //             (ss => (
            //                        (int)ss.GetValue(s)
            //                )).ToList()
            //          )
            //    );


            Window = new SharpglWindow(this.openGLControl1);
            var stl = new stl(@"C:\Users\lk\Desktop\点云样本\绝缘子带支架\绝缘子加三角支架中精.STL");
            var obj = new gl_object(stl);
            Window.Updata(obj);
            Window.sample = 1;
            Window.displayViewRange = true;

            gl_object model = obj.GetStlSurface(
                Window.param_LookAt.eyex,
                Window.param_LookAt.eyey,
                 Window.param_LookAt.eyez);
            Window1 = new SharpglWindow(this.openGLControl2, model);
            {
                Window1.sample = 10;
                Window1.displayViewRange = true;
                Window1.param_LookAt = Window.param_LookAt;
            }
        }

        private void btnEnableCamLine_Click(object sender, EventArgs e)
        {
            string enableStr = "打开相机视野线";
            string disableStr = "关闭相机视野线";
            if (btnEnableCamLine.Text == enableStr)
            {
                Window.displayViewRange = true;
                btnEnableCamLine.Text = disableStr;
            }
            else
            {
                Window.displayViewRange = false;
                btnEnableCamLine.Text = enableStr;
            }
        }

        private void openGLControl2_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = "(x,y):(" + e.X + "," + e.Y + ")";
        }

    }
}
