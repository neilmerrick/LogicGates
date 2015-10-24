using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using LGSupport;

namespace Gates
{
    public class Bit: LogicGate, ILogicGate
    {
        public enum BitType { Input, Output, Both };
        public static readonly new Size small = new Size(6, 6);
        public static readonly new Size medium = new Size(8, 8);
        public static readonly new Size large = new Size(10, 10);

        private IOPoint input, output;

        public IOPoint InputPoint { get { return input; } }
        public IOPoint OutputPoint { get { return output; } }

        public Boolean Input { set { input.IO = value; } }
        public Boolean Output { get { return output.IO; } }

        private ToolStripMenuItem rem = new ToolStripMenuItem();
        private ToolStripMenuItem tog = new ToolStripMenuItem();

        public delegate void BitTypeEventHandler(Object sender, EventArgs e);
        public event BitTypeEventHandler BitTypeChanged;

        public delegate void BitChangedEventHandler(Object sender, EventArgs e);
        public event BitChangedEventHandler BitChanged;

        protected virtual void OnBitTypeChanged(EventArgs e)
        {
            if (BitTypeChanged != null)
            {
                BitTypeChanged(this, e);
            }
        }

        protected virtual void OnBitChanged(EventArgs e)
        {
            if (BitChanged != null)
            {
                BitChanged(this, e);
            }
        }
        private Boolean dragged = false;

        private BitType _bittype = BitType.Both;

        public BitType Bittype { get { return _bittype; } set { _bittype = value; OnBitTypeChanged(EventArgs.Empty); } }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            input.Rect = new RectangleF(new Point(0,0), this.Size);
            output.Rect = new RectangleF(new Point(0,0), this.Size);
            UpdateLocation();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.Draggable)
            {
                if (base.state == State.MOVING)
                {
                    UpdateLocation();
                    dragged = true;
                }
            }
            base.OnMouseMove(e);
        }

        public void UpdateLocation()
        {
            UpdateLocation(new PointF(this.Location.X + this.Size.Width * 0.5f, this.Location.Y + this.Size.Height * 0.5f),
                           new PointF(this.Location.X + this.Size.Width * 0.5f, this.Location.Y + this.Size.Height * 0.5f));
        }

        public void UpdateLocation(Boolean callParent)
        {
            UpdateLocation(new PointF(this.Location.X + this.Size.Width * 0.5f, this.Location.Y + this.Size.Height * 0.5f),
                           new PointF(this.Location.X + this.Size.Width * 0.5f, this.Location.Y + this.Size.Height * 0.5f),
                           callParent);
        }

        public void UpdateLocation(PointF inploc, PointF outloc) 
        {
            UpdateLocation(inploc, outloc, false);
        }

        public void UpdateLocation(PointF inploc, PointF outloc, Boolean callParent)
        {
            if (callParent)
            {
                System.Reflection.MethodInfo mi = this.Parent.GetType().GetMethod("UpdateLocation");
                if (mi != null) { mi.Invoke(this.Parent, null); } else { updatelocation(inploc, outloc); }
            }
            else
                updatelocation(inploc, outloc);
        }

        private void updatelocation(PointF inploc, PointF outloc)
        {
            input.Location = inploc;
            output.Location = outloc;
        }

        public void CancelConnection()
        {
            rem.Enabled = false;
        }

        public Bit()
            : this("Input", "Output", null)
        {
        }

        public Bit(String inputname, String outputname, object parent)
        {
            input = new IOPoint(inputname);
            output = new IOPoint(outputname);

            if (parent == null)
            {
                input.Parent = this;
                output.Parent = this;
            }
            else
            {
                input.Parent = parent;
                output.Parent = parent;
            }

            input.Changed += new IOPoint.ChangedEventHandler(inputChanged);
            output.Changed += new IOPoint.ChangedEventHandler(outputChanged);

            ClickableChanged += new LogicGate.ClickableEventHandler(Clickable_Changed);
            DeletableChanged += new LogicGate.DeleteableEventHandler(Deletable_Changed);
            BitTypeChanged += new Bit.BitTypeEventHandler(BitType_Changed);

            rem.Text = "Remove Connections";
            rem.Click += new EventHandler(Remove_Click);
            rem.Enabled = false;
            rem.AutoSize = true;

            tog.Text = "Toggle On/Off";
            tog.Click += new EventHandler(Toggle);
            tog.AutoSize = true;

            cms.Items.AddRange(new ToolStripItem[]{tog, rem});
            cms.AutoSize = true;
            cms.Opening += new CancelEventHandler(cmsOpen);
            this.ContextMenuStrip = cms;
        }

        public void Set()
        {
            output.IO = input.IO;
            this.Invalidate();
        }

        private void inputChanged(object sender, EventArgs e)
        {
            ((IOPoint)sender).CheckPoint();
            Set();
            if (this.Parent != null && this.Parent.GetType() == typeof(Nibble)) ((Nibble)this.Parent).Calc();
            OnBitChanged(EventArgs.Empty);
        }

        private void outputChanged(object sender, EventArgs e)
        {
            IOPoint iop = (IOPoint)sender;
            foreach (IOPoint iop2 in iop.Connected)
            {
                iop2.IO = iop.IO;
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            if (_bittype == BitType.Input)
            {
                input.onPaint(g);
            }
            else
            {
                output.onPaint(g);
            }
        }

        private void Toggle(Object sender, EventArgs e)
        {
            if (!Connections.Connecting)
            {
                if (input.Connected.Count == 0 && this.Clickable) input.IO = !input.IO;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (!dragged)
            {
                if (e.Button == MouseButtons.Left)
                {
                    UpdateLocation(true);

                    if (!Connections.Connecting)
                    {
                        if (_bittype != BitType.Input)
                        {
                            Connections.connectFrom(OutputPoint);
                        }
                    }
                    else
                    {
                        if (!OutputPoint.Connecting && _bittype != BitType.Output)
                        {
                            Connections.connectTo(InputPoint);
                            tog.Enabled = false;
                        }
                        else if (OutputPoint.Connecting)
                        {
                            Connections.CancelConnection();
                            if (Clickable && _bittype != BitType.Output) tog.Enabled = true;
                        }
                    }
                    Set();
                    this.Parent.Refresh();
                }
            }
            else
            {
                dragged = false;
            }
        }

        private void Remove_Click(Object sender, EventArgs e)
        {
            DeleteConnections();
            if (Clickable) tog.Enabled = true;
            this.Parent.Refresh();
        }

        public override void DeleteConnections()
        {
            if (_bittype == BitType.Input) Connections.removeConnections(input);
            else if (_bittype == BitType.Output) Connections.removeConnections(output);
            else
            {
                Connections.removeConnections(input);
                Connections.removeConnections(output);
            }
        }

        private void Clickable_Changed(Object sender, EventArgs e)
        {
            if (Clickable)
            {
                if (_bittype != BitType.Output) tog.Enabled = true;
            }
            else
            {
                tog.Enabled = false;
            }
        }

        private void Deletable_Changed(Object sender, EventArgs e)
        {
            if (Deletable)
            {
                del.Visible = true;
            }
            else
            {
                del.Visible = false;
            }
        }

        private void BitType_Changed(Object sender, EventArgs e)
        {
            if (_bittype == BitType.Output)
            {
                tog.Enabled = false;
            }
            else
            {
                if (Clickable) tog.Enabled = true;
            }
        }

        private void cmsOpen(Object sender, CancelEventArgs e)
        {
            if (_bittype == BitType.Input)
            {
                rem.Enabled = (input.Connected.Count > 0);
                if (Clickable) tog.Enabled = (input.Connected.Count == 0);
            }
            else if (_bittype == BitType.Output)
            {
                rem.Enabled = (output.Connected.Count > 0);
            }
            else if (_bittype == BitType.Both)
            {
                rem.Enabled = ((input.Connected.Count + output.Connected.Count) > 0);
                if (Clickable) tog.Enabled = (input.Connected.Count == 0);
            }
        }
    }
}
