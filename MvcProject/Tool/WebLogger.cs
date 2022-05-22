using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.Tool
{
    public static class WebLogger
    {
        internal static readonly Logger GeneralLogger = LogManager.GetLogger("General");
        internal static readonly Logger WebError = LogManager.GetLogger("WebError");
    }
}