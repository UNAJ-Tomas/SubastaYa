export const SubastaView = {
    renderizarUsuario(usuarioId, saldo) {
        document.getElementById('lblUsuario').textContent = `Usuario #${usuarioId}`;
        document.getElementById('lblSaldo').textContent = `Disponible: $${saldo}`;
    },

    renderizarSubastas(subastas, onPujarCallback) {
        const contenedor = document.getElementById('contenedorSubastas');
        contenedor.innerHTML = '';

        if (subastas.length === 0) {
            contenedor.innerHTML = `
                <div class="col-12 text-center py-5">
                    <div class="p-5 bg-white rounded-4 shadow-sm border border-light mx-auto" style="max-width: 500px;">
                        <i class="bi bi-inbox fs-1 text-muted"></i>
                        <p class="text-secondary mt-3 mb-0">No hay subastas disponibles en este momento.</p>
                    </div>
                </div>
            `;
            return;
        }

        subastas.forEach(subasta => {
            const pujaMayor = subasta.pujas && subasta.pujas.length > 0
                ? Math.max(...subasta.pujas.map(p => p.monto))
                : subasta.precio_base;

            let liderTexto = 'Sin ofertas aún';
            if (subasta.pujas && subasta.pujas.length > 0) {
                const pujaGanadora = subasta.pujas.find(p => p.monto === pujaMayor);
                if (pujaGanadora && (pujaGanadora.usuarioNombre || pujaGanadora.comprador_id)) {
                    liderTexto = pujaGanadora.usuarioNombre || `Usuario #${pujaGanadora.comprador_id}`;
                }
            }

            // Imagen por defecto si no viene cargada o está vacía
            const imagenSrc = subasta.url_imagen || subasta.urlImagen || 'https://via.placeholder.com/300x200?text=Sin+Imagen';

            const fechaFin = new Date(subasta.fecha_fin);
            const ahora = new Date();
            const esFinalizada = subasta.estado === 'finalizada' || fechaFin < ahora;

            const badgeEstado = esFinalizada
                ? `<span class="badge bg-danger bg-opacity-10 text-danger px-3 py-2 rounded-pill fw-semibold"><i class="bi bi-x-circle-fill me-1"></i> Finalizada</span>`
                : `<span class="badge bg-warning bg-opacity-25 text-dark px-3 py-2 rounded-pill fw-semibold"><i class="bi bi-clock me-1"></i> ${fechaFin.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} hs</span>`;

            // Si está finalizada, podemos deshabilitar el botón de ofertar o cambiar su aspecto
            const botonOfertarHtml = esFinalizada
                ? `<button class="btn btn-secondary w-100 fw-semibold py-2 rounded-pill shadow-sm" disabled>
                       <i class="bi bi-lock-fill me-1"></i> Subasta Finalizada
                   </button>`
                : `<button class="btn btn-success w-100 fw-semibold py-2 rounded-pill btn-pujar shadow-sm" data-id="${subasta.id}" data-minimo="${pujaMayor + subasta.incremento_minimo}">
                       <i class="bi bi-hammer me-1"></i> Ofertar / Pujar
                   </button>`;

            const card = document.createElement('div');
            card.className = 'col-md-6 col-lg-4'; 
            card.innerHTML = `
                <div class="card card-subasta h-100 shadow-sm border-0 overflow-hidden">
                    <!-- Imagen de la subasta -->
                    <img src="${imagenSrc}" class="card-img-top" alt="${subasta.titulo}" style="height: 180px; object-fit: cover;">
                    
                    <div class="card-body d-flex flex-column justify-content-between p-3">
                        <div>
                            <!-- Cabecera de la tarjeta: ID y Vencimiento / Estado -->
                            <div class="d-flex justify-content-between align-items-center mb-3">
                                <span class="badge bg-success bg-opacity-10 text-success px-3 py-2 rounded-pill fw-semibold">
                                    <i class="bi bi-tag-fill me-1"></i> ID #${subasta.id}
                                </span>
                                ${badgeEstado}
                            </div>

                            <!-- Título y Descripción -->
                            <h5 class="card-title fw-bold text-dark text-truncate mb-2">${subasta.titulo}</h5>
                            <p class="card-text text-secondary small mb-3">${subasta.descripcion || 'Sin descripción disponible'}</p>
                        </div>

                        <div>
                            <!-- Bloque de Monto Actual -->
                            <div class="bg-light rounded-3 p-3 mb-2 border border-light">
                                <div class="d-flex justify-content-between align-items-center mb-1">
                                    <span class="text-secondary small fw-medium">Monto actual:</span>
                                    <span class="fw-bold fs-4 text-success">$${pujaMayor}</span>
                                </div>
                                <!-- Indicador de quién va ganando -->
                                <div class="d-flex align-items-center gap-1 text-muted small pt-1 border-top border-200">
                                    <i class="bi bi-trophy-fill text-warning"></i>
                                    <span>Liderando: <strong class="text-dark">${liderTexto}</strong></span>
                                </div>
                            </div>

                            <!-- Botón de Ofertar / Finalizada -->
                            ${botonOfertarHtml}
                        </div>
                    </div>
                </div>
            `;

            if (!esFinalizada) {
                card.querySelector('.btn-pujar').addEventListener('click', () => {
                    onPujarCallback(subasta.id, pujaMayor + subasta.incremento_minimo);
                });
            }

            contenedor.appendChild(card);
        });
    },

    mostrarMensaje(mensaje) {
        alert(mensaje);
    }
};


export function initDepositoView(saldoInicial) {
    const inputMonto = document.getElementById('inputMonto');
    const lblSaldoActual = document.getElementById('lblSaldoActual');
    const resumenSaldoActual = document.getElementById('resumenSaldoActual');
    const resumenDeposito = document.getElementById('resumenDeposito');
    const resumenTotalFinal = document.getElementById('resumenTotalFinal');
    const botonesMonto = document.querySelectorAll('.btn-monto');

    let saldoBase = saldoInicial; 

    function actualizarResumen() {
        if (inputMonto == null) {
            return;
        }
        let valorTexto = inputMonto.value.replace(/\./g, '').replace(',', '.');
        let montoIngresado = parseFloat(inputMonto.value) || 0;
        lblSaldoActual.textContent = `$${saldoBase.toLocaleString()}`;
        resumenSaldoActual.textContent = `$${saldoBase.toLocaleString()}`;
        resumenDeposito.textContent = `$${montoIngresado.toLocaleString()}`;
        resumenTotalFinal.textContent = `$${(saldoBase + montoIngresado).toLocaleString()}`;
    }

    if (inputMonto != null) {
        inputMonto.addEventListener('input', actualizarResumen);
    }

    botonesMonto.forEach(btn => {
        btn.addEventListener('click', () => {
            botonesMonto.forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
            inputMonto.value = btn.getAttribute('data-valor');
            actualizarResumen();
        });
    });

    // Inicializar valores al cargar
    actualizarResumen();
}