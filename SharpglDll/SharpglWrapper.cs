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

    //相机
     class Camera
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


    //事件集合
    class EventHandle
    {
        Camera cam;
        public SharpglPointcloud sharpglPointscloud;
        OpenGLControl openGLControl;
        float zoomStep;

        bool mouseDown;
        int clickX, clickY;


        public EventHandle(Camera cam, SharpglPointcloud sharpglPointscloud, OpenGLControl openGLControl,float zoomStep= 0.1f)
        {
            this.cam = cam;
            this.sharpglPointscloud = sharpglPointscloud;
            this.openGLControl = openGLControl;
            this.zoomStep = zoomStep;
        }
        public void SW_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                sharpglPointscloud.Scala -= zoomStep;
            }
            else
            {
                sharpglPointscloud.Scala += zoomStep;
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
            cam.UpdateRotate(gl);
            cam.UpdateMove(gl);
            gl.Scale(sharpglPointscloud.Scala, sharpglPointscloud.Scala, sharpglPointscloud.Scala);
            gl.Begin(OpenGL.GL_POINTS);
            Draw(gl);
            gl.End();       
        }
        public void SW_OpenGLInit(object sender, EventArgs args)
        {

        }
        public void SW_Resize(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 绘制执行的代码,对应四种不同模式
        /// </summary>
        /// <param name="gl"></param>
        private void Draw(OpenGL gl)
        {
            bool isRenderColor = sharpglPointscloud.IsRenderColor;
            bool isComplex = sharpglPointscloud.IsComplex;
            if (isRenderColor && isComplex)
                DrawWindowColorCombine(gl);
            else if (isRenderColor && !isComplex)
                DrawWindowColor(gl);
            else if (!isRenderColor && isComplex)
                DrawWindowCombine(gl);
            else if (!isRenderColor && !isComplex)
                DrawWindow(gl);
            else
                throw new Exception("在Opengl显示时发生预料之外的结果");
        }
        private void DrawWindow(OpenGL gl)
        {
            int loopCount = sharpglPointscloud.X.Length;
            for (int i = 0; i < loopCount; i += sharpglPointscloud.Sample)
            {
                gl.Vertex
                 (sharpglPointscloud.X[i],
                  sharpglPointscloud.Y[i],
                  sharpglPointscloud.Z[i]);
            }
        }
        private void DrawWindowColor(OpenGL gl)
        {
            int loopCount = sharpglPointscloud.X.Length;
            Color color = sharpglPointscloud.RenderColor[0];
            for (int i = 0; i < loopCount; i += sharpglPointscloud.Sample)
            {
                gl.Color(color.R, color.G, color.B);
                gl.Vertex
                 (sharpglPointscloud.X[i],
                  sharpglPointscloud.Y[i],
                  sharpglPointscloud.Z[i]);
            }
        }
        private void DrawWindowCombine(OpenGL gl)
        {
            int combineCount = sharpglPointscloud.ComplexPointcloud.Count;
            for (int i = 0; i < combineCount; i++)
            {
                int pointCount = sharpglPointscloud.X.Length;
                for (int j = 0; i < pointCount; j += sharpglPointscloud.Sample)
                {
                    gl.Vertex
                     (sharpglPointscloud.X[j],
                      sharpglPointscloud.Y[j],
                      sharpglPointscloud.Z[j]);
                }
            }
        }
        private void DrawWindowColorCombine(OpenGL gl)
        {
            int combineCount = sharpglPointscloud.ComplexPointcloud.Count;
            var colorList = sharpglPointscloud.RenderColor;
            for (int i = 0; i < combineCount; i++)
            {
                int pointCount = sharpglPointscloud.X.Length;
                for (int j = 0; i < pointCount; j += sharpglPointscloud.Sample)
                {
                    gl.Color(colorList[i].R, colorList[i].G, colorList[i].B);
                    gl.Vertex
                     (sharpglPointscloud.X[j],
                      sharpglPointscloud.Y[j],
                      sharpglPointscloud.Z[j]);
                }
            }

        }
    }


    //显示的模型
    public class SharpglPointcloud : PointcloudF
    {
        //采样
        int sample;
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

        //伸缩值
        public float Scala
        {
            get;
            set;
        }

        //颜色
        public bool IsRenderColor
        {
            get;set;
        }
        Color[] renderColor = new Color[6] { Color.Red, Color.Green, Color.Blue, Color.Cyan, Color.Yellow,Color.Brown };
        public Color[] RenderColor
        {
            get
            {
                return renderColor.Take(complexPointcloud.Count).ToArray();
            }
        }


        //是否为多个组合
        List<SharpglPointcloud> complexPointcloud;
        public List<SharpglPointcloud> ComplexPointcloud
        {
            get
            {
                return complexPointcloud;
            }
        }
        public bool IsComplex
        {
            get
            {
                if (complexPointcloud.Count > 1)
                    return true;
                else
                    return false;
            }

        }


        public SharpglPointcloud() : base(new float[3] { -1, 0, 1 }, new float[3] { -1, 0, 1 }, new float[3] { -1, 0, 1 })
        {
            IsRenderColor = false;
            sample = 1;
            complexPointcloud = new List<SharpglPointcloud>();
        }

        public SharpglPointcloud(float[] x,float[] y,float[] z, float scala = 1, int sample = 1)
            : base(x, y, z)
        {
            IsRenderColor = false;
            this.sample = sample;
            this.Scala = scala;
            complexPointcloud = new List<SharpglPointcloud>();
            complexPointcloud.Add(this);
        }
        public SharpglPointcloud(PointcloudF pointcloud, float scala = 1, int sample = 1)
            : this(pointcloud.X, pointcloud.Y, pointcloud.Z)
        {
        }


        //点云的组合
        public void Combine(SharpglPointcloud pointcloud)
        {
            complexPointcloud.Add(pointcloud);
        }
        public void Combine(SharpglPointcloud[] pointclouds)
        {
            complexPointcloud.AddRange(pointclouds);
        }
    }


    //外部调用
    public class SharpglWrapper
    {
        OpenGLControl SW;
        Camera camera;
        SharpglPointcloud initPointcloud;
        EventHandle eventHandle;


        public SharpglWrapper(Control fatherControl)
        {
            SW = new OpenGLControl();
            camera = new Camera();
            initPointcloud = new SharpglPointcloud();
            eventHandle = new EventHandle(camera, initPointcloud, SW);
            SW.OpenGLInitialized += new EventHandler(eventHandle.SW_OpenGLInit);
            SW.OpenGLDraw += new RenderEventHandler(eventHandle.SW_OpenGLDraw);
            SW.Resize += new EventHandler(eventHandle.SW_Resize);
            fatherControl.Controls.Add(SW);
        }

        public SharpglWrapper(OpenGLControl window)
        {
            SW = window;
            camera = new Camera();
            initPointcloud = new SharpglPointcloud();
            eventHandle = new EventHandle(camera, initPointcloud, SW);

            SW.OpenGLInitialized += new EventHandler(eventHandle.SW_OpenGLInit);
            SW.OpenGLDraw += new RenderEventHandler(eventHandle.SW_OpenGLDraw);
            SW.Resize += new EventHandler(eventHandle.SW_Resize);
        }


        /// <summary>
        /// 设定窗体的采样
        /// </summary>
        /// <param name="sample"></param>
        public void SetModelSample(int sample)
        {
            eventHandle.sharpglPointscloud.Sample = sample;
        }



        /// <summary>
        /// 设置点云
        /// </summary>
        /// <param name="pcIn"></param>
        public void SetDraw(SharpglPointcloud pcIn)
        {
            eventHandle.sharpglPointscloud = pcIn;
        }


        /// <summary>
        /// 获取当前窗体的点云
        /// </summary>
        /// <returns></returns>
        public SharpglPointcloud GetPointcloud()
        {
            return eventHandle.sharpglPointscloud;
        }
        public List<SharpglPointcloud> GetPointclouds()
        {
            return eventHandle.sharpglPointscloud.ComplexPointcloud;
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
        public void RemoveEventMove()

        {
            SW.MouseDown -= new MouseEventHandler(eventHandle.SW_MouseDown);
            SW.MouseMove -= new MouseEventHandler(eventHandle.SW_MouseMove);
            SW.MouseUp -= new MouseEventHandler(eventHandle.SW_MouseUp);
        }


        /// <summary>
        /// 注册鼠标滑轮事件
        /// </summary>
        public void AddEventZoom()
        {
            SW.MouseWheel += new MouseEventHandler(eventHandle.SW_MouseWheel);
        }
        public void RemoveEventZoom()
        {
            SW.MouseWheel -= new MouseEventHandler(eventHandle.SW_MouseWheel);
        }
    }
}
