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
