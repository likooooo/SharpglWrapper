using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL;
namespace SharpglWrapper
{
    //Sharpgl窗体的属性
    public class SW_Model
    {
        public const uint Lines = OpenGL.GL_LINES;
        public const uint LineLoop = OpenGL.GL_LINE_LOOP;
        public const uint Quads = OpenGL.GL_QUADS;


        /**
         * **/
        private uint model;
        public uint GetModelType()
        {
            return model;
        }


        /**构造函数
         * **/
        public SW_Model(uint model= SW_Model.LineLoop)
        {
            this.model = model;
        }


        /**uint->SW_Model的隐式转换
         * **/
        public static explicit operator SW_Model(uint model)
        {
            SW_Model m = new SW_Model(model);
            return m;
        }
    }

  
}
