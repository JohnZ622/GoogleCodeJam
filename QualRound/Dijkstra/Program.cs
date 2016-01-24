using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra
{
    internal class Program
    {
        private enum Quarternion
        {i, j, k, one, neg_i, neg_j, neg_k, neg_one};

        static void Main(string[] args)
        {
            Quarternion x = Quarternion.i;
            Quarternion y = Quarternion.neg_one;
            Quarternion result = BruteMultiply2(x, y);
            // if (result == Quarternion.neg_i) {  System.Console.WriteLine("Good");}
            
            
            StreamReader myStream = File.OpenText(args[0]);
            int numCases = Int32.Parse(myStream.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                System.Console.Write("Case #{0}: ", i);
                string line1 = myStream.ReadLine();
                string line2 = myStream.ReadLine();
                RunCase(line1, line2);

            }
        }

        static void RunCase(string line1, string line2)
        {
            string[] split = line1.Split(' ');
            int L = Int32.Parse(split[0]);
            long X = Int64.Parse(split[1]);
            if (L*X < 3)
            {
                System.Console.WriteLine("NO");
                return;
            }

            if (X%4 == 0) // if x mod 4 = 0, then the whole thing multiplies out to +1
            {
                System.Console.WriteLine("NO");
                return;
            }
            
            Quarternion[] segment = new Quarternion[L];
            for (int i = 0; i < segment.Length; i++)
            {
                segment[i] = Parse(line2[i]);
            }

            Quarternion segmentProduct = BruteMultiply(segment);
            if (X%4 == 1 && segmentProduct != Quarternion.neg_one)
            {
                System.Console.WriteLine("NO");
                return;
            }
            else if (X%4 == 2 && BruteMultiply2(segmentProduct, segmentProduct) != Quarternion.neg_one)
            {
                System.Console.WriteLine("NO");
                return;

            }
            else if (X%4 == 3 && BruteMultiply(segmentProduct, segmentProduct, segmentProduct) != Quarternion.neg_one)
            {
                System.Console.WriteLine("NO");
                return;
            }


            Quarternion[] criticalSegment = new Quarternion[1];
            if (X <= 15)
            {
                criticalSegment = copySegment(segment, (int)X);
            }
            else
            {
                if (X%4 == 1)
                {
                    criticalSegment = copySegment(segment, 17);
                }
                else if (X%4 == 2)
                {
                    criticalSegment = copySegment(segment, 18);
                }
                else if (X % 4 == 3)
                {
                    criticalSegment = copySegment(segment, 19);
                }
            }

            if (BruteForceCheck(criticalSegment)) { System.Console.WriteLine("YES"); }
            else { System.Console.WriteLine("NO"); }
            return;
        }

        static Quarternion[] copySegment(Quarternion[] input, int copies)
        {
            Quarternion[] result = new Quarternion[input.Length*copies];
            for (int i = 0; i < copies; i++)
            {
                input.CopyTo(result, input.Length*i);
            }
            return result;
        }

        static bool BruteForceCheck(Quarternion[] segment)
        {
            Quarternion[] segmentLeft = new Quarternion[segment.Length];

            // Compute left products
            segmentLeft[0] = segment[0];
            for (int i = 1; i < segment.Length; i++)
            {
                segmentLeft[i] = BruteMultiply2(segmentLeft[i - 1], segment[i]);
            }
            if (segmentLeft[segment.Length - 1] != Quarternion.neg_one)
            {
                return false;
            }

            // Compute right products
            Quarternion[] segmentRight = new Quarternion[segment.Length];
            segmentRight[segmentRight.Length - 1] = segment[segment.Length - 1];
            for (int i = segmentRight.Length - 2; i >= 0; i--)
            {
                segmentRight[i] = BruteMultiply2(segment[i], segmentRight[i + 1]);
            }
            
            // Brute force for two concatenated segments
            for (int i = 1; i < segment.Length-1; i++)
            {
                if (segmentLeft[i-1] == Quarternion.i) // if the left substring is i
                {
                    for (int j = i + 1; j < segment.Length; j++)
                    {
                        if (segmentRight[j] == Quarternion.k &&
                            BruteMultiply(segment.Skip(i).Take(j - i).ToArray()) == Quarternion.j)
                        {
                             // System.Console.WriteLine("YES");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static Quarternion Parse(char c)
        {
            switch (c)
            {
                case 'i':
                    return Quarternion.i;
                case 'j': return Quarternion.j;
                case 'k': return Quarternion.k;
                default: throw new Exception();
            }
        }

        static Quarternion BruteMultiply(params Quarternion[] input)
        {
            Quarternion result = Quarternion.one;
            for (int i = 0; i < input.Length; i++)
            {
                result = BruteMultiply2(result, input[i]);
            }
            return result;
        }
        

        static bool IsNegative(Quarternion x)
        {
            if (x == Quarternion.neg_i || x == Quarternion.neg_j || x == Quarternion.neg_k || x == Quarternion.neg_one)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static Quarternion Abs(Quarternion x)
        {
            switch (x)
            {
                case Quarternion.neg_i: return Quarternion.i;
                case Quarternion.neg_j: return Quarternion.j;
                case Quarternion.neg_k: return Quarternion.k;
                case Quarternion.neg_one: return Quarternion.one;
                default:
                    return x;
            }
        }

        private static Quarternion Negate(Quarternion x)
        {
            if (IsNegative(x))
            {
                return Abs(x);
            }
            else
            {
                // positive
                switch (x)
                {
                    case Quarternion.i: return Quarternion.neg_i;
                    case Quarternion.j: return Quarternion.neg_j;
                    case Quarternion.k: return Quarternion.neg_k;
                    case Quarternion.one: return Quarternion.neg_one;
                    default:
                        throw new Exception();
                }
            }
        }

        static Quarternion BruteMultiply2(Quarternion x, Quarternion y)
        {
            bool x_neg = IsNegative(x);
            bool y_neg = IsNegative(y);
            if (x_neg)
            {
                x = Abs(x);
            }
            if (y_neg)
            {
                y = Abs(y);
            }

            Quarternion z = Quarternion.one;

            switch (x)
            {
                case Quarternion.one:
                    switch (y)
                    {
                        case Quarternion.one: z = Quarternion.one;
                            break;
                        case Quarternion.i: z = Quarternion.i;
                            break;
                        case Quarternion.j: z = Quarternion.j;
                            break;
                        case Quarternion.k: z = Quarternion.k;
                            break;
                    }
                    break;                    
                case Quarternion.i:
                    switch (y)
                    {
                        case Quarternion.one: z = Quarternion.i;
                            break;
                        case Quarternion.i: z = Quarternion.neg_one;
                            break;
                        case Quarternion.j: z = Quarternion.k;
                            break;
                        case Quarternion.k: z = Quarternion.neg_j;
                            break;
                    }
                    break;
                case Quarternion.j:
                    switch (y)
                    {
                        case Quarternion.one: z = Quarternion.j;
                            break;
                        case Quarternion.i: z = Quarternion.neg_k;
                            break;
                        case Quarternion.j: z = Quarternion.neg_one;
                            break;
                        case Quarternion.k:
                            z = Quarternion.i;
                            break;
                    }
                    break;
                case Quarternion.k:
                    switch (y)
                    {
                        case Quarternion.one: z = Quarternion.k;
                            break;
                        case Quarternion.i: z = Quarternion.j;
                            break;
                        case Quarternion.j: z = Quarternion.neg_i;
                            break;
                        case Quarternion.k: z = Quarternion.neg_one;
                            break;
                    }
                    break;
            }
            if (x_neg && !y_neg || !x_neg && y_neg)
            {
                return Negate(z);
            }
            else
            {
                return z;
            }
        }
    }
}
