using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023_12
{
    internal class Old
    {
        List<string> lines;
        void part1()
        {
            ulong total = 0;
            foreach (string line in lines)
            {
                int curr = 0;

                string init = line.Split(' ')[0] + ".";
                var pp = line.Split(' ')[1].Split(',');

                List<int> perms = new();
                foreach (var p in pp)
                    perms.Add(int.Parse(p));

                total += tryAll(init, ref perms);
            }
            Console.WriteLine($"\npart1 =\t\t{total}");
        }
        ulong tryAll(string a, ref List<int> perms)
        {
            List<int> permo = new();
            int cnting = 0;
            char prev = '_';
            foreach (char c in a)
            {
                if (c == '?')
                {
                    break;
                }
                if (c == '#')
                {
                    cnting++;
                }
                else if (cnting != 0)
                {
                    permo.Add(cnting);
                    if (permo.Count > perms.Count)
                        return 0;
                    if (permo[permo.Count - 1] != perms[permo.Count - 1])
                        return 0;
                    cnting = 0;
                }
                else
                {
                    cnting = 0;
                }
            }
            //Console.WriteLine(a);
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == '?')
                {
                    ulong count = 0;
                    count += tryAll(a.Remove(i, 1).Insert(i, "."), ref perms);
                    count += tryAll(a.Remove(i, 1).Insert(i, "#"), ref perms);
                    return count;
                }
            }
            /*
            if (cnting != 0)
                permo.Add(cnting);
            if (permo[permo.Count - 1] != perms[permo.Count - 1])
                return 0;*/
            if (permo.Count != perms.Count)
                return 0;
            return 1;
        }
    }
}
