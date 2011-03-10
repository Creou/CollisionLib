using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionLib
{
    public class DistinctPairDetector : IEqualityComparer<CollilisionPair>
    {
        public bool Equals(CollilisionPair x, CollilisionPair y)
        {
            if ((x.Object1 == y.Object1 && x.Object2 == y.Object2) ||
               (x.Object1 == y.Object2 && x.Object2 == y.Object1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(CollilisionPair obj)
        {
            return 1;
            int hc = obj.GetHashCode();
            return hc;
        }
    }
}
