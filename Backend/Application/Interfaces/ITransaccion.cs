namespace Application.Interfaces
{

    //siempre se tiene que heredad de Disposable una interfaz nativa de .NET
    //que indica que un objeto maneja recursos no administrdos como conexciones a base de datos
    //y necesitra liberarlos explicitamente cuando termine de usarse
    public interface ITransaccion :IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}