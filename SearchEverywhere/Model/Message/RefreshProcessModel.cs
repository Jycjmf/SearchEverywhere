namespace SearchEverywhere.Model.Message;

public class RefreshProcessModel
{
    public RefreshProcessModel(bool isAdd, ListItemModel processInfo)
    {
        IsAdd = isAdd;
        ProcessInfo = processInfo;
    }

    public bool IsAdd { get; set; }
    public ListItemModel ProcessInfo { get; set; }
}