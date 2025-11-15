namespace SchoolDomain.Core.Base;

/// <summary>
/// Clase base para todos los servicios de negocio
/// </summary>
public abstract class BaseService
{
    protected virtual void ValidateNotNull(object obj, string paramName)
    {
        if (obj == null)
            throw new ArgumentNullException(paramName);
    }

    protected virtual void ValidateNotNullOrEmpty(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{fieldName} no puede estar vacío", fieldName);
    }

    protected virtual void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentException($"{fieldName} debe ser mayor a 0", fieldName);
    }

    protected virtual void ValidateRange(int value, int min, int max, string fieldName)
    {
        if (value < min || value > max)
            throw new ArgumentException($"{fieldName} debe estar entre {min} y {max}", fieldName);
    }

    protected virtual List<string> ValidateEntity(Dictionary<string, Func<bool>> validations)
    {
        var errors = new List<string>();
        foreach (var validation in validations)
        {
            if (!validation.Value())
                errors.Add(validation.Key);
        }
        return errors;
    }
}
