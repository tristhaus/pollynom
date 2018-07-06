using System.Collections.Generic;

namespace PollyNom.BusinessLogic
{
    public class GoodDotsGenerator
    {
        public List<Dots.GoodDot> Generate()
        {
            List<Dots.GoodDot> list = new List<Dots.GoodDot>();
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
