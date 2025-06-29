namespace PerfumeStoreApi.Context.Dtos;

public class OperationResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
    
    public static OperationResult<T> CreateSuccess(T data, string message = "")
    {
        return new OperationResult<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }
    
    public static OperationResult<T> CreateFailure(string error)
    {
        return new OperationResult<T>
        {
            Success = false,
            Errors = new List<string> { error }
        };
    }
    
    public static OperationResult<T> CreateFailure(List<string> errors)
    {
        return new OperationResult<T>
        {
            Success = false,
            Errors = errors
        };
    }
}