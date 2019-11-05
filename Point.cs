using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApproximationGUI
{
    public class Point
    {
        public String Name { get;}
        public double Value { get; set; }
        public double Arg { get; set; }
        public static int counter = 1;

        public Point() : this(0,0) { }
        public Point(double arg,double value)
        {

            Arg = arg;
            Value = value;
            Name = "X"+counter.ToString();
            Interlocked.Increment(ref counter);
        }
             
        ~Point()
        {
            Interlocked.Decrement(ref counter);
        }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(Name))  return base.ToString();
            return $"{Name}:{Value}";
        }
    }
}
