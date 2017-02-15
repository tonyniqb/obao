using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obao.common
{
    public class ParameterComparer : IComparer<Parameter>
    {
        public int Compare(Parameter x, Parameter y)
        {
            if (x.Name == y.Name)
            {
                return string.Compare(x.Value, y.Value);
            }
            else
            {
                return string.Compare(x.Name, y.Name);
            }
        }
    }
}
