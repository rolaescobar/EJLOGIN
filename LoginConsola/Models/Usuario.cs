using System;
using System.Collections.Generic;

namespace LoginConsola.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nickname { get; set; } = null!;

    public string Contrasena { get; set; } = null!;
}
