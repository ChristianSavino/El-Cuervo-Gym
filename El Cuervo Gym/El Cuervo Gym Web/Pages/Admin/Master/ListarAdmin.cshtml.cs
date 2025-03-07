using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Domain.Requests;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Master
{
    public class ListarAdminModel : PageModel
    {
        private readonly ICLogger _logger;
        private readonly IAdminService _adminService;

        public ListarAdminModel(ICLogger logger, IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        public class AdminFiltro
        {
            public string Usuario { get; set; }
            public int? NumAdmin { get; set; }
            public bool IncluirDadosDeBaja { get; set; }
        }
        
        [BindProperty(SupportsGet = true)]
        public AdminFiltro Filtro { get; set; }
        public List<DatosAdmin> Admins { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                FiltroAdmin filtroAdmin;
                if (Filtro != null)
                {
                    filtroAdmin = new FiltroAdmin
                    {
                        Usuario = Filtro.Usuario,
                        NumAdmin = Filtro.NumAdmin,
                        IncluirDadosDeBaja = Filtro.IncluirDadosDeBaja
                    };
                }
                else
                {
                    filtroAdmin = new FiltroAdmin()
                    {
                        IncluirDadosDeBaja = false
                    };
                }

                var admins = await _adminService.FiltrarAdmins(filtroAdmin);
                Admins = admins.ToList();
            }
            catch (Exception ex)
            {
                var contexto = "Listar Admin";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }
    }
}