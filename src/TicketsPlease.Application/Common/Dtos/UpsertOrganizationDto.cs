// <copyright file="UpsertOrganizationDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO zum Erstellen/Bearbeiten einer Organisation.
/// </summary>
public record UpsertOrganizationDto
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UpsertOrganizationDto"/> class.
  /// </summary>
  public UpsertOrganizationDto()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UpsertOrganizationDto"/> class.
  /// </summary>
  /// <param name="name">Name.</param>
  /// <param name="subscriptionLevel">Subscription Level.</param>
  /// <param name="isActive">Active status.</param>
  /// <param name="slaCheckIntervalMinutes">SLA Check Interval.</param>
  /// <param name="quietHoursStart">Quiet Hours Start.</param>
  /// <param name="quietHoursEnd">Quiet Hours End.</param>
  /// <param name="timeZoneId">Time Zone.</param>
  /// <param name="notifyOnLow">Notify Low.</param>
  /// <param name="notifyOnMedium">Notify Medium.</param>
  /// <param name="notifyOnHigh">Notify High.</param>
  /// <param name="notifyOnBlocker">Notify Blocker.</param>
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

  /// <summary>
  /// Gets or sets the name.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the subscription level.
  /// </summary>
  public string SubscriptionLevel { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets a value indicating whether the organization is active.
  /// </summary>
  public bool IsActive { get; set; }

  /// <summary>
  /// Gets or sets the SLA check interval.
  /// </summary>
  public int SlaCheckIntervalMinutes { get; set; }

  /// <summary>
  /// Gets or sets the quiet hours start.
  /// </summary>
  public TimeSpan? QuietHoursStart { get; set; }

  /// <summary>
  /// Gets or sets the quiet hours end.
  /// </summary>
  public TimeSpan? QuietHoursEnd { get; set; }

  /// <summary>
  /// Gets or sets the time zone ID.
  /// </summary>
  public string TimeZoneId { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets a value indicating whether to notify on low priority.
  /// </summary>
  public bool NotifyOnLow { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether to notify on medium priority.
  /// </summary>
  public bool NotifyOnMedium { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether to notify on high priority.
  /// </summary>
  public bool NotifyOnHigh { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether to notify on blocker priority.
  /// </summary>
  public bool NotifyOnBlocker { get; set; }
}
