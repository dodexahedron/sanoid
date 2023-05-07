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
using System.Runtime.CompilerServices;
using System.Text.Json;
using Corvus.Json;
using Corvus.Json.Internal;

namespace Sanoid.Schema;
public readonly partial struct SanoidMonitoringSchemaJson
{
    /// <summary>
    /// A type generated from a JsonSchema specification.
    /// </summary>
    public readonly partial struct MonitorTypeEntity
    {
        /// <summary>
        /// Permitted values.
        /// </summary>
        public static class EnumValues
        {
            /// <summary>
            /// Nagios.
            /// </summary>
            /// <remarks>
            /// {Description}.
            /// </remarks>
            public static readonly MonitorTypeEntity Nagios = MonitorTypeEntity.Parse("\"Nagios\"");
            /// <summary>
            /// [{Title} || Item 0] (with predictable naming).
            /// </summary>
            /// <remarks>
            /// {Description}.
            /// </remarks>
            internal static readonly MonitorTypeEntity Item0 = MonitorTypeEntity.Parse("\"Nagios\"");
        }
    }
}