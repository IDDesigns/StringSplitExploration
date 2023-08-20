using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.Primitives;

namespace StringSplit;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net70)]
public class ParsingStrings
{
    private readonly string _guids = $"{Guid.NewGuid()},{Guid.NewGuid()},{Guid.NewGuid()},,{Guid.NewGuid()},{Guid.NewGuid()}";
    private static readonly char[] _chars = { ',' };

    [Benchmark]
    public int StringSplit()
    {
        int count = 0;

        var parts = _guids.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (Guid.TryParse(part, out Guid guid))
            {
                count += 1;
            }
        }

        return count;
    }

    [Benchmark]
    public int Tokenizer()
    {
        int count = 0;

        StringTokenizer tokenizer = new StringTokenizer(_guids, _chars);

        foreach (var part in tokenizer)
        {
            if (Guid.TryParse(part, out Guid guid))
            {
                count += 1;
            }
        }

        return count;
    }

    [Benchmark]
    public int Tokenise()
    {
        int count = 0;

        foreach (var part in _guids.AsSpan().Tokenise())
        {
            if (Guid.TryParse(part, out Guid guid))
            {
                count += 1;
            }
        }

        return count;
    }
}