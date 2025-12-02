const managerPedidos = new DataManager('/Pedidos/GetJson');

const columnasPedidos = [
    { data: 'idPedido', render: (i) => `<span class="badge bg-secondary">#${i.idPedido}</span>` },
    { data: 'fecha' },
    { data: 'proveedor', render: (i) => `<strong class="text-light">${i.proveedor}</strong>` },
    { data: 'estado', render: (i) => `<span class="badge bg-success">${i.estado}</span>` },
    { data: 'total', render: (i) => `<span class="fw-bold" style="color: #4ade80;">$${i.total.toFixed(2)}</span>` },
    {
        render: (i) => `
            <div class="text-end">
                <a href="/Pedidos/Details/${i.idPedido}" class="btn btn-sm btn-light text-info" title="Ver Detalles">
                    <i class="fas fa-file-invoice me-1"></i> Detalles
                </a>
            </div>
        `
    }
];

function initPedidos() {
    managerPedidos.loadTable('tablaPedidos', columnasPedidos);
}