namespace SearchEverywhere.Model;

public class PlayStatusModel
{
    public enum Status
    {
        Play,
        Pause,
        Stop
    }

    public PlayStatusModel(Status currentStatus, bool forcePlay, string filePath)
    {
        CurrentStatus = currentStatus;
        ForcePlay = forcePlay;
        FilePath = filePath;
    }

    public Status CurrentStatus { get; set; }
    public bool ForcePlay { get; set; }
    public string FilePath { get; set; }
}