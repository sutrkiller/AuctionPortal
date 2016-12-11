using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.AppInfrastructure;
using BL.Repositories;
using BL.Services;
using BrockAllen.MembershipReboot;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DAL.EF;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using UserAccount = Autentization.UserAccount;

namespace BL.Bootstrap
{
    public class BusinessLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(

                Component.For<Func<DbContext>>()
                         .Instance(() => new AuctionDbContext())
                         .LifestyleTransient(),

                Component.For<IUnitOfWorkProvider>()
                         .ImplementedBy<AppUnitOfWorkProvider>()
                         .LifestyleSingleton(),

                Component.For<IUnitOfWorkRegistry>()
                         .Instance(new HttpContextUnitOfWorkRegistry(new ThreadLocalUnitOfWorkRegistry()))
                         .LifestyleSingleton(),

                Component.For<IUserAccountRepository<UserAccount>>()
                         .ImplementedBy<UserAccountManager>()
                         .LifestyleTransient(),

                Component.For(typeof(IRepository<,>))
                         .ImplementedBy(typeof(EntityFrameworkRepository<,>))
                         .LifestyleTransient(),

                Classes.FromAssemblyContaining<AppUnitOfWork>()
                       .BasedOn(typeof(AppQuery<>))
                       .LifestyleTransient(),

                Classes.FromAssemblyContaining<AppUnitOfWork>()
                       .BasedOn(typeof(IRepository<,>))
                       .LifestyleTransient(),

                Classes.FromThisAssembly()
                       .BasedOn<BaseService>()
                       .WithServiceDefaultInterfaces()
                       .LifestyleTransient(),

                Classes.FromThisAssembly()
                       .InNamespace("BL.Facades")
                       .LifestyleTransient()
                );
        }
    }
}
