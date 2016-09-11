using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Helpers
{
    static class OperandSplitter
    {
        /// <summary>
        /// Finds left and right numbers so that
        /// left *@operand* right = goal
        /// </summary>
        /// <returns>True if split was successful, false otherwise</returns>
        public static bool Split(int goal, char @operator, out int left, out int right)
        {
            left = right = -1;
            int naturalModification = Configuration.AllowZero ? 0 : 1;
            if (goal < naturalModification)
                throw new ArgumentOutOfRangeException("goal");
            if (@operator == '+')
            {
                // If goal is too big, try to even out the operands as much as possible
                if (goal / 2 >= Configuration.MaxOperandValue)
                {
                    left = goal / 2;
                    right = goal - left;
                    return true;
                }
                
                int lowerBound = Math.Max(naturalModification, goal - Configuration.MaxOperandValue);
                int upperBound = Math.Min(Configuration.MaxOperandValue, goal-naturalModification);

                if (upperBound < lowerBound)
                    return false;

                left = ExpressionGenerator.Random.Next(lowerBound, upperBound + 1);
                right = goal - left;

                return true;
            }
            else if(@operator == '-')
            {
                if(goal >= Configuration.MaxOperandValue)
                {
                    left = goal + naturalModification;
                    right = naturalModification;
                    return true;
                }

                left = ExpressionGenerator.Random.Next(goal+naturalModification, Configuration.MaxOperandValue + 1);
                right = left - goal;
                return true;
            }
            else if(@operator == '*')
            {
                int threshold;
                if(goal >= Configuration.MaxOperandValue * Configuration.MaxOperandValue)
                    threshold = (int)Math.Sqrt((double)Configuration.MaxOperandValue);
                    
                else
                    threshold = goal / 2;

                for (int i = 0; threshold - i >= 1 && threshold + i <= goal; i++)
                {
                    if (goal % (threshold - i) == 0)
                    {
                        left = threshold - i;
                        right = goal / left;
                        break;
                    }
                    if (goal % (threshold + i) == 0)
                    {
                        left = threshold + i;
                        right = goal / left;
                        break;
                    }
                }

                if (left > -1 && right > -1)
                    return true;

                if (ExpressionGenerator.Random.Next(2) == 0)
                {
                    left = 1;
                    right = goal;
                }
                else
                {
                    left = goal;
                    right = 1;
                }
                return true;
            }
            else if(@operator == '/')
            {
                if(goal >= Configuration.MaxOperandValue)
                {
                    left = goal;
                    right = 1;
                    return true;
                }

                int maxMultiplier = Configuration.MaxOperandValue / goal;
                right = ExpressionGenerator.Random.Next(1, maxMultiplier + 1);
                left = goal * right;
                return true;
            }
            return false;
        }
    }
}
