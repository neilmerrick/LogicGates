using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGSupport;

namespace Gates
{
    public class LogicGate2: LogicGate
    {
        protected Bit input0, input1, output;

        public IOPoint InputPoint0 { get { return input0.InputPoint; } }
        public IOPoint InputPoint1 { get { return input1.InputPoint; } }
        public IOPoint OutputPoint { get { return output.OutputPoint; } }

        public bool Input0 { set { input0.InputPoint.IO = value; } }

        public bool Input1 { set { input1.InputPoint.IO = value; } }

        public bool Output { get { return output.OutputPoint.IO; } }

        public static Boolean IsLogicGate2(object c)
        {
            return (
                c.GetType() == typeof(AndGate) || c.GetType() == typeof(OrGate) || c.GetType() == typeof(XOrGate) ||
                c.GetType() == typeof(NandGate) || c.GetType() == typeof(NorGate)
                );
        }
    }
}
