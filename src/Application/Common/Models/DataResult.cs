namespace Application.Common.Models;

public class DataResult<TData> : Result
    where TData : class, new()
{
    private DataResult(ResultTypes resultType) : base(resultType) { }
    private DataResult(ResultTypes resultType, TData data) : this(resultType) => Data = data;

    public static DataResult<TData> Success(TData data) => new DataResult<TData>(ResultTypes.Success, data);
    public static new DataResult<TData> Error(string message) => (DataResult<TData>)new DataResult<TData>(ResultTypes.Success).AddMessage(message);
    public static DataResult<TData> Error(TData data, string message) => (DataResult<TData>)new DataResult<TData>(ResultTypes.Success, data).AddMessage(message);
    public static new DataResult<TData> Warning(string message) => (DataResult<TData>)new DataResult<TData>(ResultTypes.Warning).AddMessage(message);
    public static DataResult<TData> Warning(TData data, string message) => (DataResult<TData>)new DataResult<TData>(ResultTypes.Warning, data).AddMessage(message);
    public static new DataResult<TData> Information(string message) => (DataResult<TData>)new DataResult<TData>(ResultTypes.Warning).AddMessage(message);
    public static DataResult<TData> Information(TData data, string message) => (DataResult<TData>)new DataResult<TData>(ResultTypes.Warning, data).AddMessage(message);

    public TData Data { get; init; }
}
