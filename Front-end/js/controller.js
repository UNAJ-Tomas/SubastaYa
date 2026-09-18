import { SubastaModel } from './model.js';

import { SubastaView } from './view.js';



//const USUARIO_ACTUAL_ID = 1; // Simulamos el usuario logueado

const USUARIO_ACTUAL_ID = sessionStorage.getItem('usuarioId') || 1;
let subastasActuales = []; // Variable genérica para almacenar las subastas de la vista actual



document.addEventListener('DOMContentLoaded', async () => {




    try {

        const billetera = await SubastaModel.obtenerBilleteraUsuario(USUARIO_ACTUAL_ID);

        if (billetera) {

            SubastaView.renderizarUsuario(USUARIO_ACTUAL_ID, billetera.saldoDisponible);

        }

    } catch (e) {


    }



    //vista: DEPOSITAR.HTML

    if (document.getElementById('inputMonto')) {

        await inicializarVistaDepositar(USUARIO_ACTUAL_ID);

        return;

    }



    //vista: MISUBASTAS.HTML

    if (document.getElementById('contenedorSubastas') && window.location.pathname.includes('miSubastas.html')) {

        await inicializarVistaMisSubastas();

        configurarFiltrosYEventos();

        return;

    }



 /*    //vista: TRANSACCIONES.HTML (Historial)

    if (document.getElementById('contenedorHistorial') && window.location.pathname.includes('transacciones.html')) {

        await inicializarVistaHistorial();

        return;

    } */



    //vista: CREAR SUBASTA

    const formCrearSubasta = document.getElementById('formCrearSubasta');

    if (formCrearSubasta) {

        inicializarVistaCrearSubasta();

        return;

    }



    // Vistas generales / HOME.HTML

    await cargarDatosHome();

    configurarFiltrosYEventos();



    const btnActualizar = document.getElementById('btnActualizar');

    if (btnActualizar) {

        btnActualizar.addEventListener('click', async () => {

            await cargarDatosHome();

            aplicarFiltros();

        });

    }

});

function configurarFiltrosYEventos() {

    document.getElementById('btnCerrarSesion')?.addEventListener('click', () => {
    sessionStorage.clear();
    window.location.href = '../login.html';
});

    const inputBuscar = document.getElementById('inputBuscar');

    const selectFiltroEstado = document.getElementById('selectFiltroEstado');

    const btnLimpiarFiltros = document.getElementById('btnLimpiarFiltros');

    if (inputBuscar) {

        inputBuscar.removeEventListener('input', aplicarFiltros); // Evitar duplicados

        inputBuscar.addEventListener('input', aplicarFiltros);

    }

    if (selectFiltroEstado) {

        selectFiltroEstado.removeEventListener('change', aplicarFiltros);

        selectFiltroEstado.addEventListener('change', aplicarFiltros);

    }

    if (btnLimpiarFiltros) {
        btnLimpiarFiltros.replaceWith(btnLimpiarFiltros.cloneNode(true)); 
        const nuevoBtnLimpiar = document.getElementById('btnLimpiarFiltros');
        nuevoBtnLimpiar.addEventListener('click', () => {
            if (inputBuscar) inputBuscar.value = '';
            if (selectFiltroEstado) selectFiltroEstado.value = 'todos';
            SubastaView.renderizarSubastas(subastasActuales, manejarIntentoPuja);
        });
    }
}


// ==========================================

// FUNCIONES DE APOYO - HOME

// ==========================================

async function cargarDatosHome() {

    subastasActuales = await SubastaModel.obtenerSubastasActivas();

    SubastaView.renderizarSubastas(subastasActuales, manejarIntentoPuja);

}



// ==========================================

// FUNCIÓN DE FILTRADO GENERAL

// ==========================================
function aplicarFiltros() {
    const inputBuscar = document.getElementById('inputBuscar');
    const selectFiltroEstado = document.getElementById('selectFiltroEstado');

    const texto = inputBuscar
        ? inputBuscar.value.toLowerCase()
        : '';

    const valorSeleccionado = selectFiltroEstado
        ? selectFiltroEstado.value
        : 'todos';

    const subastasFiltradas = subastasActuales.filter(s => {

        // Filtro por texto
        const coincideTexto =
            s.titulo.toLowerCase().includes(texto) ||
            (s.descripcion &&
             s.descripcion.toLowerCase().includes(texto));

        // La API devuelve el ID como "categoria_id"
        const idCategoria = s.categoria_id;

        // Filtro por categoría
        const coincideCategoria =
            valorSeleccionado === 'todos' ||
            String(idCategoria) === String(valorSeleccionado);

        console.log({
            titulo: s.titulo,
            idCategoria: idCategoria,
            valorSeleccionado: valorSeleccionado,
            coincideCategoria: coincideCategoria
        });

        return coincideTexto && coincideCategoria;
    });

    console.log("SUBASTAS ORIGINALES:", subastasActuales);
    console.log("SUBASTAS FILTRADAS:", subastasFiltradas);
    console.log("CANTIDAD ORIGINAL:", subastasActuales.length);
    console.log("CANTIDAD FILTRADA:", subastasFiltradas.length);

    SubastaView.renderizarSubastas(
        subastasFiltradas,
        manejarIntentoPuja
    );
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

        try {
            const billeteraActualizada = await SubastaModel.obtenerBilleteraUsuario(USUARIO_ACTUAL_ID);
            if (billeteraActualizada) {
                SubastaView.renderizarUsuario(USUARIO_ACTUAL_ID, billeteraActualizada.saldoDisponible);
            }
        } catch (e) {
            console.error('No se pudo actualizar el saldo', e);
        }

        if (window.location.pathname.includes('miSubastas.html')) {
            await inicializarVistaMisSubastas();
            aplicarFiltros();
        } else {
            await cargarDatosHome();
            aplicarFiltros();
        }
    } else {
        SubastaView.mostrarMensaje(`Error: ${resultado.mensaje}`);
    }
}



// ==========================================

// FUNCIÓN PARA GESTIONAR LA VISTA DE DEPÓSITO

// ==========================================


/* import { initDepositoView } from './view.js';
//import { obtenerSaldoUsuario } from './model.js'; 

document.addEventListener('DOMContentLoaded', async () => {
    const saldoInicial = await SubastaModel.obtenerBilleteraUsuario(USUARIO_ACTUAL_ID);
    initDepositoView(saldoInicial.saldoDisponible);
}); */


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
    subastasActuales = await SubastaModel.obtenerSubastasParticipadas(USUARIO_ACTUAL_ID);

    if (!subastasActuales || subastasActuales.length === 0) {
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

    SubastaView.renderizarSubastas(subastasActuales, manejarIntentoPuja);
}



function inicializarVistaCrearSubasta() {
    const form = document.getElementById('formCrearSubasta');

    form.addEventListener('submit', async (e) => {
        e.preventDefault();

        const inputArchivo = document.getElementById('imagenFile');
        const archivo = inputArchivo.files[0];

        if (!archivo) {
            alert('Por favor, selecciona una imagen para la subasta.');
            return;
        }

        // Función para convertir el archivo a Base64
        const convertirABase64 = (file) => {
            return new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => resolve(reader.result);
                reader.onerror = error => reject(error);
            });
        };

        try {
            // Convertimos la imagen elegida a Base64
            const imagenBase64 = await convertirABase64(archivo);

            const nuevaSubasta = {
                vendedorId: USUARIO_ACTUAL_ID,
                categoriaId: parseInt(document.getElementById('categoriaId').value),
                titulo: document.getElementById('titulo').value,
                descripcion: document.getElementById('descripcion').value,
                urlImagen: imagenBase64, 
                precioBase: parseFloat(document.getElementById('precioInicial').value),
                incrementoMinimo: 100,
                fechaInicio: new Date().toISOString(),
                fechaFin: new Date(document.getElementById('fechaFin').value).toISOString()
            };

            const resultado = await SubastaModel.crearSubasta(nuevaSubasta);

            if (resultado.exito) {
                alert('¡Subasta creada con éxito!');
                window.location.href = 'home.html';
            } else {
                alert(`Error al crear la subasta: ${resultado.mensaje}`);
            }
        } catch (error) {
            console.error('Error al procesar la imagen:', error);
            alert('Hubo un error al procesar la imagen seleccionada.');
        }
    });
}