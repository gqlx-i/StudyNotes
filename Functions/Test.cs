using StudyNotes.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Functions
{
    public class Test
    {
        [Method( Params=new object[] { new int[] { 1, 2, 3 } })]
        public void fun1(List<int> list)
        {
            foreach (var item in list)
            {
                Debug.WriteLine(item);
            }
        }

        //返回值测试
        [Method(111)]
        public Point fun2(int a)
        {
            return new Point(a, a*a);
        }

        //静态类测试
        [Method()]
        public static void fun3()
        {
            Debug.WriteLine("fun3");
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void test()
        {

        }
    }
}
