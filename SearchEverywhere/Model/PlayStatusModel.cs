namespace SearchEverywhere.Model;

public class PlayStatusModel
{
    public enum Status
    {
        Play,
        Pause,
        Stop
    }

    public PlayStatusModel(Status currentStatus, bool forcePlay)
    {
        CurrentStatus = currentStatus;
        ForcePlay = forcePlay;
    }

    public Status CurrentStatus { get; set; }
    public bool ForcePlay { get; set; }
}