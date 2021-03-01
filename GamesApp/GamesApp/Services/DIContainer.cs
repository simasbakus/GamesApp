using Autofac;
using GamesApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GamesApp.Services
{
    class DIContainer
    {
        private static IContainer _container;
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainPageViewModel>();

            builder.RegisterType<RepositoryGames>().As<IRepositoryGames>();
            builder.RegisterType<HttpService>().As<IHttpService>();

            _container = builder.Build();
        }

        public static object Resolve(Type typeName)
        {
            return _container.Resolve(typeName);
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
