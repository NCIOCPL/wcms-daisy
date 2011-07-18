using System;
using System.IO;
using NCI.Logging;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;

namespace NCI.Web.CDE.Modules
{
    public class VelocityTemplate
    {
        public static string MergeTemplateWithResults(string template, object obj)
        {
            try
            {
                Velocity.Init();
                VelocityContext context = new VelocityContext();
                context.Put("DynamicSearch", obj);
                StringWriter writer = new StringWriter();
                Velocity.Evaluate(context, writer, "", template);
                return writer.GetStringBuilder().ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError("VelocityTemplate:MergeTemplateWithResults", "Failed to when evaluating results template and object." ,NCIErrorLevel.Error);
                throw (ex);
            }
        }
    }
}
