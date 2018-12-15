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
            context.Properties.Set(BuildProps.CompanyName, "Nexsys Technologies");
            context.Properties.Set(BuildProps.CompanyCopyright, "Copyright (C) 2018 Nexsys");
            context.Properties.Set(BuildProps.ProductId, "ShamWowDoc");
            context.Properties.Set(BuildProps.ProductName, "ShamWow");
        }

        protected override void ConfigureTargets(ITaskContext context)
        {
            var compile = context.CreateTarget("compile")
                        .SetDescription("Compiles the solution.")
                        .AddCoreTask(x => x.Build("ShamWow.sln"));
        }
    }
}
