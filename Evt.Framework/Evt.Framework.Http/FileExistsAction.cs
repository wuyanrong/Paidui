using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Web;

namespace Evt.Framework.Http
{
    public enum FileExistsAction
    {
        Overwrite,
        Append,
        Cancel,
    }
}
