using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using LunaBot.Database;
using LunaBot.ServerUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LunaBot.Modules
{
    class CommandsAdmin : ModuleBase<SocketCommandContext>
    {
        [Command("demote", RunMode = RunMode.Async)]
        public async Task DemoteAsync(IUser requestedUser)
        {
            SocketUser author = Context.User;

            // User to demote
            ulong parsedUserId = requestedUser.Id;

            using (DiscordContext db = new DiscordContext())
            {
                ulong userId = author.Id;
                if (db.Users.Where(x => x.DiscordId == userId).FirstOrDefault().Privilege < User.Privileges.Admin)
                {
                    Logger.Warning(author.Id.ToString(), "User tried to use demote command and failed");
                    await ReplyAsync($"Nice try. Dont want me calling your parents, right?");
                    return;
                }

                User user = db.Users.Where(x => x.DiscordId == parsedUserId).FirstOrDefault();
                {
                    if (user.Privilege == User.Privileges.User)
                    {
                        Logger.Info(author.Id.ToString(), $"User <@{requestedUser.Id}> isn't a mod.");
                        await ReplyAsync($"<@{requestedUser.Id}> isn't a `mod`.");

                        return;
                    }

                    user.Privilege = User.Privileges.User;

                    SocketGuildChannel channel = Context.Channel as SocketGuildChannel;
                    IReadOnlyCollection<SocketRole> guildRoles = channel.Guild.Roles;

                    List<SocketRole> roles = new List<SocketRole>()
                    {
                        guildRoles.Where(x => x.Name.Equals("Mod")).FirstOrDefault(),
                        guildRoles.Where(x => x.Name.Equals("Staff")).FirstOrDefault()
                    };

                    await channel.Guild.GetUser((ulong)parsedUserId).RemoveRolesAsync(roles);

                    Logger.Info(author.Id.ToString(), $"Demoted <@{requestedUser.Id}> from moderator");
                    await ReplyAsync($"<@{requestedUser.Id}> is no longer `moddlet`!");
                }

                db.SaveChanges();

                await BotReporting.ReportAsync(ReportColors.adminCommand,
                        (SocketTextChannel)Context.Channel,
                        $"Demote Command by {Context.User.Username}",
                        $"<@{requestedUser.Id}> has been demoted to user.",
                        Context.User,
                        (SocketUser)requestedUser).ConfigureAwait(false);
            }
        }

        [Command("promote", RunMode = RunMode.Async)]
        public async Task PromoteAsync(IUser requestedUser)
        {
            SocketUser author = Context.User;

            // User to ascend
            ulong parsedUserId = requestedUser.Id;

            using (DiscordContext db = new DiscordContext())
            {
                ulong userId = author.Id;
                if (db.Users.Where(x => x.DiscordId == userId).FirstOrDefault().Privilege < User.Privileges.Admin)
                {
                    Logger.Warning(author.Id.ToString(), "User tried to use ascend command and failed");
                    await ReplyAsync($"Nice try. Dont want me calling your parents, right?");
                    return;
                }

                User user = db.Users.Where(x => x.DiscordId == parsedUserId).FirstOrDefault();
                {
                    if (user.Privilege >= User.Privileges.Moderator)
                    {
                        Logger.Info(author.Id.ToString(), $"User <@{author.Id}> already mod or above.");
                        await ReplyAsync($"<@{author.Id}> is already `mod` or above.");

                        return;
                    }

                    user.Privilege = User.Privileges.Moderator;

                    SocketGuildChannel channel = Context.Channel as SocketGuildChannel;
                    IReadOnlyCollection<SocketRole> guildRoles = channel.Guild.Roles;

                    List<SocketRole> roles = new List<SocketRole>()
                    {
                        guildRoles.Where(x => x.Name.Equals("Mod")).FirstOrDefault(),
                        guildRoles.Where(x => x.Name.Equals("Staff")).FirstOrDefault()
                    };

                    await channel.Guild.GetUser(parsedUserId).AddRolesAsync(roles);

                    Logger.Info(author.Id.ToString(), $"Made <@{requestedUser.Id}> moderator");
                    await ReplyAsync($"SMACK! <@{requestedUser.Id}> has been made `moddlet`!");
                }

                db.SaveChanges();

                await BotReporting.ReportAsync(ReportColors.adminCommand,
                        (SocketTextChannel)Context.Channel,
                        $"Promote Command by {Context.User.Username}",
                        $"<@{requestedUser.Id}> has been promoted to moderator.",
                        Context.User,
                        (SocketUser)requestedUser).ConfigureAwait(false);
            }
        }

        [Command("purge", RunMode = RunMode.Async)]
        public async Task PurgeAsync()
        {
            SocketUser author = Context.User;

            using (DiscordContext db = new DiscordContext())
            {
                if (db.Users.Where(x => x.DiscordId == author.Id).FirstOrDefault().Privilege < User.Privileges.Admin)
                {
                    Logger.Debug(author.Username, "User attempted pruge command");
                    await ReplyAsync("Do you want to start a riot? ");
                }

                await BotReporting.ReportAsync(ReportColors.adminCommand,
                            (SocketTextChannel)Context.Channel,
                            $"Purge Command by {Context.User.Username}",
                            $"<@{Context.User.Id}> started a purge.",
                            Context.User).ConfigureAwait(false);

                SocketGuildChannel channel = Context.Channel as SocketGuildChannel;
                List<SocketGuildUser> users = channel.Guild.Users.ToList();

                await ReplyAsync("Let the purge begin! :trumpet: ");
                Logger.Debug(author.Username, "Purging the server!");

                DateTime twoWeeksAgo = DateTime.UtcNow.AddDays(-14);

                foreach (SocketGuildUser u in users)
                {
                    User databaseUser = db.Users.Where(x => x.DiscordId == u.Id).FirstOrDefault();

                    if (databaseUser == null)
                    {
                        Logger.Warning("System", $"{u.Username} not registered!");
                        continue;
                    }

                    if (databaseUser.Privilege >= User.Privileges.Moderator)
                    {
                        Logger.Info("System", $"Skipping: {u.Username}, user is moderator or higher.");
                        continue;
                    }

                    if (u.Id == 155149108183695360 || u.Id == UserIds.Luna)
                    {
                        Logger.Info("System", $"Skipping: {u.Username}, bot");
                        continue;
                    }

                    // check if user has messaged in the past 2 weeks. Kick if false
                    if (databaseUser.LastMessage.Subtract(twoWeeksAgo).TotalDays < 0)//&& databaseUser.TutorialFinished == true)
                    {
                        Thread.Sleep(500);
                        Logger.Info("System", $"Purging:  {u.Username} for inactivity.");
                        await KickUserHelper.KickAsync(channel as SocketTextChannel, u);
                    }
                    else if (databaseUser.TutorialFinished == false)
                    {
                        Logger.Verbose("System", $"Skipping: {u.Username}, tutorial not finished.");
                    }
                    else
                    {
                        Logger.Verbose("System", $"Skipping: {u.Username}, active user.");
                    }
                }

                await ReplyAsync("Purging finished. You all, are the lucky few...");
            }

        }

        /// <summary>
        /// Cleans the server of the following:
        /// - Removes under 18 years old from NSFW
        /// - Checks NSFW and MONK Tags (ToDo)
        /// - Removes old profiles (ToDo)
        /// </summary>
        /// <returns>Task with completion status</returns>
        [Command("cleanserver", RunMode = RunMode.Async)]
        public async Task CleanServerAsync()
        {
            SocketUser author = Context.User;

            await BotReporting.ReportAsync(ReportColors.adminCommand,
                        (SocketTextChannel)Context.Channel,
                        $"CleanServer Command by {Context.User.Username}",
                        $"",
                        Context.User).ConfigureAwait(false);

            using (DiscordContext db = new DiscordContext())
            {
                // Check Priviledges
                ulong userId = author.Id;
                if (db.Users.Where(x => x.DiscordId == userId).FirstOrDefault().Privilege < User.Privileges.Admin)
                {
                    Logger.Warning(author.Id.ToString(), "User tried to use ascend command and failed");
                    await ReplyAsync($"Although we appreciate that you want to clean the server, this vacuum requires special operator's license.");
                    return;
                }

                await ReplyAsync("Removing users from NSFW rooms...");

                // Iterate through the users and remove underaged users from NSFW rooms
                foreach (SocketGuildUser user in Context.Guild.Users)
                {
                    Logger.Verbose("System", $"Checking for {user.Nickname}");
                    User databaseUser = db.Users.Where(x => x.DiscordId == user.Id).First();
                    if (databaseUser.Age < 18 && databaseUser.Age > 0)
                    {
                        Logger.Verbose("System", $"User age {databaseUser.Age}");
                        SocketRole role = user.Roles.Where((r) => r.Name == "SFW").FirstOrDefault();
                        if(role == null)
                        {
                            await ReplyAsync($"{user.Username} is under 18 and in NSFW rooms. Adding SFW tab.");
                            await user.AddRoleAsync(Context.Guild.Roles.Where((r) => r.Name == "SFW").FirstOrDefault());
                        }
                        Logger.Verbose("System", "User underaged and not in NSFW rooms");
                    }
                    else
                    {
                        Logger.Verbose("System", "User not underaged.");
                    }
                }

                await ReplyAsync("Finished NSFW check.");

                await ReplyAsync("Removing unused rooms...");

                // Remove rooms of users no longer in the server
                int roomsRemovedCount = 0;
                foreach (SocketGuildChannel channel in Context.Guild.TextChannels.Where(x => x.Name.StartsWith("room-")))
                {
                    if (channel == null)
                        continue;

                    // Get user ID from room name
                    string roomUserId = channel.Name.Substring(5);

                    // If user isn't in the server anymore, remove the room
                    if (Context.Guild.GetUser(ulong.Parse(roomUserId)) == null)
                    {
                        Logger.Info("System", $"Found empty room {channel.Name}, total: {roomsRemovedCount}");
                        roomsRemovedCount++;
                        await channel.DeleteAsync();
                    }
                }

                await ReplyAsync($"Finished removing rooms. Total: {roomsRemovedCount}");

                // Fix monk and other roles
                
            }
        }

        [Command("fixrooms", RunMode = RunMode.Async)]
        public async Task FixRoomsAsync()
        {
            await ReplyAsync("Checking rooms...");

            int roomCount = 0;

            using (DiscordContext db = new DiscordContext())
            {
                foreach (SocketGuildUser user in Context.Guild.Users)
                {
                    Logger.Verbose("system", $"Room check for {user.Username}");
                    SocketTextChannel pRoom = Context.Guild.TextChannels.Where(x => x.Name.Contains(user.Id.ToString())).FirstOrDefault();

                    if (pRoom == null)
                    {
                        Logger.Verbose("system", $"No room found. Creating room.");
                        await ReplyAsync($"No room found for {user.Username}. Room created");
                        RestTextChannel newPersonalRoom = await RoomUtilities.CreatePersonalRoomAsync(Context.Guild, user);

                        db.Users.Where(u => u.DiscordId == user.Id).FirstOrDefault().PersonalRoom = newPersonalRoom.Id;

                        roomCount++;
                    }
                    else
                    {
                        db.Users.Where(u => u.DiscordId == user.Id).FirstOrDefault().PersonalRoom = pRoom.Id;
                    }
                }
            }

            await ReplyAsync($"Finished checking rooms. Rooms created: {roomCount}");
        }

        [Command("say", RunMode = RunMode.Async)]
        public async Task SayAsync([Remainder] string message)
        {
            using(DiscordContext db = new DiscordContext())
            {
                User databaseUser = db.Users.Where((u) => u.DiscordId == Context.User.Id).First();

                if(databaseUser == null)
                {
                    await ReplyAsync("Error, user not found. Please ask a staff member for assistance.");
                    return;
                }
                else if(databaseUser.Privilege < User.Privileges.Admin)
                {
                    await ReplyAsync("Nice try lowly human.");
                    return;
                }

                await Context.Message.DeleteAsync();

                await ReplyAsync(message);
            }
        }
    }
}
