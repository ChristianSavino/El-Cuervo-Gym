-- ADMIN

CREATE SCHEMA IF NOT EXISTS adm;

CREATE TABLE IF NOT EXISTS Adm.Admin (
    Id SERIAL PRIMARY KEY,
    Usuario VARCHAR(100) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    Estado INT NOT NULL,
    IsMaster BOOLEAN NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_admin_usuario_password
ON Adm.Admin (Usuario, Password);

-- SOCIO

CREATE SCHEMA IF NOT EXISTS Soc;

CREATE TABLE IF NOT EXISTS Soc.Socio (
    Id SERIAL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Documento INT NOT NULL,
    Telefono INT NOT NULL,
    ObraSocial VARCHAR(100),
    NumeroObraSocial VARCHAR(100),
    NumeroEmergencia INT,
    ContactoEmergencia VARCHAR(100),
    FechaSubscripcion TIMESTAMP NOT NULL,
    ProximoVencimientoCuota TIMESTAMP NOT NULL,
    Estado INT NOT NULL,
    IdAdmin INT NOT NULL,
    FOREIGN KEY (IdAdmin) REFERENCES Adm.Admin(Id)
);

CREATE INDEX IF NOT EXISTS idx_socio_documento
ON Soc.Socio (Documento);

CREATE INDEX IF NOT EXISTS idx_socio_estado
ON Soc.Socio (Estado);

CREATE INDEX IF NOT EXISTS idx_socio_documento
ON Soc.Socio (Id,Documento);

CREATE INDEX IF NOT EXISTS idx_socio_documento
ON Soc.Socio (FechaSubscripcion);

CREATE INDEX IF NOT EXISTS idx_socio_documento
ON Soc.Socio (ProximoVencimientoCuota);

-- PAGO

CREATE TABLE IF NOT EXISTS Soc.Pago (
    Id SERIAL PRIMARY KEY,
    IdSocio INT NOT NULL,
    FechaPago TIMESTAMP NOT NULL,
    FechaCuota TIMESTAMP NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL,
    Estado INT NOT NULL,
    IdAdmin INT NOT NULL,
    MetodoPago INT NOT NULL,
    Comprobante VARCHAR(100),
    FOREIGN KEY (IdSocio) REFERENCES Soc.Socio(Id),
    FOREIGN KEY (IdAdmin) REFERENCES Adm.Admin(Id)
);

CREATE INDEX IF NOT EXISTS idx_pago_idsocio
ON Soc.Pago (IdSocio);

CREATE INDEX IF NOT EXISTS idx_pago_fechapago
ON Soc.Pago (FechaPago);

CREATE INDEX IF NOT EXISTS idx_pago_fechacuota
ON Soc.Pago (FechaCuota);

-- FUNC ADMIN

DROP FUNCTION IF EXISTS Adm.LoginAdmin;
CREATE OR REPLACE FUNCTION Adm.LoginAdmin(
    p_usuario VARCHAR,
    p_password VARCHAR
)
RETURNS TABLE (
    Id INT,
    Usuario VARCHAR,
    IsMaster BOOLEAN
) AS $$
BEGIN
    RETURN QUERY
    SELECT a.Id, a.Usuario, a.IsMaster
    FROM Adm.Admin a
    WHERE a.Usuario = p_usuario AND a.Password = MD5(p_password) AND a.Estado = 1;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Adm.InsertarAdmin;
CREATE OR REPLACE FUNCTION Adm.InsertarAdmin(
    p_usuario VARCHAR,
    p_password VARCHAR,
    p_estado INT,
    p_is_master BOOLEAN
)
RETURNS INT AS $$
DECLARE
    v_id INT;
BEGIN
    INSERT INTO adm.Admin (Usuario, Password, Estado, IsMaster) VALUES (p_usuario, MD5(p_password), p_estado, p_is_master) RETURNING Id INTO v_id;
    RETURN v_id;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS adm.FiltrarAdmin;
CREATE OR REPLACE FUNCTION adm.FiltrarAdmin(
    p_usuario VARCHAR,
    p_id INT,
    p_incluir_dados_de_baja BOOLEAN
)
RETURNS TABLE (
    Id INT,
    Usuario VARCHAR,
    Estado INT,
    IsMaster BOOLEAN
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a.Id,
        a.Usuario,
        a.Estado,
        a.IsMaster
    FROM Adm.Admin a
    WHERE 
        (p_usuario IS NULL OR a.usuario::TEXT ILIKE '%' || p_usuario || '%')
        AND (p_id IS NULL OR a.Id = p_id)
        AND (p_incluir_dados_de_baja IS TRUE OR a.Estado = 1)
    ORDER BY a.Id;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS adm.ObtenerAdminPorId;
CREATE OR REPLACE FUNCTION adm.ObtenerAdminPorId(
    p_id INT
)
RETURNS TABLE (
    Id INT,
    Usuario VARCHAR,
    Estado INT,
    IsMaster BOOLEAN
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a.Id,
        a.Usuario,
        a.Estado,
        a.IsMaster
    FROM Adm.Admin a
    WHERE 
        a.Id = p_id;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Adm.ActualizarAdmin;
CREATE OR REPLACE FUNCTION Adm.ActualizarAdmin(
    p_id INT,
    p_usuario VARCHAR,
    p_password VARCHAR,
    p_estado INT,
    p_is_master BOOLEAN
)
RETURNS INT AS $$
DECLARE
    v_affected_rows INT;
BEGIN
    UPDATE Adm.Admin
    SET
        Usuario = p_usuario,
        Password = MD5(p_password),
        Estado = p_estado,
        IsMaster = p_is_master
    WHERE 
        Id = p_id;

    GET DIAGNOSTICS v_affected_rows = ROW_COUNT;
    RETURN v_affected_rows;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Adm.DarDeBajaAdmin;
CREATE OR REPLACE FUNCTION Adm.DarDeBajaAdmin(
    p_id INT
)
RETURNS INT AS $$
DECLARE
    v_affected_rows INT;
BEGIN
    UPDATE Adm.Admin
    SET Estado = 2
    WHERE Id = p_id;

    GET DIAGNOSTICS v_affected_rows = ROW_COUNT;
    RETURN v_affected_rows;
END;
$$ LANGUAGE plpgsql;

-- FUNC SOCIO

DROP FUNCTION IF EXISTS Soc.InsertarSocio;
CREATE OR REPLACE FUNCTION Soc.InsertarSocio(
    p_nombre VARCHAR,
    p_apellido VARCHAR,
    p_documento INT,
    p_telefono INT,
    p_obra_social VARCHAR,
    p_numero_obra_social VARCHAR,
    p_numero_emergencia INT,
    p_contacto_emergencia VARCHAR,
    p_fecha_subscripcion TIMESTAMP,
    p_proximo_vencimiento_cuota TIMESTAMP,
    p_estado INT,
    p_id_admin INT
)
RETURNS INT AS $$
DECLARE
    v_id INT;
BEGIN
    INSERT INTO Soc.Socio (
        Nombre,
        Apellido,
        Documento,
        Telefono,
        ObraSocial,
        NumeroObraSocial,
        NumeroEmergencia,
        ContactoEmergencia,
        FechaSubscripcion,
        ProximoVencimientoCuota,
        Estado,
        IdAdmin
    ) VALUES (
        p_nombre,
        p_apellido,
        p_documento,
        p_telefono,
        p_obra_social,
        p_numero_obra_social,
        p_numero_emergencia,
        p_contacto_emergencia,
        p_fecha_subscripcion,
        p_proximo_vencimiento_cuota,
        p_estado,
        p_id_admin
    ) RETURNING Id INTO v_id;

    RETURN v_id;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Soc.FiltrarSocios;
CREATE OR REPLACE FUNCTION Soc.FiltrarSocios(
    p_nombre VARCHAR,
    p_documento VARCHAR,
    p_numero_socio VARCHAR,
    p_fecha_inicio TIMESTAMP,
    p_fecha_fin TIMESTAMP,
    p_cuotas_vencidas BOOLEAN,
    p_incluir_dados_de_baja BOOLEAN
)
RETURNS TABLE (
    Id INT,
    Nombre VARCHAR,
    Apellido VARCHAR,
    Documento INT,
    Telefono INT,
    ObraSocial VARCHAR,
    NumeroObraSocial VARCHAR,
    NumeroEmergencia INT,
    ContactoEmergencia VARCHAR,
    FechaSubscripcion TIMESTAMP,
    ProximoVencimientoCuota TIMESTAMP,
    Estado INT,
    IdAdmin INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        s.Id,
        s.Nombre,
        s.Apellido,
        s.Documento,
        s.Telefono,
        s.ObraSocial,
        s.NumeroObraSocial,
        s.NumeroEmergencia,
        s.ContactoEmergencia,
        s.FechaSubscripcion,
        s.ProximoVencimientoCuota,
        s.Estado,
        s.IdAdmin
    FROM Soc.Socio s
    WHERE 
        (p_nombre IS NULL OR (s.Nombre ILIKE '%' || p_nombre || '%' OR s.Apellido ILIKE '%' || p_nombre || '%'))
        AND (p_documento IS NULL OR s.Documento::TEXT ILIKE '%' || p_documento || '%')
        AND (p_numero_socio IS NULL OR s.Id::TEXT ILIKE '%' || p_numero_socio || '%')
        AND (p_fecha_inicio IS NULL OR s.FechaSubscripcion >= p_fecha_inicio)
        AND (p_fecha_fin IS NULL OR s.FechaSubscripcion < p_fecha_fin)
        AND (p_cuotas_vencidas IS FALSE OR s.ProximoVencimientoCuota < CURRENT_DATE)
        AND (p_incluir_dados_de_baja IS TRUE OR s.Estado = 1)
    ORDER BY s.FechaSubscripcion DESC;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Soc.ObtenerSocioPorId;
CREATE OR REPLACE FUNCTION Soc.ObtenerSocioPorId(
    p_id INT
)
RETURNS TABLE (
    Id INT,
    Nombre VARCHAR,
    Apellido VARCHAR,
    Documento INT,
    Telefono INT,
    ObraSocial VARCHAR,
    NumeroObraSocial VARCHAR,
    NumeroEmergencia INT,
    ContactoEmergencia VARCHAR,
    FechaSubscripcion TIMESTAMP,
    ProximoVencimientoCuota TIMESTAMP,
    Estado INT,
    IdAdmin INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        s.Id,
        s.Nombre,
        s.Apellido,
        s.Documento,
        s.Telefono,
        s.ObraSocial,
        s.NumeroObraSocial,
        s.NumeroEmergencia,
        s.ContactoEmergencia,
        s.FechaSubscripcion,
        s.ProximoVencimientoCuota,
        s.Estado,
        s.IdAdmin
    FROM Soc.Socio s
    WHERE s.Id = p_id;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Soc.ObtenerSocioConPagosPorId;
CREATE OR REPLACE FUNCTION Soc.ObtenerSocioConPagosPorId(
    p_id INT
)
RETURNS TABLE (
    Id INT,
    Nombre VARCHAR,
    Apellido VARCHAR,
    Documento INT,
    Telefono INT,
    ObraSocial VARCHAR,
    NumeroObraSocial VARCHAR,
    NumeroEmergencia INT,
    ContactoEmergencia VARCHAR,
    FechaSubscripcion TIMESTAMP,
    ProximoVencimientoCuota TIMESTAMP,
    Estado INT,
    IdAdmin INT,
    PagosJson JSON
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        s.Id,
        s.Nombre,
        s.Apellido,
        s.Documento,
        s.Telefono,
        s.ObraSocial,
        s.NumeroObraSocial,
        s.NumeroEmergencia,
        s.ContactoEmergencia,
        s.FechaSubscripcion,
        s.ProximoVencimientoCuota,
        s.Estado,
        s.IdAdmin,
        (
            SELECT json_agg(
                json_build_object(
                    'Id', p.Id,
                    'IdSocio', p.IdSocio,
                    'FechaPago', p.FechaPago,
                    'FechaCuota', p.FechaCuota,
                    'Monto', p.Monto,
                    'Estado', p.Estado,
                    'IdAdmin', p.IdAdmin,
                    'MetodoPago', p.MetodoPago,
                    'Comprobante', p.Comprobante
                )
            )
            FROM Soc.Pago p
            WHERE p.IdSocio = s.Id
        ) AS PagosJson
    FROM Soc.Socio s
    WHERE s.Id = p_id;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Soc.ActualizarSocio;
CREATE OR REPLACE FUNCTION Soc.ActualizarSocio(
    p_id INT,
    p_nombre VARCHAR,
    p_apellido VARCHAR,
    p_documento INT,
    p_telefono INT,
    p_obra_social VARCHAR,
    p_numero_obra_social VARCHAR,
    p_numero_emergencia INT,
    p_contacto_emergencia VARCHAR,
    p_fecha_subscripcion TIMESTAMP,
    p_proximo_vencimiento_cuota TIMESTAMP,
    p_estado INT,
    p_id_admin INT
)
RETURNS INT AS $$
DECLARE
    v_affected_rows INT;
BEGIN
    UPDATE Soc.Socio
    SET
        Nombre = p_nombre,
        Apellido = p_apellido,
        Documento = p_documento,
        Telefono = p_telefono,
        ObraSocial = p_obra_social,
        NumeroObraSocial = p_numero_obra_social,
        NumeroEmergencia = p_numero_emergencia,
        ContactoEmergencia = p_contacto_emergencia,
        FechaSubscripcion = p_fecha_subscripcion,
        ProximoVencimientoCuota = p_proximo_vencimiento_cuota,
        Estado = p_estado,
        IdAdmin = p_id_admin
    WHERE Id = p_id;

    GET DIAGNOSTICS v_affected_rows = ROW_COUNT;
    RETURN v_affected_rows;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Soc.DarDeBajaSocio;
CREATE OR REPLACE FUNCTION Soc.DarDeBajaSocio(
    p_id INT
)
RETURNS INT AS $$
DECLARE
    v_affected_rows INT;
BEGIN
    UPDATE Soc.Socio
    SET Estado = 2 -- Estado 2 representa "dado de baja"
    WHERE Id = p_id;

    GET DIAGNOSTICS v_affected_rows = ROW_COUNT;
    RETURN v_affected_rows;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Soc.ActualizarProximaFechaVencimiento;
CREATE OR REPLACE FUNCTION Soc.ActualizarProximaFechaVencimiento(
    p_id INT,
    p_nueva_fecha TIMESTAMP
)
RETURNS INT AS $$
DECLARE
    v_affected_rows INT;
BEGIN
    UPDATE Soc.Socio
    SET ProximoVencimientoCuota = p_nueva_fecha
    WHERE Id = p_id;

    GET DIAGNOSTICS v_affected_rows = ROW_COUNT;
    RETURN v_affected_rows;
END;
$$ LANGUAGE plpgsql;

-- FUNC PAGO

DROP FUNCTION IF EXISTS Soc.InsertarPago;
CREATE OR REPLACE FUNCTION Soc.InsertarPago(
    p_id_socio INT,
    p_fecha_pago TIMESTAMP,
    p_fecha_cuota TIMESTAMP,
    p_monto DECIMAL,
    p_estado INT,
    p_id_admin INT,
    p_metodo_pago INT,
    p_comprobante VARCHAR
)
RETURNS INT AS $$
DECLARE
    v_id INT;
BEGIN
    INSERT INTO Soc.Pago (
        IdSocio,
        FechaPago,
        FechaCuota,
        Monto,
        Estado,
        IdAdmin,
        MetodoPago,
        Comprobante
    ) VALUES (
        p_id_socio,
        p_fecha_pago,
        p_fecha_cuota,
        p_monto,
        p_estado,
        p_id_admin,
        p_metodo_pago,
        p_comprobante
    ) RETURNING Id INTO v_id;

    RETURN v_id;
END;
$$ LANGUAGE plpgsql;

--
DROP FUNCTION IF EXISTS Soc.FiltrarPagos;
CREATE OR REPLACE FUNCTION Soc.FiltrarPagos(
    p_nombre VARCHAR,
    p_documento INT,
    p_numero_socio INT,
    p_fecha_inicio TIMESTAMP,
    p_fecha_fin TIMESTAMP,
    p_incluir_dados_de_baja BOOLEAN,
    p_metodo_pago INT
)
RETURNS TABLE (
    Id INT,
    IdSocio INT,
    FechaPago TIMESTAMP,
    FechaCuota TIMESTAMP,
    Monto DECIMAL(10, 2),
    Estado INT,
    MetodoPago INT,
    Comprobante VARCHAR,
    Nombre TEXT,
    Documento INT,
    IdAdmin INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.Id,
        p.IdSocio,
        p.FechaPago,
        p.FechaCuota,
        p.Monto,
        p.Estado,
        p.MetodoPago,
        p.Comprobante,
        s.Nombre || ' ' || s.Apellido AS Nombre,
        s.Documento,
        p.idAdmin
    FROM Soc.Pago p
    JOIN Soc.Socio s ON p.IdSocio = s.Id
    WHERE 
        (p_nombre IS NULL OR (s.Nombre ILIKE '%' || p_nombre || '%' OR s.apellido ILIKE '%' || p_nombre || '%'))
        AND (p_documento IS NULL OR s.Documento = p_documento)
        AND (p_numero_socio IS NULL OR s.Id = p_numero_socio)
        AND (p_fecha_inicio IS NULL OR p.FechaPago >= p_fecha_inicio)
        AND (p_fecha_fin IS NULL OR p.FechaPago < p_fecha_fin)
        AND (p_incluir_dados_de_baja IS TRUE OR p.Estado = 1)
        AND (p_metodo_pago IS NULL OR p.MetodoPago = p_metodo_pago)
    ORDER BY p.FechaPago DESC;
END;
$$ LANGUAGE plpgsql;
--

DROP FUNCTION IF EXISTS Soc.ObtenerPagoPorId;
CREATE OR REPLACE FUNCTION Soc.ObtenerPagoPorId(
    p_id INT
)
RETURNS TABLE (
    Id INT,
    IdSocio INT,
    FechaPago TIMESTAMP,
    FechaCuota TIMESTAMP,
    Monto DECIMAL(10, 2),
    Estado INT,
    MetodoPago INT,
    Comprobante VARCHAR,
    Nombre TEXT,
    Documento INT,
    IdAdmin INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        p.Id,
        p.IdSocio,
        p.FechaPago,
        p.FechaCuota,
        p.Monto,
        p.Estado,
        p.MetodoPago,
        p.Comprobante,
        s.Nombre || ' ' || s.Apellido AS Nombre,
        s.Documento,
        p.idAdmin
    FROM Soc.Pago p
    JOIN Soc.Socio s ON p.IdSocio = s.Id
    WHERE p.Id = p_id;
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Soc.ExistePagosPosteriores;
CREATE OR REPLACE FUNCTION Soc.ExistePagosPosteriores(
    p_id INT,
    p_id_socio INT,
    p_fecha_cuota TIMESTAMP
)
RETURNS BOOLEAN AS $$
BEGIN
    RETURN EXISTS (
        SELECT 1
        FROM Soc.Pago p
        WHERE 
            p.Id <> p_id
            AND p.IdSocio = p_id_socio
            AND p.FechaCuota >= p_fecha_cuota
            AND p.Estado = 1
    );
END;
$$ LANGUAGE plpgsql;

--

DROP FUNCTION IF EXISTS Soc.DarDeBajaPago;
CREATE OR REPLACE FUNCTION Soc.DarDeBajaPago(
    p_id INT
)
RETURNS INT AS $$
DECLARE
    v_affected_rows INT;
BEGIN
    UPDATE Soc.Pago
    SET Estado = 2 -- Estado 2 representa "dado de baja"
    WHERE Id = p_id;

    GET DIAGNOSTICS v_affected_rows = ROW_COUNT;
    RETURN v_affected_rows;
END;
$$ LANGUAGE plpgsql;

-- Error Logging

CREATE TABLE IF NOT EXISTS adm.Errores (
    Id SERIAL PRIMARY KEY,
    Contexto VARCHAR(255) NOT NULL,
    TipoError VARCHAR(255) NOT NULL,
    MensajeException TEXT NOT NULL,
    StackTrace TEXT NOT NULL,
    InfoExtra TEXT,
    FechaHora TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

DROP FUNCTION IF EXISTS adm.InsertarError;
CREATE OR REPLACE FUNCTION adm.InsertarError(
    p_contexto VARCHAR,
    p_tipo_error VARCHAR,
    p_mensaje_exception TEXT,
    p_stack_trace TEXT,
    p_info_extra TEXT
)
RETURNS VOID AS $$
BEGIN
    INSERT INTO adm.Errores (
        Contexto,
        TipoError,
        MensajeException,
        StackTrace,
        InfoExtra,
        FechaHora
    ) VALUES (
        p_contexto,
        p_tipo_error,
        p_mensaje_exception,
        p_stack_trace,
        p_info_extra,
        CURRENT_TIMESTAMP
    );
END;
$$ LANGUAGE plpgsql;

-- PARAMETROS

-- Crear tabla de parámetros en el esquema adm con FechaModificacion por defecto como CURRENT_TIMESTAMP
CREATE TABLE IF NOT EXISTS adm.Parametros (
    Id SERIAL PRIMARY KEY,
    Clave VARCHAR(100) NOT NULL,
    Valor VARCHAR(255) NOT NULL,
    Descripcion TEXT,
    FechaModificacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Crear índice para búsqueda rápida por clave
CREATE INDEX IF NOT EXISTS idx_parametros_clave
ON adm.Parametros (Clave);

-- Función para insertar un parámetro solo si no existen datos en la tabla
CREATE OR REPLACE FUNCTION adm.InsertarParametroSiNoExiste(
    p_clave VARCHAR,
    p_valor VARCHAR,
    p_descripcion TEXT
)
RETURNS VOID AS $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM adm.Parametros WHERE clave = p_clave) THEN
        INSERT INTO adm.Parametros (Clave, Valor, Descripcion)
        VALUES (p_clave, p_valor, p_descripcion);
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Llamar a la función para insertar el parámetro
SELECT adm.InsertarParametroSiNoExiste('DiasMaxPermitidos', '2', 'Dias máximos permitidos antes de que venza la cuota');
SELECT adm.InsertarParametroSiNoExiste('ValorCuota', '24000', 'Valor por default de la cuota');


-- Función para actualizar un parámetro y la fecha de modificación
CREATE OR REPLACE FUNCTION adm.ActualizarParametro(
    p_clave VARCHAR,
    p_valor VARCHAR,
    p_descripcion TEXT
)
RETURNS VOID AS $$
BEGIN
    UPDATE adm.Parametros
    SET Valor = p_valor,
        Descripcion = p_descripcion,
        FechaModificacion = CURRENT_TIMESTAMP
    WHERE Clave = p_clave;
END;
$$ LANGUAGE plpgsql;