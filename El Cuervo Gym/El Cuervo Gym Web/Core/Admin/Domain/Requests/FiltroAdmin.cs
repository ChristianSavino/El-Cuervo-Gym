namespace El_Cuervo_Gym_Web.Core.Admin.Domain.Requests
{
    public class FiltroAdmin
    {
        public string Usuario { get; set; }
        public int? NumAdmin { get; set; }
        public bool IncluirDadosDeBaja { get; set; }
    }
}
