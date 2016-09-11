using System;

namespace ExpressionGenerator
{
    class Configuration
    {
        [Flags]
        public enum Operators //[[3]]
        {
            NONE = 0,
            ADD  = 1,
            SUB  = 2,
            MUL  = 4,
            DIV  = 8,
            ALL  = 15,
        }

        public static int       MaxOperandValue;
        public static bool      AllowZero;
        public static Operators AllowedOperators;
    }
}
