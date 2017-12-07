using Autofac;


namespace DependencyInjection
{
    public class AutofacRepository : IRepository
    {
        public string GetInfo()
        {
            return "autofac!";
        }
    }

    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacRepository>().As<IRepository>();
        }
    }
}
