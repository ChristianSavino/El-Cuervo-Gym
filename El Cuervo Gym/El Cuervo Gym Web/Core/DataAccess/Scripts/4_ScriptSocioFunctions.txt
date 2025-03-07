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

-- FILTRAR

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

-- OBTENER POR ID

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

-- OBTENER POR ID CON PAGOS

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

-- UPDATE SOCIO

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

-- BAJA SOCIO

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

-- ACTUALIZAR FECHA VENCIMIENTO

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

-- LOGIN POR NUMERO SOCIO Y DOCUMENTO

DROP FUNCTION IF EXISTS Soc.LogearSocio;
CREATE OR REPLACE FUNCTION Soc.LogearSocio(
    p_documento INT,
    p_numero_socio INT
)
RETURNS BOOLEAN AS $$
DECLARE
    v_exists BOOLEAN;
BEGIN
    SELECT EXISTS (
        SELECT 1 
        FROM Soc.Socio s
        WHERE s.Documento = p_documento
        AND s.Id = p_numero_socio
        AND s.Estado = 1
    ) INTO v_exists;

    RETURN v_exists;
END;
$$ LANGUAGE plpgsql;