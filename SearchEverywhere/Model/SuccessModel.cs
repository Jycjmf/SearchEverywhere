namespace SearchEverywhere.Model;

public class SuccessModel<TRes>
{
    public SuccessModel(bool isSuccess, TRes result)
    {
        IsSuccess = isSuccess;
        Result = result;
    }

    public bool IsSuccess { get; set; }
    public TRes Result { get; set; }
}