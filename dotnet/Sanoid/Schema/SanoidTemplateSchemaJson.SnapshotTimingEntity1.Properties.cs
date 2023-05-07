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
public readonly partial struct SanoidTemplateSchemaJson
{
    /// <summary>
    /// A type generated from a JsonSchema specification.
    /// </summary>
    public readonly partial struct SnapshotTimingEntity1
    {
        /// <summary>
        /// JSON property name for <see cref = "UseLocalTime"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> UseLocalTimeUtf8JsonPropertyName = new byte[]{85, 115, 101, 76, 111, 99, 97, 108, 84, 105, 109, 101};
        /// <summary>
        /// JSON property name for <see cref = "UseLocalTime"/>.
        /// </summary>
        public const string UseLocalTimeJsonPropertyName = "UseLocalTime";
        /// <summary>
        /// JSON property name for <see cref = "HourlyMinute"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> HourlyMinuteUtf8JsonPropertyName = new byte[]{72, 111, 117, 114, 108, 121, 77, 105, 110, 117, 116, 101};
        /// <summary>
        /// JSON property name for <see cref = "HourlyMinute"/>.
        /// </summary>
        public const string HourlyMinuteJsonPropertyName = "HourlyMinute";
        /// <summary>
        /// JSON property name for <see cref = "DailyTime"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> DailyTimeUtf8JsonPropertyName = new byte[]{68, 97, 105, 108, 121, 84, 105, 109, 101};
        /// <summary>
        /// JSON property name for <see cref = "DailyTime"/>.
        /// </summary>
        public const string DailyTimeJsonPropertyName = "DailyTime";
        /// <summary>
        /// JSON property name for <see cref = "WeeklyDay"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> WeeklyDayUtf8JsonPropertyName = new byte[]{87, 101, 101, 107, 108, 121, 68, 97, 121};
        /// <summary>
        /// JSON property name for <see cref = "WeeklyDay"/>.
        /// </summary>
        public const string WeeklyDayJsonPropertyName = "WeeklyDay";
        /// <summary>
        /// JSON property name for <see cref = "WeeklyTime"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> WeeklyTimeUtf8JsonPropertyName = new byte[]{87, 101, 101, 107, 108, 121, 84, 105, 109, 101};
        /// <summary>
        /// JSON property name for <see cref = "WeeklyTime"/>.
        /// </summary>
        public const string WeeklyTimeJsonPropertyName = "WeeklyTime";
        /// <summary>
        /// JSON property name for <see cref = "MonthlyDay"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> MonthlyDayUtf8JsonPropertyName = new byte[]{77, 111, 110, 116, 104, 108, 121, 68, 97, 121};
        /// <summary>
        /// JSON property name for <see cref = "MonthlyDay"/>.
        /// </summary>
        public const string MonthlyDayJsonPropertyName = "MonthlyDay";
        /// <summary>
        /// JSON property name for <see cref = "MonthlyTime"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> MonthlyTimeUtf8JsonPropertyName = new byte[]{77, 111, 110, 116, 104, 108, 121, 84, 105, 109, 101};
        /// <summary>
        /// JSON property name for <see cref = "MonthlyTime"/>.
        /// </summary>
        public const string MonthlyTimeJsonPropertyName = "MonthlyTime";
        /// <summary>
        /// JSON property name for <see cref = "YearlyMonth"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> YearlyMonthUtf8JsonPropertyName = new byte[]{89, 101, 97, 114, 108, 121, 77, 111, 110, 116, 104};
        /// <summary>
        /// JSON property name for <see cref = "YearlyMonth"/>.
        /// </summary>
        public const string YearlyMonthJsonPropertyName = "YearlyMonth";
        /// <summary>
        /// JSON property name for <see cref = "YearlyDay"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> YearlyDayUtf8JsonPropertyName = new byte[]{89, 101, 97, 114, 108, 121, 68, 97, 121};
        /// <summary>
        /// JSON property name for <see cref = "YearlyDay"/>.
        /// </summary>
        public const string YearlyDayJsonPropertyName = "YearlyDay";
        /// <summary>
        /// JSON property name for <see cref = "YearlyTime"/>.
        /// </summary>
        public static readonly ReadOnlyMemory<byte> YearlyTimeUtf8JsonPropertyName = new byte[]{89, 101, 97, 114, 108, 121, 84, 105, 109, 101};
        /// <summary>
        /// JSON property name for <see cref = "YearlyTime"/>.
        /// </summary>
        public const string YearlyTimeJsonPropertyName = "YearlyTime";
        /// <summary>
        /// Gets UseLocalTime.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.UseLocalTimeEntity UseLocalTime
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(UseLocalTimeUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.UseLocalTimeEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(UseLocalTimeJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.UseLocalTimeEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Gets HourlyMinute.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.HourlyMinuteEntity HourlyMinute
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(HourlyMinuteUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.HourlyMinuteEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(HourlyMinuteJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.HourlyMinuteEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Gets DailyTime.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.DailyTimeEntity DailyTime
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(DailyTimeUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.DailyTimeEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(DailyTimeJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.DailyTimeEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Gets WeeklyDay.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyDayEntity WeeklyDay
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(WeeklyDayUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyDayEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(WeeklyDayJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyDayEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Gets WeeklyTime.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyTimeEntity WeeklyTime
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(WeeklyTimeUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyTimeEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(WeeklyTimeJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyTimeEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Gets MonthlyDay.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyDayEntity MonthlyDay
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(MonthlyDayUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyDayEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(MonthlyDayJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyDayEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Gets MonthlyTime.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyTimeEntity MonthlyTime
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(MonthlyTimeUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyTimeEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(MonthlyTimeJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyTimeEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Gets YearlyMonth.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyMonthEntity YearlyMonth
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(YearlyMonthUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyMonthEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(YearlyMonthJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyMonthEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Gets YearlyDay.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyDayEntity YearlyDay
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(YearlyDayUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyDayEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(YearlyDayJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyDayEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Gets YearlyTime.
        /// </summary>
        public Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyTimeEntity YearlyTime
        {
            get
            {
                if ((this.backing & Backing.JsonElement) != 0)
                {
                    if (this.jsonElementBacking.ValueKind != JsonValueKind.Object)
                    {
                        return default;
                    }

                    if (this.jsonElementBacking.TryGetProperty(YearlyTimeUtf8JsonPropertyName.Span, out JsonElement result))
                    {
                        return new Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyTimeEntity(result);
                    }
                }

                if ((this.backing & Backing.Object) != 0)
                {
                    if (this.objectBacking.TryGetValue(YearlyTimeJsonPropertyName, out JsonAny result))
                    {
                        return result.As<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyTimeEntity>();
                    }
                }

                return default;
            }
        }

        /// <summary>
        /// Creates an instance of a <see cref = "SnapshotTimingEntity1"/>.
        /// </summary>
        public static SnapshotTimingEntity1 Create(Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.UseLocalTimeEntity? useLocalTime = null, Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.HourlyMinuteEntity? hourlyMinute = null, Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.DailyTimeEntity? dailyTime = null, Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyDayEntity? weeklyDay = null, Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyTimeEntity? weeklyTime = null, Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyDayEntity? monthlyDay = null, Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyTimeEntity? monthlyTime = null, Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyMonthEntity? yearlyMonth = null, Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyDayEntity? yearlyDay = null, Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyTimeEntity? yearlyTime = null)
        {
            var builder = ImmutableDictionary.CreateBuilder<JsonPropertyName, JsonAny>();
            if (useLocalTime is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.UseLocalTimeEntity useLocalTime__)
            {
                builder.Add(UseLocalTimeJsonPropertyName, useLocalTime__.AsAny);
            }

            if (hourlyMinute is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.HourlyMinuteEntity hourlyMinute__)
            {
                builder.Add(HourlyMinuteJsonPropertyName, hourlyMinute__.AsAny);
            }

            if (dailyTime is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.DailyTimeEntity dailyTime__)
            {
                builder.Add(DailyTimeJsonPropertyName, dailyTime__.AsAny);
            }

            if (weeklyDay is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyDayEntity weeklyDay__)
            {
                builder.Add(WeeklyDayJsonPropertyName, weeklyDay__.AsAny);
            }

            if (weeklyTime is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyTimeEntity weeklyTime__)
            {
                builder.Add(WeeklyTimeJsonPropertyName, weeklyTime__.AsAny);
            }

            if (monthlyDay is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyDayEntity monthlyDay__)
            {
                builder.Add(MonthlyDayJsonPropertyName, monthlyDay__.AsAny);
            }

            if (monthlyTime is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyTimeEntity monthlyTime__)
            {
                builder.Add(MonthlyTimeJsonPropertyName, monthlyTime__.AsAny);
            }

            if (yearlyMonth is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyMonthEntity yearlyMonth__)
            {
                builder.Add(YearlyMonthJsonPropertyName, yearlyMonth__.AsAny);
            }

            if (yearlyDay is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyDayEntity yearlyDay__)
            {
                builder.Add(YearlyDayJsonPropertyName, yearlyDay__.AsAny);
            }

            if (yearlyTime is Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyTimeEntity yearlyTime__)
            {
                builder.Add(YearlyTimeJsonPropertyName, yearlyTime__.AsAny);
            }

            return builder.ToImmutable();
        }

        /// <summary>
        /// Sets UseLocalTime.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithUseLocalTime(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.UseLocalTimeEntity value)
        {
            return this.SetProperty(UseLocalTimeJsonPropertyName, value);
        }

        /// <summary>
        /// Sets HourlyMinute.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithHourlyMinute(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.HourlyMinuteEntity value)
        {
            return this.SetProperty(HourlyMinuteJsonPropertyName, value);
        }

        /// <summary>
        /// Sets DailyTime.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithDailyTime(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.DailyTimeEntity value)
        {
            return this.SetProperty(DailyTimeJsonPropertyName, value);
        }

        /// <summary>
        /// Sets WeeklyDay.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithWeeklyDay(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyDayEntity value)
        {
            return this.SetProperty(WeeklyDayJsonPropertyName, value);
        }

        /// <summary>
        /// Sets WeeklyTime.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithWeeklyTime(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyTimeEntity value)
        {
            return this.SetProperty(WeeklyTimeJsonPropertyName, value);
        }

        /// <summary>
        /// Sets MonthlyDay.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithMonthlyDay(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyDayEntity value)
        {
            return this.SetProperty(MonthlyDayJsonPropertyName, value);
        }

        /// <summary>
        /// Sets MonthlyTime.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithMonthlyTime(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyTimeEntity value)
        {
            return this.SetProperty(MonthlyTimeJsonPropertyName, value);
        }

        /// <summary>
        /// Sets YearlyMonth.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithYearlyMonth(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyMonthEntity value)
        {
            return this.SetProperty(YearlyMonthJsonPropertyName, value);
        }

        /// <summary>
        /// Sets YearlyDay.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithYearlyDay(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyDayEntity value)
        {
            return this.SetProperty(YearlyDayJsonPropertyName, value);
        }

        /// <summary>
        /// Sets YearlyTime.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <returns>The entity with the updated property.</returns>
        public SnapshotTimingEntity1 WithYearlyTime(in Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyTimeEntity value)
        {
            return this.SetProperty(YearlyTimeJsonPropertyName, value);
        }

        private static ValidationContext __CorvusValidateUseLocalTime(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.UseLocalTimeEntity>().Validate(validationContext, level);
        }

        private static ValidationContext __CorvusValidateHourlyMinute(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.HourlyMinuteEntity>().Validate(validationContext, level);
        }

        private static ValidationContext __CorvusValidateDailyTime(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.DailyTimeEntity>().Validate(validationContext, level);
        }

        private static ValidationContext __CorvusValidateWeeklyDay(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyDayEntity>().Validate(validationContext, level);
        }

        private static ValidationContext __CorvusValidateWeeklyTime(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.WeeklyTimeEntity>().Validate(validationContext, level);
        }

        private static ValidationContext __CorvusValidateMonthlyDay(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyDayEntity>().Validate(validationContext, level);
        }

        private static ValidationContext __CorvusValidateMonthlyTime(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.MonthlyTimeEntity>().Validate(validationContext, level);
        }

        private static ValidationContext __CorvusValidateYearlyMonth(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyMonthEntity>().Validate(validationContext, level);
        }

        private static ValidationContext __CorvusValidateYearlyDay(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyDayEntity>().Validate(validationContext, level);
        }

        private static ValidationContext __CorvusValidateYearlyTime(in JsonObjectProperty property, in ValidationContext validationContext, ValidationLevel level)
        {
            return property.ValueAs<Sanoid.Schema.SanoidTemplateSchemaJson.SnapshotTimingEntity1.YearlyTimeEntity>().Validate(validationContext, level);
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
                if (property.NameEquals(UseLocalTimeUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateUseLocalTime;
                    return true;
                }
                else if (property.NameEquals(HourlyMinuteUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateHourlyMinute;
                    return true;
                }
                else if (property.NameEquals(DailyTimeUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateDailyTime;
                    return true;
                }
                else if (property.NameEquals(WeeklyDayUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateWeeklyDay;
                    return true;
                }
                else if (property.NameEquals(WeeklyTimeUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateWeeklyTime;
                    return true;
                }
                else if (property.NameEquals(MonthlyDayUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateMonthlyDay;
                    return true;
                }
                else if (property.NameEquals(MonthlyTimeUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateMonthlyTime;
                    return true;
                }
                else if (property.NameEquals(YearlyMonthUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateYearlyMonth;
                    return true;
                }
                else if (property.NameEquals(YearlyDayUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateYearlyDay;
                    return true;
                }
                else if (property.NameEquals(YearlyTimeUtf8JsonPropertyName.Span))
                {
                    propertyValidator = __CorvusValidateYearlyTime;
                    return true;
                }
            }
            else
            {
                if (property.NameEquals(UseLocalTimeJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateUseLocalTime;
                    return true;
                }
                else if (property.NameEquals(HourlyMinuteJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateHourlyMinute;
                    return true;
                }
                else if (property.NameEquals(DailyTimeJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateDailyTime;
                    return true;
                }
                else if (property.NameEquals(WeeklyDayJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateWeeklyDay;
                    return true;
                }
                else if (property.NameEquals(WeeklyTimeJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateWeeklyTime;
                    return true;
                }
                else if (property.NameEquals(MonthlyDayJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateMonthlyDay;
                    return true;
                }
                else if (property.NameEquals(MonthlyTimeJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateMonthlyTime;
                    return true;
                }
                else if (property.NameEquals(YearlyMonthJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateYearlyMonth;
                    return true;
                }
                else if (property.NameEquals(YearlyDayJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateYearlyDay;
                    return true;
                }
                else if (property.NameEquals(YearlyTimeJsonPropertyName))
                {
                    propertyValidator = __CorvusValidateYearlyTime;
                    return true;
                }
            }

            propertyValidator = null;
            return false;
        }
    }
}