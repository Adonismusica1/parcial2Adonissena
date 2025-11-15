// Clase base
public abstract class MiembroDeLaComunidad
{
    public string? Nombre { get; set; }
    public string? Identificacion { get; set; }

    public virtual void MostrarRol()
    {
        Console.WriteLine("Soy un miembro de la comunidad.");
    }
}

// Subclase: Empleado
public abstract class Empleado : MiembroDeLaComunidad
{
    public string? Departamento { get; set; }
    public decimal Salario { get; set; }

    public override void MostrarRol()
    {
        Console.WriteLine("Soy un empleado de la institución.");
    }
}

// Subclase de Empleado: Docente
public abstract class Docente : Empleado
{
    public string? AreaAcademica { get; set; }

    public override void MostrarRol()
    {
        Console.WriteLine("Soy un docente.");
    }
}

// Docente → Administrador
public class Administrador : Docente
{
    public string? Cargo { get; set; }

    public override void MostrarRol()
    {
        Console.WriteLine($"Soy un administrador académico: {Cargo}");
    }
}

// Docente → Maestro
public class Maestro : Docente
{
    public string? Materia { get; set; }

    public override void MostrarRol()
    {
        Console.WriteLine($"Soy un maestro de {Materia}");
    }
}

// Empleado → Administrativo
public class Administrativo : Empleado
{
    public string? Funcion { get; set; }

    public override void MostrarRol()
    {
        Console.WriteLine($"Soy personal administrativo, función: {Funcion}");
    }
}

// Estudiante
public class Estudiante : MiembroDeLaComunidad
{
    public string? Carrera { get; set; }

    public override void MostrarRol()
    {
        Console.WriteLine($"Soy un estudiante de {Carrera}");
    }
}

// ExAlumno
public class ExAlumno : MiembroDeLaComunidad
{
    public int AñoGraduacion { get; set; }

    public override void MostrarRol()
    {
        Console.WriteLine($"Soy un exalumno, promoción {AñoGraduacion}");
    }
}