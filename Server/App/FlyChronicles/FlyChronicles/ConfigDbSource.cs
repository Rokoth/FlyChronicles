using System;
using FlyCronicles.Contract;
using FlyCronicles.DeployerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FlyChronicles
{
    public class ConfigDbSource(Action<DbContextOptionsBuilder> optionsAction, string connectionString) : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction = optionsAction;
        private readonly string _connectionString = connectionString;

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            IDeployService deployService = new DeployService(_connectionString);
            return new ConfigDbProvider(_optionsAction, deployService);
        }
    }
}
