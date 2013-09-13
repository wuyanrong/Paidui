using System;
using System.Web.Mvc;

namespace Evt.Framework.Mvc
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class ModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new ModelBinder();
        }
    }
}
