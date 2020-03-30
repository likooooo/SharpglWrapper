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
            Window.sample = 10;

            gl_object model = obj.GetStlSurface(0,0,
                Window.param_LookAt.eyez);
            Window1 = new SharpglWindow(this.openGLControl2, model);
            {
                Window1.sample = 10;
                Window1.param_LookAt = Window.param_LookAt;
            }
        }

        private void openGLControl2_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = "(x,y):(" + e.X + "," + e.Y + ")";
        }

        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            SharpGL.OpenGL gl = this.openGLControl1.OpenGL;
            //清除深度缓存 
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //重置当前指定的矩阵为单位矩阵,将当前的用户坐标系的原点移到了屏幕中心
            gl.LoadIdentity();

            //坐标轴变换位置到(-1.5,0,-6)
            gl.LineWidth(3);
            gl.LookAt(0, 0, 3, 0, 0, -1, 0, 1, 0);
            gl.Ortho(-3, 3, -3, 3, -3, 3);
            gl.Begin(OpenGL.GL_LINES);

            gl.Color(0.3, 0.3, 0.3);
            gl.Vertex(0.0f, 0f, 0.0f);
            gl.Vertex(1, 1, 0);

            gl.Color(1, 0.3, 0.3);
            gl.Vertex(0.0f, 0f, 0.0f);
            gl.Vertex(0, -2, 0);
            gl.End();
        
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
 
        }
    }
}
