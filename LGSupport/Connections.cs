using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
//using Gates;
//using Components;

namespace LGSupport
{
    public class Connections
    {
        public static List<Connector> connections = new List<Connector>();
        public static bool Connecting;
        private static Connector conn;
        public static Control ctl;

        public static PointF ConnectingFromLocation { get { if (conn != null) return conn.InputPoint.Location; else return new PointF(0f, 0f); } }

        public static void connectFrom(IOPoint sender)
        {
            if (!Connecting)
            {
                conn = new Connector();
                CrossConnect(conn.InputPoint, sender, true);
                /*
                if (Support.IsLogicGate1(ctl))
                {
                    CrossConnect(conn.InputPoint, ((LogicGate1)ctl).OutputPoint, true);
                }
                else if (Support.IsLogicGate2(ctl))
                {
                    CrossConnect(conn.InputPoint, ((LogicGate2)ctl).OutputPoint, true);
                }
                else if (Support.IsNibble(ctl))
                {
                    Nibble g = (Nibble)ctl;
                    switch (type)
                    {
                        case "output1": CrossConnect(conn.InputPoint, g.OutputPoint1, true); break;
                        case "output2": CrossConnect(conn.InputPoint, g.OutputPoint2, true); break;
                        case "output3": CrossConnect(conn.InputPoint, g.OutputPoint3, true); break;
                        case "output4": CrossConnect(conn.InputPoint, g.OutputPoint4, true); break;
                    }
                }
                */
                Connecting = true;
            }
        }

        public static void CrossConnect(IOPoint input, IOPoint output)
        {
            CrossConnect(input, output, false);
        }

        public static void CrossConnect(IOPoint input, IOPoint output, Boolean connecting)
        {
            input.Connected.Add(output);
            output.Connected.Add(input);
            output.Connecting = connecting;
        }

        public static void connectTo(IOPoint sender)
        {
            if (Connecting)
            {
                /*
                if (Support.IsLogicGate1(ctl))
                {
                    CrossConnect(((LogicGate1)ctl).InputPoint, conn.OutputPoint);
                }
                else if (Support.IsLogicGate2(ctl))
                {
                    switch (type)
                    {
                        case "input1": CrossConnect(((LogicGate2)ctl).InputPoint1, conn.OutputPoint); break;
                        case "input2": CrossConnect(((LogicGate2)ctl).InputPoint2, conn.OutputPoint); break;
                    }
                }
                else if (Support.IsNibble(ctl))
                {
                    switch (type)
                    {
                        case "input1": CrossConnect(((Nibble)ctl).InputPoint1, conn.OutputPoint); break;
                        case "input2": CrossConnect(((Nibble)ctl).InputPoint2, conn.OutputPoint); break;
                        case "input3": CrossConnect(((Nibble)ctl).InputPoint3, conn.OutputPoint); break;
                        case "input4": CrossConnect(((Nibble)ctl).InputPoint4, conn.OutputPoint); break;
                    }
                    ((Nibble)ctl).CheckConnections();
                }
                */

                CrossConnect(conn.OutputPoint, sender);
                foreach (IOPoint iop in conn.InputPoint.Connected)
                {
                    iop.Connecting = false;
                }

                connections.Add(conn);
                conn.Input = conn.InputPoint.Connected[0].IO;
                conn = null;
                Connecting = false;
            }
        }

        public static void CancelConnection()
        {
            foreach (IOPoint iop in conn.InputPoint.Connected)
            {
                iop.Connecting = false;
                iop.Connected.Remove(conn.InputPoint);
                //((Bit)iop.Parent).CancelConnection();
            }
            conn = null;
            Connecting = false;

        }

        public static void removeConnections(IOPoint iop)
        {
            foreach (IOPoint iop2 in iop.Connected)
            {
                try
                {
                    Connector c = (Connector)iop2.Parent;
                    if (!c.InputPoint.Connected[0].Equals(iop))
                    {
                        c.InputPoint.Connected[0].Remove(c.InputPoint);
                    }
                    if (!c.OutputPoint.Connected[0].Equals(iop))
                    {
                        c.OutputPoint.Connected[0].Remove(c.OutputPoint);
                    }
                    connections.Remove(c);
                }
                catch
                {
                    // Ignore error
                }
            }
            iop.Clear();
        }

    }
}
