using GenericReositoryDll.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepositoryDll.Configuration
{
  public static class GenericRepositoryConfigurator
  {
    public static void InjectServices(IServiceCollection services , DbContext appContext)
    {
      services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
    }
  }
}
