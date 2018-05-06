using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL;

namespace BL
{
  public static class RepositoryFactory
  {

    private static BarometerDbContext ctx;
    private static IPlatformRepostiory platformRepo;
    private static UserRepository userRepo;
    private static AlertRepository alertRepo;
    private static ChartRepository chartRepo;
    private static ItemRepository itemRepo;
    private static SocialMediaRepository smRepo;
    private static DataRepository dataRepository;
    private static ChartItemDataRepository chartItemDataRepository;

        public static void CreateContext()
    {
      if (ctx == null)
      {
        ctx = new BarometerDbContext();
        platformRepo = new PlatformRepository(ctx);
        userRepo = new UserRepository(ctx);
        alertRepo = new AlertRepository(ctx);
        chartRepo = new ChartRepository(ctx);
        itemRepo = new ItemRepository(ctx);
        smRepo = new SocialMediaRepository(ctx);
        dataRepository = new DataRepository(ctx);
        chartItemDataRepository = new ChartItemDataRepository(ctx);
      }
      

    }

    public static IPlatformRepostiory CreatePlatformRepository()
    {
      return platformRepo;
    }

    public static UserRepository CreateUserRepository()
    {
      return userRepo;
    }

  
    public static SocialMediaRepository CreateSocialMediaRepository()
    {
      return smRepo;
    }

    public static AlertRepository CreateAlertRepository()
    {
      return alertRepo;
    }

    public static ChartRepository CreateChartRepository()
    {
      return chartRepo;
    }

    public static ItemRepository CreateItemRepository()
    {
      return itemRepo;
    }

    public static DataRepository CreateDataRepository()
    {
        return dataRepository;
    }

    public static ChartItemDataRepository CreateChartItemDataRepository()
    {
        return chartItemDataRepository;
    }

    }
}
