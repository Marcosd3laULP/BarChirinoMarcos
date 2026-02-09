document.addEventListener("DOMContentLoaded", function () {

    const appElement = document.getElementById("pedidoApp");
    if (!appElement) return;

    new Vue({
        el: "#pedidoApp",
        data: {
            idPedido: appElement.querySelector('input[name="IdPedido"]').value,
            idPlato: appElement.querySelector('input[name="IdPlato"]').value,

            idBebida: "",
            subTotal: parseInt(appElement.dataset.precioPlato),

            cargando: false,
            error: null
        },
        watch: {
            idBebida() {
                this.calcularSubtotal();
            }
        },
        methods: {
            calcularSubtotal() {
                this.cargando = true;
                this.error = null;

                axios.post('/Pedido/CalcularSubtotal', {
                    idPedido: this.idPedido,
                    idPlato: this.idPlato,
                    idBebida: this.idBebida || null
                })
                .then(response => {
                    this.subTotal = response.data.subTotal;
                })
                .catch(() => {
                    this.error = "Error al calcular el subtotal";
                })
                .finally(() => {
                    this.cargando = false;
                });
            }
        }
    });

});
