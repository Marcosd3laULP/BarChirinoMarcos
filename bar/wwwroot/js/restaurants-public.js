new Vue({
    el: "#appRestaurantes",
    data: {
        restaurantes: [],
        page: 1,
        pageSize: 6,
        total: 0,

        filtroNombre: "",
        filtroUbicacion: "",
        filtroEspecialidad: ""
    },
    methods: {

        cargarRestaurantes() {
            axios.get('/Cliente/GetRestaurantesPublicos', {
                params: {
                    page: this.page,
                    pageSize: this.pageSize,
                    nombre: this.filtroNombre,
                    ubicacion: this.filtroUbicacion,
                    especialidad: this.filtroEspecialidad
                }
            })
            .then(r => {
                this.restaurantes = r.data.data;
                this.total = r.data.total;
            });
        },

        buscar() {
            this.page = 1;
            this.cargarRestaurantes();
        },

        nextPage() {
            if (this.page * this.pageSize < this.total) {
                this.page++;
                this.cargarRestaurantes();
            }
        },

        prevPage() {
            if (this.page > 1) {
                this.page--;
                this.cargarRestaurantes();
            }
        },

        verRestaurante(idRes) {
            window.location.href = `/Cliente/ExplorarPorRestaurante/${idRes}`;
        }
    },

    mounted() {
        this.cargarRestaurantes();
    },

    computed: {
        totalPages() {
            return Math.ceil(this.total / this.pageSize);
        }
    }
});
