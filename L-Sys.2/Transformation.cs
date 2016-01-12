using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_Sys._2
{
    class Transformation
    {
        public char seed;
        public List<String> transes = new List<String>();

        public Transformation(char _seed,String _trans)
        {
            seed = _seed;
            transes.Add(_trans);
        }

        public void addTrans(String _Trans)
        {
            transes.Add(_Trans);
        }

        public String getTransformationString()
        {
            Random ran = new Random();
            return transes[ran.Next(0, transes.Count)];
        }

    }
}
