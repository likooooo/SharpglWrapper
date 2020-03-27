using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpglWrapper
{
    public delegate void DrawEventHandler(SharpGL.OpenGL gL);
    public delegate (double minX, double maxX, double minY, double maxY, double minZ, double maxZ) BoundingBox();
    public class gl_tuple
    {
        internal vector3[] points;
        internal pc_texture pc_Texture;
        internal pc_verctor3 Pc_Verctor;
        internal stl_trangle[] stl_Trangle;
        internal stl_facet[] stl_Facet;

        public uint drawMod;

        public DrawEventHandler DrawEventHandler;
        BoundingBox getBoundingBox;
        public (double minX, double maxX, double minY, double maxY, double minZ, double maxZ) BoundingBox
        { get => getBoundingBox(); }
        public gl_tuple(pc_verctor3 Pc_Verctor)
        {
            this.Pc_Verctor = Pc_Verctor;
            drawMod = SharpGL.OpenGL.GL_POINTS;

        }
        public gl_tuple(stl_trangle[] stl_Trangle, stl_facet[] stl_Facet)
        {
            this.stl_Trangle = stl_Trangle;
            this.stl_Facet = stl_Facet;
            drawMod = SharpGL.OpenGL.GL_POINTS;//GL_TRIANGLES;
            DrawEventHandler = new DrawEventHandler(DrawStl);
            getBoundingBox = new BoundingBox(GetBoundingBox_stl);
        }

        void DrawStl(SharpGL.OpenGL gl)
        {
            int m_div = 1;
            for (int i = 0; i < stl_Trangle.Length; i++)
            {
                //gl.Color(1.0f, 0.0f, 0.0f);
                //gl.Normal(stl_Facet[i].x, stl_Facet[i].y, stl_Facet[i].z);
                gl.Vertex(stl_Trangle[i].point0.x, stl_Trangle[i].point0.y, stl_Trangle[i].point0.z);
                //gl.Color(1.0f, 0.0f, 0.0f);
                //gl.Normal(stl_Facet[i].x, stl_Facet[i].y, stl_Facet[i].z);
                gl.Vertex(stl_Trangle[i].point1.x, stl_Trangle[i].point1.y, stl_Trangle[i].point1.z);
                //gl.Color(1.0f, 0.0f, 0.0f);
                //gl.Normal(stl_Facet[i].x, stl_Facet[i].y, stl_Facet[i].z);
                gl.Vertex(stl_Trangle[i].point2.x, stl_Trangle[i].point2.y, stl_Trangle[i].point2.z);
                //gl.Color(1.0f, 0.0f, 0.0f);
                //gl.Normal(vnorms[i] / m_div, vnorms[i + 1] / m_div, vnorms[i + 2] / m_div);
                //gl.Vertex((-verts[i] + verts[1]) / m_div, (-verts[i + 1] + verts[2]) / m_div, (-verts[i + 2] + verts[3]) / m_div);
                //i += 3;
                //gl.Color3f(0.0f, 1.0f, 0.0f);
                //gl.Normal3f(vnorms[i] / m_div, vnorms[i + 1] / m_div, vnorms[i + 2] / m_div);
                //gl.Vertex3f((-verts[i] + verts[1]) / m_div, (-verts[i + 1] + verts[2]) / m_div, (-verts[i + 2] + verts[3]) / m_div);
                //i += 3;
                ////glColor3f(0.0f, 0.0f, 1.0f);
                //gl.Normal3f(vnorms[i] / m_div, vnorms[i + 1] / m_div, vnorms[i + 2] / m_div);
                //gl.Vertex3f((-verts[i] + verts[1]) / m_div, (-verts[i + 1] + verts[2]) / m_div, (-verts[i + 2] + verts[3]) / m_div);
                //i += 2;

            }
        }
        (double minX, double maxX, double minY, double maxY, double minZ, double maxZ)
            GetBoundingBox_stl()
        {
            return null;
        }
    }
}
