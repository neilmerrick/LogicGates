using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Gates;
using LGSupport;

namespace Components
{
    public class FullAdder: LGComponent
    {
        private Bit inputC, outputC;
        private XOrGate xor1, xor2;
        private AndGate and1, and2;
        private OrGate or1;

        private Rectangle r;

        public IOPoint OutputC { get { return outputC.OutputPoint; } }

        public FullAdder()
        {
            inputC = new Bit();
            input1 = new Bit();
            input0 = new Bit();
            outputS = new Bit();
            outputC = new Bit();

            input0.Draggable = false;
            input1.Draggable = false;
            inputC.Draggable = false;
            outputS.Draggable = false;
            outputC.Draggable = false;

            input0.Deletable = false;
            input1.Deletable = false;
            inputC.Deletable = false;
            outputS.Deletable = false;
            outputC.Deletable = false;

            input0.Bittype = Bit.BitType.Input;
            input1.Bittype = Bit.BitType.Input;
            inputC.Bittype = Bit.BitType.Input;

            outputS.Bittype = Bit.BitType.Output;
            outputC.Bittype = Bit.BitType.Output;
            
            xor1 = new XOrGate();
            xor2 = new XOrGate();
            and1 = new AndGate();
            and2 = new AndGate();
            or1 = new OrGate();

            Connections.CrossConnect(input0.OutputPoint, xor1.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint, xor1.InputPoint1);
            Connections.CrossConnect(input0.OutputPoint, and1.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint, and1.InputPoint1);
            Connections.CrossConnect(xor1.OutputPoint, xor2.InputPoint0);
            Connections.CrossConnect(inputC.OutputPoint, xor2.InputPoint1);
            Connections.CrossConnect(xor1.OutputPoint, and2.InputPoint0);
            Connections.CrossConnect(inputC.OutputPoint, and2.InputPoint1);
            Connections.CrossConnect(xor2.OutputPoint, outputS.InputPoint);
            Connections.CrossConnect(and1.OutputPoint, or1.InputPoint0);
            Connections.CrossConnect(and2.OutputPoint, or1.InputPoint1);
            Connections.CrossConnect(or1.OutputPoint, outputC.InputPoint);

            this.Controls.Add(input0);
            this.Controls.Add(input1);
            this.Controls.Add(inputC);
            this.Controls.Add(outputS);
            this.Controls.Add(outputC);

            this.ContextMenuStrip = cms;
        }

        public void UpdateLocation()
        {
            UpdateBitLocation(input0, 0, 0.5f);
            UpdateBitLocation(input1, 0, 0.25f);
            UpdateBitLocation(inputC, 0, 0.75f);
            UpdateBitLocation(outputS, this.Width - outputS.Width, 2f / 3f);
            UpdateBitLocation(outputC, this.Width - outputC.Width, 1f / 3f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            setSize(input0);
            setSize(input1);
            setSize(inputC);
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
            e.Graphics.DrawRectangle(Support.p, r);
            base.OnPaint(e);
            e.Graphics.DrawString("FA", this.Font, Support.boff, r, Support.sf());
        }

        public override void DeleteConnections()
        {
            input0.DeleteConnections();
            input1.DeleteConnections();
            inputC.DeleteConnections();
            outputS.DeleteConnections();
            outputC.DeleteConnections();
        }

        public IOPoint InputC { get { return inputC.InputPoint; } }
    }
}
