const API_URL = 'https://localhost:7083/api';

// Función auxiliar para obtener los headers con el token JWT guardado
function getAuthHeaders() {
    const token = sessionStorage.getItem('tokenJWT');
    const headers = {
        'Content-Type': 'application/json'
    };
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }
    return headers;
}

export const SubastaModel = {
    async obtenerSubastasActivas() {
        try {
            const response = await fetch(`${API_URL}/Subastas`, {
                headers: getAuthHeaders()
            });
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
                headers: getAuthHeaders(),
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
            const response = await fetch(`${API_URL}/billeteras/usuario/${usuarioId}`, {
                headers: getAuthHeaders()
            });
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
                headers: getAuthHeaders(),
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
            const response = await fetch(`${API_URL}/subastas/usuario/${usuarioId}/participadas`, {
                headers: getAuthHeaders()
            });
            if (!response.ok) throw new Error('Error al cargar tus subastas');
            return await response.json();
        } catch (error) {
            console.error('Error en Model (Subastas Participadas):', error);
            return [];
        }
    },

    async crearSubasta(datosSubasta) {
        try {
            const response = await fetch(`${API_URL}/Subastas`, {
                method: 'POST',
                headers: getAuthHeaders(),
                body: JSON.stringify(datosSubasta)
            });

            if (!response.ok) {
                let errorMessage = 'Error al crear la subasta';
                try {
                    const errorData = await response.json();
                    errorMessage = errorData.message || errorData.mensaje || errorMessage;
                } catch (e) {}
                throw new Error(errorMessage);
            }
            
            // Verificamos si la respuesta tiene contenido JSON antes de parsearla
            const contentType = response.headers.get("content-type");
            let data = null;
            if (contentType && contentType.includes("application/json")) {
                data = await response.json();
            }

            return { exito: true, data };
        } catch (error) {
            return { exito: false, mensaje: error.message };
        }
    }
};