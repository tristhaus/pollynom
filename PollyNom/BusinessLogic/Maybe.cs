using System;

namespace PollyNom.BusinessLogic
{

    public interface Maybe<T>
    {
        bool HasValue();
        T Value();
    }

    public class Some<T> : Maybe<T>
    {
        private readonly T t;

        public Some(T t)
        {
            this.t = t;
        }

        public bool HasValue()
        {
            return true;
        }

        public T Value()
        {
            return t;
        }
    }

    public class None<T> : Maybe<T>
    {
        public bool HasValue()
        {
            return false;
        }

        public T Value()
        {
            throw new NotImplementedException();
        }
    }
}
