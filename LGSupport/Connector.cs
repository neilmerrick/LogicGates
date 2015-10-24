using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
//using Gates;

namespace LGSupport
{
    public class Connector
    {
        private IOPoint input, output;

        public IOPoint InputPoint { get { return input; } set { input = value; } }
        public IOPoint OutputPoint { get { return output; } set { output = value; } }
        public Boolean Input { set { input.IO = value; } }
        public Boolean Output { get { return output.IO; } }

        private void Set()
        {
            output.IO = input.IO;
        }

        public Connector()
        {
            input = new IOPoint("Input");
            output = new IOPoint("Output");

            input.Parent = this;
            output.Parent = this;

            input.Changed += new IOPoint.ChangedEventHandler(InputChanged);
            output.Changed += new IOPoint.ChangedEventHandler(OutputChanged);
        }

        private void InputChanged(object sender, EventArgs e)
        {
            Set();
        }

        private void OutputChanged(object sender, EventArgs e)
        {
            IOPoint iop = (IOPoint)sender;
            foreach (IOPoint iop2 in iop.Connected)
            {
                iop2.IO = iop.IO;
            }
        }

        public void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(Support.p, input.Connected[0].Location, output.Connected[0].Location);
        }
    }
}
