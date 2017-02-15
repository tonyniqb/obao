using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace obao.common
{
    public class Parameter
    {
        public Parameter(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name
        { get; set; }

        public string Value
        { get; set; }

        public override string ToString()
        {
            return string.Format("{0}={1}", this.Name, this.Value);
        }
    }
}
