//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
using System.Text.Json;
using Corvus.Json;

namespace Sanoid.Schema;
public readonly partial struct SanoidTemplateSchemaJson
{
    public readonly partial struct SnapshotTimingEntity1
    {
        /// <summary>
        /// A type generated from a JsonSchema specification.
        /// </summary>
        public readonly partial struct UseLocalTimeEntity
        {
            /// <inheritdoc/>
            public ValidationContext Validate(in ValidationContext validationContext, ValidationLevel level = ValidationLevel.Flag)
            {
                ValidationContext result = validationContext;
                if (level > ValidationLevel.Flag)
                {
                    result = result.UsingResults();
                }

                if (level > ValidationLevel.Basic)
                {
                    result = result.UsingStack();
                    result = result.PushSchemaLocation("https://endjin.com/Sanoid.template.schema.json#/definitions/SnapshotTiming/properties/UseLocalTime");
                }

                JsonValueKind valueKind = this.ValueKind;
                result = this.ValidateType(valueKind, result, level);
                if (level == ValidationLevel.Flag && !result.IsValid)
                {
                    return result;
                }

                if (level != ValidationLevel.Flag)
                {
                    result = result.PopLocation();
                }

                return result;
            }
        }
    }
}