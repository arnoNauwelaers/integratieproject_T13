﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using DAL;
using DAL.EF;
using DAL.Repositories;

namespace BL.Managers
{
  public class PlatformManager
  {
    private PlatformRepository repo;
    private ApplicationUserManager userManager;
    private static int idCounter = 0;
    public PlatformManager()
    {
      repo = RepositoryFactory.CreatePlatformRepository();
      userManager = new ApplicationUserManager();
    }

    public Platform MakePlatformWithID()
    {
      idCounter += 1;
      Platform p = new Platform() { Id = idCounter };
      return p;
      
    }
    public Platform AddPlatform(Platform p)
    {
      return repo.CreatePlatform(p);
    }

    public void ChangePlatform(Platform p)
    {
      repo.UpdatePlatform(p);
    }

    public Platform GetPlatform(int id)
    {
      return repo.ReadPlatform(id);
    }

    public List<Platform> GetPlatforms()
    {
      return repo.ReadPlatforms();
    }

    public void RemovePlatform(int i)
    {
      repo.DeletePlatform(i);
    }

    public void RemovePlatform(Platform p)
    {
      repo.DeletePlatform(p);
    }

 

    



  }
}
