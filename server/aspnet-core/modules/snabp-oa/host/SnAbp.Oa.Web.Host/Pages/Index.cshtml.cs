using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace SnAbp.Oa.Pages
{
    public class IndexModel : OaPageModel
    {
        public void OnGet()
        {
            
        }

        public async Task OnPostLoginAsync()
        {
            await HttpContext.ChallengeAsync("oidc");
        }
    }
}