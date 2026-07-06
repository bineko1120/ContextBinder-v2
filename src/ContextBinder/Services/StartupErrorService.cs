using System.Text;
using ContextBinder.Models;

namespace ContextBinder.Services;

public sealed class StartupErrorService
{
    public const string LogFileName = "startup-error.log";

    private readonly string _fallbackDirectory;

    public StartupErrorService(string? fallbackDirectory = null)
    {
        _fallbackDirectory = Path.GetFullPath(fallbackDirectory ?? AppContext.BaseDirectory);
    }

    public string? WriteStartupError(Exception exception, StorageLocation? storageLocation)
    {
        foreach (string directory in GetCandidateDirectories(storageLocation).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            try
            {
                Directory.CreateDirectory(directory);
                string logPath = Path.Combine(directory, LogFileName);
                File.AppendAllText(logPath, CreateLogText(exception), Encoding.UTF8);
                return logPath;
            }
            catch (Exception ex) when (IsLogWriteException(ex))
            {
            }
        }

        return null;
    }

    public static string CreateUserMessage(Exception exception, string? logPath)
    {
        string logMessage = string.IsNullOrWhiteSpace(logPath)
            ? "起動エラーログを保存できませんでした。"
            : $"起動エラーログ: {logPath}";

        return $"ContextBinder の起動中にエラーが発生しました。\n\n{exception.Message}\n\n{logMessage}";
    }

    private IEnumerable<string> GetCandidateDirectories(StorageLocation? storageLocation)
    {
        if (!string.IsNullOrWhiteSpace(storageLocation?.DataDirectory))
        {
            yield return storageLocation.DataDirectory;
        }

        yield return _fallbackDirectory;
    }

    private static string CreateLogText(Exception exception)
    {
        return $"""
[{DateTimeOffset.Now:O}] ContextBinder startup error
{exception}

""";
    }

    private static bool IsLogWriteException(Exception ex)
    {
        return ex is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException;
    }
}
