using Evt.Framework.Common;
using Evt.Framework.DataAccess;

namespace Evt.Framework.Mvc
{
    /// <summary>
    /// 数据模型基类。
    /// </summary>
    [ModelBinder]
    public class DataModel : Model, IModel
    {
        public virtual void OnException(MessageException messageException)
        {
            throw messageException;
        }
    }
}
