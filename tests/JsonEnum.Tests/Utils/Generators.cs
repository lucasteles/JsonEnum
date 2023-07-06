using FsCheck;

namespace JsonEnum.Tests.Utils;

public class Generators
{
    protected Generators() { }

    public static Arbitrary<Task<T>> TaskGenerator<T>()
    {
        var generator =
            from v in Arb.From<T>().Generator
            select Task.FromResult(v);

        return Arb.From(generator);
    }
}
