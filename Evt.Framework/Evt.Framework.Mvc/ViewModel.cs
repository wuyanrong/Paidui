using Evt.Framework.Common;

namespace Evt.Framework.Mvc
{
    /// <summary>
    /// 视图模型基类。
    /// </summary>
    [ModelBinder]
    public class ViewModel : IModel
    {
        public virtual void OnException(MessageException messageException)
        {
            throw messageException;
        }

        //public virtual void OnNullException(MessageException messageException)
        //{
        //    throw messageException;
        //}

        //public virtual void OnValidateException(MessageException messageException)
        //{
        //    throw messageException;
        //}
    }
}
