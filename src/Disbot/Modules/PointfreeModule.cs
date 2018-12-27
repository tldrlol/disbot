using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord.Commands;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Disbot.Modules
{
    [Group("pointfree")]
    [UsedImplicitly]
    public class PointfreeModule : ModuleBase
    {
        [Command, Summary("Pipe Haskell code through pointfree.io")]
        [UsedImplicitly]
        public async Task Pointfree([Remainder] string code)
        {
            // make it work with code blocks, quoted strings
            code = new[] { '"', '`' }.Aggregate(code, (s, c) => s.Trim(c));

            using (var httpClient = new HttpClient())
            {
                var content = await httpClient.GetStringAsync($"http://pointfree.io/snippet?code={code}");
                var json = JsonConvert.DeserializeObject<JObject>(content);
                await ReplyAsync($"```haskell\n{json["code"]}```");
            }
        }
    }
}
