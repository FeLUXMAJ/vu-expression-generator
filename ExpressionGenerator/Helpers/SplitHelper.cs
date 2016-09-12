using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGenerator.Helpers
{
    static class SplitHelper
    {
        public static void SplitNumberOfOperands(int leftValue, int rightValue, int totalNumberOfOperands, out int operandsToLeft, out int operandsToRight)
        {
            operandsToLeft = 1;
            operandsToRight = totalNumberOfOperands-1;
            if (totalNumberOfOperands > 1)
            {
                // Probabilistic split
                if ((leftValue < Configuration.MaxOperandValue && rightValue < Configuration.MaxOperandValue) ||
                   (leftValue > Configuration.MaxOperandValue && rightValue > Configuration.MaxOperandValue))
                {
                    double splitPercentage = GetSplitPercentage(ExpressionGenerator.Random.Next(100));
                    int splitPart = (int)(splitPercentage * totalNumberOfOperands);
                    if (splitPart == 0)
                        splitPart = 1;

                    if (leftValue < rightValue)
                    {
                        operandsToRight = splitPart;
                        operandsToLeft = totalNumberOfOperands - splitPart;
                    }
                    else
                    {
                        operandsToLeft = splitPart;
                        operandsToRight = totalNumberOfOperands - splitPart;
                    }

                    // More operands (on average) should be assigned to the bigger side
                    double probabilityThreshold = 100 / ((double)leftValue/rightValue + 1); //[[10]]
                    if (ExpressionGenerator.Random.NextDouble() * 100 < probabilityThreshold)
                        MathHelper.Swap(ref operandsToLeft, ref operandsToRight);
                }
                // One of the values is higher than the allowed maximum. Assign more operands to that side.
                else
                {
                    if (leftValue > rightValue)
                    {
                        operandsToLeft = (int)(totalNumberOfOperands * 0.8); //[[10]]
                        operandsToRight = totalNumberOfOperands - operandsToLeft;
                    }
                    else
                    {
                        operandsToRight = (int)(totalNumberOfOperands * 0.8);
                        operandsToLeft = totalNumberOfOperands - operandsToRight;
                    }
                }
            }
        }

        /// <summary>
        /// Finds left and right numbers so that
        /// left *@operand* right = goal
        /// </summary>
        /// <returns>True if split was successful, false otherwise</returns>
        public static bool SplitValue(int goal, char @operator, out int left, out int right, bool presetFormat = false)
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

                if (left < 0 || right < 0)
                    return false;
                return true;
            }
            else if(@operator == '-')
            {
                if(goal >= Configuration.MaxOperandValue)
                {
                    if ((Configuration.AllowedOperators.HasFlag(Configuration.Operators.ADD) ||
                        Configuration.AllowedOperators.HasFlag(Configuration.Operators.MUL)) && !presetFormat)
                        return false;
                    left = goal + naturalModification;
                    right = naturalModification;
                    if (left < 0 || right < 0)
                        return false;
                    return true;
                }

                left = ExpressionGenerator.Random.Next(goal+naturalModification, Configuration.MaxOperandValue + 1);
                right = left - goal;
                if (left > Configuration.MaxOperandValue || right > Configuration.MaxOperandValue)
                    return false;
                if (left < 0 || right < 0)
                    return false;
                return true;
            }
            else if(@operator == '*')
            {
                if (goal > Configuration.MaxOperandValue && MathHelper.IsPrime(goal))
                    return false;
                int threshold;
                if(goal >= Configuration.MaxOperandValue * Configuration.MaxOperandValue)
                    threshold = (int)Math.Sqrt(goal);
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
                if (left < 0 || right < 0)
                    return false;
                return true;
            }
            else if(@operator == '/')
            {
                if(goal >= Configuration.MaxOperandValue)
                {
                    if ((Configuration.AllowedOperators.HasFlag(Configuration.Operators.ADD) ||
                        Configuration.AllowedOperators.HasFlag(Configuration.Operators.MUL)) && !presetFormat)
                        return false;
                    left = goal;
                    right = 1;
                    if (left < 0 || right < 0)
                        return false;
                    return true;
                }

                int maxMultiplier = Configuration.MaxOperandValue / goal;
                right = ExpressionGenerator.Random.Next(1, maxMultiplier + 1);
                left = goal * right;
                if (left > Configuration.MaxOperandValue || right > Configuration.MaxOperandValue)
                    return false;
                if (left < 0 || right < 0)
                    return false;
                return true;
            }
            return false;
        }

        private static double GetSplitPercentage(int number)
        {
            if (number < 0 || number >= 100)
                throw new ArgumentOutOfRangeException("number");
            if (number < 10)
                return 0.1;
            if (number < 25)
                return 0.2;
            if (number < 40)
                return 0.3;
            if (number < 70)
                return 0.4;
            return 0.5;
        }
    }
}
