using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
namespace SharpglWrapper
{
    public class gl_Camera : gl_object
    {
        double x, y, z;
        double rotateX, rotateY, rotateZ;

        public double CamPosX
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
        public double CamPosY
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
        public double CamPosZ
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
        public double RotateX
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
        public double RotateY
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
        public double RotateZ
        {
            get => rotateZ;
            set => rotateZ = value;
        }

        //观察对象的中心x,y,z
        public double[] TargetCenter { get; set; }
        public double[] UpXyz
        {
            get;set;
        }
        
        public gl_Camera(OpenGL gl):base(gl)
        {
            x = 0;
            y = 0;
            z = -6;
            rotateX = 0;
            rotateY = 0;
            rotateZ = 0;
            UpXyz = new double[] { 0, 1, 0 };
        }

        public gl_Camera(OpenGL gl,double x,double y,double z ) : base(gl)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            UpXyz = new double[] { 0, 1, 0 };
        }


        public void UpdateCamPos(double targetCenterX, double targetCenterY, double targetCenterZ)
        {
            GL.LookAt(CamPosX, CamPosY, CamPosZ, targetCenterX, targetCenterY, targetCenterZ, UpXyz[0], UpXyz[1], UpXyz[2]);
        }
        public void UpdateCamPos()
        {
            GL.LookAt(CamPosX, CamPosY, CamPosZ, TargetCenter[0], TargetCenter[1], TargetCenter[2], UpXyz[0], UpXyz[1], UpXyz[2]);
            //GL.Rotate(RotateX, RotateY, RotateZ);
        }
    }
}
