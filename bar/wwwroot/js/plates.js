new Vue({
    el: "#app",
    data: {
        platos: [],
        page: 1,
        pageSize: 5,
        total: 0
    },
    methods: {
        cargarPlatos() {
            axios.get('/Plato/GetPlatos', {
                params: {
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
        confirmarBaja(id) {
            if (confirm("¿Eliminar plato?")) {
                window.location.href = `/Plato/Baja/${id}`;
            }
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
