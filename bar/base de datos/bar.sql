-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 12-02-2026 a las 10:20:27
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `bar`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `aderezo`
--

CREATE TABLE `aderezo` (
  `IdAderezo` int(11) NOT NULL,
  `Nombre` varchar(200) NOT NULL,
  `Tipo` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `bebida`
--

CREATE TABLE `bebida` (
  `IdBebida` int(11) NOT NULL,
  `Nombre` varchar(200) NOT NULL,
  `Tipo` varchar(200) NOT NULL,
  `Costo` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `guarnicion`
--

CREATE TABLE `guarnicion` (
  `IdGuarnicion` int(11) NOT NULL,
  `Nombre` varchar(200) NOT NULL,
  `Tipo` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedido`
--

CREATE TABLE `pedido` (
  `IdPedido` int(11) NOT NULL,
  `IdUsuario` int(11) NOT NULL,
  `Monto` int(11) NOT NULL,
  `Fecha` date NOT NULL,
  `Estado` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedidodetalle`
--

CREATE TABLE `pedidodetalle` (
  `IdDetalle` int(11) NOT NULL,
  `IdPlato` int(11) NOT NULL,
  `IdPedido` int(11) NOT NULL,
  `IdGuarnicion` int(11) DEFAULT NULL,
  `IdBebida` int(11) DEFAULT NULL,
  `IdAderezo` int(11) DEFAULT NULL,
  `SubTotal` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `plato`
--

CREATE TABLE `plato` (
  `IdPlato` int(11) NOT NULL,
  `IdRes` int(11) NOT NULL,
  `Nombre` varchar(200) NOT NULL,
  `Ingredientes` varchar(200) NOT NULL,
  `Descripcion` varchar(200) NOT NULL,
  `Costo` int(11) NOT NULL,
  `Imagen` varchar(200) DEFAULT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `plato`
--

INSERT INTO `plato` (`IdPlato`, `IdRes`, `Nombre`, `Ingredientes`, `Descripcion`, `Costo`, `Imagen`, `Estado`) VALUES
(1, 1, 'Empanadas', 'Carne, cebolla, pimenton, condmiento para carne', 'ricas empanadas al horno las clasicas', 10000, '/platos/11797f92-fad3-4934-9f2b-a3d2c8a3153c.jfif', 1),
(2, 1, 'Milanesa Napolitana', 'Milanesa de vaca, pure de tomate, queso mozzarella, oregano', 'La super milanga, bien rica bien tatanga', 5000, '/platos/784d7ec4-34a5-488b-bc2f-54e57ec41792.jfif', 1),
(3, 2, 'Tacos al pastor', 'tapa para quesadillas, carne vacuna, cebolla, aji, limon', 'taquitos chidos', 8000, '/platos/563f7f77-3371-43b8-9a3c-e45340fd8066.png', 1),
(4, 2, 'Burrito', 'pollo, queso, aji, sal tortilla para quesadilla', 'el borrito', 2000, '/platos/66446fe7-0084-4cae-a446-d4222a976207.jfif', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `restaurante`
--

CREATE TABLE `restaurante` (
  `IdRes` int(11) NOT NULL,
  `Nombre` varchar(200) NOT NULL,
  `Ubicacion` varchar(200) NOT NULL,
  `Especialidad` varchar(200) NOT NULL,
  `Imagen` varchar(200) DEFAULT NULL,
  `IdUsuario` int(11) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `restaurante`
--

INSERT INTO `restaurante` (`IdRes`, `Nombre`, `Ubicacion`, `Especialidad`, `Imagen`, `IdUsuario`, `Estado`) VALUES
(1, 'El meson de la PC', 'San Francisco del Monte de Oro', 'Comida nacional', '/restaurante/4c906e6f-2827-491c-ba78-99f70d0290c4.jpg', 1, 1),
(2, 'De Mexico Al Mundo', 'San Luis', 'Comida mexicana', NULL, 2, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `IdUsuario` int(11) NOT NULL,
  `Rol` enum('admin','user','resto','') NOT NULL,
  `Nombre` varchar(200) NOT NULL,
  `Apellido` varchar(200) NOT NULL,
  `Nick` varchar(200) NOT NULL,
  `Email` varchar(200) NOT NULL,
  `PasswordHash` varchar(200) NOT NULL,
  `Telefono` varchar(200) NOT NULL,
  `Domicilio` varchar(200) NOT NULL,
  `Avatar` varchar(200) DEFAULT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`IdUsuario`, `Rol`, `Nombre`, `Apellido`, `Nick`, `Email`, `PasswordHash`, `Telefono`, `Domicilio`, `Avatar`, `Estado`) VALUES
(1, 'resto', 'Branguer', 'NoventayUnMil', 'AdminSuper', 'branguer91000@gmail.com', 'uAPpo6FwXJB3bYNZRpdcX7loBzqQq/AsGeHL2QWoHNk4YvdXlo4tlUcNeAQoYifI', '02664553401', 'Nogoli', '/avatars/00834cf5-5b00-440b-bc25-d123e884db9b.png', 0),
(2, 'resto', 'Don', 'Erensto', 'Cocinero Mexicano', 'emailTrue@gmail.com', 'lgITk5gmbODgWSTbKIg4JQo244Tq6WNerZAPuetB+hVG4vIkSP6NIGreWXJWQwH8', '02664553401', 'Mexico', NULL, 0),
(3, 'user', 'presentador', 'del proyecto', 'Presentador de la clientela', 'emailfalso@gmail.com', 'xs6oNL+mHT5u5LhtCauteE+1PAwR6DF9fFK9D0xX+zqdSn0+A9tJFNrRRGp6YhLc', '12345678', 'Net Core', '/img/avatars/2db0bc37-0676-49dc-aa8f-c4e8f43aadc2.jpeg', 0);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `aderezo`
--
ALTER TABLE `aderezo`
  ADD PRIMARY KEY (`IdAderezo`);

--
-- Indices de la tabla `bebida`
--
ALTER TABLE `bebida`
  ADD PRIMARY KEY (`IdBebida`);

--
-- Indices de la tabla `guarnicion`
--
ALTER TABLE `guarnicion`
  ADD PRIMARY KEY (`IdGuarnicion`);

--
-- Indices de la tabla `pedido`
--
ALTER TABLE `pedido`
  ADD PRIMARY KEY (`IdPedido`),
  ADD KEY `IdUsuario` (`IdUsuario`);

--
-- Indices de la tabla `pedidodetalle`
--
ALTER TABLE `pedidodetalle`
  ADD PRIMARY KEY (`IdDetalle`),
  ADD KEY `IdPedido` (`IdPedido`),
  ADD KEY `IdPlato` (`IdPlato`),
  ADD KEY `IdAderezo` (`IdAderezo`),
  ADD KEY `IdBebida` (`IdBebida`),
  ADD KEY `IdGuarnicion` (`IdGuarnicion`);

--
-- Indices de la tabla `plato`
--
ALTER TABLE `plato`
  ADD PRIMARY KEY (`IdPlato`),
  ADD KEY `IdRes` (`IdRes`);

--
-- Indices de la tabla `restaurante`
--
ALTER TABLE `restaurante`
  ADD PRIMARY KEY (`IdRes`),
  ADD KEY `IdUsuario` (`IdUsuario`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`IdUsuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `aderezo`
--
ALTER TABLE `aderezo`
  MODIFY `IdAderezo` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `bebida`
--
ALTER TABLE `bebida`
  MODIFY `IdBebida` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `guarnicion`
--
ALTER TABLE `guarnicion`
  MODIFY `IdGuarnicion` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `pedido`
--
ALTER TABLE `pedido`
  MODIFY `IdPedido` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `pedidodetalle`
--
ALTER TABLE `pedidodetalle`
  MODIFY `IdDetalle` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `plato`
--
ALTER TABLE `plato`
  MODIFY `IdPlato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `restaurante`
--
ALTER TABLE `restaurante`
  MODIFY `IdRes` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `IdUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `pedido`
--
ALTER TABLE `pedido`
  ADD CONSTRAINT `pedido_ibfk_2` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`);

--
-- Filtros para la tabla `pedidodetalle`
--
ALTER TABLE `pedidodetalle`
  ADD CONSTRAINT `pedidodetalle_ibfk_1` FOREIGN KEY (`IdPedido`) REFERENCES `pedido` (`IdPedido`),
  ADD CONSTRAINT `pedidodetalle_ibfk_2` FOREIGN KEY (`IdPlato`) REFERENCES `plato` (`IdPlato`),
  ADD CONSTRAINT `pedidodetalle_ibfk_3` FOREIGN KEY (`IdAderezo`) REFERENCES `aderezo` (`IdAderezo`) ON DELETE SET NULL,
  ADD CONSTRAINT `pedidodetalle_ibfk_4` FOREIGN KEY (`IdBebida`) REFERENCES `bebida` (`IdBebida`) ON DELETE SET NULL,
  ADD CONSTRAINT `pedidodetalle_ibfk_5` FOREIGN KEY (`IdGuarnicion`) REFERENCES `guarnicion` (`IdGuarnicion`) ON DELETE SET NULL;

--
-- Filtros para la tabla `plato`
--
ALTER TABLE `plato`
  ADD CONSTRAINT `plato_ibfk_1` FOREIGN KEY (`IdRes`) REFERENCES `restaurante` (`IdRes`);

--
-- Filtros para la tabla `restaurante`
--
ALTER TABLE `restaurante`
  ADD CONSTRAINT `restaurante_ibfk_1` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
