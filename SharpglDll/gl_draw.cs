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

    public class gl_draw
    {



        //伸缩
        double scala;
        internal double Scala
        {
            get => scala;
            set
            {
                scala = value;
                //Gl_Param_Ortho = (data.BoundingBox.minX * scala,
                //    data.BoundingBox.maxX * scala,
                //    data.BoundingBox.minY * scala,
                //    data.BoundingBox.maxY * scala,
                //    data.BoundingBox.minZ * scala,
                //    data.BoundingBox.maxZ * scala);
            }
        }
        internal double zoomStep;

        internal OpenGL Gl { get => WindowHandle.OpenGL; }
        internal OpenGLControl WindowHandle { get; private set; }
        public gl_Camera cam;

        public gl_tuple data;


        private gl_draw()
        {
            scala = 1;
            zoomStep = 0.1;
        }
        public gl_draw(Control fatherControl) : this()
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

        public gl_draw(OpenGLControl WindowHandle) : this()
        {
            this.WindowHandle = WindowHandle;
            cam = new gl_Camera(Gl);
        }
        public void Draw()
        {
            data.Draw(Gl);
        }

    }    
}
