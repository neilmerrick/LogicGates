using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Gates;
using LGSupport;

namespace Components
{
    public class HalfAdder: LGComponent
    {
        private Bit outputC;
        private AndGate and1;
        private XOrGate xor1;
        Rectangle r;

        public IOPoint OutputC { get { return outputC.OutputPoint; } }

        public HalfAdder()
        {
            input0 = new Bit("Input", "Output", this);
            input1 = new Bit("Input", "Output", this);
            outputS = new Bit("Input", "Output", this);
            outputC = new Bit("Input", "Output", this);

            input0.Draggable = false;
            input1.Draggable = false;
            outputS.Draggable = false;
            outputC.Draggable = false;

            input0.Deletable = false;
            input1.Deletable = false;
            outputS.Deletable = false;
            outputC.Deletable = false;

            input0.Bittype = Bit.BitType.Input;
            input1.Bittype = Bit.BitType.Input;
            outputS.Bittype = Bit.BitType.Output;
            outputC.Bittype = Bit.BitType.Output;

            and1 = new AndGate();
            xor1 = new XOrGate();

            Connections.CrossConnect(input0.OutputPoint, xor1.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint, xor1.InputPoint1);
            Connections.CrossConnect(input0.OutputPoint, and1.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint, and1.InputPoint1);
            Connections.CrossConnect(xor1.OutputPoint, outputS.InputPoint);
            Connections.CrossConnect(and1.OutputPoint, outputC.InputPoint);

            this.Controls.Add(input0);
            this.Controls.Add(input1);
            this.Controls.Add(outputS);
            this.Controls.Add(outputC);

            this.ContextMenuStrip = cms;
        }

        public void UpdateLocation()
        {
            UpdateBitLocation(input0, 0, 2f / 3f);
            UpdateBitLocation(input1, 0, 1f / 3f);
            UpdateBitLocation(outputS, this.Width - outputS.Width, 2f / 3f);
            UpdateBitLocation(outputC, this.Width - outputC.Width, 1f / 3f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            setSize(input0);
            setSize(input1);
            setSize(outputS);
            setSize(outputC);
            UpdateLocation();
            r = new Rectangle(input0.Width / 2, input0.Height / 2, this.Width - input0.Width, this.Height - input0.Height);
        }

        public override void Refresh()
        {
            base.Refresh();
            this.Parent.Refresh();
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {

            if (base.state == State.MOVING)
                UpdateLocation();

            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Support.p, r);
            e.Graphics.DrawString("HA", this.Font, Support.boff, r, Support.sf());
        }

        public override void DeleteConnections()
        {
            input0.DeleteConnections();
            input1.DeleteConnections();
            outputS.DeleteConnections();
            outputC.DeleteConnections();
        }
    }
}
