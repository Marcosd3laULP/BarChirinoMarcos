new Vue({
    el: '#app-list',
    data: {
        pedidos: [],
        page: 1,
        pageSize: 5,
        total: 0,
        desde: '',
        hasta: ''
    },
    methods: {
        cargar() {
            axios.get(urlListado, {
                params: {
                    page: this.page,
                    desde: this.desde,
                    hasta: this.hasta
                }
            }).then(r => {
                this.pedidos = r.data.pedidos
                this.total = r.data.total
            })
        },
        buscar() {
            this.page = 1
            this.cargar()
        },
        next() {
            if (this.page * this.pageSize < this.total) {
                this.page++
                this.cargar()
            }
        },
        prev() {
            if (this.page > 1) {
                this.page--
                this.cargar()
            }
        },
        completar(id) {
            axios.post('/Pedido/CambiarEstado', null, 
                { params: { idPedido: id}
            })
                .then(() => this.cargar())
        },

        formatearFecha(fecha) {
        if (!fecha) return '';

        const d = new Date(fecha);
        return d.toLocaleDateString('es-AR');
    }
    },
    mounted() {
        this.cargar()
    },
    computed: {
    totalPages() {
        return Math.ceil(this.total / this.pageSize)
    }
}

})
