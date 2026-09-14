const API_URL = 'http://localhost:5288/api';

export const SubastaModel = {
    async obtenerSubastasActivas() {
        try {
            const response = await fetch(`${API_URL}/subastas`);
            if (!response.ok) throw new Error('Error al obtener subastas');
            return await response.json();
        } catch (error) {
            console.error('Error en Model (Subastas):', error);
            return [];
        }
    },

    async depositar(usuarioId, monto, metodoPago) {
        try {
            const response = await fetch(`${API_URL}/billeteras/cargar`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ 
                    usuarioId: usuarioId, 
                    monto: monto, 
                    metodoPago: metodoPago 
                })
            });

            if (!response.ok) {
                let errorMessage = 'Error al procesar el depósito';
                try {
                    const errorData = await response.json();
                    errorMessage = errorData.mensaje || errorData.message || errorMessage;
                } catch (e) {}
                throw new Error(errorMessage);
            }

            const data = await response.json();
            return { exito: true, data };

        } catch (error) {
            return { exito: false, mensaje: error.message };
        }
    },

    async obtenerBilleteraUsuario(usuarioId) {
        try {
            const response = await fetch(`${API_URL}/billeteras/usuario/${usuarioId}`);
            if (!response.ok) throw new Error('Error al obtener billetera');
            return await response.json();
        } catch (error) {
            console.error('Error en Model (Billetera):', error);
            return null;
        }
    },

    async registrarPuja(subastaId, compradorId, monto) {
        try {
            const response = await fetch(`${API_URL}/subastas/${subastaId}/pujas`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ subastaId, compradorId, monto })
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Error al procesar la puja');
            }
            return { exito: true };
        } catch (error) {
            return { exito: false, mensaje: error.message };
        }
    },

   async obtenerSubastasParticipadas(usuarioId) {
    try {
        const response = await fetch(`${API_URL}/subastas/usuario/${usuarioId}/participadas`);
        if (!response.ok) throw new Error('Error al cargar tus subastas');
        return await response.json();
    } catch (error) {
        console.error('Error en Model (Subastas Participadas):', error);
        return [];
    }
    },

    async crearSubasta(datosSubasta) {
        try {
            const response = await fetch(`${API_URL}/subastas`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(datosSubasta)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Error al crear la subasta');
            }
            
            const data = await response.json();
            return { exito: true, data };
        } catch (error) {
            return { exito: false, mensaje: error.message };
        }
    }
};