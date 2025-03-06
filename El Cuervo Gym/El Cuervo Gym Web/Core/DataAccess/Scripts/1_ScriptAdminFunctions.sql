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