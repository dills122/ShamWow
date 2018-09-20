using FlubuCore.Context;
using FlubuCore.Scripting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Build
{
    class BuildScript : DefaultBuildScript
    {
        protected override void ConfigureBuildProperties(IBuildPropertiesContext context)
        {
            context.Properties.Set(BuildProps.CompanyName, "Flubu");
            context.Properties.Set(BuildProps.CompanyCopyright, "Copyright (C) 2010-2016 Flubu");
            context.Properties.Set(BuildProps.ProductId, "FlubuCoreExample");
            context.Properties.Set(BuildProps.ProductName, "FlubuCoreExample");
        }
        protected override void ConfigureTargets(ITaskContext session)
        {
            var compile = session.CreateTarget("compile")
              .SetDescription("Compiles the solution.")
              .AddCoreTask(x => x.Build("..\\ShamWow.sln"));
        }
    }
}
