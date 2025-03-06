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