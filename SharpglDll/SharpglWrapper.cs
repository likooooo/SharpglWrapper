using PointcloudWrapper;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SharpglWrapper
{
    public enum DrawType
    {
        不绘制,
        绘制例子,
        绘制点云,
    }


    interface ISharpglWrapperPara
    {
        //视窗中心的位置
        float CurrentX
        {
            get;
        }
        float CurrentY
        {
            get;
        }
        float CurrentZ
        {
            get;
        }
        //视窗中心的旋转角度
        float RotateX
        {
            get;

        }
        float RotateY
        {
            get;

        }
        float RotateZ
        {
            get;
        }
        float Zoom
        {
            get;
        }
         PointcloudF Pointcloud
        {
            get;
        }
        //是否绘制点云颜色
        bool RenderColor
        {
            get;
        }
    }
    public class SharpglAction: ISharpglWrapperPara
    {
        //点云
        PointcloudF pointcloud;    
        public PointcloudF Pointcloud
        {
            get
            {
                return pointcloud;
            }
        }
        //视窗中心的位置
        protected float currentX, currentY, currentZ, rotateX, rotateY, rotateZ;
        protected float zoom;
        public float CurrentX
        {
            get
            {
                return currentX;
            }
        }
        public float CurrentY
        {
            get
            {
                return currentY;
            }
        }
        public float CurrentZ
        {
            get
            {
                return currentZ;
            }
        }
        //视窗中心的旋转角度
        public float RotateX
        {
            get
            {
                return rotateX;
            }
        }
        public float RotateY
        {
            get
            {
                return rotateY;
            }
        }
        public float RotateZ
        {
            get
            {
                return rotateZ;
            }
        }
        public float Zoom
        {
            get
            {
                return zoom;
            }
        }
        //是否绘制点云颜色
        public bool RenderColor
        {
            get;
            set;
        }


        //鼠标按下绘制事件所需要的内容
        protected bool eventLock = false;
        protected bool isMouseDown = false;
        protected int clickX, clickY;
        protected float ddx = 0, ddy = 0;
        //绘制的方式
        protected uint model;
        protected DrawType drawType;
        //绘制的对象
        protected OpenGLControl SW;
        //光照
        protected float[] lightPos = new float[] { -1, -3, -1, 1 };
        protected IList<double[]> viewDefaultPos = new List<double[]>();
        protected double[] lookatValue = { 1, 1, 2, 0, 0, 0, 0, 1, 0 };
        protected IList<float[]> lightColor = new List<float[]>();
        protected void SetLightColor(OpenGL gl)
        {
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, lightColor[0]);//环境光
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, lightColor[1]);//漫反射
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, lightColor[2]);//镜面光
        }


        /**界面旋转，拖动，伸缩(未完成),绘制(必须做完)
         * **/
        public void AddEvent()
        {
            if (eventLock)
                return;
            eventLock = !eventLock;
            SW.MouseWheel += new MouseEventHandler(SW_MouseWheel);
            SW.MouseDown += new MouseEventHandler(SW_MouseDown);
            SW.MouseMove += new MouseEventHandler(SW_MouseMove);
            SW.MouseUp += new MouseEventHandler(SW_MouseUp);
            SW.OpenGLInitialized += new System.EventHandler(SW_OpenGLInit);
            SW.OpenGLDraw += new SharpGL.RenderEventHandler(SW_OpenGLDraw);
            SW.Resize += new System.EventHandler(SW_Resize);
        }
        public void CancelEvent()
        {
            if (!eventLock)
                return;
            eventLock = !eventLock;
            SW.MouseWheel -= new MouseEventHandler(SW_MouseWheel);
            SW.MouseDown -= new MouseEventHandler(SW_MouseDown);
            SW.MouseMove -= new MouseEventHandler(SW_MouseMove);
            SW.MouseUp -= new MouseEventHandler(SW_MouseUp);
            SW.OpenGLInitialized -= new System.EventHandler(SW_OpenGLInit);
            SW.OpenGLDraw -= new SharpGL.RenderEventHandler(SW_OpenGLDraw);
            SW.Resize -= new System.EventHandler(SW_Resize);
        }


        public void SetDraw(DrawType drawType = DrawType.绘制例子)
        {
            this.drawType = drawType;
        }
        public void SetDraw(PointcloudF pcIn, float centerX, float centerY, float centerZ, DrawType drawType = DrawType.绘制点云)
        {
            this.drawType = drawType;
            currentX = centerX;
            currentY = centerY;
            currentZ = centerZ;
            this.pointcloud = pcIn.Clone();
        }




        protected void SW_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                currentZ += 1;
            }
            else
            {
                currentZ -= 1;
            }
        }
        protected void SW_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            clickX = e.X;
            clickY = e.Y;
        }

        protected void SW_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                currentX = ddx + (e.X - clickX) * 1f / SW.Width;
                currentY = ddy + (e.Y - clickY) * 1f / SW.Height;
            }
        }
        protected void SW_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            ddx = CurrentX;
            ddy = CurrentY;
        }
        protected void SW_OpenGLDraw(object sender, RenderEventArgs args)
        {
            Draw();
        }
        protected void SW_OpenGLInit(object sender, EventArgs args)
        {
            OpenGL gl = SW.OpenGL;
            //四个视图的缺省位置
            viewDefaultPos.Add(new double[] { 1, 1, 2, 0, 0, 0, 0, 1, 0 });     //透视
            viewDefaultPos.Add(new double[] { 0, 0, 2, 0, 0, 0, 0, 1, 0 });     //前视 
            viewDefaultPos.Add(new double[] { 5, 0, 0, 0, 0, 0, 0, 1, 0 });     //左视
            viewDefaultPos.Add(new double[] { 0, 13, 0, -1, 0, 0, 0, 1, 0 });   //顶视
            lookatValue = (double[])viewDefaultPos[0].Clone();

            lightColor.Add(new float[] { 1f, 0, 0, 1f });  //环境光(ambient light)
            lightColor.Add(new float[] { 1f, 1f, 1f, 1f });  //漫射光(diffuse light)
            lightColor.Add(new float[] { 1f, 1f, 1f, 1f });  //镜面反射光(specular light)

            SetLightColor(gl);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, lightPos);
            gl.ColorMaterial(OpenGL.GL_FRONT, OpenGL.GL_DIFFUSE);//diffuse-漫反射光 ambient-环境光 
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);//使用物体颜色材质
            gl.Enable(OpenGL.GL_LIGHTING);//使用灯光
            gl.Enable(OpenGL.GL_LIGHT0);//使用0号灯光
            gl.ClearColor(0, 0, 0, 0);
        }
        protected void SW_Resize(object sender, EventArgs e)
        {
            OpenGL gl = SW.OpenGL;

            //  设置当前矩阵模式,对投影矩阵应用随后的矩阵操作
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            // 重置当前指定的矩阵为单位矩阵,将当前的用户坐标系的原点移到了屏幕中心
            gl.LoadIdentity();

            // 创建透视投影变换
            //
            gl.Perspective(15.0f, SW.Width / SW.Height, 0, 0.5);

            // 视点变换
            //
            gl.LookAt(CurrentX, CurrentY, CurrentZ, CurrentX, CurrentY, CurrentZ, 0, 1, 0);
            // 设置当前矩阵为模型视图矩阵
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }





        /**绘制
         * **/
        private void Draw()
        {
            OpenGL gl = this.SW.OpenGL;
            //清除深度缓存 
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            //定义视野范围
            gl.Viewport((int)CurrentX, (int)CurrentY, SW.Width, SW.Height);
            gl.LoadIdentity();
            //依据设定的对象，读取数据
            switch (drawType)
            {
                case DrawType.不绘制:
                    break;
                case DrawType.绘制点云:
                    gl.Translate(CurrentX, CurrentY, CurrentZ);
                    gl.Rotate(RotateX, RotateY, RotateZ);

                    gl.Begin(model);
                    AddPointclouds(gl);
                    break;
                case DrawType.绘制例子:
                    gl.Translate(-1.5f, 0, -6);
                    gl.Begin(model);
                    AddSampleLine(gl);
                    break;
            }
            gl.End();
        }


        /**添加模型
         * **/
        private void AddSampleLine(OpenGL gl)
        {
            //  gl.Color(0.0f, 1.0f, 0.50f);
            for (int i = -1; i < 2; i++)
            {
                gl.Vertex(i, i, i);
            }
        }
        private void AddPointclouds(OpenGL gl)
        {
            // gl.Color(0.0f, 1.0f, 0.50f);
            //是否绘制颜色
            if (RenderColor)
            {
                //Vertex vertex;
                //System.Drawing.Color color;
                //for (int row = 0; row < 720; row++)
                //    for (int col = 0; col < 1280; col++)
                //    {
                //        vertex = GetPoint(row, col);
                //        if (vertex.x == 0 && vertex.y == 0 && vertex.z == 0) continue;
                //        color = currentBmp.GetPixel(col, row);
                //        gl.Color(color.R / 255, color.G / 255, color.B / 255);
                //        gl.Vertex(vertex.x, vertex.y, vertex.z);
                //    }
            }
            else
            {
                for (int i = 0; i < Pointcloud.X.Length; i++)
                {
                    SetVertex
                          (gl, Pointcloud.X[i], Pointcloud.Y[i], Pointcloud.Z[i]);
                }
            }
        }
        private void SetVertex(OpenGL gl, float x, float y, float z)
        {
            gl.Vertex(x * zoom, y * zoom, z * zoom);
        }
    }
    public class SharpglWrapper: SharpglAction
    {   
        /**构造函数
         * **/
        public SharpglWrapper(OpenGLControl window)
        {
            SW = window;
            model = OpenGL.GL_POINTS;
            drawType = DrawType.绘制例子;
            AddEvent();
            rotateX = -180f;
        }

    }
}
