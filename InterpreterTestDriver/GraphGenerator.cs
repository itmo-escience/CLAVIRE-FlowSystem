using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterpreterTestDriver
{
    class GraphGenerator
    {
        public static bool[][] GenerateVerticalLineGraph(int n)
        {
            bool[][] ret;

            ret = new bool[n][];

            for (int i = 0; i < n; i++)
            {
                ret[i] = new bool[n];

                for (int j = 0; j < n; j++)
                {
                    ret[i][j] = false;
                }

                // нижняя диагональ
                if (i > 0)
                    ret[i][i - 1] = true;
            }
            return ret;
        }

        public static bool[][] GenerateHorizontalLineGraph(int N)
        {
            bool[][] ret;

            ret = new bool[N][];

            for (int i = 0; i < N; i++)
            {
                ret[i] = new bool[N];

                for (int j = 0; j < N; j++)
                {
                    ret[i][j] = false;
                }

                // нижняя диагональ
                if (i > 0)
                    ret[i][0] = true;
            }
            return ret;
        }

    }
}
