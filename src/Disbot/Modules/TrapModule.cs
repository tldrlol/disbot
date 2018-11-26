using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Discord.Commands;

using JetBrains.Annotations;

using Serilog;

namespace Disbot.Modules
{
    [Group("trap")]
    [UsedImplicitly]
    public class TrapModule : ModuleBase
    {
        private readonly Dictionary<string, string> _trapCards = new Dictionary<string, string>
        {
            ["whatswrongwithu"] = "https://images-cdn.9gag.com/photo/aPYyqKg_460s.jpg",
            ["bitch"] = "https://i.pinimg.com/originals/c4/d6/7e/c4d67e39c33fef32b17f2e5e1728f06c.jpg",
            ["sawit"] = "https://pm1.narvii.com/6714/c60057ad7768c4c04084bdf9f44e3b93409b0fed_hq.jpg",
            ["chuck"] = "https://cdn.instructables.com/FU8/FJQR/FSLF83DD/FU8FJQRFSLF83DD.LARGE.jpg",
            ["trump"] = "https://www.dailydot.com/wp-content/uploads/da3/a4/trumpcard.jpg",
            ["wtf"] = "https://pics.me.me/w-t-f-trap-card-when-a-post-or-statement-is-ridiculous-19249753.png",
            ["nou"] = "https://i.kym-cdn.com/photos/images/newsfeed/000/063/502/NU.jpg",
            ["cheese"] = "https://i.kym-cdn.com/photos/images/original/000/063/517/cheese_Trap_card-s333x493-14356-580.jpg",
            ["mungus"] = "https://i.redd.it/zxa9u3ku4kpz.jpg",
            ["thot"] = "https://scontent-atl3-1.cdninstagram.com/vp/51a6aaa44a67016b25bdd7ded76de877/5C45591D/t51.2885-15/e35/28154584_407651403009390_4297132354654175232_n.jpg"
        };

        [Command("list"), Alias("all"), Summary("Plays a trap card")]
        [UsedImplicitly]
        public Task List()
        {
            var keys = _trapCards.Select(x => x.Key);

            return ReplyAsync(string.Join(", ", keys));
        }

        [Command, Summary("Plays a trap card"), Priority(-1)]
        [UsedImplicitly]
        public async Task Trap([Remainder] string key)
        {
            if (!_trapCards.ContainsKey(key))
            {
                return;
            }

            var url = _trapCards[key];

            try
            {
                using (var client = new HttpClient())
                {
                    var req = await client.GetStreamAsync(url);

                    await Context.Channel.SendFileAsync(req, Path.GetFileName(url), $"{Context.User.Mention} played a trap card");

                    await Context.Message.DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed playing trap card {trapCard}", key);
            }
        }
    }
}
