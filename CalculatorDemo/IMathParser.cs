using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorDemo
{
    public interface IMathParser
    {
        public List<string> Normalize(string expression);
    }
}
