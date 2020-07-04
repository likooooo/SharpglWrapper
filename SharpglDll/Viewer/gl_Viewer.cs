using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using SharpGL.Enumerations;
using CameraAttribute;

namespace SharpglWrapper.Viewer
{
    //窗体属性
    public struct Param_Viewport
    {
        public int x, y;
        public int width;
        public int height;
        public Param_Viewport(OpenGLControl windowHandle)
        {
            x = 0;
            y = 0;
            width = windowHandle.Width;
            height = windowHandle.Height;
        }
        public void Flush(OpenGL gl)
        {
            gl.Viewport(x, y, width, height);
        }
    }
    public struct Param_Ortho
    {
        public double minX, maxX, minY, maxY, minZ, maxZ;

        public Param_Ortho(gl_object obj)
        {
            var boudingBox = obj.BoundingBox;
            double stepx = 0.5 * (boudingBox.maxX - boudingBox.minX);
            double stepy = 0.5 * (boudingBox.maxY - boudingBox.minY);
            double stepz = 0.5 * (boudingBox.maxZ - boudingBox.minZ);
            minX = boudingBox.minX - stepx;
            maxX = boudingBox.maxX + stepx;
            minY = boudingBox.minY - stepy;
            maxY = boudingBox.maxY + stepy;            
            maxZ = boudingBox.maxZ + stepz;
            minZ = boudingBox.minZ - stepz;
            // minZ = maxZ < 0 ? boudingBox.minZ : -maxZ;
        }

        public void Flush(OpenGL gl)
        {
            gl.Ortho(minX, maxX, minY, maxY, minZ, maxZ);
        }
    }
    public struct Param_LookAt
    {
        public double eyex, eyey, eyez, centerx, centery, centerz, upx, upy, upz;
        public Param_LookAt(gl_object obj, Param_Ortho param_Ortho)
        {
            eyex = (param_Ortho.maxX + param_Ortho.minX) / 2;
            eyey = (param_Ortho.maxY + param_Ortho.minY) / 2;
            eyez = 1.5 * obj.BoundingBox.maxZ;

            ////模型移到中心：
            //GL.Translate(obj.BoundingBox.maxX - param_LookAt.eyex,
            //    obj.BoundingBox.maxY - param_LookAt.eyey,
            //    obj.BoundingBox.maxZ - param_LookAt.eyez);
            //double dx = obj.BoundingBox.maxX - eyex;
            //double dy = obj.BoundingBox.maxY - eyey;
            //double dz = obj.BoundingBox.maxZ - eyez;
            //eyex -= dx;
            //eyey -= dy;
            //eyez -= dz;

            centerx = eyex;//(param_Ortho.maxX + param_Ortho.minX) / 2;
            centery = eyey;//(param_Ortho.maxY + param_Ortho.minY) / 2;
            //z轴看向最远处
            centerz = eyez < 0 ? param_Ortho.minZ : -eyez;

            upx = 0;
            upy = 1;
            upz = 0;
        }

        public void Flush(OpenGL gl)
        {
            gl.LookAt(eyex, eyey, eyez, centerx, centery, centerz, upx, upy, upz);
        }
    }
    public struct Param_Light
    {
        float[] light1_ambient;
        float[] light1_diffuse;
        float[] light1_specular;
        float[] light1_position;
        float[] spot_direction;

        public Param_Light(bool isempty)
        {
            light1_ambient = new float[] { 0.9f, 0.9f, 0.9f, 1.0f };
            light1_diffuse = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };
            light1_specular = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };
            light1_position = new float[] { 200.0f, 200.0f, 200.0f, 1.0f };
            spot_direction = new float[] { 1.0f, -1.0f, -1.0f };
        }


        public void Flush(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Light(LightName.Light1, LightParameter.Ambient, light1_ambient);
            gl.Light(LightName.Light1, LightParameter.Diffuse, light1_diffuse);
            gl.Light(LightName.Light1, LightParameter.Specular, light1_specular);
            gl.Light(LightName.Light1, LightParameter.Position, light1_position);
            gl.Enable(OpenGL.GL_LIGHT1);

            float[] bgcol = new float[4];
            gl.GetFloat(OpenGL.GL_COLOR_CLEAR_VALUE, bgcol);

            // Enable Depth Testing
            float[] specref = { 1.0f, 1.0f, 1.0f, 1.0f };
            gl.Enable(OpenGL.GL_DEPTH_TEST);

            // Enable lighting
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_TWO_SIDE, OpenGL.GL_TRUE);
            gl.Enable(OpenGL.GL_LIGHTING);

            // Enable color tracking
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);

            // Set Material properties to follow glColor values
            gl.ColorMaterial(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT_AND_DIFFUSE);

            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SPECULAR, specref);
            gl.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SHININESS, 128);

            gl.ShadeModel(OpenGL.GL_SMOOTH);

            gl.Enable(OpenGL.GL_AUTO_NORMAL);
            gl.Enable(OpenGL.GL_NORMALIZE);

            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
        }
    }


    //模型属性
    public class gl_Model 
    {
        public float RotateX, RotateY, RotateZ;
        public double TranslateX, TranslateY, TranslateZ;
        public double Scala;
        public uint sample { get => obj.sample;set { obj.sample = value; } }
        protected gl_object obj;

        public gl_Model()
        {
            Scala = 1;
        }
        public gl_Model(gl_object obj):this()
        {
            this.obj = obj.DepthClone();
        }

        protected void Flush(OpenGL gl)
        {
            obj.Draw_gl_object(gl);
        }

    }


    //viwer
    public class gl_Viewer : gl_Model
    {
        //widget param

        public OpenGLControl windowHandle { get; private set; }
        public OpenGL GL { get => windowHandle.OpenGL; }


        //opengl param
        public Param_Ortho param_Ortho { get; private set; }
        public Param_Viewport param_Viewport { get; private set; }
        public Param_LookAt param_LookAt { get; set; }
        public Param_Light param_Light { get; private set; }


        //virtualCamera
        //是否显示视锥
        public bool displayViewRange { get;  set; }
        RealsenseD435_Hardware_Attribute d435 = new RealsenseD435_Hardware_Attribute();
    

        public gl_Viewer(OpenGLControl WindowHandle, gl_object objModel) : base(objModel)
        {
            this.windowHandle = WindowHandle;

            param_Viewport = new Param_Viewport(WindowHandle);
            param_Ortho = new Param_Ortho(objModel);
            param_LookAt = new Param_LookAt(objModel, param_Ortho);
            param_Light = new Param_Light();
        }
        public gl_Viewer(OpenGLControl WindowHandle) : base()
        {
            this.windowHandle = WindowHandle;

            param_Viewport = new Param_Viewport(WindowHandle);
            param_Light = new Param_Light();
        }


        //绘制
        public void gl_Flush()
        {
            GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            GL.ClearColor(0.8f, 0.8f, 0.8f, 1);
            param_Viewport.Flush(GL);
            GL.MatrixMode(OpenGL.GL_PROJECTION);
            GL.LoadIdentity();
            GL.Scale(Scala, Scala, Scala);//缩放改成移动镜头


            param_Ortho.Flush(GL);
            param_LookAt.Flush(GL);

            //绘制坐标系
            //gl_GenCoordinate();

            //模型移到中心：
            GL.Translate(obj.BoundingBox.maxX - param_LookAt.eyex,
                obj.BoundingBox.maxY - param_LookAt.eyey,
                obj.BoundingBox.maxZ - param_LookAt.eyez);

            GL.Rotate(0, 45, 45);
            //gl_GenBoundingbox();
            //绘制可视线
            //gl_GenCameraVisibleRange(displayViewRange);

            //旋转：
            //1.模型移动至中心
            double xSub = (obj.BoundingBox.maxX + obj.BoundingBox.minX) - TranslateX;
            double ySub = (obj.BoundingBox.maxY + obj.BoundingBox.minY) - TranslateY;
            double zSub = (obj.BoundingBox.maxZ + obj.BoundingBox.minZ) - TranslateY;
            GL.Translate(xSub, ySub, zSub);
            //2.旋转
            GL.Rotate(RotateX, RotateY, RotateZ);
            //3.移回来
            GL.Translate(-xSub, -ySub, -zSub);

            //平移：
            GL.Translate(TranslateX, TranslateY, TranslateZ);     
            base.Flush(GL);

            GL.Flush();
        }


        public void gl_GenCameraVisibleRange(bool displayViewRange)
        {
            if (!displayViewRange) return;
            GL.LineWidth(1);

            //double camX = (obj.BoundingBox.maxX + obj.BoundingBox.minX) / 2;
            //double camY = (obj.BoundingBox.maxY + obj.BoundingBox.minY) / 2;
            //double camZ = 1.5 * obj.BoundingBox.maxZ;
            vector3 viewCenter = new vector3(param_LookAt.eyex - param_LookAt.centerx,
                param_LookAt.eyey - param_LookAt.centery,
                param_LookAt.eyez - param_LookAt.centerz);
            double camX = param_LookAt.eyex;
            double camY = param_LookAt.eyey;
            double camZ = param_LookAt.eyez;
            //double camX = viewCenter.x;
            //double camY = viewCenter.y;
            //double camZ = viewCenter.z;
            //double dx = obj.BoundingBox.maxX - camX;
            //double dy = obj.BoundingBox.maxY - camY;
            //double dz = obj.BoundingBox.maxZ - camZ;
            //camX += dx;
            //camY += dy;
            //camZ += dz;

            double h = d435.Depth.FOV.H;
            double v = d435.Depth.FOV.V;
            double d = d435.Depth.FOV.D;
            h /= 2;
            v /= 2;
            //上下线段，x=0
            double tan = Math.Tan(v);
            //y  /.
            //| / . ->maxY
            //|/v .
            //-------->z
            double length = viewCenter.z;
            double maxY = length * tan;
            double minY = -maxY;

            //左右线段,y=0
            tan = Math.Tan(v);
            //|  /   . 
            //| /    .
            //|/h    .
            //-------->z
            //|
            //|
            //|  
            //x
            tan = Math.Tan(h);
            double maxX = length * tan;
            double minX = -maxX;

        
            //绘制
            GL.Begin(OpenGL.GL_LINES);
            {
                //相机-minY
                GL.Color(1, 0, 0);
                GL.Vertex(camX, camY, camZ);
                GL.Vertex(0, minY, length);
                //相机-maxY
                GL.Color(0, 1, 0);
                GL.Vertex(camX, camY, camZ);
                GL.Vertex(0, maxY, length);
                //相机-minX
                GL.Color(0, 0, 1);
                GL.Vertex(camX, camY, camZ);
                GL.Vertex(minX, 0, length);
                //相机-maxX
                GL.Color(1, 1, 1);
                GL.Vertex(camX, camY, camZ);
                GL.Vertex(maxX, 0, length);
            }
            GL.End();
        }

        void gl_GenCoordinate()
        {
            GL.LineWidth(1);
            //绘制
            GL.Begin(OpenGL.GL_LINES);
            {
                //X轴
                GL.Color(1, 0, 0);
                GL.Vertex(param_Ortho.minX, param_LookAt.eyey, param_LookAt.eyez);
                //GL.Color(1, 0, 0);
                GL.Vertex(param_Ortho.maxX, param_LookAt.eyey, param_LookAt.eyez);
                //Y轴
                GL.Color(0, 1, 0);
                GL.Vertex(param_LookAt.eyex, param_Ortho.minY, param_LookAt.eyez);
                //GL.Color(0, 1, 0);
                GL.Vertex(param_LookAt.eyex, param_Ortho.maxY, param_LookAt.eyez);
                //Z轴
                GL.Color(0, 0, 1);
                GL.Vertex(param_LookAt.eyex, 0, param_Ortho.minZ);
                //GL.Color(0, 0, 1);
                GL.Vertex(param_LookAt.eyex, 0, param_Ortho.maxZ);

            }
            GL.End();
            GL.Flush();
        }
        void gl_GenBoundingbox()
        {
            //绘制外接矩形
            (double xMin, double xMax, double yMin, double yMax, double zMin, double zMax) = obj.BoundingBox;
            GL.Begin(OpenGL.GL_LINES);
            {
                GL.Color(0, 1, 1);
                GL.Vertex(xMin, yMin, zMin);
                GL.Vertex(xMax, yMin, zMin);

                GL.Vertex(xMin, yMin, zMax);
                GL.Vertex(xMax, yMin, zMax);

                GL.Vertex(xMin, yMin, zMin);
                GL.Vertex(xMin, yMin, zMax);

                GL.Vertex(xMax, yMin, zMin);
                GL.Vertex(xMax, yMin, zMax);
                //上半区
                GL.Vertex(xMin, yMax, zMin);
                GL.Vertex(xMax, yMax, zMin);

                GL.Vertex(xMin, yMax, zMax);
                GL.Vertex(xMax, yMax, zMax);

                GL.Vertex(xMin, yMax, zMin);
                GL.Vertex(xMin, yMax, zMax);

                GL.Vertex(xMax, yMax, zMin);
                GL.Vertex(xMax, yMax, zMax);
                //竖直
                GL.Vertex(xMin, yMin, zMin);
                GL.Vertex(xMin, yMax, zMin);

                GL.Vertex(xMin, yMin, zMax);
                GL.Vertex(xMin, yMax, zMax);

                GL.Vertex(xMax, yMin, zMax);
                GL.Vertex(xMax, yMax, zMax);

                GL.Vertex(xMax, yMin, zMin);
                GL.Vertex(xMax, yMax, zMin);
            }
            GL.End();
        }
        public void UpdataWindow()
        {
            param_Viewport = new Param_Viewport(windowHandle);
        }
        //更新数据
        public void Updata(gl_object obj)
        {
            base.obj = obj.DepthClone();
            Scala = 1;
            param_Ortho = new Param_Ortho(base.obj);
            param_LookAt = new Param_LookAt(base.obj, param_Ortho);
        }
    }
}
