using System.Collections.Generic;

namespace PollyNom.BusinessLogic
{
    public class ListPointLogical
    {
        List<PointLogical> list;

        public ListPointLogical()
        {
            this.list = new List<PointLogical>();
        }

        public IReadOnlyList<PointLogical> Points
        {
            get
            {
                return list.AsReadOnly();
            }
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public void Add(PointLogical point)
        {
            if (list.Count == 0 || list[list.Count-1].X < point.X )
            {
                list.Add(point);
                return;
            }
            else
            {
                list.Add(point);
                list.Sort();
            }
        }
    }
}
