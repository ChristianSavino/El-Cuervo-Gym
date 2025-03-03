namespace El_Cuervo_Gym_Web.Core.Utils
{
    public static class Helper
    {
        public static string ValorCuotaParamName = "ValorCuota";
        public static string DiasMaxPermitidoParamName = "DiasMaxPermitidos";
        
        public static DateTime ObtenerProximoVencimientoDeCuota(DateTime fechaSubscripcion)
        {
            var proximaCuotaPago = fechaSubscripcion.AddMonths(1);
            if (proximaCuotaPago.Date < DateTime.Now.Date)
            {
                proximaCuotaPago = new DateTime(DateTime.Now.Year, DateTime.Now.Month, fechaSubscripcion.Day);
                proximaCuotaPago = proximaCuotaPago.AddMonths(1);
            }

            return proximaCuotaPago;
        }
    }
}
