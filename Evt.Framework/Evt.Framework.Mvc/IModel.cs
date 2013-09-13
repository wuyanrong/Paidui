using Evt.Framework.Common;

namespace Evt.Framework.Mvc
{
    /// <summary>
    /// 代表一个模型。
    /// </summary>
    public interface IModel
    {
        void OnException(MessageException messageException);
    }
}
