using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gates;
using LGSupport;
using System.Drawing;
using System.Windows.Forms;

namespace Components
{
    public class FullSubtractor: LGComponent
    {
        private Bit inputB, outputB;
        private XOrGate xor1, xor2;
        private AndGate and1, and2;
        private OrGate or1;
        private NotGate not1, not2;

        public IOPoint InputB { get { return inputB.InputPoint; } }
        public IOPoint OutputB { get { return outputB.OutputPoint; } }
        private Rectangle r;

        public FullSubtractor()
        {
            input0 = new Bit();
            input1 = new Bit();
            inputB = new Bit();
            outputS = new Bit();
            outputB = new Bit();

            input0.Draggable = false;
            input1.Draggable = false;
            inputB.Draggable = false;
            outputS.Draggable = false;
            outputB.Draggable = false;

            input0.Deletable = false;
            input1.Deletable = false;
            inputB.Deletable = false;
            outputS.Deletable = false;
            outputB.Deletable = false;

            input0.Bittype = Bit.BitType.Input;
            input1.Bittype = Bit.BitType.Input;
            inputB.Bittype = Bit.BitType.Input;

            outputS.Bittype = Bit.BitType.Output;
            outputB.Bittype = Bit.BitType.Output;
            
            xor1 = new XOrGate();
            xor2 = new XOrGate();
            and1 = new AndGate();
            and2 = new AndGate();
            or1 = new OrGate();
            not1 = new NotGate();
            not2 = new NotGate();

            Connections.CrossConnect(input0.OutputPoint, xor1.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint, xor1.InputPoint1);
            Connections.CrossConnect(input0.OutputPoint, not1.InputPoint);
            Connections.CrossConnect(not1.OutputPoint, and1.InputPoint0);
            Connections.CrossConnect(input1.OutputPoint, and1.InputPoint1);
            Connections.CrossConnect(xor1.OutputPoint, xor2.InputPoint0);
            Connections.CrossConnect(inputB.OutputPoint, xor2.InputPoint1);
            Connections.CrossConnect(xor1.OutputPoint, not2.InputPoint);
            Connections.CrossConnect(not2.OutputPoint, and2.InputPoint0);
            Connections.CrossConnect(inputB.OutputPoint, and2.InputPoint1);
            Connections.CrossConnect(xor2.OutputPoint, outputS.InputPoint);
            Connections.CrossConnect(and1.OutputPoint, or1.InputPoint0);
            Connections.CrossConnect(and2.OutputPoint, or1.InputPoint1);
            Connections.CrossConnect(or1.OutputPoint, outputB.InputPoint);

            this.Controls.Add(input0);
            this.Controls.Add(input1);
            this.Controls.Add(inputB);
            this.Controls.Add(outputS);
            this.Controls.Add(outputB);

            this.ContextMenuStrip = cms;
        }

        public void UpdateLocation()
        {
            UpdateBitLocation(input0, 0, 0.5f);
            UpdateBitLocation(input1, 0, 0.25f);
            UpdateBitLocation(inputB, 0, 0.75f);
            UpdateBitLocation(outputS, this.Width - outputS.Width, 2f / 3f);
            UpdateBitLocation(outputB, this.Width - outputB.Width, 1f / 3f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            setSize(input0);
            setSize(input1);
            setSize(inputB);
            setSize(outputS);
            setSize(outputB);
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
            e.Graphics.DrawString("FS", this.Font, Support.boff, r, Support.sf());
        }

        public override void DeleteConnections()
        {
            input0.DeleteConnections();
            input1.DeleteConnections();
            inputB.DeleteConnections();
            outputS.DeleteConnections();
            outputB.DeleteConnections();
        }
    }
}
