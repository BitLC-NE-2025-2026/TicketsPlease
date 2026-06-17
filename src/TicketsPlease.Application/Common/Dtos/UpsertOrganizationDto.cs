// <copyright file="UpsertOrganizationDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO zum Erstellen/Bearbeiten einer Organisation.
/// </summary>
/// <param name="Name">Name.</param>
/// <param name="SubscriptionLevel">Level.</param>
/// <param name="IsActive">Status.</param>
/// <param name="SlaCheckIntervalMinutes">Check interval in minutes.</param>
/// <param name="QuietHoursStart">Quiet hours start time.</param>
/// <param name="QuietHoursEnd">Quiet hours end time.</param>
/// <param name="TimeZoneId">Organization timezone ID.</param>
/// <param name="NotifyOnLow">Notify on low priority SLA breaches.</param>
/// <param name="NotifyOnMedium">Notify on medium priority SLA breaches.</param>
/// <param name="NotifyOnHigh">Notify on high priority SLA breaches.</param>
/// <param name="NotifyOnBlocker">Notify on blocker priority SLA breaches.</param>
public record UpsertOrganizationDto
{
    public UpsertOrganizationDto()
    {
    }

    public UpsertOrganizationDto(
        string name,
        string subscriptionLevel,
        bool isActive,
        int slaCheckIntervalMinutes,
        TimeSpan? quietHoursStart,
        TimeSpan? quietHoursEnd,
        string timeZoneId,
        bool notifyOnLow,
        bool notifyOnMedium,
        bool notifyOnHigh,
        bool notifyOnBlocker)
    {
        this.Name = name;
        this.SubscriptionLevel = subscriptionLevel;
        this.IsActive = isActive;
        this.SlaCheckIntervalMinutes = slaCheckIntervalMinutes;
        this.QuietHoursStart = quietHoursStart;
        this.QuietHoursEnd = quietHoursEnd;
        this.TimeZoneId = timeZoneId;
        this.NotifyOnLow = notifyOnLow;
        this.NotifyOnMedium = notifyOnMedium;
        this.NotifyOnHigh = notifyOnHigh;
        this.NotifyOnBlocker = notifyOnBlocker;
    }

    public string Name { get; set; } = string.Empty;
    public string SubscriptionLevel { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int SlaCheckIntervalMinutes { get; set; }
    public TimeSpan? QuietHoursStart { get; set; }
    public TimeSpan? QuietHoursEnd { get; set; }
    public string TimeZoneId { get; set; } = string.Empty;
    public bool NotifyOnLow { get; set; }
    public bool NotifyOnMedium { get; set; }
    public bool NotifyOnHigh { get; set; }
    public bool NotifyOnBlocker { get; set; }
}
