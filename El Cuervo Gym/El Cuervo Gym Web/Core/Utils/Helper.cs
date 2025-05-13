namespace El_Cuervo_Gym_Web.Core.Utils
{
    public static class Helper
    {
        public static string ValorCuotaParamName = "ValorCuota";
        public static string DiasMaxPermitidoParamName = "DiasMaxPermitidos";
        
        public static DateTime ObtenerProximoVencimientoDeCuota(DateTime fechaSubscripcion)
        {
            var now = DateTime.Now;
            var proximaCuotaPago = fechaSubscripcion.AddMonths(1);
            if (proximaCuotaPago.Date < now.Date)
            {
                proximaCuotaPago = ObtenerFechaDiaEnMesActual(fechaSubscripcion.Day);
                if (proximaCuotaPago.Date < now.Date)
                {
                    proximaCuotaPago = proximaCuotaPago.AddMonths(1);
                }
            }

            return proximaCuotaPago.Date;
        }

        public static DateTime ObtenerProximoVencimientoDeCuota(DateTime fechaSubscripcion, DateTime fechaProxVencimiento)
        {
            var now = DateTime.Now;
            if(fechaProxVencimiento.Date > now.Date && fechaProxVencimiento.Date > fechaSubscripcion.Date)
            {
                return fechaProxVencimiento.Date;
            }
            var proximaCuotaPago = fechaSubscripcion.AddMonths(1);
            if (proximaCuotaPago.Date < now.Date)
            {
                proximaCuotaPago = ObtenerFechaDiaEnMesActual(fechaSubscripcion.Day);
                if (proximaCuotaPago.Date < now.Date)
                {
                    proximaCuotaPago = proximaCuotaPago.AddMonths(1);
                }
            }

            return proximaCuotaPago.Date;
        }

        public static DateTime ObtenerFechaDiaEnMesActual(int dia)
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, dia).Date;
        }
    }
}
