﻿namespace El_Cuervo_Gym_Web.Core.Utils
{
    public enum Estado
    {
        Activo = 1,
        Baja = 2
    }

    public enum TipoPago
    {
        Efectivo = 1,
        Tarjeta = 2,
        Transferencia = 3,
        MercadoPago = 4,
        Inicial = 5,
        Otro = 6,
        ReIngreso = 7
    }

    public enum TipoIngreso
    {
        Normal = 1,
        Alerta = 2,
        Prohibir = 3
    }
}
