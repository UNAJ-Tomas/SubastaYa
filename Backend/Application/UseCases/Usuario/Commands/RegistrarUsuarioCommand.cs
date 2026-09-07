namespace Application.UseCases.Usuario.Commands
{
    public record RegistrarUsuarioCommand(
        string Nombre,
        string Email,
        string Password
    );
}