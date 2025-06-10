using Open.Nat;

namespace El_Cuervo_Gym_Web.Core.UPnP
{
    public class UpnpPortForwardingService : IHostedService
    {
        private readonly int _internalPort;
        private readonly int _externalPort;
        private const string Description = "ElCuervoGym Server";

        private NatDevice? _device;

        public UpnpPortForwardingService(IConfiguration configuration)
        {
            var portsString = configuration["Hosting:Ports"];
            var ports = portsString?.Split(';') ?? new[] { "5000", "7019" };

            _internalPort = int.Parse(ports[1]);
            _externalPort = _internalPort;
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
