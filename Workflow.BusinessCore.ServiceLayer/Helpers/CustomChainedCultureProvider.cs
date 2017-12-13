using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Workflow.BusinessCore.ServiceLayer.Helpers
{
    /// <summary>
    /// CLasse custom permettant de gérer finement l'attribution de la culture de la requête.
    /// </summary>
    public class CustomChainedCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            // Accès à la culture par défaut de la requête
//            IRequestCultureFeature cultureFeature = httpContext.Features.Get<IRequestCultureFeature>();

            CookieRequestCultureProvider cookieCulture = new CookieRequestCultureProvider();
            AcceptLanguageHeaderRequestCultureProvider headerCulture = new AcceptLanguageHeaderRequestCultureProvider();

            // 1 - On teste les cookies
            ProviderCultureResult res = await cookieCulture.DetermineProviderCultureResult(httpContext);
            if (res != null)
                if ((Options == null) || Options.SupportedUICultures.Any(c => res.UICultures.Contains(c.TwoLetterISOLanguageName)))
                    return res;

            // 2 - On teste l'entête http
            res = await headerCulture.DetermineProviderCultureResult(httpContext);
            if (res != null)
                if ((Options == null) || Options.SupportedUICultures.Any(c => res.UICultures.Contains(c.TwoLetterISOLanguageName)))
                return res;

            // 3 - Valeur par défaut...
            // Par défaut en dur, à améliorer.
            return new ProviderCultureResult("en-US");
        }

    }
}
