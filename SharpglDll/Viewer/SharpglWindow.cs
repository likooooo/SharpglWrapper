using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using System.Windows.Forms;

namespace SharpglWrapper.Viewer
{
    public class SharpglWindow : gl_Viewer
    {
        Event_gl_Draw drawHandle;
        Event_gl_MouseMove mouseMoveHandle;
        Event_gl_MouseWheel mouseWheelHandle;


        public SharpglWindow(OpenGLControl window):base(window)
        {
            drawHandle = new Event_gl_Draw(this);
            mouseMoveHandle = new Event_gl_MouseMove(this);
            mouseWheelHandle = new Event_gl_MouseWheel(this);
        }
        public SharpglWindow(OpenGLControl window, gl_object obj) : base(window, obj)
        {
            drawHandle = new Event_gl_Draw(this);
            mouseMoveHandle = new Event_gl_MouseMove(this);
            mouseWheelHandle = new Event_gl_MouseWheel(this);
        }
        public SharpglWindow(OpenGLControl window, asc obj) : this(window, new gl_object(obj))
        {

        }
        public SharpglWindow(OpenGLControl window, stl obj) : this(window, new gl_object(obj))
        {

        }
    }
}


