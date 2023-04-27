namespace Application.Common.Models;

public class Result
{
    public ResultTypes ResultType { get; private set; }
    public List<string> Messages { get; private set; }
    public int Code { get; private set; }

    protected Result(ResultTypes resultType) => ResultType = resultType;
    private Result(ResultTypes resultType, string message) : this(resultType) => AddMessage(message);

    public static Result Success(string message) => new Result(ResultTypes.Success, message);
    public static Result Error(string message) => new Result(ResultTypes.Error, message);
    public static Result Warning(string message) => new Result(ResultTypes.Warning, message);
    public static Result Information(string message) => new Result(ResultTypes.Information, message);

    public Result AddMessage(string message)
    {
        Messages ??= new List<string>();
        Messages.Add(message);
        return this;
    }

    public Result WithCode(int code)
    {
        Code = code;
        return this;
    }
}
