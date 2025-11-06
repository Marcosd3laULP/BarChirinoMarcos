# BarChirinoMarcos
proyecto final de laboratorio II

Narrativa del proyecto
Final de laboratorio II

Objetivo general:
Desarrollar una aplicación de comidas, que permita poder registrar usuarios.
Los usuarios podrán seleccionar un plato ya definido en la sección de “comidas” y poder hacer un pedido del plato seleccionado. Confirmando su pedido luego de hecho el pago.
Los usuarios tendrán acceso a modificar datos de su perfil, también podrán añadir una foto de perfil.
Azul: Hay una delgada línea en la que se decide si esto se va a desarrollar o quede afuera de los planes de desarrollo.
La aplicación le otorga al usuario la opción de poder crear su propio plato, con los ingredientes que el desee, pudiendo añadir o no guarniciones, salsas, bebidas o aderezos. Hasta incluso dándole el acceso para que defina cuanto tiempo se debe cocinar su plato. Cada ajuste o adición que haga el usuario, el sistema lo calculará y le dará el costo final de su plato personalizado.
El usuario aparte de poder guardar el plato que creo, podrá también publicarlo en la sección de “platos especiales”, teniendo la posibilidad de añadir imágenes sobre su plato. Los usuarios podrán ver los platos especiales hechos por otros usuarios y hacer pedido del plato y dejar reseñas sobre el mismo.
En el mismo perfil del usuario, aparecen los platos que el haya creado y también los platos que marco como favorito, tanto platos definidos como platos “especiales”.

Objetivos específicos:
•	Crear el login básico de inicio de sesión y registro de nuevo usuario
•	Desarrollar la vista de perfil con todas las opciones de personalización
•	Desarrollar el ABMC de “usuario”, “restaurante” y “platos” (los platos comunes y especiales)
•	Desarrollar el sistema de pedido del plato 
•	Desarrollar la lógica de creación de un plato especial
•	Construir el sistema de publicación de platos y el sistema de comentarios.
•	Desarrollar la lógica de guardar platos y exponerlos en el perfil.

CONCLUSIÓN FINAL:
Para darle a usted profe un resumen de las entidades que van a conformar el proyecto que se va a desarrollar.
Este proyecto contaría con 10 entidades (o tablas) en total:
•	Usuario
•	Restaurante 
•	Plato
•	Pedido
•	Plato especial
•	Ingrediente
•	Composición (una tabla intermedia entre “plato especial” e “Ingrediente”)
•	Bebidas
•	Guarnición
•	Comentarios

Para un mejor desarrollo del proyecto, se intentará seguir las normas del ciclo de vida iterativo. En esta primera iteración se trabajará con los requisitos claros:
•	Crear el login básico de inicio de sesión y registro de nuevo usuario
•	Desarrollar la vista de perfil con todas las opciones de personalización
•	Desarrollar el ABMC de “usuario”, “restaurante” y “platos” (solo platos definidos)
•	Desarrollar el sistema de pedido del plato 


Iteración 1:
Captura de requisitos:
Entidades:
•	Usuario (pudiendo ser usuario común, administrador o un representante de un restaurante)
Casos de usos:
•	Registrarse
•	Definir rol
•	Iniciar sesión
•	Modificar perfil
•	Seleccionar plato
•	Hacer pedido
•	Seleccionar bebida
•	Seleccionar aderezo
•	Pagar pedido
•	Visualizar pedidos

Análisis:
Entidades:
•	Usuario
o	Rol
o	Nombre
o	Apellido
o	Nick
o	Email
o	Contraseña
o	Teléfono
o	Domicilio

•	Plato
o	Nombre
o	Ingredientes
o	Descripción
o	Costo
o	Con guarnición
o	Con bebida
o	Con aderezos
•	Restaurante
o	Nombre
o	Ubicación
o	Especialidad
•	Pedido
o	Usuario
o	Plato
o	Monto
o	Fecha
o	Guarnición
o	Bebida 
o	Aderezo 

•	Bebida
o	Nombre
o	Tipo
o	Costo
•	Aderezo
o	Nombre
o	Tipo
o	Costo
•	Guarnición
o	Nombre 
o	Tipo 
o	Costo 
Diseño en BD:
Entidades:
•	Usuario
o	IdUsuario (PK int) 
o	Nombre (String)
o	Rol (enum: ‘admin’, ‘user’, ‘resto’)
o	Apellido (String)
o	Nick (String)
o	Email (String)
o	Contraseña (String)
o	Teléfono (int)
o	Domicilio (String)

•	Plato
o	IdPlato (PK int)
o	IdRes (FK restaurante)
o	Nombre (String)
o	Ingredientes (String)
o	Descripción (String)
o	Costo (int)
o	IdGuarnición (FK / null) 
o	Con bebida (bool)
o	Con aderezos (bool)
•	Restaurante
o	IdRes (PK int)
o	Nombre (String)
o	Ubicación (String)
o	Especialidad (String)
•	Pedido
o	IdPedido (PK int)
o	Usuario (FK usuario)
o	Plato (FK plato)
o	Monto (int)
o	Fecha (datetime)
o	Guarnición (String /null)
o	Bebida (FK /null)
o	Aderezo (FK /null)
•	Bebida
o	IdBebida (PK int)
o	Nombre (String)
o	Tipo (String)
o	Costo (String)
•	Aderezo
o	IdAderezo (PK int)
o	Nombre (String)
o	Tipo (String)
o	Costo (String)
•	Guarnición
o	IdGuarnicion (PK int)
o	Nombre (String)
o	Tipo (String)
o	Costo (String)
Implementación:
Eso lo vera en el proyecto profe :>
Testing:
Nos encargaremos de que será bien robusto a los errores :3

Iteración 2:
¡Muy pronto!
