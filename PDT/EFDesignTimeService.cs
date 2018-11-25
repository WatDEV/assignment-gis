using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Internal;

namespace PDT
{
	public class EfDesignTimeService : IDesignTimeServices
	{
		public void ConfigureDesignTimeServices(IServiceCollection services)
		{
			new EntityFrameworkRelationalServicesBuilder(services).TryAddProviderSpecificServices(x =>
			{
				x.TryAddSingleton<INpgsqlOptions, NpgsqlOptions>(p =>
				{
					var dbOption = new DbContextOptionsBuilder()
						.UseNpgsql("connection string",
							ob => ob.UseNetTopologySuite()).Options;
					var npgOptions = new NpgsqlOptions();
					npgOptions.Initialize(dbOption);
					return npgOptions;
				});
			});
		}
	}
}
