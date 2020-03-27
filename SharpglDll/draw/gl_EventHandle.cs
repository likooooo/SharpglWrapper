using PointcloudWrapper;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LK.Caculation;
using System.Drawing;

namespace SharpglWrapper.draw
{
    public class Event_gl_MouseWheel 
    {
        gl_graw graw;

        bool register;
        public bool Register
        {
            get => register;
            private set
            {
                if (register == value) return;
                register = value;
                if (register)
                    graw.WindowHandle.MouseWheel += new MouseEventHandler(SW_MouseWheel);
                else
                    graw.WindowHandle.MouseWheel -= new MouseEventHandler(SW_MouseWheel);
            }
        }


        public Event_gl_MouseWheel(gl_graw graw)
        {
            this.graw = graw;
            register = false;
            Register = true;
        }

        void SW_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                graw.Scala -= graw.zoomStep;
            else
                graw.Scala += graw.zoomStep;
            graw.Scala = graw.Scala <= 0 ? 1 : graw.Scala;
        }
    }
    public class Event_gl_MouseMove
    {
        gl_graw graw;
        bool register;
        public bool Register
        {
            get => register;
            private set
            {
                if (register == value) return;
                register = value;
                if (register)
                {
                    graw.WindowHandle.MouseDown += new MouseEventHandler(SW_MouseDown);
                    graw.WindowHandle.MouseMove += new MouseEventHandler(SW_MouseMove);
                    graw.WindowHandle.MouseUp += new MouseEventHandler(SW_MouseUp);
                }
                else
                {
                    graw.WindowHandle.MouseDown -= new MouseEventHandler(SW_MouseDown);
                    graw.WindowHandle.MouseMove -= new MouseEventHandler(SW_MouseMove);
                    graw.WindowHandle.MouseUp -= new MouseEventHandler(SW_MouseUp);
                }
            }
        }
        bool mouseDown;
        int clickX, clickY;
        public Event_gl_MouseMove(gl_graw graw) 
        {
            this.graw = graw;
            register = false;
            Register = true;
        }
        public void Dispose() => Register = false;

        void SW_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            clickX = e.X;
            clickY = e.Y;
        }
        void SW_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                //每次移动的图像距离
                float yMove = (e.X - clickX) * 1f / graw.WindowHandle.Width;
                float xMove = (e.Y - clickY) * 1f / graw.WindowHandle.Height;
                clickX = e.X;
                clickY = e.Y;
                graw.cam.RotateX += 180f * xMove;
                graw.cam.RotateY += 180f * yMove;
            }
        }
        void SW_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
    public class Event_gl_Draw
    {
        gl_graw graw;
        bool register;
        public bool Register
        {
            get => register;
            private set
            {
                if (register == value) return;
                register = value;
                if (register)
                {
                    graw.WindowHandle.OpenGLInitialized += new EventHandler(SW_OpenGLInit);
                    graw.WindowHandle.OpenGLDraw += new RenderEventHandler(SW_OpenGLDraw);
                    graw.WindowHandle.Resize += new EventHandler(SW_Resize);
                }

                else
                {
                    graw.WindowHandle.OpenGLInitialized -= new EventHandler(SW_OpenGLInit);
                    graw.WindowHandle.OpenGLDraw -= new RenderEventHandler(SW_OpenGLDraw);
                    graw.WindowHandle.Resize -= new EventHandler(SW_Resize);
                }
            }
        }



        public Event_gl_Draw(gl_graw graw)
        {
            this.graw = graw;
            register = false;
            Register = true;
        }


        public void SW_OpenGLDraw(object sender, RenderEventArgs args)
        {

            //graw.Gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //// Black background        
            //graw.Gl.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);

            //graw.Gl.LoadIdentity();
            ////graw.cam.UpdateRotate();
            ////graw.cam.UpdateMove();
            //graw.Gl.Translate(0, 0, 5);
            //graw.Gl.Scale(graw.Scala, graw.Scala, graw.Scala);
            //graw.data.drawMod = OpenGL.GL_POINTS;
            //graw.Gl.PushMatrix();
            ////graw.Gl.Viewport()
            //graw.Gl.Begin(graw.mod);

            ////graw.Draw();
            //graw.Gl.End();

            //graw.Gl.PopMatrix();


            SharpGL.OpenGL gl = graw.Gl;
            //清除深度缓存 
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);


            //重置当前指定的矩阵为单位矩阵,将当前的用户坐标系的原点移到了屏幕中心
            gl.LoadIdentity();

            //坐标轴变换位置到(-1.5,0,-6)
            var minX = graw.data.stl_Trangle.Select(s => s.X.Min()).Min();
            var maxX = graw.data.stl_Trangle.Select(s => s.X.Max()).Max();
            var minY = graw.data.stl_Trangle.Select(s => s.Y.Min()).Min();
            var maxY = graw.data.stl_Trangle.Select(s => s.Y.Max()).Max();
            var minZ = graw.data.stl_Trangle.Select(s => s.Z.Min()).Min();
            var maxZ = graw.data.stl_Trangle.Select(s => s.Z.Max()).Max();
            //gl.Translate((maxX + minX) / 2, (maxY + minY) / 2, -6*(maxY-minY));

            //1.将x,y移动至点xy中心
            //2.z比x长3倍
            //gl.Translate(0, 0, -6);
            double stepX = (maxX - minX) / 2;
            double stepY = (maxY - minY) / 2;
            var x = graw.data.stl_Trangle[0].X;
            var y = graw.data.stl_Trangle[0].Y;
            var z = graw.data.stl_Trangle[0].Z;
            graw.cam.UpdateRotate();
            graw.cam.UpdateMove();
            gl.LookAt(minX + stepX, minY + stepY, maxZ, minX + stepX, minY + stepY, minZ, 0, 1, 0);
            gl.Begin(OpenGL.GL_TRIANGLES);
            {
                ////顶点 
                //gl.Vertex(0.0f, 1.0f, 1.0f);
                ////左端点      
                //gl.Vertex(-1.0f, -1.0f, 1.0f);
                ////右端点       
                //gl.Vertex(1.0f, -1.0f, 1.0f);
                graw.Draw();
            }
            gl.End();

            gl.Flush();   //强制刷新


            //graw.Gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);     //设置窗口将被清除成黑色
            //graw.Gl.MatrixMode(OpenGL.GL_PROJECTION);           //将当前矩阵指定为投影矩阵
            //graw.Gl.LoadIdentity();                      //把当前矩阵设为单位矩阵
            //graw.Gl.Ortho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);//指定绘制图像时使用的坐标系统 glOrtho(Xmin,Xmax,Ymin,Ymax,Zmin,Zmax);  

            //graw.Gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT);  //清除颜色缓冲区

            //graw.Gl.Color(1.0, 1.0, 1.0);      //设置绘制的颜色为白色（红，绿，蓝，）
            //graw.Gl.PointSize(1);
            //graw.Gl.Begin(OpenGL.GL_POLYGON);            //标志着一个顶点数据列表的开始，GL_POLYGON表示绘制简单的凸多边形
            //graw.Gl.Vertex(0.25, 0.25, 0.0);
            //graw.Gl.Vertex(0.75, 0.25, 0.0);
            //graw.Gl.Vertex(0.75, 0.75, 0.0);
            //graw.Gl.Vertex(0.25, 0.75, 0.0);
            //graw.Gl.End();                        //标志着一个顶点数据列表的结束

            //graw.Gl.Flush();
        }

        public void SW_OpenGLInit(object sender, EventArgs args)
        {
            graw.Gl.ClearColor(0, 0, 0, 0);
            graw.Gl.Viewport(0, 0, graw.WindowHandle.Width/2, graw.WindowHandle.Height);
        }
        public void SW_Resize(object sender, EventArgs e)
        {           
            graw.Gl.Viewport(0, 0, graw.WindowHandle.Width , graw.WindowHandle.Height);

            //OpenGL gl = graw.Gl;

            ////  设置当前矩阵模式,对投影矩阵应用随后的矩阵操作
            //gl.MatrixMode(OpenGL.GL_PROJECTION);

            //// 重置当前指定的矩阵为单位矩阵,将当前的用户坐标系的原点移到了屏幕中心
            //gl.LoadIdentity();

            //// 创建透视投影变换
            //gl.Perspective(30.0f, (double)graw.WindowHandle.Width / (double)graw.WindowHandle.Height, 5, 100.0);

            //// 视点变换
            //gl.LookAt(-5, 5, -5, 0, 0, 0, 0, 1, 0);

            //// 设置当前矩阵为模型视图矩阵
            //gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }
    }

}
