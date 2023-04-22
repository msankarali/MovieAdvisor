namespace Application.Common.Models;

public class Result
{
    public bool Success { get; private set; }
    public string[] Errors { get; private set; }
    public int Code { get; private set; }

    public static Result SuccessOperation() => new Result { Success = true };
    public static Result SuccessOperation(int code) => new Result { Success = true, Code = code };

    public static Result ErrorOperation(string errorMessage) => new Result { Success = false, Errors = new[] { errorMessage } };
    public static Result ErrorOperation(string errorMessage, int code) => new Result { Success = false, Errors = new[] { errorMessage }, Code = code };
    public static Result ErrorOperation(string[] errorMessages) => new Result { Success = false, Errors = errorMessages };
    public static Result ErrorOperation(string[] errorMessages, int code) => new Result { Success = false, Errors = errorMessages, Code = code };
}
