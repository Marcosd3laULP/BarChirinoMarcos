new Vue({
    el: '#app',
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
            axios.get('/Pedido/ListarPedidosCliente', {
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
            axios.post('/Pedido/CambiarEstado', { idPedido: id })
                .then(() => this.cargar())
        }
    },
    mounted() {
        this.cargar()
    }
})
