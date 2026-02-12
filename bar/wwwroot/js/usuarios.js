new Vue({
    el: '#app-usuarios',
    data: {
        usuarios: [],
        page: 1,
        pageSize: 6,
        total: 0,
        filtroNick: '',
        filtroEmail: '',
        filtroRoles: []
    },
    methods: {
        cargar() {
    axios.get('/Admin/GetUsuarios', {
        params: {
            page: this.page,
            pageSize: this.pageSize,
            nick: this.filtroNick,
            email: this.filtroEmail,
            roles: this.filtroRoles
        }
    })
    .then(r => {
        console.log("Respuesta:", r.data);
        this.usuarios = r.data.data;
        this.total = r.data.total;
    });
},

        testCambio() {
        console.log("Cambio detectado");
    },
        buscar() {
            this.page = 1
            this.cargar()
        },
        irAPagina(n) {
            this.page = n
            this.cargar()
        },
        nextPage() {
            if (this.page < this.totalPages) {
                this.page++
                this.cargar()
            }
        },
        prevPage() {
            if (this.page > 1) {
                this.page--
                this.cargar()
            }
        },
        eliminar(id) {
            axios.post('/Admin/BajaUsuario', null, 
                { params: { idUsuario: id}
            })
                .then(() => this.cargar())
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
