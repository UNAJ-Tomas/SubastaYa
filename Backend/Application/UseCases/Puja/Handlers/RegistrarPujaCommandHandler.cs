using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Auditoria_log.Commands;
using Application.UseCases.Auditoria_log.Handlers;
using Application.UseCases.Subasta.Commands;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.Puja.Handlers
{
    public class RegistrarPujaCommandHandler
    {
        private readonly ISubastaRepository _subastaRepository;
        private readonly IBilleteraRepository _billeteraRepository;
        private readonly ITransaccionLedgerRepository _ledgerRepository;
        private readonly CrearAuditoria_LogCommandHandler _crearauditoriaCommandHandler;

        public RegistrarPujaCommandHandler(
            ISubastaRepository subastaRepository,
            IBilleteraRepository billeteraRepository,
            ITransaccionLedgerRepository ledgerRepository,
            CrearAuditoria_LogCommandHandler crearauditoriaCommandHandler)
        {
            _subastaRepository = subastaRepository;
            _billeteraRepository = billeteraRepository;
            _ledgerRepository = ledgerRepository;
            _crearauditoriaCommandHandler = crearauditoriaCommandHandler;
        }

        public async Task<bool> Handle(RegistrarPujaCommand command, CancellationToken cancellationToken = default)
        {
            // Inicia la transacción a través de la interfaz del repositorio
            using var transaction = await _subastaRepository.IniciarTransaccionAsync(cancellationToken);

            try
            {
                var subasta = await _subastaRepository.GetByIdAsync(command.SubastaId);

                if (subasta == null)
                    throw new NotFoundException("La subasta no existe.");

                if (subasta.estado != EstadoSubasta.ACTIVA || DateTime.Now > subasta.fecha_fin)
                    throw new ValidationException("La subasta no se encuentra activa para recibir ofertas.");

                // 1. Validar monto mínimo
                var pujaAnterior = subasta.Pujas?.OrderByDescending(p => p.monto).FirstOrDefault();
                var montoMinimoRequerido = pujaAnterior != null
                    ? pujaAnterior.monto + subasta.incremento_minimo
                    : subasta.precio_base;

                if (command.Monto < montoMinimoRequerido)
                    throw new ValidationException($"El monto ofertado debe ser de al menos {montoMinimoRequerido}.");

                // 2. Validar billetera del ofertante
                var billeteraNuevoComprador = await _billeteraRepository.GetByUsuarioIdAsync(command.CompradorId);
                if (billeteraNuevoComprador == null)
                    throw new NotFoundException($"No se encontró la billetera para el usuario {command.CompradorId}.");

                if (billeteraNuevoComprador.saldo_disponible < command.Monto)
                    throw new ValidationException("Saldo insuficiente en la billetera para realizar esta oferta.");

                // 3. Devolución de fondos al postor anterior
                if (pujaAnterior != null)
                {
                    var billeteraAnterior = await _billeteraRepository.GetByUsuarioIdAsync(pujaAnterior.comprador_id);
                    if (billeteraAnterior != null)
                    {
                        billeteraAnterior.saldo_disponible += pujaAnterior.monto;
                        billeteraAnterior.saldo_retenido -= pujaAnterior.monto;
                        await _billeteraRepository.UpdateAsync(billeteraAnterior);

                        await _ledgerRepository.AddAsyc(new Transaccion_Ledger
                        {
                            billetera_id = billeteraAnterior.id,
                            subasta_id = subasta.id,
                            tipo = "DEVOLUCION_PUJA",
                            monto = pujaAnterior.monto,
                            fecha = DateTime.Now
                        });
                    }
                }

                // 4. Retención de fondos al nuevo comprador
                billeteraNuevoComprador.saldo_disponible -= command.Monto;
                billeteraNuevoComprador.saldo_retenido += command.Monto;
                await _billeteraRepository.UpdateAsync(billeteraNuevoComprador);

                await _ledgerRepository.AddAsyc(new Transaccion_Ledger
                {
                    billetera_id = billeteraNuevoComprador.id,
                    subasta_id = subasta.id,
                    tipo = "RETENCION_PUJA",
                    monto = command.Monto,
                    fecha = DateTime.Now
                });

                // 5. Registrar la nueva puja
                var nuevaPuja = new Domain.Entities.Puja
                {
                    subasta_id = command.SubastaId,
                    comprador_id = command.CompradorId,
                    monto = command.Monto,
                    fecha_puja = DateTime.Now
                };

                subasta.Pujas ??= new List<Domain.Entities.Puja>();
                subasta.Pujas.Add(nuevaPuja);

                // 6. Anti-Sniping (Últimos 60s -> Extiende 2 min)
                var tiempoRestante = subasta.fecha_fin - DateTime.Now;
                if (tiempoRestante <= TimeSpan.FromSeconds(60))
                {
                    CrearAuditoria_LogCommand command_auditoria = new CrearAuditoria_LogCommand
                    {
                        Entidad = "Puja",
                        Entidad_id = command.SubastaId,
                        Accion = "PUJA_RECHAZADA",
                        Usuario_id = command.CompradorId,
                    };
                    await _crearauditoriaCommandHandler.HandleAsync(command_auditoria, tiempoRestante.ToString());

                    subasta.fecha_fin = subasta.fecha_fin.AddMinutes(2);
                }

                await _subastaRepository.UpdateAsync(subasta);

                // Confirma la transacción en la base de datos
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (ValidationException ex)
            {
                // En caso de error de validación, revierte los cambios
                await transaction.RollbackAsync(cancellationToken);
                CrearAuditoria_LogCommand command_auditoria = new CrearAuditoria_LogCommand
                {
                    Entidad = "Puja",
                    Entidad_id = command.SubastaId,
                    Accion = "PUJA_RECHAZADA",
                    Usuario_id = command.CompradorId,
                };
                await _crearauditoriaCommandHandler.HandleAsync(command_auditoria, ex.Message);
                //throw new ValidationException(ex.Message);
                throw;
            }
            catch (NotFoundException ex)
            {
                // En caso de error de validación, revierte los cambios
                await transaction.RollbackAsync(cancellationToken);
                CrearAuditoria_LogCommand command_auditoria = new CrearAuditoria_LogCommand
                {
                    Entidad = "Puja",
                    Entidad_id = command.SubastaId,
                    Accion = "PUJA_RECHAZADA",
                    Usuario_id = command.CompradorId,
                };
                await _crearauditoriaCommandHandler.HandleAsync(command_auditoria, ex.Message);
                //throw new ValidationException(ex.Message);
                throw;
            }
            catch (FormatException)
            {
                await transaction.RollbackAsync(cancellationToken);
                CrearAuditoria_LogCommand command_auditoria = new CrearAuditoria_LogCommand
                {
                    Entidad = "Puja",
                    Entidad_id = command.SubastaId,
                    Accion = "PUJA_RECHAZADA",
                    Usuario_id = command.CompradorId,
                };
                await _crearauditoriaCommandHandler.HandleAsync(command_auditoria, "Oferta invalida. No se aceptan letras como oferta.");
                throw;
            }
            catch
            {
                // En caso de error, revierte los cambios
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}