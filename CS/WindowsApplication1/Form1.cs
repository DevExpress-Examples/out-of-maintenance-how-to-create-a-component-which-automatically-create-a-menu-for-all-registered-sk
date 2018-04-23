using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars;

namespace WindowsApplication1
{
    public partial class Form1 : XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }
    }
    public class MyDesignTimeSkinsBarMenuHelper: Component
    {

        private BarManager _BarManager;
        public BarManager BarManager
        {
            get { return _BarManager; }
            set { _BarManager = value; CreateMenu(); }
        }

        private DefaultLookAndFeel _LookAndFeel;
        public DefaultLookAndFeel LookAndFeel
        {
            get { return _LookAndFeel; }
            set { _LookAndFeel = value; CreateMenu(); }
        }

        void CreateMenu()
        {

            if (BarManager != null && LookAndFeel != null)
            {
                if (DesignMode)
                {
                    new MySkinsBarMenuHelper(BarManager, LookAndFeel);
                }
                else
                {
                    Form form = BarManager.Form as Form;
                    if (form != null) form.Load += form_Load;
                }
            }

        }

        void form_Load(object sender, EventArgs e)
        {
            new MySkinsBarMenuHelper(BarManager, LookAndFeel);
        }

        public MyDesignTimeSkinsBarMenuHelper()
        {
            
        }
    }
}