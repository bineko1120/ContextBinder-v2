namespace ContextBinder.Services;

public sealed class BackupService
{
    public string? CreateBackupIfExists(string sourceFilePath, string backupDirectory)
    {
        if (!File.Exists(sourceFilePath))
        {
            return null;
        }

        Directory.CreateDirectory(backupDirectory);

        string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        string fileName = $"{Path.GetFileNameWithoutExtension(sourceFilePath)}.{timestamp}{Path.GetExtension(sourceFilePath)}";
        string destinationPath = Path.Combine(backupDirectory, fileName);
        File.Copy(sourceFilePath, destinationPath, overwrite: false);
        return destinationPath;
    }

    public void PruneBackups(string backupDirectory, int maxBackupCount)
    {
        if (!Directory.Exists(backupDirectory) || maxBackupCount <= 0)
        {
            return;
        }

        FileInfo[] backupFiles = new DirectoryInfo(backupDirectory)
            .GetFiles("*.json")
            .OrderByDescending(file => file.CreationTimeUtc)
            .ToArray();

        foreach (FileInfo file in backupFiles.Skip(maxBackupCount))
        {
            file.Delete();
        }
    }
}
