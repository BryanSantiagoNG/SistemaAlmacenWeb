const managerArticulos = new DataManager('/Articulos/GetJson');

const columnasArticulos = [
    {
        data: 'codigo',
        render: (i) => {
            let html = `<div class="d-flex flex-column align-items-start">`;
            html += `<span class="badge" style="background-color: #444;">${i.codigo || 'S/C'}</span>`;
            if (i.codigoBarras) {
                html += `<small class="text-muted mt-1" style="font-size: 0.8rem;">
                            <i class="fas fa-barcode"></i> ${i.codigoBarras}
                         </small>`;
            }
            html += `</div>`;
            return html;
        }
    },
    { data: 'descripcion', render: (i) => `<strong>${i.descripcion}</strong>` },
    { data: 'marca' },
    {
        data: 'stock',
        render: (i) => {
            let color = i.stock <= 5 ? 'danger' : 'success';
            return `<span class="badge bg-${color} p-2">${i.stock}</span>`;
        }
    },
    { data: 'precioVenta', render: (i) => `<span class="fw-bold text-success">$${i.precioVenta.toFixed(2)}</span>` },
    { data: 'proveedor' },
    {
        render: (i) => `
            <div class="text-end">
                <div class="btn-group shadow-sm" role="group">
                    <a href="/Articulos/Edit/${i.idArticulo}" class="btn btn-sm btn-light text-warning" title="Editar">
                        <i class="fas fa-pen"></i>
                    </a>
                    <a href="/Articulos/Details/${i.idArticulo}" class="btn btn-sm btn-light text-info" title="Ver Detalles">
                        <i class="fas fa-eye"></i>
                    </a>
                    <button onclick="borrarArticulo(${i.idArticulo})" class="btn btn-sm btn-light text-danger" title="Eliminar">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </div>
        `
    }
];

function initArticulos() {
    managerArticulos.loadTable('tablaArticulos', columnasArticulos);

    const inputBuscar = document.getElementById('txtBuscar');
    if (inputBuscar) {
        inputBuscar.addEventListener('keyup', (e) => {
            managerArticulos.search(e.target.value);
        });
    }
}

function borrarArticulo(id) {
    const deleteManager = new DataManager('/Articulos/DeleteConfirmedApi');
    deleteManager.deleteItem(id);
}