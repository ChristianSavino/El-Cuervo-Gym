using Open.Nat;

namespace El_Cuervo_Gym_Web.Core.UPnP
{
    public class UpnpPortForwardingService : IHostedService
    {
        private readonly int _internalPort;
        private readonly int _externalPort;
        private const string Description = "GymServer";

        private NatDevice? _device;

        public UpnpPortForwardingService(IConfiguration configuration)
        {
            try
            {
                var portsString = configuration["Hosting:Ports"];
                Console.WriteLine($"[INFO] Leyendo puertos de configuración: {portsString}");

                var ports = portsString?.Split(';') ?? Array.Empty<string>();

                if (ports.Length < 2 ||
                    !int.TryParse(ports[1], out _internalPort) ||
                    _internalPort < 1 ||
                    _internalPort > 65535)
                {
                    Console.WriteLine("[WARN] Puerto inválido o no especificado. Usando valor por defecto: 7019");
                    _internalPort = 7019;
                }

                _externalPort = _internalPort;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al leer configuración de puertos: {ex.Message}. Usando valores por defecto.");
                _internalPort = _externalPort = 7019;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var discoverer = new NatDiscoverer();
                var cts = new CancellationTokenSource(5000);

                _device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                var existingMappings = await _device.GetAllMappingsAsync();
                var conflict = existingMappings.FirstOrDefault(m => m.PublicPort == _externalPort && m.Protocol == Protocol.Tcp);

                if (conflict != null)
                {
                    await _device.DeletePortMapAsync(conflict);
                }

                var mapping = new Mapping(Protocol.Tcp, _internalPort, _externalPort, 0, Description);
                await _device.CreatePortMapAsync(mapping);

                Console.WriteLine($"Port Forwading realizado en {_internalPort} como {Description}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al configurar UPnP: {ex.Message}");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_device != null)
                {
                    await _device.DeletePortMapAsync(new Mapping(Protocol.Tcp, _internalPort, _externalPort));
                    Console.WriteLine($"Puerto cerrado correctamente");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo eliminar el puerto: {ex.Message}");
            }
        }
    }
}
