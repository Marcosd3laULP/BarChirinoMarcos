new Vue({
    el: "#appRestaurante",
    data: {
        platos: [],
        page: 1,
        pageSize: 6,
        total: 0
    },
    methods: {
        cargarPlatos() {
            axios.get('/Cliente/GetPlatosPorRestaurante', {
                params: {
                    idRes: ID_RESTAURANTE,
                    page: this.page,
                    pageSize: this.pageSize
                }
            })
            .then(r => {
                this.platos = r.data.data;
                this.total = r.data.total;
            });
        },

        nextPage() {
            if (this.page * this.pageSize < this.total) {
                this.page++;
                this.cargarPlatos();
            }
        },

        prevPage() {
            if (this.page > 1) {
                this.page--;
                this.cargarPlatos();
            }
        },

        verDetalle(idPlato) {
            window.location.href = `/Cliente/Detalle/${idPlato}`;
        }
    },
    mounted() {
        this.cargarPlatos();
    },
    computed: {
        totalPages() {
            return Math.ceil(this.total / this.pageSize);
        }
    }
});
