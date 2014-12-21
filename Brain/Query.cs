using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    public partial class Query : Form
    {
        public Query()
        {
            InitializeComponent();

            add();
        }

        void add()
        {
            temporalQuery.Items.Add("jupiter");
            temporalQuery.Items.Add("saturn");
            //temporalQuery.
            temporalQuery.SelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

        }

        public int Index
        {
            get
            {
                return temporalQuery.SelectedIndex;
            }
        }

        public int Interval
        {
            get
            {
                return (int)temporalNumber.Value;
            }
        }
    }
}
