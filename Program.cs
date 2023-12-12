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


part2();
part1();
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

void part2()
{
    ulong total = 0;
    int ll = 0;
    long totalMs = 0;
    foreach (string line in lines)
    {

        string init = line.Split(' ')[0];
        var pp = line.Split(' ')[1].Split(',');

        init = init + "?" + init + "?" + init + "?" + init + "?" + init + ".";

        List<int> perms = new();
        for(int i=0;i<5;i++)
            foreach (var p in pp)
                perms.Add(int.Parse(p));


        alreadyKnown.Clear();//cache invalidate for next part dummy!
        Stopwatch sw = Stopwatch.StartNew();
        ulong curr = tryAllPart2(init,ref perms,0);
        totalMs += sw.ElapsedMilliseconds;
        Console.WriteLine($"line {++ll} =\t\t{curr}\t\tin {sw.ElapsedMilliseconds}ms");
        total += curr;
    }
    Console.WriteLine($"\npart2 =\t\t{total}\t\tin {totalMs}ms");
}



ulong tryAllPart2(string a, ref List<int> perms, int alreadySolved)
{
    Tuple<int, string> remains = new(alreadySolved, a);
    if (alreadyKnown.ContainsKey(remains))
        return alreadyKnown[remains];

    //when we solved just check rest is dots or ? we can dot
    if (alreadySolved == perms.Count)
    {
        //len 0 covered after foreach that wont return
        /*if (a.Length == 0)
        {
            alreadyKnown[remains] = 1;
            return 1;
        }*/
        foreach (char c in a)
        {
            if (c == '#')
            {
                alreadyKnown[remains] = 0;
                return alreadyKnown[remains];
            }
        }
        alreadyKnown[remains] = 1;
        return alreadyKnown[remains];
    }
    //solving perms[alreadySolved]
    int need = perms[alreadySolved];
    int dotsSkip = 0;
    for (; dotsSkip < a.Length; dotsSkip++)
        if (a[dotsSkip] != '.') break;
    for (int i = dotsSkip; i < a.Length; i++)
    {
        if (a[i] == '#')
        {
            need--;
        }
        else if (a[i] == '.')
        {
            if (need == 0)
            {
                string rest = a.Substring(i);//first is solved a single way check how rest goes and return it
                alreadyKnown[remains] = tryAllPart2(rest, ref perms, alreadySolved + 1);
                return alreadyKnown[remains];
            }
            else
            {
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
    /* -- No exception thrown, enable again if you change things and results are bad
    if (need == 0)
    {
        throw new Exception("shouldn't happen");
    }
    foreach (char c in a)
    {
        if (c != '.')
            throw new Exception("this shouldn't happen either");
    }*/
    return 0;
}
ulong tryAll(string a,ref List<int> perms)
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
    for (int i=0;i<a.Length;i++)
    {
        if (a[i] == '?')
        {
            ulong count = 0;
            count += tryAll(a.Remove(i, 1).Insert(i, "."),ref perms);
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