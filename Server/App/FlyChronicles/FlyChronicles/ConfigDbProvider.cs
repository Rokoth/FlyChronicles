﻿using System;
using System.Linq;
using FlyChronicles.DB.Context;
using FlyCronicles.Contract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FlyChronicles
{
    public class ConfigDbProvider(Action<DbContextOptionsBuilder> options, IDeployService deployService) : ConfigurationProvider
    {
        private readonly Action<DbContextOptionsBuilder> _options = options;
        private readonly IDeployService _deployService = deployService;

        public override void Load()
        {
            try
            {
                LoadInternal();
            }
            catch
            {
                try
                {
                    _deployService.Deploy().Wait();
                    LoadInternal();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void LoadInternal()
        {
            var builder = new DbContextOptionsBuilder<DbPgContext>();
            _options(builder);

            using var context = new DbPgContext(builder.Options);
            var items = context.Settings
                .AsNoTracking()
                .ToList();

            foreach (var item in items)
            {
                Data.Add(item.ParamName, item.ParamValue);
            }
        }
    }
}
