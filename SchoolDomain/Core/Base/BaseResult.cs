namespace SchoolDomain.Core.Base;

/// <summary>
/// Clase base para respuestas de resultados
/// </summary>
public class BaseResult
{
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    public static BaseResult Success(string message = "Operación exitosa")
    {
        return new BaseResult { IsSuccess = true, Message = message };
    }

    public static BaseResult Failure(string message, List<string>? errors = null)
    {
        return new BaseResult 
        { 
            IsSuccess = false, 
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}

/// <summary>
/// Clase base para respuestas con datos genéricos
/// </summary>
public class BaseResult<T> : BaseResult
{
    public T? Data { get; set; }

    public static BaseResult<T> Success(T data, string message = "Operación exitosa")
    {
        return new BaseResult<T> 
        { 
            IsSuccess = true, 
            Message = message,
            Data = data
        };
    }

    public new static BaseResult<T> Failure(string message, List<string>? errors = null)
    {
        return new BaseResult<T> 
        { 
            IsSuccess = false, 
            Message = message,
            Errors = errors ?? new List<string>(),
            Data = default
        };
    }
}
