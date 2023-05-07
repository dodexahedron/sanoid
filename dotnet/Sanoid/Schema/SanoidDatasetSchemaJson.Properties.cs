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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Corvus.Json;
using Corvus.Json.Internal;

namespace Sanoid.Schema;
/// <summary>
/// A type generated from a JsonSchema specification.
/// </summary>
public readonly partial struct SanoidDatasetSchemaJson
{
    /// <summary>
    /// JSON property name for <see cref = "Template"/>.
    /// </summary>
    public static readonly ReadOnlyMemory<byte> TemplateUtf8JsonPropertyName = new byte[]{84, 101, 109, 112, 108, 97, 116, 101};
    /// <summary>
    /// JSON property name for <see cref = "Template"/>.
    /// </summary>
    public const string TemplateJsonPropertyName = "Template";
    /// <summary>
    /// JSON property name for <see cref = "Comments"/>.
    /// </summary>
    public static readonly ReadOnlyMemory<byte> CommentsUtf8JsonPropertyName = new byte[]{36, 99, 111, 109, 109, 101, 110, 116, 115};
    /// <summary>
    /// JSON property name for <see cref = "Comments"/>.
    /// </summary>
    public const string CommentsJsonPropertyName = "$comments";
    /// <summary>
    /// JSON property name for <see cref = "Overrides"/>.
    /// </summary>
    public static readonly ReadOnlyMemory<byte> OverridesUtf8JsonPropertyName = new byte[]{79, 118, 101, 114, 114, 105, 100, 101, 115};
    /// <summary>
    /// JSON property name for <see cref = "Overrides"/>.
    /// </summary>
    public const string OverridesJsonPropertyName = "Overrides";
    /// <summary>
    /// Gets Template.
    /// </summary>
    public Corvus.Json.JsonUriReference Template
    {
        get
        {
            if ((this.backing & Backing.JsonElement) != 0)
            {
                if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                {
                    return default;
                }

                if (this.jsonElementBacking.TryGetProperty(TemplateUtf8JsonPropertyName.Span, out JsonElement result))
                {
                    return new Corvus.Json.JsonUriReference(result);
                }
            }

            if ((this.backing & Backing.Object) != 0)
            {
                if (this.objectBacking.TryGetValue(TemplateJsonPropertyName, out JsonAny result))
                {
                    return result.As<Corvus.Json.JsonUriReference>();
                }
            }

            return default;
        }
    }

    /// <summary>
    /// Gets Comments.
    /// </summary>
    public Corvus.Json.JsonString Comments
    {
        get
        {
            if ((this.backing & Backing.JsonElement) != 0)
            {
                if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                {
                    return default;
                }

                if (this.jsonElementBacking.TryGetProperty(CommentsUtf8JsonPropertyName.Span, out JsonElement result))
                {
                    return new Corvus.Json.JsonString(result);
                }
            }

            if ((this.backing & Backing.Object) != 0)
            {
                if (this.objectBacking.TryGetValue(CommentsJsonPropertyName, out JsonAny result))
                {
                    return result.As<Corvus.Json.JsonString>();
                }
            }

            return default;
        }
    }

    /// <summary>
    /// Gets Overrides.
    /// </summary>
    public Sanoid.Schema.SanoidTemplateSchemaJson Overrides
    {
        get
        {
            if ((this.backing & Backing.JsonElement) != 0)
            {
                if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                {
                    return default;
                }

                if (this.jsonElementBacking.TryGetProperty(OverridesUtf8JsonPropertyName.Span, out JsonElement result))
                {
                    return new Sanoid.Schema.SanoidTemplateSchemaJson(result);
                }
            }

            if ((this.backing & Backing.Object) != 0)
            {
                if (this.objectBacking.TryGetValue(OverridesJsonPropertyName, out JsonAny result))
                {
                    return result.As<Sanoid.Schema.SanoidTemplateSchemaJson>();
                }
            }

            return default;
        }
    }

    /// <summary>
    /// Creates an instance of a <see cref = "SanoidDatasetSchemaJson"/>.
    /// </summary>
    public static SanoidDatasetSchemaJson Create(Corvus.Json.JsonUriReference template, Corvus.Json.JsonString? comments = null, Sanoid.Schema.SanoidTemplateSchemaJson? overrides = null)
    {
        var builder = ImmutableDictionary.CreateBuilder<JsonPropertyName, JsonAny>();
        builder.Add(TemplateJsonPropertyName, template.AsAny);
        if (comments is Corvus.Json.JsonString comments__)
        {
            builder.Add(CommentsJsonPropertyName, comments__.AsAny);
        }

        if (overrides is Sanoid.Schema.SanoidTemplateSchemaJson overrides__)
        {
            builder.Add(OverridesJsonPropertyName, overrides__.AsAny);
        }

        return builder.ToImmutable();
    }

    /// <summary>
    /// Sets Template.
    /// </summary>
    /// <param name = "value">The value to set.</param>
    /// <returns>The entity with the updated property.</returns>
    public SanoidDatasetSchemaJson WithTemplate(in Corvus.Json.JsonUriReference value)
    {
        return this.SetProperty(TemplateJsonPropertyName, value);
    }

    /// <summary>
    /// Sets $comments.
    /// </summary>
    /// <param name = "value">The value to set.</param>
    /// <returns>The entity with the updated property.</returns>
    public SanoidDatasetSchemaJson WithComments(in Corvus.Json.JsonString value)
    {
        return this.SetProperty(CommentsJsonPropertyName, value);
    }

    /// <summary>
    /// Sets Overrides.
    /// </summary>
    /// <param name = "value">The value to set.</param>
    /// <returns>The entity with the updated property.</returns>
    public SanoidDatasetSchemaJson WithOverrides(in Sanoid.Schema.SanoidTemplateSchemaJson value)
    {
        return this.SetProperty(OverridesJsonPropertyName, value);
    }

    private static ValidationContext __CorvusValidateTemplate(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
    {
        return property.ValueAs<Corvus.Json.JsonUriReference>().Validate(validationContext, level);
    }

    private static ValidationContext __CorvusValidateComments(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
    {
        return property.ValueAs<Corvus.Json.JsonString>().Validate(validationContext, level);
    }

    private static ValidationContext __CorvusValidateOverrides(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
    {
        return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson>().Validate(validationContext, level);
    }

    /// <summary>
    /// Tries to get the validator for the given property.
    /// </summary>
    /// <param name = "property">The property for which to get the validator.</param>
    /// <param name = "hasJsonElementBacking"><c>True</c> if the object containing the property has a JsonElement backing.</param>
    /// <param name = "propertyValidator">The validator for the property, if provided by this schema.</param>
    /// <returns><c>True</c> if the validator was found.</returns>
    private bool __TryGetCorvusLocalPropertiesValidator(in JsonObjectProperty property, bool hasJsonElementBacking, [NotNullWhen(true)] out ObjectPropertyValidator? propertyValidator)
    {
        if (hasJsonElementBacking)
        {
            if (property.NameEquals(TemplateUtf8JsonPropertyName.Span))
            {
                propertyValidator = __CorvusValidateTemplate;
                return true;
            }
            else if (property.NameEquals(CommentsUtf8JsonPropertyName.Span))
            {
                propertyValidator = __CorvusValidateComments;
                return true;
            }
            else if (property.NameEquals(OverridesUtf8JsonPropertyName.Span))
            {
                propertyValidator = __CorvusValidateOverrides;
                return true;
            }
        }
        else
        {
            if (property.NameEquals(TemplateJsonPropertyName))
            {
                propertyValidator = __CorvusValidateTemplate;
                return true;
            }
            else if (property.NameEquals(CommentsJsonPropertyName))
            {
                propertyValidator = __CorvusValidateComments;
                return true;
            }
            else if (property.NameEquals(OverridesJsonPropertyName))
            {
                propertyValidator = __CorvusValidateOverrides;
                return true;
            }
        }

        propertyValidator = null;
        return false;
    }
}