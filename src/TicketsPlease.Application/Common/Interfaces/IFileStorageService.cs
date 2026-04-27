// <copyright file="IFileStorageService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Definiert den Dienst fÃ¼r die physische Speicherung von Dateien.
/// </summary>
public interface IFileStorageService
{
  /// <summary>
  /// Speichert eine Datei und gibt den Pfad/Key zurÃ¼ck.
  /// </summary>
  /// <param name="content">Der Datenstrom der Datei.</param>
  /// <param name="fileName">Der gewÃ¼nschte Dateiname (zur ErweiterungsprÃ¼fung).</param>
  /// <param name="ct">Abbruchtoken.</param>
  /// <returns>Der Speicherpfad (BlobPath).</returns>
  public Task<string> SaveFileAsync(Stream content, string fileName, CancellationToken ct = default);

  /// <summary>
  /// Ruft eine Datei ab.
  /// </summary>
  /// <param name="blobPath">Der Speicherpfad.</param>
  /// <param name="ct">Abbruchtoken.</param>
  /// <returns>Der Datenstrom der Datei.</returns>
  public Task<Stream> GetFileAsync(string blobPath, CancellationToken ct = default);

  /// <summary>
  /// LÃ¶scht eine Datei.
  /// </summary>
  /// <param name="blobPath">Der Speicherpfad.</param>
  /// <param name="ct">Abbruchtoken.</param>
  /// <returns>Ein Task.</returns>
  public Task DeleteFileAsync(string blobPath, CancellationToken ct = default);
}
