using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using DAL;
using DAL.EF;

namespace BL
{
  public class PlatformManager : IPlatformManager
  {
    private IPlatformRepostiory repo;
    public PlatformManager()
    {
      repo = new PlatformRepository();
    }
    public Platform AddPlatform(Platform p)
    {
      return repo.CreatePlatform(p);
    }

    public void ChangePlatform(Platform p)
    {
      repo.UpdatePlatform(p);
    }

    public List<Platform> GetPlatforms()
    {
      return repo.ReadPlatforms();
    }

    public void RemovePlatform(int i)
    {
      repo.DeletePlatform(i);
    }
  }
}
