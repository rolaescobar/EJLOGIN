using System;
using System.Linq;
using LoginConsola.Models;
using Microsoft.EntityFrameworkCore;

// =====================================
// 1. CONFIGURACIÓN DE SQL SERVER
// =====================================

string conexion =
    @"Server=LAPTOP-CTDBT2LP\SQLEXPRESS;Database=DB_LOGIN;Trusted_Connection=True;TrustServerCertificate=True;";
    
var opciones = new DbContextOptionsBuilder<LoginContext>()
    .UseSqlServer(conexion)
    .Options;

// =====================================
// 2. VERIFICAR CONEXIÓN A SQL SERVER
// =====================================

Console.WriteLine("Verificando conexión a SQL Server...");

try
{
    using var db = new LoginContext(opciones);

    if (!db.Database.CanConnect())
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("ERROR: No se pudo conectar a SQL Server.");
        Console.ResetColor();
        return;
    }

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("CONEXIÓN EXITOSA A SQL SERVER");
    Console.ResetColor();

    Console.WriteLine("Base de datos: DB_LOGIN");

    int totalUsuarios = db.Usuarios.Count();

    Console.WriteLine("Tabla Usuarios: ACCESIBLE");
    Console.WriteLine("=====================================");
    Console.WriteLine($"Usuarios registrados: {totalUsuarios}");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERROR DE CONEXIÓN O ACCESO A LA BASE DE DATOS");
    Console.ResetColor();

    Console.WriteLine("Detalle: " + ex.Message);
    return;
}

while (true)
{
    Console.WriteLine("\n===== CRUD DE USUARIOS =====");
    Console.WriteLine("1. Registrar usuario");
    Console.WriteLine("2. Listar usuarios");
    Console.WriteLine("3. Buscar usuario por ID");
    Console.WriteLine("4. Actualizar usuario");
    Console.WriteLine("5. Eliminar usuario");
    Console.WriteLine("6. Iniciar sesión");
    Console.WriteLine("7. Salir");

    Console.Write("\nSeleccione una opción: ");
    string? opcion = Console.ReadLine();

    try
    {
        switch (opcion)
        {
            case "1":
                Registrar();
                break;

            case "2":
                //Listar();
                break;

            case "3":
               // Buscar();
                break;

            case "4":
              //  Actualizar();
                break;

            case "5":
             //   Eliminar();
                break;

            case "6":
                Login();
                break;

            case "7":
                Console.WriteLine("Saliendo del sistema...");
                return;

            default:
                Console.WriteLine("Opción incorrecta.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}

// =====================================
// 4. CREATE - REGISTRAR USUARIO
// =====================================

void Registrar()
{
    using var db = new LoginContext(opciones);

    Console.Write("\nIngrese nickname: ");
    string nickname = Console.ReadLine()?.Trim() ?? "";

    Console.Write("Ingrese contraseña: ");
    string password = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(nickname) ||
        string.IsNullOrWhiteSpace(password) ||
        nickname.Length > 50)
    {
        Console.WriteLine("Datos inválidos.");
        return;
    }

    bool existe = db.Usuarios
        .Any(u => u.Nickname == nickname);

    if (existe)
    {
        Console.WriteLine("El usuario ya existe.");
        return;
    }

    Usuario nuevo = new Usuario
    {
        Nickname = nickname,
        Contrasena = BCrypt.Net.BCrypt.HashPassword(password)
    };

    db.Usuarios.Add(nuevo);
    db.SaveChanges();

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Usuario registrado correctamente.");
    Console.ResetColor();
}

void Login()
{
    using var db = new LoginContext(opciones);

    Console.Write("\nIngrese nickname: ");
    string nickname = Console.ReadLine()?.Trim() ?? "";

    Console.Write("Ingrese contraseña: ");
    string password = Console.ReadLine() ?? "";

    var usuario = db.Usuarios
        .AsNoTracking()
        .FirstOrDefault(u => u.Nickname == nickname);

    bool valido = false;

    if (usuario != null &&
        usuario.Contrasena.StartsWith("$2"))
    {
        try
        {
            valido = BCrypt.Net.BCrypt.Verify(
                password,
                usuario.Contrasena);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            valido = false;
        }
    }

    if (valido)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nINICIO DE SESIÓN EXITOSO");
        Console.WriteLine($"Bienvenido, {usuario!.Nickname}");
        Console.ResetColor();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Usuario o contraseña incorrectos.");
        Console.ResetColor();
    }
}