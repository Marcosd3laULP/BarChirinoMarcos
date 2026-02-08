new Vue({
    el: "#appClient",
    data: {
        platos: [],
        page: 1,
        pageSize: 6,
        total: 0,

        filtroNombre: "",
        filtroIngredientes: "",
        filtroCostoMax: ""
    },
    methods: {
        cargarPlatos() {

           const params = {
                    page: this.page,
                    pageSize: this.pageSize,
                    nombre: this.filtroNombre,
                    ingredientes: this.filtroIngredientes,

                };
        
            if(this.filtroCostoMax !== "" && this.filtroCostoMax !== null){
                params.costoMax = this.filtroCostoMax;
            }

            axios.get('/Cliente/GetPlatosPublicos', {
                params: params
            })
            .then(r => {
                this.platos = r.data.data;
                this.total = r.data.total;
            });
        },

        buscar() {
            this.page = 1;
            this.cargarPlatos();
        },

        verDetalle(idPlato) {
        window.location.href = `/Cliente/Detalle/${idPlato}`;
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

        verRestaurante(idRes) {
            window.location.href = `/Cliente/Restaurante/${idRes}`;
        },

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
