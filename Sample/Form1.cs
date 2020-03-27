using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using SharpglWrapper.draw;
using SharpglWrapper;

namespace Sample
{
    public partial class Form1 : Form
    {
        SharpglWindow Window;



        public Form1()
        {
            InitializeComponent();
            Window = new SharpglWindow(this.openGLControl1);
            stl_io stl = new stl_io(@"C:\Users\lk\Desktop\点云样本\绝缘子带支架\a.STL");
            Window.Updae(new gl_tuple(stl.stl_trangle, stl.stl_facet));
        }



        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
 
        }
    }
}
