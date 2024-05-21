global using IMessageWrapped.Helpers;
using System.Diagnostics.CodeAnalysis;


namespace IMessageWrapped.Helpers;

public record ResultTuple<TV, TE>
{
    private ResultTuple(TV? v, TE? e, bool success)
    {
        Value = v;
        Error = e;
        Success = success;
    }

    public TV? Value { get; init; }
    public TE? Error { get; init; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }

    public static implicit operator ResultTuple<TV, TE>((TV, NullParam?) t)
    {
        return new ResultTuple<TV, TE>(t.Item1, default, true);
    }

    public static implicit operator ResultTuple<TV, TE>((NullParam?, TE) t)
    {
        return new ResultTuple<TV, TE>(default, t.Item2, false);
    }

    public void Deconstruct(out TV? v, out TE? e)
    {
        v = Value;
        e = Error;
    }

    public struct NullParam;
}