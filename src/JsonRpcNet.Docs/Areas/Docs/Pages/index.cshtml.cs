using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace JsonRpcNet.Docs.MyFeature.Pages
{
    public class JsonRpcDocsModel : PageModel
    {
        private readonly bool Enabled = false;

        public JsonRpcDocsModel(IOptions<JsonRpcNetDocsOptions> options)
        {
            Enabled = options.Value.Enabled;
        }

        public IActionResult OnGet()
        {
            if (!Enabled)
            {
                return NotFound();
            }

            return Page();
        }
    }
}