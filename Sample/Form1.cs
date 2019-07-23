using System.Windows.Forms;
namespace Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SharpglWrapper.SharpglWrapper sharpgl = new SharpglWrapper.SharpglWrapper(this.openGLControl1);
        }
    }
}
