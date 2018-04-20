using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;

namespace DAL
{
  public interface IPlatformRepostiory
  {
    List<Platform> ReadPlatforms();
    Platform CreatePlatform(Platform p);
    void UpdatePlatform(Platform p);
    void DeletePlatform(int id);
    Platform ReadPlatform(int id);
  }
}
