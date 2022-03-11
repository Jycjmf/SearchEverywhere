namespace SearchEverywhere.Model;

public class SuccessModel<TRes>
{
    public SuccessModel(bool isSuccess, TRes result)
    {
        IsSuccess = isSuccess;
        Result = result;
    }

    public SuccessModel(bool isSuccess, TRes result, string msg)
    {
        IsSuccess = isSuccess;
        Result = result;
        Msg = msg;
    }

    public string Msg { get; set; } = "Success.";
    public bool IsSuccess { get; set; }
    public TRes Result { get; set; }
}