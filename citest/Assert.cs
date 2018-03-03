using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class Assert {

    public static void IsTrue(bool b)
    {
        if (!b)
            throw new AssertException("");
    }

    public static void AreEqual(object a, object b)
    {
        if (a == b)
            throw new AssertException("");
    }

    public static void Contains(string find, string inString)
    {
        if (!inString.Contains(find))
            throw new AssertException("");
    }
}
