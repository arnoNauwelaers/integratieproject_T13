﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using System.Data.Entity;

namespace DAL.EF
{
  public class PlatformRepository : IPlatformRepostiory
  {

    private BarometerDbContext ctx;

    public PlatformRepository(BarometerDbContext ctx)
    {
      this.ctx = ctx;
    }
    public Platform CreatePlatform(Platform p)
    {
      ctx.Platforms.Add(p);
      ctx.SaveChanges();
      return p;
    }

    public void DeletePlatform(int id)
    {
      DeletePlatform(ctx.Platforms.Find(id));
      
    }

    public void DeletePlatform(Platform p)
    {
      ctx.Platforms.Remove(p);
      ctx.SaveChanges();
    }

    public Platform ReadPlatform(int id)
    {
      return ctx.Platforms.Find(id);
    }

    public List<Platform> ReadPlatforms()
    {
      return ctx.Platforms.ToList();
    }

    public void UpdatePlatform(Platform p)
    {
            ctx.Entry(p).State = EntityState.Modified;
    }
  }
}
