using System.Diagnostics;
using System.Security;

List<string> lines = new();
using (StreamReader reader = new(args[0]))
{
    while (!reader.EndOfStream)
    {
        lines.Add(reader.ReadLine());
    }
}

Dictionary<Tuple<int, string>, ulong> alreadyKnown = new();


solve(false);
solve(true);
void solve(bool part2)
{
    Stopwatch totalSw = Stopwatch.StartNew();
    ulong total = 0;
    int ll = 0;
    foreach (string line in lines)
    {

        string init = line.Split(' ')[0];
        var pp = line.Split(' ')[1].Split(',');

        if(part2)
            init = init + "?" + init + "?" + init + "?" + init + "?" + init;
        init += ".";

        List<int> perms = new();
        for(int i=0;i<(part2?5:1);i++)
            foreach (var p in pp)
                perms.Add(int.Parse(p));


        alreadyKnown.Clear();//cache invalidate for next part dummy!
        //Stopwatch sw = Stopwatch.StartNew();
        ulong curr = tryAllPart2(init,ref perms,0);
        //Console.WriteLine($"line {++ll} =\t\t{curr}\t\tin {sw.ElapsedMilliseconds}ms");
        total += curr;
    }
    Console.WriteLine($"part{(part2?2:1)} =\t{total.ToString().PadLeft(20)}\tin {totalSw.ElapsedMilliseconds}ms");
}
ulong tryAllPart2(string a, ref List<int> perms, int alreadySolved)
{
    Tuple<int, string> remains = new(alreadySolved, a);
    if (alreadyKnown.ContainsKey(remains))
        return alreadyKnown[remains];

    //when we solved just check rest is dots or ? we can dot
    if (alreadySolved == perms.Count)
    {
        foreach (char c in a)
        {
            if (c == '#')
            {
                return 0;//don't save this

                alreadyKnown[remains] = 0;
                return alreadyKnown[remains];
            }
        }
        return 1;//don't save this
        alreadyKnown[remains] = 1;
        return alreadyKnown[remains];
    }
    //solving perms[alreadySolved]
    int need = perms[alreadySolved];
    //int needs = perms.GetRange(alreadySolved, perms.Count - alreadySolved).Sum();
    if (need > a.Length)
    {
        alreadyKnown[remains] = 0;
        return alreadyKnown[remains];
    }
    int dotsSkip = 0;
    for (; dotsSkip < a.Length; dotsSkip++)
        if (a[dotsSkip] != '.') break;
    for (int i = dotsSkip; i < a.Length; i++)
    {
        if (a[i] == '#')
        {
            if (--need < 0)
            {
                return 0;//don't save this
                alreadyKnown[remains] = 0;
                return alreadyKnown[remains];
            }
        }
        else if (a[i] == '.')
        {
            if (need == 0)
            {
                return tryAllPart2(a.Substring(i), ref perms, alreadySolved + 1);//don't save this
                string rest = a.Substring(i);//first is solved a single way check how rest goes and return it
                alreadyKnown[remains] = tryAllPart2(rest, ref perms, alreadySolved + 1);
                return alreadyKnown[remains];
            }
            else
            {
                return 0;//don't save this
                alreadyKnown[remains] = 0;//first is broken going this way remember and return it's impossible path
                return alreadyKnown[remains];
            }
        }
        else //a[i]=='?'
        {
            ulong bothWays = 0;
            //try .
            bothWays += tryAllPart2(a.Remove(i, 1).Insert(i, "."), ref perms, alreadySolved);
            //try #
            bothWays += tryAllPart2(a.Remove(i, 1).Insert(i, "#"), ref perms, alreadySolved);
            alreadyKnown[remains] = bothWays;
            return alreadyKnown[remains];
        }
    }
    //last return 0 is for cases where we still need something but we only have dots now
    //should only happen if we still have need and rest are dots
    return 0;//don't save this
    alreadyKnown[remains] = 0;
    return alreadyKnown[remains];
}