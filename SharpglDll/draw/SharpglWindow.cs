using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using System.Windows.Forms;

namespace SharpglWrapper.draw
{
    public class SharpglWindow 
    {
        gl_graw drawPtr;
        Event_gl_Draw drawHandle;
        Event_gl_MouseMove mouseMoveHandle;
        Event_gl_MouseWheel mouseWheelHandle;

        public SharpglWindow(Control fatherControl) 
        {
            drawPtr = new gl_graw(fatherControl);
            drawHandle = new Event_gl_Draw(drawPtr);
            mouseMoveHandle = new Event_gl_MouseMove(drawPtr);
            mouseWheelHandle = new Event_gl_MouseWheel(drawPtr);
        }


        public SharpglWindow(OpenGLControl window)
        {
            drawPtr = new gl_graw(window);
            drawHandle = new Event_gl_Draw(drawPtr);
            mouseMoveHandle = new Event_gl_MouseMove(drawPtr);
            mouseWheelHandle = new Event_gl_MouseWheel(drawPtr);
        }

        public void Updae(gl_tuple data)
        {
            drawPtr.data = data;
            var minX = data.stl_Trangle.Select(s => s.X.Min()).Min();
            var maxX = data.stl_Trangle.Select(s => s.X.Max()).Max();
            var minY = data.stl_Trangle.Select(s => s.Y.Min()).Min();
            var maxY = data.stl_Trangle.Select(s => s.Y.Max()).Max();
            var minZ = data.stl_Trangle.Select(s => s.Z.Min()).Min();
            var maxZ = data.stl_Trangle.Select(s => s.Z.Max()).Max();
        }
    }
}
