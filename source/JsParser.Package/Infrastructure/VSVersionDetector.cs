using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace JsParser.Package.Infrastructure
{
    public static class VSVersionDetector
    {
        public static bool IsVs2012(string version)
        {
            float v;
            if (float.TryParse(version, NumberStyles.Any, CultureInfo.InvariantCulture, out v))
            {
                return v >= 11;
            }

            return false;
        }
    }
}
