new Vue({
    el: "#platoEdit",
    data: {
        imagenActual: document.querySelector("input[name='Imagen']")?.value || "",
        imagenPreview: null,
        ingredientes: document.querySelector("input[name='Ingredientes']")?.value || ""
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
        }
    }
});
