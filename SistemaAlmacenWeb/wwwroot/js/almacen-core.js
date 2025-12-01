/* MOTOR DE GESTIÓN VISUAL v2.0 (Con Buscador) */
class DataManager {
    constructor(apiUrl) {
        this.apiUrl = apiUrl;
        this.cachedData = []; // Aquí guardaremos los datos para no recargar
    }

    // Carga datos del servidor y los guarda en memoria
    async loadTable(tableId, columns) {
        this.tableId = tableId;
        this.columns = columns; // Guardamos config de columnas
        const tableBody = document.querySelector(`#${tableId} tbody`);

        tableBody.innerHTML = '<tr><td colspan="100%" class="text-center p-3">Cargando... <i class="fas fa-spinner fa-spin"></i></td></tr>';

        try {
            const response = await fetch(this.apiUrl);
            if (!response.ok) throw new Error('Error de red');

            // Guardamos los datos en la variable de la clase
            this.cachedData = await response.json();

            // Renderizamos todo
            this.renderData(this.cachedData);

        } catch (error) {
            console.error(error);
            tableBody.innerHTML = '<tr><td colspan="100%" class="text-center text-danger">Error al cargar datos.</td></tr>';
        }
    }

    // Función interna para pintar la tabla
    renderData(dataToRender) {
        const tableBody = document.querySelector(`#${this.tableId} tbody`);
        tableBody.innerHTML = '';

        if (dataToRender.length === 0) {
            tableBody.innerHTML = '<tr><td colspan="100%" class="text-center text-muted p-3">No se encontraron coincidencias.</td></tr>';
            return;
        }

        dataToRender.forEach(item => {
            let row = '<tr>';
            this.columns.forEach(col => {
                let cellData = col.render ? col.render(item) : item[col.data];
                row += `<td>${cellData !== null && cellData !== undefined ? cellData : ''}</td>`;
            });
            row += '</tr>';
            tableBody.innerHTML += row;
        });
    }

    // NUEVA FUNCIÓN: Filtrar datos en memoria
    search(term) {
        if (!term) {
            this.renderData(this.cachedData); // Si está vacío, mostrar todo
            return;
        }

        term = term.toLowerCase();

        const filtered = this.cachedData.filter(item => {
            // Buscamos en las propiedades clave
            return (item.codigo && item.codigo.toLowerCase().includes(term)) ||
                (item.codigoBarras && item.codigoBarras.toLowerCase().includes(term)) ||
                (item.descripcion && item.descripcion.toLowerCase().includes(term)) ||
                (item.marca && item.marca.toLowerCase().includes(term));
        });

        this.renderData(filtered);
    }

    // Borrado (igual que antes)
    async deleteItem(id) {
        const result = await Swal.fire({
            title: '¿Eliminar registro?',
            text: "Esta acción no se puede deshacer",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#9f111b',
            cancelButtonColor: '#292c37',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar',
            background: '#292c37',
            color: '#cccccc'
        });

        if (result.isConfirmed) {
            try {
                // Ajustar URL base si es necesario
                const urlBorrado = this.apiUrl.replace('GetJson', 'DeleteConfirmedApi') + '/' + id;
                // O usar la URL directa si se prefiere configurar aparte
                const response = await fetch('/Articulos/DeleteConfirmedApi/' + id, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' }
                });

                if (response.ok) {
                    Swal.fire({
                        title: '¡Eliminado!',
                        icon: 'success',
                        background: '#292c37',
                        color: '#cccccc',
                        confirmButtonColor: '#9f111b'
                    }).then(() => window.location.reload());
                } else {
                    Swal.fire('Error', 'No se pudo eliminar.', 'error');
                }
            } catch (error) {
                console.error(error);
                Swal.fire('Error', 'Error de conexión.', 'error');
            }
        }
    }
}