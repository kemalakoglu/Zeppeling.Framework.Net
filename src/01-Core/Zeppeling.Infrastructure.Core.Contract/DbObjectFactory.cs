using Zeppeling.Framework.Core.Abstraction;
using Zeppeling.Infrastructure.Core.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Zeppeling.Infrastructure.Core.Contract
{
    public class DbObjectFactory : IDbObjectFactory
    {
        private IConfiguration Config;

        public DbObjectFactory(IConfiguration config)
        {
            this.Config = config;
        }

        public ICollection<IDbObject> GetDbObjects()
        {
            List<IDbObject> response = new List<IDbObject>();

            string[] castleConfigAllFiles = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""), "Zeppeling.*.config", SearchOption.AllDirectories);
            string[] castleConfigFiles = castleConfigAllFiles.Where(x => !x.Contains("Deployment")).ToArray();
            //XmlDocument doc = new XmlDocument();

            foreach (var item in castleConfigFiles)
            {
                XDocument configFile = XDocument.Load(item);
                List<ZeppelingInfrastructureConfiguration> items = (from _dbObject in configFile.Element("components").Elements("component")
                              select new ZeppelingInfrastructureConfiguration
                              {
                                  ContextKey = _dbObject.Element("ContextKey").Value,
                                  ContextName = _dbObject.Element("ContextName").Value,
                                  MockIntegration = bool.Parse(_dbObject.Element("MockIntegration").Value)
                              }).ToList();

                foreach (var config in items)
                {
                    ZeppelingInfrastructureContextFactory factory = new ZeppelingInfrastructureContextFactory(new ZeppelingInfrastructureConfiguration
                    {
                        ContextKey = config.ContextKey,
                        ContextName = config.ContextName,
                        MockIntegration = config.MockIntegration
                    }, this.Config);

                    response.Add(factory);
                }
            }

            return response;
        }
    }
}
