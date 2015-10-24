using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Gates;
using Components;
using Components4;
using LGSupport;

namespace LogicGates
{
    public partial class frmLogicGates : Form
    {
        private Size s;
        private float fs;
        private int namecntr = 0;
        private String dir;

        private const String filter = "Logic Gate Project (*.lgp)|*.lgp|Logic Gate Instruction (*.lgi)|*.lgi";

        Project p;
        //private Boolean dirty;

        /*  =========================================================================================
         *  Initialisers and Form Events
         *  =========================================================================================
         */

        public frmLogicGates()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //dirty = false;
        }

        private void frmLogicGates_Load(object sender, EventArgs e)
        {
            s = LogicGate.medium;
            fs = LogicGate.FontSize[1];
            Connections.Connecting = false;

            dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LogicGates";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            newProject();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            foreach (Connector conn in Connections.connections)
            {
                conn.OnPaint(e);
            }
            if (Connections.Connecting)
            {
                if (!Connections.ConnectingFromLocation.Equals(new PointF(0f, 0f)))
                {
                    e.Graphics.DrawLine(Support.p, Connections.ConnectingFromLocation, Cursor.Position);
                }
            }
        }

        // onKeyUp - Press ESC key to exit while connecting gates.
        private void onKeyUp(Object sender, KeyEventArgs e)
        {
            if (Connections.Connecting && e.KeyCode == Keys.Escape)
            {
                Connections.CancelConnection();
                e.Handled = true;
                this.Refresh();
            }
        }

        /* =========================================================================================
         * Resize Controls
         * =========================================================================================
         */

        private void menuItemChecked(object sender)
        {
            ToolStripMenuItem t = (ToolStripMenuItem)sender;

            smallToolStripMenuItem.Checked = (t.Name == smallToolStripMenuItem.Name);
            mediumToolStripMenuItem.Checked = (t.Name == mediumToolStripMenuItem.Name);
            largeToolStripMenuItem.Checked = (t.Name == largeToolStripMenuItem.Name);
        }

        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuItemChecked(sender);
            s = LogicGate.small;
            fs = LogicGate.FontSize[0];
            resizeGate();
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuItemChecked(sender);
            s = LogicGate.medium;
            fs = LogicGate.FontSize[1];
            resizeGate();
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuItemChecked(sender);
            s = LogicGate.large;
            fs = LogicGate.FontSize[2];
            resizeGate();
        }

        protected void resizeGate()
        {
            foreach (Control c in Controls)
            {
                Font f = new Font(c.Font.FontFamily, fs);
                if (LogicGate.IsNibble(c))
                {
                    c.Size = s.Equals(LogicGate.small) ? Nibble.small : s.Equals(LogicGate.medium) ? Nibble.medium : Nibble.large;
                    c.Font = f;
                }
                else if (LogicGate.isLogicGate(c))
                {
                    c.Size = s;
                    c.Font = f;
                }
                else if (LogicGate.IsBit(c))
                {
                    c.Size = new Size((int)(s.Width * 0.08f), (int)(s.Height * 0.08f));
                }
                else if (LGComponent.IsComponent(c))
                {
                    c.Size = s.Equals(LogicGate.small) ? LGComponent.small : s.Equals(LogicGate.medium) ? LGComponent.medium : LGComponent.large;
                }
                c.Invalidate();
            }
        }

        /* =========================================================================================
         * Exit 
         * =========================================================================================
         */

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /* =========================================================================================
         * Create Inputs
         * =========================================================================================
         */

        private void bitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bit g = new Bit();
            g.Location = calcLocation();
            g.Size = s.Equals(LogicGate.small) ? Bit.small : s.Equals(LogicGate.medium) ? Bit.medium : Bit.large;
            g.Input = false;
            //g.ContextMenuStrip = cmsControl;
            g.Name = g.GetType().Name + namecntr++;
            this.Controls.Add(g);
        }

        private void nibbleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nibble g = new Nibble();
            g.Location = calcLocation();
            g.Size = s.Equals(LogicGate.small) ? Nibble.small : s.Equals(LogicGate.medium) ? Nibble.medium : Nibble.large;
            g.Font = new Font(g.Font.FontFamily, fs);
            g.Value = 0;
            //g.ContextMenuStrip = cmsControl;
            g.Name = g.GetType().Name + namecntr++;
            this.Controls.Add(g);
        }

        /* =========================================================================================
         * Create 1-Bit Components
         * =========================================================================================
         */

        private void halfAdderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HalfAdder ha = new HalfAdder();
            ha.Location = calcLocation();
            ha.Size = s.Equals(LogicGate.small) ? LGComponent.small : s.Equals(LogicGate.medium) ? LGComponent.medium : LGComponent.large;
            ha.Font = new Font(ha.Font.FontFamily, fs);
            ha.Name = ha.GetType().Name + namecntr++;
            this.Controls.Add(ha);
        }

        private void fullAdderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FullAdder fa = new FullAdder();
            fa.Location = calcLocation();
            fa.Size = s.Equals(LogicGate.small) ? LGComponent.small : s.Equals(LogicGate.medium) ? LGComponent.medium : LGComponent.large;
            fa.Font = new Font(fa.Font.FontFamily, fs);
            fa.Name = fa.GetType().Name + namecntr++;
            this.Controls.Add(fa);
        }

        private void halfSubtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HalfSubtractor hs = new HalfSubtractor();
            hs.Location = calcLocation();
            hs.Size = s.Equals(LogicGate.small) ? LGComponent.small : s.Equals(LogicGate.medium) ? LGComponent.medium : LGComponent.large;
            hs.Font = new Font(hs.Font.FontFamily, fs);
            hs.Name = hs.GetType().Name + namecntr++;
            this.Controls.Add(hs);
        }

        private void fullSubtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FullSubtractor fulls = new FullSubtractor();
            fulls.Location = calcLocation();
            fulls.Size = s.Equals(LogicGate.small) ? LGComponent.small : s.Equals(LogicGate.medium) ? LGComponent.medium : LGComponent.large;
            fulls.Font = new Font(fulls.Font.FontFamily, fs);
            fulls.Name = fulls.GetType().Name + namecntr++;
            this.Controls.Add(fulls);
        }

        /* =========================================================================================
         * Create 4-Bit Components
         * =========================================================================================
         */

        private void andToolStripMenuItem_Click(object sender, EventArgs e)
        {
            And4 g = new And4();
            g.Location = calcLocation();
            g.Size = s.Equals(LogicGate.small) ? LGComponent4.small : s.Equals(LogicGate.medium) ? LGComponent4.medium : LGComponent4.large;
            g.Font = new Font(g.Font.FontFamily, fs);
            g.Name = g.GetType().Name + namecntr++;
            this.Controls.Add(g);
        }

        private void xorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XOr4 g = new XOr4();
            g.Location = calcLocation();
            g.Size = s.Equals(LogicGate.small) ? LGComponent4.small : s.Equals(LogicGate.medium) ? LGComponent4.medium : LGComponent4.large;
            g.Font = new Font(g.Font.FontFamily, fs);
            g.Name = g.GetType().Name + namecntr++;
            this.Controls.Add(g);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add4 g = new Add4();
            g.Location = calcLocation();
            g.Size = s.Equals(LogicGate.small) ? LGComponent4.small : s.Equals(LogicGate.medium) ? LGComponent4.medium : LGComponent4.large;
            g.Font = new Font(g.Font.FontFamily, fs);
            g.Name = g.GetType().Name + namecntr++;
            this.Controls.Add(g);
        }

        private void notToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Not4 g = new Not4();
            g.Location = calcLocation();
            g.Size = s.Equals(LogicGate.small) ? LGComponent4.small : s.Equals(LogicGate.medium) ? LGComponent4.medium : LGComponent4.large;
            g.Font = new Font(g.Font.FontFamily, fs);
            g.Name = g.GetType().Name + namecntr++;
            this.Controls.Add(g);
        }

        private void incToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inc4 g = new Inc4();
            g.Location = calcLocation();
            g.Size = s.Equals(LogicGate.small) ? LGComponent4.small : s.Equals(LogicGate.medium) ? LGComponent4.medium : LGComponent4.large;
            g.Font = new Font(g.Font.FontFamily, fs);
            g.Name = g.GetType().Name + namecntr++;
            this.Controls.Add(g);
        }

        private void orToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Or4 g = new Or4();
            g.Location = calcLocation();
            g.Size = s.Equals(LogicGate.small) ? LGComponent4.small : s.Equals(LogicGate.medium) ? LGComponent4.medium : LGComponent4.large;
            g.Font = new Font(g.Font.FontFamily, fs);
            g.Name = g.GetType().Name + namecntr++;
            this.Controls.Add(g);
        }

        private void subToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sub4 g = new Sub4();
            g.Location = calcLocation();
            g.Size = s.Equals(LogicGate.small) ? LGComponent4.small : s.Equals(LogicGate.medium) ? LGComponent4.medium : LGComponent4.large;
            g.Font = new Font(g.Font.FontFamily, fs);
            g.Name = g.GetType().Name + namecntr++;
            this.Controls.Add(g);
        }

        /* =========================================================================================
         * Create Logic Gate Objects
         * =========================================================================================
         */

        private LogicGate setUp(LogicGate g)
        {
            g.Size = s;
            g.Font = new Font(g.Font.FontFamily, fs);
            g.Location = calcLocation();
            g.Name = g.GetType().Name + namecntr++;
            return g;
        }

        private void andGateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AndGate g = (AndGate)setUp(new AndGate());
            g.Input0 = false;
            g.Input1 = false;
            this.Controls.Add(g);
        }

        private void orGateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrGate g = (OrGate)setUp(new OrGate());
            g.Input0 = false;
            g.Input1 = false;
            this.Controls.Add(g);
        }

        private void xorGateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XOrGate g = (XOrGate)setUp(new XOrGate());
            g.Input0 = false;
            g.Input1 = false;
            this.Controls.Add(g);
        }

        private void notGateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotGate g = (NotGate)setUp(new NotGate());
            g.Input = false;
            this.Controls.Add(g);
        }

        private void nandGateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NandGate g = (NandGate)setUp(new NandGate());
            g.Input0 = false;
            g.Input1 = false;
            this.Controls.Add(g);
        }

        private void norGateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NorGate g = (NorGate)setUp(new NorGate());
            g.Input0 = false;
            g.Input1 = false;
            this.Controls.Add(g);
        }

        /* =========================================================================================
         * Calculate Location for New Controls
         * =========================================================================================
         */

        private Point calcLocation()
        {
            Point sp = new Point(s.Width / 2, s.Height / 2);
            int rows, row, col;

            Size offset = new Size((int)(s.Width * 1.1f), (int)(s.Height * 1.1f));
            Size pos = new Size(0, 0);

            rows = (this.ClientRectangle.Width - s.Width) / offset.Width;

            row = Controls.Count;
            col = 0;
            while (row >= rows)
            {
                row -= rows;
                col += 1;
            }

            return new Point(sp.X + offset.Width * row, sp.Y + offset.Height * col);
        }

        /* =========================================================================================
         * Saving and Loading
         * =========================================================================================
         */

        private void Save()
        {
            Cursor csr = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            NameControls();

            foreach (Control c in Controls)
            {
                if (LogicGate1.IsLogicGate1(c) || LogicGate2.IsLogicGate2(c) || LogicGate.IsNibble(c))
                {
                    SerializedLogicGate slg = new SerializedLogicGate();
                    slg.Location = c.Location;
                    slg.Type = c.GetType().FullName;
                    slg.Name = c.Name;
                    if (LogicGate1.IsLogicGate1(c))
                    {
                        LogicGate1 g = (LogicGate1)c;
                        if (g.InputPoint.Connected.Count == 0) { slg.inputs.Add(new SerializedLogicGate.Inputs(g.InputPoint.IO, true)); }
                        else { slg.inputs.Add(new SerializedLogicGate.Inputs(false, false)); }
                        getConnected(g.OutputPoint, ref slg);
                    }
                    else if (LogicGate2.IsLogicGate2(c))
                    {
                        LogicGate2 g = (LogicGate2)c;
                        if (g.InputPoint0.Connected.Count == 0) { slg.inputs.Add(new SerializedLogicGate.Inputs(g.InputPoint0.IO, true)); }
                        else slg.inputs.Add(new SerializedLogicGate.Inputs(false, false));
                        if (g.InputPoint1.Connected.Count == 0) { slg.inputs.Add(new SerializedLogicGate.Inputs(g.InputPoint1.IO, true)); }
                        else slg.inputs.Add(new SerializedLogicGate.Inputs(false, false));
                        getConnected(g.OutputPoint, ref slg);
                    }
                    else if (LogicGate.IsNibble(c))
                    {
                        Nibble g = (Nibble)c;
                        if (g.InputPoint0.Connected.Count == 0) slg.inputs.Add(new SerializedLogicGate.Inputs(g.InputPoint0.IO, true));
                        else slg.inputs.Add(new SerializedLogicGate.Inputs(false, false));
                        if (g.InputPoint1.Connected.Count == 0) slg.inputs.Add(new SerializedLogicGate.Inputs(g.InputPoint1.IO, true));
                        else slg.inputs.Add(new SerializedLogicGate.Inputs(false, false));
                        if (g.InputPoint2.Connected.Count == 0) slg.inputs.Add(new SerializedLogicGate.Inputs(g.InputPoint2.IO, true));
                        else slg.inputs.Add(new SerializedLogicGate.Inputs(false, false));
                        if (g.InputPoint3.Connected.Count == 0) slg.inputs.Add(new SerializedLogicGate.Inputs(g.InputPoint3.IO, true));
                        else slg.inputs.Add(new SerializedLogicGate.Inputs(false, false));

                        getConnected(g.OutputPoint0, ref slg);
                        getConnected(g.OutputPoint1, ref slg);
                        getConnected(g.OutputPoint2, ref slg);
                        getConnected(g.OutputPoint3, ref slg);
                    }
                    p.LogicGates.Add(slg);
                }
            }

            p.Size = s;
            p.FontSize = fs;

            System.Xml.Serialization.XmlSerializer formatter = new System.Xml.Serialization.XmlSerializer(typeof(Project));
            Stream stream = new FileStream(p.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, p);
            stream.Close();
            Cursor.Current = csr;
        }

        private void FileLoad(String filename)
        {
        }

        private void NameControls()
        {
            int cntr = 0;
            foreach (Control c in Controls) {
                if (c.Name == String.Empty)
                {
                    c.Name = c.GetType().Name + cntr;
                    cntr++;
                }
            }
        }

        private void getConnected(IOPoint iop, ref SerializedLogicGate slg)
        {
            SerializedLogicGate.Outputs outputlist = new SerializedLogicGate.Outputs();
            foreach (IOPoint iop2 in iop.Connected)
            {
                Connector c = (Connector)iop2.Parent;
                Debug.Print(((Control)c.OutputPoint.Connected[0].Parent).Name);
                outputlist.ConnectedTo.Add(((Control)c.OutputPoint.Connected[0].Parent).Name + "." + c.OutputPoint.Connected[0].Name);
            }
            slg.outputs.Add(outputlist);
        }

        /* =========================================================================================
         * File Menu Functions
         * =========================================================================================
         */

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newProject();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (p.FileName == "")
            {
                saveProjectAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                Save();
            }
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = dir;
            ofd.Filter = filter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileLoad(ofd.FileName);
            }
        }

        /* =========================================================================================
         * Project Functions
         * =========================================================================================
         */

        private void newProject()
        {
            p = new Project();

            List<Control> cc = new List<Control>();

            foreach (Control c in Controls) {
                if (!LogicGate1.IsLogicGate1(c) && !LogicGate2.IsLogicGate2(c) && !LogicGate.IsNibble(c)) {
                    cc.Add(c);
                }
            }

            Controls.Clear();
            Controls.AddRange(cc.ToArray());
        }

        private void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = filter;
            sfd.InitialDirectory = dir;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                p.FileName = sfd.FileName;
                Save();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control o in Controls)
                MessageBox.Show(o.Name);
        }
    }
}
