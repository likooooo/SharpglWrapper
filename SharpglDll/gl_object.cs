using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;

namespace SharpglWrapper
{
    public delegate void draw_gl_object(OpenGL gL);
    public delegate gl_object depthClone();
    public interface I_gl_object
    {
        void Draw_gl_object(OpenGL gL);
        gl_object DepthClone();
    }

    //各种模型显示，模型之间数据转换
    public class gl_object: I_gl_object
    {
        asc asc;
        stl stl;
        stl_trangleVector[] stl_Trangle { get => stl.pointsArry.ToArray(); }
        stl_facetVector[] stl_Facet { get => stl.facetArry.ToArray(); }
        ascPoint[] ascPoints { get => asc.ascPoint.ToArray(); }
        ascColor[] ascColors { get => asc.ascColor.ToArray(); }
        public uint drawMod { get; private set; }
        //显示图像的采样（for循环的步长）
        public uint sample { get; set; }

        public (double minX, double maxX, double minY, double maxY, double minZ, double maxZ)
            BoundingBox { get; private set; }


        draw_gl_object drawHandle;
        depthClone depthCloneHandle;

        public gl_object(stl stl)
        {
            this.stl = stl;
            drawMod = OpenGL.GL_TRIANGLES;

            this.BoundingBox = stl.BoundingBox;
            drawHandle = new draw_gl_object(DrawStl);
            depthCloneHandle = () => new gl_object(stl);
            sample = 1;
        }


        public gl_object(asc asc)
        {
            this.asc = asc;
            drawMod = OpenGL.GL_POINTS;

            this.BoundingBox = asc.BoundingBox;
            drawHandle = new draw_gl_object(DrawAsc);
            depthCloneHandle = () => new gl_object(asc);
            sample = 1;
        }


        //获取stl模型的视野表面的部分
        public gl_object GetStlSurface(double camX, double camY, double camZ)
        {
            var stl_Trangle = this.stl_Trangle;
            var stl_Facet = this.stl_Facet;
            if (stl_Trangle == null) throw new Exception("obj不是stl数据");

            vector3 viewVector = new vector3(0, 0, camZ);

            //var a = stl_Facet.Select((s, i) => (viewVector.DotProduct(s) > 0) ? i : -1)
            //    .Where(s => s > 0).ToArray();
            //var facet = a.Select(s => stl_Facet[s]).ToList();
            //a = a.Select(s => s ).ToArray();
            //List<vector3> trangle = a.Select(s => stl_Trangle[s] as vector3).ToList();
            ////var b = a.Select(s => s * 2).ToArray();
            ////trangle.AddRange(b.Select(s => stl_Trangle[s] as vector3).ToList());
            ////var c = a.Select(s => s * 3).ToArray();
            ////trangle.AddRange(c.Select(s => stl_Trangle[s] as vector3).ToList());
            //var asctrangle = trangle.Select(s => new ascPoint(s.x, s.y, s.z)).ToList();

            //var surface = new asc(asctrangle);
            //return new gl_object(surface);
            CameraAttribute.RealsenseD435_Hardware_Attribute d435 =
                new CameraAttribute.RealsenseD435_Hardware_Attribute();
            var a = stl_Facet.Select(s => s.GetProjectionDeg()).Select((s, i) => (s.degX < d435.Depth.FOV.H &&
             s.degX > -d435.Depth.FOV.H &&
             s.degY < d435.Depth.FOV.V &&
             s.degY > -d435.Depth.FOV.V) ? i : -1).Where(s => s > 0).ToArray();
            var facet = a.Select(s => stl_Facet[s]).ToList();
            List<vector3> trangle = a.Select(s => stl_Trangle[s] as vector3).ToList();
            var b = a.Select(s => s * 2).ToArray();
            trangle.AddRange(b.Select(s => stl_Trangle[s] as vector3).ToList());
            var c = a.Select(s => s * 3).ToArray();
            trangle.AddRange(c.Select(s => stl_Trangle[s] as vector3).ToList());
            var asctrangle = trangle.Select(s => new ascPoint(s.x, s.y, s.z)).ToList();
            var surface = new asc(asctrangle);
            return new gl_object(surface);
        }

        public gl_object DepthClone() => depthCloneHandle();

        public void Draw_gl_object(OpenGL gL) => drawHandle(gL);
        //stl的绘制
        void DrawStl(OpenGL gl)
        {
            var stl_Facet = this.stl_Facet;
            var stl_Trangle = this.stl_Trangle;
            var sample = this.sample;
            gl.Begin(OpenGL.GL_TRIANGLES);
            {
                for (uint i = 0; i < stl_Facet.Length; i += sample)
                {
                    if (i >= stl_Trangle.Length) break;
                    gl.Color(1f, 0.0f, 0.0f);
                    gl.Normal(stl_Facet[i].x, stl_Facet[i].y, stl_Facet[i].z);
                    gl.Vertex(stl_Trangle[i * 3].x, stl_Trangle[i * 3].y, stl_Trangle[i * 3].z);
                    gl.Normal(stl_Facet[i].x, stl_Facet[i].y, stl_Facet[i].z);
                    gl.Vertex(stl_Trangle[i * 3 + 1].x, stl_Trangle[i * 3 + 1].y, stl_Trangle[i * 3 + 1].z);
                    gl.Normal(stl_Facet[i].x, stl_Facet[i].y, stl_Facet[i].z);
                    gl.Vertex(stl_Trangle[i * 3 + 2].x, stl_Trangle[i * 3 + 2].y, stl_Trangle[i * 3 + 2].z);
                }
            }
            gl.End();
        }
       
        //asc的绘制
        void DrawAsc(OpenGL gl)
        {
            var ascPoints = this.ascPoints;
            var ascColors = this.ascPoints;
            var sample = this.sample;
            gl.Begin(OpenGL.GL_POINTS);
            {
                for (uint i = 0; i < ascPoints.Length; i += sample)
                {
                    //gl.Color(ascColors[i].x, ascColors[i].y, ascColors[i].z);
                    gl.Color(1, 0, 0);
                    gl.Vertex(ascColors[i].x, ascColors[i].y, ascColors[i].z);
                }
            }
            gl.End();
        }
    }
}
