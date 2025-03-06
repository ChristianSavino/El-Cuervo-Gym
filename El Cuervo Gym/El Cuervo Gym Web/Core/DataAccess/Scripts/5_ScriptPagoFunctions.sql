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