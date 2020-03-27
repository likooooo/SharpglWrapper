namespace SharpglWrapper
{
    public class gl_object
    {
        internal SharpGL.OpenGL GL;


        public gl_object(SharpGL.OpenGL GL) => this.GL = GL;
        //public gl_object() => GL = new SharpGL.OpenGL();
    }

}
