using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterpreterTestDriver
{
    class GraphSerializer
    {
        public static string SerializeGraph(bool[][] graph, string requiredFileVarName)
        {
            string ret = "";
            int n = graph.Length;

            ret += String.Format("require {0};\n", requiredFileVarName);

            ret += "flow {\n";

            for (int i = 0; i < n; i++)
            {
                //step Step3 after Step2, Step1 as SEMP.zindo ( xyz_molecule = file2 );

                string depend = "";

                IList<string> strs = new List<string>();

                for (int j = 0; j < n; j++)
                {
                    if (graph[i][j])
                    {
                        strs.Add("Step"+j.ToString());
                    }
                }

                for (int j = 0; j < strs.Count; j++)
                {
                    if (j < strs.Count - 1)
                        depend += strs[j] + ", ";
                    else
                        depend += strs[j];
                }

                if(String.IsNullOrEmpty(depend))
                    ret += String.Format("step Step{0} as SEMP.zindo (xyz_molecule = {1});\n", i, requiredFileVarName);
                else
                    ret += String.Format("step Step{0} after {1} as SEMP.zindo (xyz_molecule = {2});\n", i, depend,
                                     requiredFileVarName);
            }
            ret += "}\n";
            return ret;
        }
    }
}
