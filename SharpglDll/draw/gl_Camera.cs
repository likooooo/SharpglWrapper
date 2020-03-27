using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpglWrapper
{
    public class gl_Camera : gl_object
    {
        float x, y, z;
        float rotateX, rotateY, rotateZ;

        public float CamPosX
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public float CamPosY
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public float CamPosZ
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }
        public float RotateX
        {
            get
            {
                return rotateX;
            }
            set
            {
                rotateX = value;
            }
        }
        public float RotateY
        {
            get
            {
                return rotateY;
            }
            set
            {
                rotateY = value;
            }
        }
        public float RotateZ
        {
            get => rotateZ;
            set => rotateZ = value;
        }


        public gl_Camera(SharpGL.OpenGL gl):base(gl)
        {
            x = 0;
            y = 0;
            z = 0;
            rotateX = 0;
            rotateY = 0;
            rotateZ = 0;
        }
        public void UpdateMove()
        {
            //gl.Translate()控制模型的运动，
            //模型与相机之间的相对运动关系是反向的
            //如果相机想要Z轴抬高，
            //等效于相机不动，模型向下运动
           GL.Translate(CamPosX, CamPosY, CamPosZ);
        }

        public void UpdateRotate()
        {
            GL.Rotate(-RotateX, 1, 0, 0);
            GL.Rotate(-RotateY, 0, 1, 0);
            GL.Rotate(-RotateZ, 0, 0, 1);
        }
    }
}
