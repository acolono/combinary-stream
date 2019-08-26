using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ganss.XSS;
using Microsoft.AspNetCore.Html;

namespace CombinaryStream.Extensions
{
    public static class SanitizerExtensions
    {
        public static IHtmlContent Sanitize(this string html) {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Remove("img");
            return new HtmlString(sanitizer.Sanitize(html));
        }
    }
}
