using AA.Models;
using AA.Services;

namespace AA
{
    public class MenuApp
    {
        private readonly JsonPersistenceService _persistenceService;
        private readonly MedicoService _medicoService;
        private readonly UsuarioService _usuarioService;
        private readonly CitaService _citaService;
        private Usuario? _usuarioActual = null;

        public MenuApp()
        {
            _persistenceService = new JsonPersistenceService();
            _medicoService = new MedicoService(_persistenceService);
            _usuarioService = new UsuarioService(_persistenceService);
            _citaService = new CitaService(_persistenceService);
        }

        public void Iniciar()
        {
            int opcion = 0;
            do
            {
                
                Console.WriteLine("=== Sistema de Citas Médicas ===");
                Console.WriteLine("1 - Ver Médicos");
                Console.WriteLine("2 - Ver Pacientes");
                Console.WriteLine("3 - Ver Citas");
                Console.WriteLine("4 - Buscar");
                Console.WriteLine("5 - Iniciar Sesión");
                Console.WriteLine("6 - Registrarse");
                Console.WriteLine("0 - Salir");
                Console.Write("Selecciona una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    MostrarError("Por favor selecciona una opción válida.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        MostrarMedicos();
                        break;
                    case 2:
                        MostrarPacientes();
                        break;
                    case 3:
                        MostrarCitas();
                        break;
                    case 4:
                        MenuBusqueda();
                        break;
                    case 5:
                        Login();
                        break;
                    case 6:
                        Registrar();
                        break;
                    case 0:
                        Console.WriteLine("¡Gracias por usar el Sistema de Citas Médicas!");
                        return;
                    default:
                        MostrarError("Opción no válida.");
                        break;
                }

                PausarPantalla();
            } while (opcion != 0);
        }

        private void MostrarMedicos()
        {
            
            Console.WriteLine("=== Médicos ===");
            var medicos = _medicoService.ObtenerMedicos();
            
            if (!medicos.Any())
            {
                Console.WriteLine("No hay médicos disponibles.");
                return;
            }
            
            foreach (var medico in medicos)
            {
                Console.WriteLine($"ID: {medico.Id}, Nombre: {medico.Nombre}, Especialidad: {medico.Especialidad}");
                Console.WriteLine($"   Email: {medico.Email}, Teléfono: {medico.Telefono}");
                Console.WriteLine($"   Disponible: {(medico.Disponible ? "Sí" : "No")}");
                Console.WriteLine();
            }
        }

        private void MostrarPacientes()
        {
            
            Console.WriteLine("=== Pacientes ===");
            var usuarios = _usuarioService.ObtenerUsuarios().Where(u => u.Rol == "Paciente").ToList();
            
            if (!usuarios.Any())
            {
                Console.WriteLine("No hay pacientes registrados.");
                return;
            }
            
            foreach (var usuario in usuarios)
            {
                Console.WriteLine($"ID: {usuario.Id}, Nombre: {usuario.Nombre}, Email: {usuario.Email}");
                Console.WriteLine($"   Fecha de registro: {usuario.FechaRegistro.ToShortDateString()}, Activo: {(usuario.EstaActivo ? "Sí" : "No")}");
                Console.WriteLine();
            }
        }

        private void MostrarCitas()
        {
            
            Console.WriteLine("=== Citas ===");
            var citas = _citaService.ObtenerCitas();
            
            if (!citas.Any())
            {
                Console.WriteLine("No hay citas registradas.");
                return;
            }
            
            foreach (var cita in citas)
            {
                var usuario = _usuarioService.ObtenerUsuarioPorId(cita.IdUsuario);
                var medico = _medicoService.ObtenerMedicoPorId(cita.IdMedico);

                Console.WriteLine($"ID: {cita.Id}, Fecha: {cita.FechaCita.ToString("dd/MM/yyyy HH:mm")}");
                Console.WriteLine($"   Paciente: {usuario?.Nombre ?? "Desconocido"}, Médico: {medico?.Nombre ?? "Desconocido"}");
                Console.WriteLine($"   Motivo: {cita.Motivo}, Confirmada: {(cita.Confirmada ? "Sí" : "No")}");
                Console.WriteLine();
            }
        }

        private void MenuBusqueda()
        {
            
            Console.WriteLine("=== Búsqueda ===");
            Console.WriteLine("1 - Buscar médicos por especialidad");
            Console.WriteLine("2 - Buscar médicos por nombre");
            Console.WriteLine("3 - Buscar pacientes por nombre");
            Console.WriteLine("4 - Buscar citas por motivo");
            Console.WriteLine("5 - Buscar citas por fecha");
            Console.WriteLine("0 - Volver al menú principal");
            Console.Write("Selecciona una opción: ");

            if (!int.TryParse(Console.ReadLine(), out int opcion))
            {
                MostrarError("Por favor selecciona una opción válida.");
                return;
            }

            switch (opcion)
            {
                case 1:
                    BuscarMedicosPorEspecialidad();
                    break;
                case 2:
                    BuscarMedicosPorNombre();
                    break;
                case 3:
                    BuscarPacientesPorNombre();
                    break;
                case 4:
                    BuscarCitasPorMotivo();
                    break;
                case 5:
                    BuscarCitasPorFecha();
                    break;
                case 0:
                    return;
                default:
                    MostrarError("Opción no válida.");
                    break;
            }
        }

        private void BuscarMedicosPorEspecialidad()
        {
            
            Console.WriteLine("=== Buscar Médicos por Especialidad ===");
            Console.Write("Ingrese la especialidad a buscar: ");
            string especialidad = Console.ReadLine() ?? "";

            var resultados = _medicoService.BuscarMedicosPorEspecialidad(especialidad);
            
            if (!resultados.Any())
            {
                Console.WriteLine($"No se encontraron médicos con la especialidad '{especialidad}'.");
                return;
            }
            
            Console.WriteLine($"Resultados para especialidad '{especialidad}':");
            foreach (var medico in resultados)
            {
                Console.WriteLine($"ID: {medico.Id}, Nombre: {medico.Nombre}, Especialidad: {medico.Especialidad}");
                Console.WriteLine($"   Email: {medico.Email}, Teléfono: {medico.Telefono}");
                Console.WriteLine();
            }
        }

        private void BuscarMedicosPorNombre()
        {
            
            Console.WriteLine("=== Buscar Médicos por Nombre ===");
            Console.Write("Ingrese el nombre a buscar: ");
            string nombre = Console.ReadLine() ?? "";

            var resultados = _medicoService.BuscarMedicosPorNombre(nombre);
            
            if (!resultados.Any())
            {
                Console.WriteLine($"No se encontraron médicos con el nombre '{nombre}'.");
                return;
            }
            
            Console.WriteLine($"Resultados para nombre '{nombre}':");
            foreach (var medico in resultados)
            {
                Console.WriteLine($"ID: {medico.Id}, Nombre: {medico.Nombre}, Especialidad: {medico.Especialidad}");
                Console.WriteLine($"   Email: {medico.Email}, Teléfono: {medico.Telefono}");
                Console.WriteLine();
            }
        }

        private void BuscarPacientesPorNombre()
        {
           
            Console.WriteLine("=== Buscar Pacientes por Nombre ===");
            Console.Write("Ingrese el nombre a buscar: ");
            string nombre = Console.ReadLine() ?? "";

            var resultados = _usuarioService.BuscarUsuariosPorNombre(nombre).Where(u => u.Rol == "Paciente").ToList();
            
            if (!resultados.Any())
            {
                Console.WriteLine($"No se encontraron pacientes con el nombre '{nombre}'.");
                return;
            }
            
            Console.WriteLine($"Resultados para nombre '{nombre}':");
            foreach (var usuario in resultados)
            {
                Console.WriteLine($"ID: {usuario.Id}, Nombre: {usuario.Nombre}, Email: {usuario.Email}");
                Console.WriteLine($"   Fecha de registro: {usuario.FechaRegistro.ToShortDateString()}");
                Console.WriteLine();
            }
        }

        private void BuscarCitasPorMotivo()
        {
            
            Console.WriteLine("=== Buscar Citas por Motivo ===");
            Console.Write("Ingrese el motivo a buscar: ");
            string motivo = Console.ReadLine() ?? "";

            var resultados = _citaService.BuscarCitasPorMotivo(motivo);
            
            if (!resultados.Any())
            {
                Console.WriteLine($"No se encontraron citas con el motivo '{motivo}'.");
                return;
            }
            
            Console.WriteLine($"Resultados para motivo '{motivo}':");
            MostrarCitasResultado(resultados);
        }

        private void BuscarCitasPorFecha()
        {
            
            Console.WriteLine("=== Buscar Citas por Fecha ===");
            Console.Write("Ingrese la fecha (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime fecha))
            {
                MostrarError("Formato de fecha incorrecto.");
                return;
            }

            var resultados = _citaService.BuscarCitasPorFecha(fecha);
            
            if (!resultados.Any())
            {
                Console.WriteLine($"No se encontraron citas para la fecha {fecha.ToShortDateString()}.");
                return;
            }
            
            Console.WriteLine($"Resultados para fecha {fecha.ToShortDateString()}:");
            MostrarCitasResultado(resultados);
        }

        private void MostrarCitasResultado(List<Cita> citas)
        {
            foreach (var cita in citas)
            {
                var usuario = _usuarioService.ObtenerUsuarioPorId(cita.IdUsuario);
                var medico = _medicoService.ObtenerMedicoPorId(cita.IdMedico);

                Console.WriteLine($"ID: {cita.Id}, Fecha: {cita.FechaCita.ToString("dd/MM/yyyy HH:mm")}");
                Console.WriteLine($"   Paciente: {usuario?.Nombre ?? "Desconocido"}, Médico: {medico?.Nombre ?? "Desconocido"}");
                Console.WriteLine($"   Motivo: {cita.Motivo}, Confirmada: {(cita.Confirmada ? "Sí" : "No")}");
                Console.WriteLine();
            }
        }

        private void Login()
        {
            Console.WriteLine("=== Iniciar Sesión ===");

            Console.Write("Introduce tu nombre de usuario: ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("Introduce tu contraseña: ");
            string contrasena = Console.ReadLine() ?? "";

            // Obtener el usuario con las credenciales proporcionadas
            _usuarioActual = _usuarioService.ObtenerUsuarioPorCredenciales(nombre, contrasena);

            if (_usuarioActual != null)
            {
                Console.WriteLine($"Bienvenido, {_usuarioActual.Nombre}!");

                // Verificar el rol del usuario
                if (_usuarioActual.Rol != null && _usuarioActual.Rol.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    MenuAdmin(); // Ir al menú de administrador
                }
                else
                {
                    MenuPrivado(); // Ir al menú privado para usuarios normales
                }
            }
            else
            {
                MostrarError("Credenciales incorrectas. Inténtalo de nuevo.");
            }
        }

        private void Registrar()
        {
            
            Console.WriteLine("=== Registro de Nuevo Usuario ===");

            Console.Write("Introduce tu nombre de usuario: ");
            string nombre = Console.ReadLine() ?? "";

            var usuarioExistente = _usuarioService.BuscarUsuariosPorNombre(nombre).FirstOrDefault(u => u.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (usuarioExistente != null)
            {
                MostrarError("Ese nombre de usuario ya está en uso. Por favor, elige otro.");
                return;
            }

            Console.Write("Introduce tu correo electrónico: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Introduce tu contraseña: ");
            string contrasena = Console.ReadLine() ?? "";

            var nuevoUsuario = new Usuario
            {
                Nombre = nombre,
                Email = email,
                Contrasena = contrasena,
                Rol = "Paciente",
                EstaActivo = true
            };

            _usuarioService.AgregarUsuario(nuevoUsuario);
            Console.WriteLine("¡Usuario registrado con éxito! Ahora puedes iniciar sesión.");
        }

        private void MenuPrivado()
        {
            int opcion;
            do
            {
                
                Console.WriteLine($"=== Zona Privada de {_usuarioActual!.Nombre} ===");
                Console.WriteLine("1 - Ver tu información");
                Console.WriteLine("2 - Ver tus citas");
                Console.WriteLine("3 - Añadir cita");
                Console.WriteLine("4 - Borrar cita");
                Console.WriteLine("5 - Salir");
                Console.Write("Selecciona una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    MostrarError("Por favor selecciona una opción válida.");
                    PausarPantalla();
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        MostrarInfoPersonal();
                        break;
                    case 2:
                        MostrarCitasPersonales();
                        break;
                    case 3:
                        AgregarCita();
                        break;
                    case 4:
                        BorrarCita();
                        break;
                    case 5:
                        _usuarioActual = null;
                        Console.WriteLine("Has cerrado sesión.");
                        return;
                    default:
                        MostrarError("Opción no válida.");
                        break;
                }

                PausarPantalla();
            } while (opcion != 5);
        }

        private void MenuAdmin()
        {
            int opcion;
            do
            {
                
                Console.WriteLine("=== Menú de Administrador ===");
                Console.WriteLine("1 - Ver Citas");
                Console.WriteLine("2 - Añadir Médico");
                Console.WriteLine("3 - Borrar Médico");
                Console.WriteLine("0 - Cerrar Sesión");
                Console.Write("Selecciona una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    MostrarError("Por favor selecciona una opción válida.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        MostrarCitas();
                        break;
                    case 2:
                        AgregarMedico();
                        break;
                    case 3:
                        BorrarMedico();
                        break;
                    case 0:
                        _usuarioActual = null;
                        Console.WriteLine("Has cerrado sesión.");
                        return;
                    default:
                        MostrarError("Opción no válida.");
                        break;
                }

                PausarPantalla();
            } while (opcion != 0);
        }

        private void AgregarMedico()
        {
            
            Console.WriteLine("=== Añadir Médico ===");

            Console.Write("Introduce el nombre del médico: ");
            string nombre = Console.ReadLine() ?? "";

            Console.Write("Introduce la especialidad del médico: ");
            string especialidad = Console.ReadLine() ?? "";

            Console.Write("Introduce el email del médico: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Introduce el teléfono del médico: ");
            string telefono = Console.ReadLine() ?? "";

            var nuevoMedico = new Medico
            {
                Nombre = nombre,
                Especialidad = especialidad,
                Email = email,
                Telefono = telefono,
                Disponible = true
            };

            var medicos = _medicoService.ObtenerMedicos();
            medicos.Add(nuevoMedico);
            Console.WriteLine("¡Médico añadido con éxito!");
        }

        private void BorrarMedico()
        {
            
            Console.WriteLine("=== Borrar Médico ===");

            var medicos = _medicoService.ObtenerMedicos();
            if (!medicos.Any())
            {
                Console.WriteLine("No hay médicos disponibles para borrar.");
                return;
            }

            foreach (var medico in medicos)
            {
                Console.WriteLine($"ID: {medico.Id}, Nombre: {medico.Nombre}, Especialidad: {medico.Especialidad}");
            }

            Console.Write("Introduce el ID del médico que deseas borrar: ");
            string id = Console.ReadLine() ?? "";

            if (!int.TryParse(id, out int idInt))
            {
                MostrarError("ID inválido.");
                return;
            }
            var medicoABorrar = medicos.FirstOrDefault(m => m.Id == idInt);
            if (medicoABorrar == null)
            {
                MostrarError("No se encontró un médico con ese ID.");
                return;
            }

            medicos.Remove(medicoABorrar);

            Console.WriteLine("¡Médico borrado con éxito!");
        }

        private void MostrarInfoPersonal()
        {
            
            Console.WriteLine("=== Tu Información ===");
            Console.WriteLine($"ID: {_usuarioActual!.Id}");
            Console.WriteLine($"Nombre: {_usuarioActual.Nombre}");
            Console.WriteLine($"Correo: {_usuarioActual.Email}");
            Console.WriteLine($"Fecha de registro: {_usuarioActual.FechaRegistro}");
            Console.WriteLine($"Rol: {_usuarioActual.Rol}");
            Console.WriteLine($"Estado: {(_usuarioActual.EstaActivo ? "Activo" : "Inactivo")}");
        }

        private void MostrarCitasPersonales()
        {
            
            Console.WriteLine("=== Tus Citas ===");
            var citas = _citaService.ObtenerCitasPorUsuario(_usuarioActual!.Id);
            if (!citas.Any())
            {
                Console.WriteLine("No tienes citas programadas.");
                return;
            }

            foreach (var cita in citas)
            {
                var medico = _medicoService.ObtenerMedicoPorId(cita.IdMedico);
                Console.WriteLine($"ID: {cita.Id}, Médico: {medico?.Nombre ?? "Desconocido"}, Especialidad: {medico?.Especialidad ?? ""}");
                Console.WriteLine($"   Fecha: {cita.FechaCita.ToString("dd/MM/yyyy HH:mm")}");
                Console.WriteLine($"   Motivo: {cita.Motivo}");
                Console.WriteLine($"   Confirmada: {(cita.Confirmada ? "Sí" : "No")}");
                Console.WriteLine();
            }
        }

        private void AgregarCita()
        {
            
            Console.WriteLine("=== Añadir Cita ===");

            Console.WriteLine("Médicos disponibles:");
            var medicos = _medicoService.ObtenerMedicos().Where(m => m.Disponible).ToList();
            if (!medicos.Any())
            {
                Console.WriteLine("No hay médicos disponibles en este momento.");
                return;
            }

            foreach (var m in medicos)
            {
                Console.WriteLine($"{m.Id} - {m.Nombre} ({m.Especialidad})");
            }

            Console.Write("\nSelecciona el ID del médico: ");
            if (!int.TryParse(Console.ReadLine(), out int idMedico))
            {
                MostrarError("ID de médico inválido.");
                return;
            }

            var medico = _medicoService.ObtenerMedicoPorId(idMedico);
            if (medico == null || !medico.Disponible)
            {
                MostrarError("Médico no encontrado o no disponible.");
                return;
            }

            Console.Write("Fecha de la cita (dd/MM/yyyy): ");
            string fechaStr = Console.ReadLine() ?? "";
            
            Console.Write("Hora de la cita (HH:mm): ");
            string horaStr = Console.ReadLine() ?? "";
            
            if (!DateTime.TryParse($"{fechaStr} {horaStr}", out DateTime fechaCita))
            {
                MostrarError("Formato de fecha u hora incorrecto.");
                return;
            }
            
            if (fechaCita < DateTime.Now)
            {
                MostrarError("La fecha de la cita no puede ser en el pasado.");
                return;
            }

            Console.Write("Motivo de la cita: ");
            string motivo = Console.ReadLine() ?? "";
            
            if (string.IsNullOrWhiteSpace(motivo))
            {
                MostrarError("Debes especificar un motivo para la cita.");
                return;
            }

            var nuevaCita = new Cita
            {
                IdUsuario = _usuarioActual!.Id,
                IdMedico = idMedico,
                FechaCita = fechaCita,
                Motivo = motivo,
                Confirmada = false,
                NombrePaciente = _usuarioActual.Nombre,
                NombreMedico = medico.Nombre
            };

            _citaService.AgregarCita(nuevaCita);
            Console.WriteLine("¡Cita registrada con éxito!");
        }

        private void BorrarCita()
        {
            
            Console.WriteLine("=== Borrar Cita ===");

            var citas = _citaService.ObtenerCitasPorUsuario(_usuarioActual!.Id);
            if (!citas.Any())
            {
                Console.WriteLine("No tienes citas para borrar.");
                return;
            }

            Console.WriteLine("Tus citas:");
            foreach (var cita in citas)
            {
                var medico = _medicoService.ObtenerMedicoPorId(cita.IdMedico);
                Console.WriteLine($"ID: {cita.Id}, Médico: {medico?.Nombre ?? "Desconocido"}, Fecha: {cita.FechaCita.ToString("dd/MM/yyyy HH:mm")}");
                Console.WriteLine($"   Motivo: {cita.Motivo}");
                Console.WriteLine();
            }

            Console.Write("\nIntroduce el ID de la cita a borrar: ");
            if (!int.TryParse(Console.ReadLine(), out int idCita))
            {
                MostrarError("ID de cita inválido.");
                return;
            }

            var citaABorrar = citas.FirstOrDefault(c => c.Id == idCita);
            if (citaABorrar == null)
            {
                MostrarError("Cita no encontrada o no te pertenece.");
                return;
            }

            if (_citaService.EliminarCita(idCita))
            {
                Console.WriteLine("Cita eliminada con éxito.");
            }
            else
            {
                MostrarError("No se pudo eliminar la cita.");
            }
        }
        
        private void MostrarError(string mensaje)
        {
            Console.WriteLine($"Error: {mensaje}");
            PausarPantalla();
        }
        
        private void PausarPantalla()
        {
            Console.WriteLine("\nPulsa cualquier tecla para continuar...");
            
        }
    }
}