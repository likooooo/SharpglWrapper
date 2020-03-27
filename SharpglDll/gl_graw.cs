using PointcloudWrapper;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LK.Caculation;
using System.Drawing;

namespace SharpglWrapper
{

    public class gl_graw
    {
        //伸缩
        internal double Scala;
        internal double zoomStep;

        internal OpenGL Gl { get => WindowHandle.OpenGL; }
        internal OpenGLControl WindowHandle { get; private set; }
        public gl_Camera cam;

        public gl_tuple data;
        //显示模式
        public uint mod { get => data.drawMod; }
        DrawEventHandler DrawEventHandler { get => data.DrawEventHandler; }


        private gl_graw()
        {
            Scala = 1;
            zoomStep = 0.1;
        }
        public gl_graw(Control fatherControl) : this()
        {
            WindowHandle = new OpenGLControl();
            cam = new gl_Camera(Gl);
            fatherControl.Controls.Add(WindowHandle);

            WindowHandle.DrawFPS = false;
            WindowHandle.Location = new System.Drawing.Point(0, 0);
            WindowHandle.Name = "WindowHandle";
            WindowHandle.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            WindowHandle.RenderContextType = SharpGL.RenderContextType.DIBSection;
            WindowHandle.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            WindowHandle.Size = fatherControl.Size;
           
        }

        internal gl_graw(OpenGLControl WindowHandle) : this()
        {
            this.WindowHandle = WindowHandle;
            cam = new gl_Camera(Gl);
        }
        public void Draw()
        {
            DrawEventHandler(Gl);
        }

    }    
}
