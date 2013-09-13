using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Evt.Framework.Common;

namespace Evt.Framework.Mvc
{
    public class ModelBinder : IModelBinder
    {        
        object IModelBinder.BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IModel model = (IModel)Activator.CreateInstance(bindingContext.ModelType);

            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(model);
            foreach (PropertyDescriptor pd in pdc)
            {
                ValueProviderResult vpr = bindingContext.ValueProvider.GetValue(pd.Name);
                if (vpr == null) //如果Http请求未传入模型属性对应的Key，并且该属性不能为空，则抛出消息异常告知开发人员传入该Key
                {
                    foreach (Attribute attribute in pd.Attributes)
                    {
                        if (attribute is RequiredAttribute)
                        {
                            MessageException me = new MessageException(pd.Name, pd.Name + "键未被传入。");
                            model.OnException(me);
                            if (me.Handled)
                            {
                                return model;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    object propertyValue = vpr.ConvertTo(pd.PropertyType);
                    foreach (Attribute attribute in pd.Attributes)
                    {
                        ValidationAttribute vattr = attribute as ValidationAttribute;
                        if (vattr != null && !vattr.IsValid(propertyValue))
                        {
                            MessageException me = new MessageException(pd.Name, vattr.ErrorMessage);
                            model.OnException(me);
                            if (me.Handled)
                            {
                                return model;
                            }
                        }
                    }
                    pd.SetValue(model, propertyValue);
                }
            }

            return model;
        }
    }
}
