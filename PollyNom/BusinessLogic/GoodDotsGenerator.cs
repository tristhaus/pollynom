using System.Collections.Generic;

namespace PollyNom.BusinessLogic
{
    public class GoodDotsGenerator
    {
        public List<IDot> Generate()
        {
            List<IDot> list = new List<IDot>();
            list.Add(new Dots.GoodDot(0.0, 0.0));
            //list.Add(new Dots.GoodDot(1.0, 1.25));
            //list.Add(new Dots.GoodDot(-3.0, -3.0));
            //list.Add(new Dots.GoodDot(-3.0, 3.0));
            //list.Add(new Dots.GoodDot(3.0, -3.0));
            //list.Add(new Dots.GoodDot(1.0, 0.0));
            //list.Add(new Dots.GoodDot(0.0, -1.0));
            return list;
        }
    }
}
