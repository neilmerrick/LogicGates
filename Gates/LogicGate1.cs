using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGSupport;

namespace Gates
{
    public class LogicGate1: LogicGate
    {
        protected Bit input, output;
        public IOPoint InputPoint { get { return input.InputPoint; } }
        public IOPoint OutputPoint { get { return output.OutputPoint; } }

        public bool Input { set { input.InputPoint.IO = value; } }
        public bool Output { get { return output.OutputPoint.IO; } }

        public static Boolean IsLogicGate1(object c)
        {
            return (c.GetType() == typeof(NotGate) || c.GetType() == typeof(Bit));
        }
    }
}
