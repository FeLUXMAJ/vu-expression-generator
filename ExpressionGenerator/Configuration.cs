using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator
{
    class Configuration
    {
        [Flags]
        public enum Operators
        {
            ADD = 1,
            SUB = 2,
            MUL = 4,
            DIV = 8,
            ALL = 15,
        }

        public static int       MaxOperandValue;
        public static bool      OnlyNaturalNumbers;
        public static Operators AllowedOperators;
    }
}
