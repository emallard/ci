using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class StepAssert {

        public static void IsTrue(bool b)
        {
            if (!b)
                throw new StepAssertException("");
        }

        public static void AreEqual(object a, object b)
        {
            if (a == b)
                throw new StepAssertException("");
        }

        public static void Contains(string find, string inString)
        {
            if (!inString.Contains(find))
                throw new StepAssertException("");
        }

        public static void StartsWith(string str, string start)
        {
            if (!str.StartsWith(start))
                throw new StepAssertException("");
        }
    }
}

