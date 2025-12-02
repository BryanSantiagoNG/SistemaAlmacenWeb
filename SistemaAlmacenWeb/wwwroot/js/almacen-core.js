class DataManager {
    constructor(apiUrl) {
        this.apiUrl = apiUrl;
        this.cachedData = [];
    }

    async loadTable(tableId, columns) {
        this.tableId = tableId;
        this.columns = columns; 
        const tableBody = document.querySelector(`#${tableId} tbody`);

        tableBody.innerHTML = '<tr><td colspan="100%" class="text-center p-3">Cargando... <i class="fas fa-spinner fa-spin"></i></td></tr>';

        try {
            const response = await fetch(this.apiUrl);
            if (!response.ok) throw new Error('Error de red');

            this.cachedData = await response.json();

            this.renderData(this.cachedData);

        } catch (error) {
            console.error(error);
            tableBody.innerHTML = '<tr><td colspan="100%" class="text-center text-danger">Error al cargar datos.</td></tr>';
        }
    }

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

    search(term) {
        if (!term) {
            this.renderData(this.cachedData); 
            return;
        }

        term = term.toLowerCase();

        const filtered = this.cachedData.filter(item => {
            return (item.codigo && item.codigo.toLowerCase().includes(term)) ||
                (item.codigoBarras && item.codigoBarras.toLowerCase().includes(term)) ||
                (item.descripcion && item.descripcion.toLowerCase().includes(term)) ||
                (item.marca && item.marca.toLowerCase().includes(term));
        });

        this.renderData(filtered);
    }

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
                const urlBorrado = this.apiUrl.replace('GetJson', 'DeleteConfirmedApi') + '/' + id;
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