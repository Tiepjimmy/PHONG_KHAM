namespace ClinicManagement.WebApi.Infrastructure
{
    public class ServiceLocator(ServiceProvider currentServiceProvider)
    {
        private readonly ServiceProvider _currentServiceProvider = currentServiceProvider;
        private static ServiceProvider? _serviceProvider;

        public static ServiceLocator Current
        {
            get
            {
#pragma warning disable CS8604 // Possible null reference argument.
                return new ServiceLocator(_serviceProvider);
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }

        public static void SetLocatorProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetInstance(Type serviceType)
        {
            return _currentServiceProvider.GetService(serviceType) ?? new();
        }

#pragma warning disable CS8603 // Possible null reference return.
        public TService GetInstance<TService>() => _currentServiceProvider.GetService<TService>();
#pragma warning restore CS8603 // Possible null reference return.
    }
}
