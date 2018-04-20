using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;

namespace BL
{
  public interface IPlatformManager
  {
    Platform AddPlatform(Platform p);
    List<Platform> GetPlatforms();
    void ChangePlatform(Platform p);
    void RemovePlatform(int i);
    Platform GetPlatform(int id);
  }
}
