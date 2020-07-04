using SharpGL;
using System;
using System.Windows.Forms;

namespace SharpglWrapper.Viewer
{
    public class Event_gl_MouseWheel
    {
        gl_Viewer viewer;
        double mouseWheelStep;
        public double minVal = 0.2;
        public double maxVal = 2.5;
        bool register;
        public bool Register
        {
            get => register;
            private set
            {
                if (register == value) return;
                register = value;
                if (register)
                    viewer.windowHandle.MouseWheel += new MouseEventHandler(SW_MouseWheel);
                else
                    viewer.windowHandle.MouseWheel -= new MouseEventHandler(SW_MouseWheel);
            }
        }


        public Event_gl_MouseWheel(gl_Viewer viewer)
        {
            this.viewer = viewer;
            mouseWheelStep = 0.1;
            register = false;
            Register = true;
        }

        void SW_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                viewer.Scala -= mouseWheelStep;
            else
                viewer.Scala += mouseWheelStep;
            viewer.Scala = viewer.Scala < minVal ? minVal : viewer.Scala;
            viewer.Scala = viewer.Scala > maxVal ? maxVal : viewer.Scala;
        }
    }
    public class Event_gl_MouseMove
    {
        gl_Viewer viewer;
        public float moveStep = 0.02f;//每次旋转或者移动
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
                    viewer.windowHandle.MouseDown += new MouseEventHandler(SW_MouseDown);
                    viewer.windowHandle.MouseMove += new MouseEventHandler(SW_MouseMove);
                    viewer.windowHandle.MouseUp += new MouseEventHandler(SW_MouseUp);
                    viewer.windowHandle.KeyDown += new KeyEventHandler(SW_KeyDown);
                    viewer.windowHandle.KeyUp += new KeyEventHandler(SW_KeyUp);
                }
                else
                {
                    viewer.windowHandle.MouseDown -= new MouseEventHandler(SW_MouseDown);
                    viewer.windowHandle.MouseMove -= new MouseEventHandler(SW_MouseMove);
                    viewer.windowHandle.MouseUp -= new MouseEventHandler(SW_MouseUp);
                    viewer.windowHandle.KeyDown -= new KeyEventHandler(SW_KeyDown);
                    viewer.windowHandle.KeyUp -= new KeyEventHandler(SW_KeyUp);
                }
            }
        }
        bool mouseDown;
        int clickX, clickY;
        public Event_gl_MouseMove(gl_Viewer viewer) 
        {
            this.viewer = viewer;
            register = false;
            Register = true;
        }
        public void Dispose() => Register = false;

        bool pressShift2Move = false;
        void SW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
                pressShift2Move = true;
        }
        void SW_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
                pressShift2Move = false;

        }
        void SW_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            clickX = e.X;
            clickY = e.Y;
        }
        void SW_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown) return;
            //移动
            if (pressShift2Move)
            {
                if (viewer.param_LookAt.upy == 1)
                {
                    //移动说明
                    //  |----> xMove
                    //  |
                    //  |
                    //  yMove 
                    //图像说明
                    //    y
                    //    |
                    //--------->x
                    //    |
                    //    

                    double xMove = (e.X - clickX) > 0 ? moveStep : -moveStep;
                    double yMove = (e.Y - clickY) > 0 ? -moveStep : moveStep; //(e.Y - clickY) * 1f / viewer.windowHandle.Height;
                    clickX = e.X;
                    clickY = e.Y;
                    double xLength = viewer.param_Ortho.maxX - viewer.param_Ortho.minX;
                    double yLength = viewer.param_Ortho.maxY - viewer.param_Ortho.minY;

                    viewer.TranslateX += xLength * xMove;
                    viewer.TranslateY += yLength * yMove;

                    //x轴防止越界
                    if (viewer.TranslateX > viewer.param_Ortho.maxX)
                        viewer.TranslateX = viewer.param_Ortho.maxX;
                    else if (viewer.TranslateX < viewer.param_Ortho.minX)
                        viewer.TranslateX = viewer.param_Ortho.minX;
                    //y轴防止越界
                    if (viewer.TranslateY > viewer.param_Ortho.maxY)
                        viewer.TranslateY = viewer.param_Ortho.maxY;
                    else if (viewer.TranslateY < viewer.param_Ortho.minY)
                        viewer.TranslateY = viewer.param_Ortho.minY;


                    goto endShiftMove;
                }
                //upx+-1 upz+-1
                endShiftMove:;
            }
            else //旋转
            {
              
                {
                    //每次移动的图像距离
                    //float yMove = (e.X - clickX) * 1f / viewer.windowHandle.Width;
                    //float xMove = (e.Y - clickY) * 1f / viewer.windowHandle.Height;
                    float xMove = (e.X - clickX) > 0 ? moveStep : -moveStep;
                    float yMove = (e.Y - clickY) > 0 ? -moveStep : -moveStep;
                    clickX = e.X;
                    clickY = e.Y;

                    //鼠标右移->绕竖直轴旋转
                    viewer.RotateX += 180f * yMove;
                    viewer.RotateY += 180f * xMove;
                    viewer.RotateX %= 360;
                    viewer.RotateY %= 360;
                }
            }
        }
        void SW_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
    public class Event_gl_Draw
    {
        gl_Viewer viewer;
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
                    viewer.windowHandle.OpenGLInitialized += new EventHandler(SW_OpenGLInit);
                    viewer.windowHandle.OpenGLDraw += new RenderEventHandler(SW_OpenGLDraw);
                    viewer.windowHandle.Resize += new EventHandler(SW_Resize);
                }

                else
                {
                    viewer.windowHandle.OpenGLInitialized -= new EventHandler(SW_OpenGLInit);
                    viewer.windowHandle.OpenGLDraw -= new RenderEventHandler(SW_OpenGLDraw);
                    viewer.windowHandle.Resize -= new EventHandler(SW_Resize);
                }
            }
        }


        public Event_gl_Draw(gl_Viewer viewer)
        {
            this.viewer = viewer;
            register = false;
            Register = true;
        }

        public void SW_OpenGLDraw(object sender, RenderEventArgs args)
        {
            viewer.gl_Flush();
        }

        public void SW_OpenGLInit(object sender, EventArgs args)
        {
            viewer.param_Viewport.Flush(viewer.GL);
        }

        public void SW_Resize(object sender, EventArgs e)
        {
            viewer.UpdataWindow();

            viewer.param_Viewport.Flush(viewer.GL);
        }
        


     
    }

}
