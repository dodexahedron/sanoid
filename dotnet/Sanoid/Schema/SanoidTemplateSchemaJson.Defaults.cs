//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System.Collections.Immutable;
using System.Text.Json;
using Corvus.Json;
using Corvus.Json.Internal;

namespace Sanoid.Schema;
/// <summary>
/// A type generated from a JsonSchema specification.
/// </summary>
public readonly partial struct SanoidTemplateSchemaJson
{
    private static readonly ImmutableDictionary<JsonPropertyName, JsonAny> __CorvusDefaults = BuildDefaults();
    /// <inheritdoc/>
    public bool TryGetDefault(in JsonPropertyName name, out JsonAny value)
    {
        return __CorvusDefaults.TryGetValue(name, out value);
    }

    /// <inheritdoc/>
    public bool TryGetDefault(string name, out JsonAny value)
    {
        return __CorvusDefaults.TryGetValue(name, out value);
    }

    /// <inheritdoc/>
    public bool TryGetDefault(ReadOnlySpan<char> name, out JsonAny value)
    {
        return __CorvusDefaults.TryGetValue(name, out value);
    }

    /// <inheritdoc/>
    public bool TryGetDefault(ReadOnlySpan<byte> utf8Name, out JsonAny value)
    {
        return __CorvusDefaults.TryGetValue(utf8Name, out value);
    }

    /// <inheritdoc/>
    public bool HasDefault(in JsonPropertyName name)
    {
        return __CorvusDefaults.TryGetValue(name, out _);
    }

    /// <inheritdoc/>
    public bool HasDefault(string name)
    {
        return __CorvusDefaults.TryGetValue(name, out _);
    }

    /// <inheritdoc/>
    public bool HasDefault(ReadOnlySpan<char> name)
    {
        return __CorvusDefaults.TryGetValue(name, out _);
    }

    /// <inheritdoc/>
    public bool HasDefault(ReadOnlySpan<byte> utf8Name)
    {
        return __CorvusDefaults.TryGetValue(utf8Name, out _);
    }

    private static ImmutableDictionary<JsonPropertyName, JsonAny> BuildDefaults()
    {
        ImmutableDictionary<JsonPropertyName, JsonAny>.Builder builder = ImmutableDictionary.CreateBuilder<JsonPropertyName, JsonAny>();
        builder.Add(AutoSnapshotJsonPropertyName, JsonAny.Parse("true"));
        builder.Add(AutoPruneJsonPropertyName, JsonAny.Parse("false"));
        builder.Add(RecursiveJsonPropertyName, JsonAny.Parse("false"));
        builder.Add(UseTemplateJsonPropertyName, JsonAny.Parse("\"default\""));
        builder.Add(SkipChildrenJsonPropertyName, JsonAny.Parse("false"));
        builder.Add(FrequentPeriodJsonPropertyName, JsonAny.Parse("15"));
        return builder.ToImmutable();
    }
}