using PointcloudWrapper;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LK.Caculation;
namespace SharpglWrapper
{

    public class Camera
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
            get
            {
                return rotateZ;
            }
            set
            {
                rotateZ = value;
            }
        }


        public Camera()
        {
            x = 0;
            y = 0;
            z = 5;
            rotateX = 0;
            rotateY = 0;    
            rotateZ = 180;
        }
        public void UpdateMove(OpenGL gl)
        {
            //gl.Translate()控制模型的运动，
            //模型与相机之间的相对运动关系是反向的
            //如果相机想要Z轴抬高，
            //等效于相机不动，模型向下运动
            gl.Translate(-CamPosX, -CamPosY, -CamPosZ);
        }

        public void UpdateRotate(OpenGL gl)
        {
            gl.Rotate(-RotateX, 1, 0, 0);
            gl.Rotate(-RotateY, 0, 1, 0);
            gl.Rotate(-RotateZ, 0, 0, 1);
        }
    }

    public class Model
    {
        private int sample;
        private float x1, x2, y1, y2, z1, z2,xMean,yMean,zMean;
        private PointcloudF pointcloud;
        public float XMin
        {
            get
            {
                return x1;
            }
        }
        public float YMin
        {
            get
            {
                return y1;
            }
        }
        public float ZMin
        {
            get
            {
                return z1;
            }
        }
        public float XMax
        {
            get
            {
                return x2;
            }
        }
        public float YMax
        {
            get
            {
                return y2;
            }
        }
        public float ZMax
        {
            get
            {
                return z2;
            }
        }
        public float XMean
        {
            get
            {
                return xMean;
            }
        }
        public float YMean
        {
            get
            {
                return yMean;
            }
        }
        public float ZMean
        {
            get
            {
                return zMean;
            }
        }

        public float Scala
        {
            get;
            set;
        }
        public bool RenderColor
        {
            get; set;
        }
        public int Sample
        {
            get
            {
                return sample;
            }
            set
            {
                if (value <= 0)
                    throw new Exception("值不能小于0");
                sample = value;
            }
        }
        public PointcloudF Pointcloud
        {
            get
            {
                return pointcloud;
            }
        }

        public Model()
        {
            sample = 5;
            float[] x = new float[3] { -1, 0, 1 };
            float[] y = x;
            float[] z = x;
            pointcloud = new PointcloudF(x, y, z);
            Scala = 1;
        }
        

        public void UpdateModel(PointcloudF pcIn)
        {
            
            x1 = Caculation.Min(pcIn.X);//-1
            y1 = Caculation.Min(pcIn.Y);//-1
            z1 = Caculation.Min(pcIn.Z);//0
            x2 = Caculation.Max(pcIn.X);//2.5
            y2 = Caculation.Max(pcIn.Y);//0.5
            z2 = Caculation.Max(pcIn.Z);//3.5
            xMean = Caculation.Min(pcIn.X);
            yMean = Caculation.Min(pcIn.Y);
            zMean = Caculation.Min(pcIn.Z);
            //float[] x = pcIn.X
            //    .Where(s => s < 2.5 && s > -1)
            //    .Select(s => s - XMean).ToArray();
            //float[] y = pcIn.Y
            //    .Where(s => s < 0.5 && s > -1)
            //    .Select(s => s - YMean).ToArray();
            //float[] z = pcIn.Z
            //    .Where(s => s < 3.5)
            //    .Select(s => s - ZMean).ToArray();
            //pointcloud = new PointcloudF(x, y, z);
            pointcloud = pcIn;
        }

    }

    class EventHandle
    {
        Camera cam;
        Model model;
        OpenGLControl openGLControl;
        float zoomStep;

        bool mouseDown;
        int clickX, clickY;
        public EventHandle(Camera cam, Model model,OpenGLControl openGLControl,float zoomStep= 0.1f)
        {
            this.cam = cam;
            this.model = model;
            this.openGLControl = openGLControl;
            this.zoomStep = zoomStep;
        }
        public void SW_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                model.Scala -= zoomStep;
            }
            else
            {
                model.Scala += zoomStep;
            }
        }
        public void SW_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            clickX = e.X;
            clickY = e.Y;
        }
        public void SW_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                //每次移动的图像距离
                float yMove = (e.X - clickX)*1f / openGLControl.Width;
                float xMove = (e.Y - clickY)*1f / openGLControl.Height;
                clickX = e.X;
                clickY = e.Y;
                cam.RotateX += 180f * xMove;
                cam.RotateY += 180f * yMove;
                //    rotateX = ddx + (e.X - clickX) * 1f / openGLControl.Width;
                //rotateY = ddy - (e.Y - clickY) * 1f / openGLControl.Height;
            }
        }
        public void SW_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        public void SW_OpenGLDraw(object sender, RenderEventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            // cam.CamPosZ = 2f * model.XMax;
            // cam.UpdateRotate(gl,0.75f,-0.25f,1.75f);
            cam.UpdateRotate(gl);
            cam.UpdateMove(gl);
            gl.Scale(model.Scala, model.Scala, model.Scala);
            gl.Begin(OpenGL.GL_POINTS);
            AddPointclouds(gl);
            gl.End();
            //想绘制坐标轴，没有做好
            //gl.Begin(OpenGL.GL_LINE);
            //gl.Color(1, 0, 0);
            //gl.Vertex(-5, 0, 0);
            //gl.Color(1, 0, 0);
            //gl.Vertex(5, 0, 0);
            //gl.Color(1, 0, 0);
            //gl.Vertex(0, -5, 0);
            //gl.Color(1, 0, 0);
            //gl.Vertex(0, 5, 0);
            //gl.Color(1, 0, 0);
            //gl.Vertex(0, 0, -5);
            //gl.Color(1, 0, 0);
            //gl.Vertex(0, 0, 5);
            //gl.End();        
        }
        public void SW_OpenGLInit(object sender, EventArgs args)
        {

        }
        public void SW_Resize(object sender, EventArgs e)
        {

        }


        private void AddPointclouds(OpenGL gl)
        {
            //是否绘制颜色
            if (model.RenderColor)
            {

            }
            else
            {
                for (int i = 0; i < model.Pointcloud.X.Length; i += model.Sample)
                {
                    gl.Vertex
                     (model.Pointcloud.X[i],
                      model.Pointcloud.Y[i],
                      model.Pointcloud.Z[i]);
                }
            }
        }
    }

    public class SharpglWrapper
    {
        OpenGLControl SW;
        Camera camera;
        Model model;
        EventHandle eventHandle;
        public SharpglWrapper(OpenGLControl window)
        {
            SW = window;
            camera = new Camera();
            model = new Model();
            eventHandle = new EventHandle(camera, model, SW);

            SW.OpenGLInitialized += new EventHandler(eventHandle.SW_OpenGLInit);
            SW.OpenGLDraw += new RenderEventHandler(eventHandle.SW_OpenGLDraw);
            SW.Resize += new EventHandler(eventHandle.SW_Resize);
        }

        public void SetModelSample(int sample)
        {
            model.Sample = sample;
        }

        public void SetDraw(PointcloudF pcIn)
        {
            model.UpdateModel(pcIn);
        }
        public PointcloudF PointcloudF
        {
            get
            {
                return model.Pointcloud;
            }
        }

        /// <summary>
        /// 注册鼠标拖动事件
        /// </summary>
        public void AddEventMove()
        {
            SW.MouseDown += new MouseEventHandler(eventHandle.SW_MouseDown);
            SW.MouseMove += new MouseEventHandler(eventHandle.SW_MouseMove);
            SW.MouseUp += new MouseEventHandler(eventHandle.SW_MouseUp);
        }


        /// <summary>
        /// 注册鼠标滑轮事件
        /// </summary>
        public void AddEventZoom()
        {
            SW.MouseWheel += new MouseEventHandler(eventHandle.SW_MouseWheel);
        }
    }
}
