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

-- ERRORES

CREATE TABLE IF NOT EXISTS adm.Errores (
    Id SERIAL PRIMARY KEY,
    Contexto VARCHAR(255) NOT NULL,
    TipoError VARCHAR(255) NOT NULL,
    MensajeException TEXT NOT NULL,
    StackTrace TEXT NOT NULL,
    InfoExtra TEXT,
    FechaHora TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- PARAMETROS

CREATE TABLE IF NOT EXISTS adm.Parametros (
    Id SERIAL PRIMARY KEY,
    Clave VARCHAR(100) NOT NULL,
    Valor VARCHAR(255) NOT NULL,
    Descripcion TEXT,
    FechaModificacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_parametros_clave
ON adm.Parametros (Clave);