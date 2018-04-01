using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleArchitectureWeb.Mappings
{
    /// <summary>
    /// Configuration of files  mapping  
    /// </summary>
    public static class AutoMapperConfiguration
    {
        /// <summary>
        /// Initialize mapping files
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<AdministrationMappingProfile>();
            });
        }
    }
}
