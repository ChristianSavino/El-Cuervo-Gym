using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza.Responses
{
    public class CobranzaCorrectaModel : PageModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime SiguienteCuota { get; set; }

        public void OnGet(int id, string nombre, DateTime siguienteCuota)
        {
            Id = id;
            Nombre = nombre;
            SiguienteCuota = siguienteCuota;
        }
    }
}










