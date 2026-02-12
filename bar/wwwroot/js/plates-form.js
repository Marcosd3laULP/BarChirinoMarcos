new Vue({
    el:"#platoForm",
    data: {
        nombre: "",
        ingredientes:"",
        costo: null,
        imagenPreview: null,
        error: ""
    },


    methods: {
        previewImagen(e) {
            const file = e.target.files[0];
            if(!file) return;

            const reader = new FileReader();
            reader.onload = ev => {
                this.imagenPreview = ev.target.result;
            };
            reader.readAsDataURL(file);
        },
        validarFormulario(e) {
            this.error = "";

            if(!this.nombre) {
                this.error = "El nombre es obligatorio.";
                e.preventDefault();
                return;
            }

            if(!this.costo || this.costo <= 0){
                this.error = "El costo debe ser mayor a 0.";
                e.preventDefault();
                return;
            }
        }
    }
});