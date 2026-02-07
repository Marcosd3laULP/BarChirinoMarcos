new Vue({
    el: "#app",
    data: {
        platos: [],
        platoAEliminar: null,
        page: 1,
        pageSize: 5,
        total: 0,

        filtroNombre: "",
        filtroIngredientes: "",
        filtroCostoMax: ""
    },
    methods: {
        cargarPlatos() {
            axios.get('/Plato/GetPlatos', {
                params: {
                    page: this.page,
                    pageSize: this.pageSize,
                    nombre: this.filtroNombre,
                    ingredientes: this.filtroIngredientes,
                    costoMax: this.filtroCostoMax
                }
            })
            .then(r => {
                this.platos = r.data.data;
                this.total = r.data.total;
            });
        },

        buscar(){
            this.page = 1;
            this.cargarPlatos();
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

        pedirConfirmacion(plato){
            this.platoAEliminar = plato.idPlato;
        },


        cancelarBaja(){
            this.platoAEliminar = null;
        },


        confirmarBaja(plato) {
    fetch(`/Plato/BajaAjax/${plato.idPlato}`, {
        method: 'DELETE'
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al eliminar');
        }

        this.platos = this.platos.filter(p => p.idPlato !== plato.idPlato);
        this.platoAEliminar = null;
       
    })
    .catch(() => {
        alert('No se pudo eliminar el plato');
    });
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
