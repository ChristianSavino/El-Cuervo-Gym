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

-- INSERT PARAMETROS
SELECT adm.InsertarParametroSiNoExiste('DiasMaxPermitidos', '2', 'Dias máximos permitidos antes de que venza la cuota');
SELECT adm.InsertarParametroSiNoExiste('ValorCuota', '24000', 'Valor por default de la cuota');

--

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