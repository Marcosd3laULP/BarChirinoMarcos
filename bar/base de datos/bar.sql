-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 06-11-2025 a las 20:34:24
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

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
  `Tipo` varchar(200) NOT NULL,
  `Costo` int(11) NOT NULL
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
  `Tipo` varchar(200) NOT NULL,
  `Costo` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedido`
--

CREATE TABLE `pedido` (
  `IdPedido` int(11) NOT NULL,
  `IdPlato` int(11) NOT NULL,
  `IdUsuario` int(11) NOT NULL,
  `Monto` int(11) NOT NULL,
  `Fecha` date NOT NULL,
  `Guarnicion` varchar(200) NOT NULL,
  `IdBebida` int(11) DEFAULT NULL,
  `IdAderezo` int(11) DEFAULT NULL
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
  `IdGuarnicion` int(11) NOT NULL,
  `ConBebidas` tinyint(1) NOT NULL,
  `ConAderezo` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `restaurante`
--

CREATE TABLE `restaurante` (
  `IdRes` int(11) NOT NULL,
  `Nombre` varchar(200) NOT NULL,
  `Ubicacion` varchar(200) NOT NULL,
  `Especialidad` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  `Contraseña` varchar(200) NOT NULL,
  `Telefono` varchar(200) NOT NULL,
  `Domicilio` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  ADD KEY `IdPlato` (`IdPlato`),
  ADD KEY `IdUsuario` (`IdUsuario`),
  ADD KEY `IdAderezo` (`IdAderezo`),
  ADD KEY `IdBebida` (`IdBebida`);

--
-- Indices de la tabla `plato`
--
ALTER TABLE `plato`
  ADD PRIMARY KEY (`IdPlato`),
  ADD KEY `IdRes` (`IdRes`),
  ADD KEY `IdGuarnicion` (`IdGuarnicion`);

--
-- Indices de la tabla `restaurante`
--
ALTER TABLE `restaurante`
  ADD PRIMARY KEY (`IdRes`);

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
  MODIFY `IdPedido` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `plato`
--
ALTER TABLE `plato`
  MODIFY `IdPlato` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `restaurante`
--
ALTER TABLE `restaurante`
  MODIFY `IdRes` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `IdUsuario` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `pedido`
--
ALTER TABLE `pedido`
  ADD CONSTRAINT `pedido_ibfk_1` FOREIGN KEY (`IdPlato`) REFERENCES `plato` (`IdPlato`),
  ADD CONSTRAINT `pedido_ibfk_2` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`),
  ADD CONSTRAINT `pedido_ibfk_3` FOREIGN KEY (`IdAderezo`) REFERENCES `aderezo` (`IdAderezo`),
  ADD CONSTRAINT `pedido_ibfk_4` FOREIGN KEY (`IdBebida`) REFERENCES `bebida` (`IdBebida`);

--
-- Filtros para la tabla `plato`
--
ALTER TABLE `plato`
  ADD CONSTRAINT `plato_ibfk_1` FOREIGN KEY (`IdRes`) REFERENCES `restaurante` (`IdRes`),
  ADD CONSTRAINT `plato_ibfk_2` FOREIGN KEY (`IdGuarnicion`) REFERENCES `guarnicion` (`IdGuarnicion`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
