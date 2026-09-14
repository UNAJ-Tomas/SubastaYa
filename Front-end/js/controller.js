import { SubastaModel } from './model.js';
import { SubastaView } from './view.js';

const USUARIO_ACTUAL_ID = 1; // Simulamos el usuario logueado
let subastasGlobales = [];

document.addEventListener('DOMContentLoaded', async () => {


    // 1. Detección de vista: DEPOSITAR.HTML
    if (document.getElementById('inputMonto')) {
        await inicializarVistaDepositar();
        return; 
    }

    // 2. Detección de vista: MISUBASTAS.HTML
    if (document.getElementById('contenedorSubastas') && window.location.pathname.includes('miSubastas.html')) {
        await inicializarVistaMisSubastas();
        return;
    }

    // 3. Detección de vista: CREAR SUBASTA (Nuevo)
    const formCrearSubasta = document.getElementById('formCrearSubasta');
    if (formCrearSubasta) {
        inicializarVistaCrearSubasta();
        return;
    }

    // Vistas generales / HOME.HTML
    await cargarDatos();

    const btnActualizar = document.getElementById('btnActualizar');
    if (btnActualizar) {
        btnActualizar.addEventListener('click', async () => {
            await cargarDatos();
        });
    }

    // Buscador de subastas por texto
    const inputBuscar = document.getElementById('inputBuscar');
    if (inputBuscar) {
        inputBuscar.addEventListener('input', aplicarFiltros);
    }

    // Selector de categoría
    const selectFiltroEstado = document.getElementById('selectFiltroEstado');
    if (selectFiltroEstado) {
        selectFiltroEstado.addEventListener('change', aplicarFiltros);
    }

    // Botón limpiar filtros
    const btnLimpiarFiltros = document.getElementById('btnLimpiarFiltros');
    if (btnLimpiarFiltros) {
        btnLimpiarFiltros.addEventListener('click', () => {
            if (inputBuscar) inputBuscar.value = '';
            if (selectFiltroEstado) selectFiltroEstado.value = 'todos';
            SubastaView.renderizarSubastas(subastasGlobales, manejarIntentoPuja);
        });
    }
});

// ==========================================
// FUNCIONES DE APOYO - HOME
// ==========================================
async function cargarDatos() {
    const billetera = await SubastaModel.obtenerBilleteraUsuario(USUARIO_ACTUAL_ID);
    if (billetera) {
        SubastaView.renderizarUsuario(USUARIO_ACTUAL_ID, billetera.saldoDisponible);
    }

    subastasGlobales = await SubastaModel.obtenerSubastasActivas();
    SubastaView.renderizarSubastas(subastasGlobales, manejarIntentoPuja);
}

function aplicarFiltros() {
    const inputBuscar = document.getElementById('inputBuscar');
    const selectFiltroEstado = document.getElementById('selectFiltroEstado');

    const texto = inputBuscar ? inputBuscar.value.toLowerCase() : '';
    const categoriaSeleccionada = selectFiltroEstado ? selectFiltroEstado.value : 'todos';

    const subastasFiltradas = subastasGlobales.filter(s => {
        const coincideTexto = s.titulo.toLowerCase().includes(texto) || 
                            (s.descripcion && s.descripcion.toLowerCase().includes(texto));
        
        const categoriaSubasta = (s.categoria || s.categoriaNombre || '').toLowerCase();
        const coincideCategoria = categoriaSeleccionada === 'todos' || 
                                categoriaSubasta === categoriaSeleccionada || 
                                s.categoriaId == categoriaSeleccionada;

        return coincideTexto && coincideCategoria;
    });

    SubastaView.renderizarSubastas(subastasFiltradas, manejarIntentoPuja);
}

async function manejarIntentoPuja(subastaId, montoMinimoRequerido) {
    const montoStr = prompt(`Ingrese el monto de su oferta (Mínimo requerido: $${montoMinimoRequerido}):`, montoMinimoRequerido);

    if (!montoStr) return;

    const monto = parseFloat(montoStr);
    if (isNaN(monto) || monto < montoMinimoRequerido) {
        SubastaView.mostrarMensaje(`El monto ingresado no es válido o es menor al mínimo requerido ($${montoMinimoRequerido}).`);
        return;
    }

    const resultado = await SubastaModel.registrarPuja(subastaId, USUARIO_ACTUAL_ID, monto);

    if (resultado.exito) {
        SubastaView.mostrarMensaje('¡Puja registrada con éxito!');
        // Si estamos en la vista de mis subastas, recargamos esa misma vista
        if (window.location.pathname.includes('Subastas.html')) {
            await inicializarVistaMisSubastas();
        } else {
            await cargarDatos();
        }
    } else {
        SubastaView.mostrarMensaje(`Error: ${resultado.mensaje}`);
    }
}

// ==========================================
// FUNCIÓN PARA GESTIONAR LA VISTA DE DEPÓSITO
// ==========================================
async function inicializarVistaDepositar() {
    const billetera = await SubastaModel.obtenerBilleteraUsuario(USUARIO_ACTUAL_ID);
    let saldoActual = billetera ? billetera.saldoDisponible : 0;

    const lblSaldoActual = document.getElementById('lblSaldoActual');
    const resumenSaldoActual = document.getElementById('resumenSaldoActual');
    const inputMonto = document.getElementById('inputMonto');
    const resumenDeposito = document.getElementById('resumenDeposito');
    const resumenTotalFinal = document.getElementById('resumenTotalFinal');
    const botonesMonto = document.querySelectorAll('.btn-monto');
    const btnConfirmarDeposito = document.getElementById('btnConfirmarDeposito');

    if (lblSaldoActual) lblSaldoActual.textContent = `$${saldoActual.toLocaleString()}`;
    if (resumenSaldoActual) resumenSaldoActual.textContent = `$${saldoActual.toLocaleString()}`;

    function actualizarResumen() {
        let montoIngresado = parseFloat(inputMonto.value) || 0;
        if (resumenDeposito) resumenDeposito.textContent = `$${montoIngresado.toLocaleString()}`;
        if (resumenTotalFinal) resumenTotalFinal.textContent = `$${(saldoActual + montoIngresado).toLocaleString()}`;
    }

    if (inputMonto) {
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

    if (btnConfirmarDeposito) {
        btnConfirmarDeposito.addEventListener('click', async () => {
            const montoIngresado = parseFloat(inputMonto.value);
            const metodoPago = document.getElementById('selectMetodoPago').value;

            if (isNaN(montoIngresado) || montoIngresado <= 0) {
                alert('Por favor, ingresa un monto válido a depositar.');
                return;
            }

            if (!metodoPago) {
                alert('Por favor, seleccioná un método de pago.');
                return;
            }

            const resultado = await SubastaModel.depositar(USUARIO_ACTUAL_ID, montoIngresado, metodoPago);

            if (resultado.exito) {
                alert(`¡Depósito de $${montoIngresado.toLocaleString()} realizado con éxito!`);
                window.location.href = 'home.html';
            } else {
                alert(`Error al depositar: ${resultado.mensaje}`);
            }
        });
    }
}

// ==========================================
// FUNCIÓN PARA GESTIONAR "MIS SUBASTAS"
// ==========================================
async function inicializarVistaMisSubastas() {
    const subastasParticipadas = await SubastaModel.obtenerSubastasParticipadas(USUARIO_ACTUAL_ID);
    
    if (!subastasParticipadas || subastasParticipadas.length === 0) {
        const contenedor = document.getElementById('contenedorSubastas');
        if (contenedor) {
            contenedor.innerHTML = `
                <div class="col-12 text-center py-5">
                    <p class="text-muted fs-5">Aún no has participado en ninguna subasta.</p>
                    <a href="home.html" class="btn btn-success rounded-pill px-4">Ver subastas activas</a>
                </div>`;
        }
        return;
    }

    SubastaView.renderizarSubastas(subastasParticipadas, manejarIntentoPuja);
}

function inicializarVistaCrearSubasta() {
    const form = document.getElementById('formCrearSubasta');

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const nuevaSubasta = {
            vendedorId: USUARIO_ACTUAL_ID,
            categoriaId: parseInt(document.getElementById('categoriaId').value),
            titulo: document.getElementById('titulo').value,
            descripcion: document.getElementById('descripcion').value,
            urlImagen: document.getElementById('urlImagen').value,
            precioBase: parseFloat(document.getElementById('precioInicial').value), // Mapeado desde precioInicial del form
            incrementoMinimo: 100, // Valor por defecto si no tienes el input en el HTML
            fechaInicio: new Date().toISOString(),
            fechaFin: document.getElementById('fechaFin').value
        };

        const resultado = await SubastaModel.crearSubasta(nuevaSubasta);

        if (resultado.exito) {
            alert('¡Subasta creada con éxito!');
            window.location.href = 'home.html';
        } else {
            alert(`Error al crear la subasta: ${resultado.mensaje}`);
        }
    });
}