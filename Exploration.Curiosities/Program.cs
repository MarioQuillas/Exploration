namespace Exploration.Curiosities
{
    using System;

    interface I<out T> { }

    class Base { }

    class D1 : Base { }

    class D2 : D1 { }

    class A : I<double> { }

    class B : I<Base> { }

    class Program
    {
        static void Main(string[] args)
        {
            I<double> a = new A();
            I<Base> b = new B();

            I<D2> c = null;
            I<Base> d = c;

            I<DateTime> f = null;

            I<int> g = null;

            //I<object> toto = g;

            int i = 12;
            object o = i;
            short s = (short)o; // invalidcastexception


        }
    }
}
