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