using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandingOvation
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader myStream = File.OpenText(args[0]);
            int numCases = Int32.Parse(myStream.ReadLine());
            for (int i = 1; i <= numCases; i++)
            {
                System.Console.Write("Case #{0}: ", i);
                RunCase(myStream.ReadLine());
            }

        }

        static void RunCase(string input)
        {
            string[] split = input.Split(' ');
            int shynessMax = Int32.Parse(split[0]);
            int audiences = 0;
            int numRequired = 0;
            int numStanding = 0;
            

            for (int i = 0; i <= shynessMax; i++)
            {
                audiences = Int32.Parse(split[1][i].ToString());
                if (numStanding >= i)
                {
                    numStanding += audiences;
                }
                else if (numStanding == (i-1))
                {
                    numRequired++;
                    numStanding++;
                    numStanding += audiences;
                }
                else { throw new Exception();}
                
            }
            Console.WriteLine(numRequired);
        }
    }
}
