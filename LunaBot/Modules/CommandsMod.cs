﻿using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using LunaBot.Commands;
using LunaBot.Database;
using LunaBot.ServerUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LunaBot.Modules
{
    class CommandsMod : ModuleBase<SocketCommandContext>
    {
        [Command("set")]
        public async Task SetAsync(IUser requestedUser, string attribute, [Remainder] string content)
        {
            using (DiscordContext db = new DiscordContext())
            {
                SocketUser author = Context.User;

                // check privileges
                ulong userId = author.Id;
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if ((int)user.Privilege < 1)
                {
                    Logger.Warning(author.Username, "Not enough permissions.");
                    await ReplyAsync("Can't let you do that Dave.");
                    return;
                }

                // Modify given user
                userId = requestedUser.Id;
                user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    switch (attribute.ToLower())
                    {
                        case "description":
                        case "desc":
                            Logger.Info(author.Username, $"Changed user <@{requestedUser.Id}>'s description from {user.Description} to {content}");
                            user.Description = content;
                            await ReplyAsync($"Success: <@{requestedUser.Id}>'s description updated to {content}");
                            break;
                        case "fur":
                        case "f":
                            Logger.Info(author.Username, $"Changed user <@{requestedUser.Id}>'s fur from {user.Fur} to {content}");
                            user.Fur = content;
                            await ReplyAsync($"Success: <@{requestedUser.Id}>'s fur updated to {content}");
                            break;
                        case "level":
                        case "lvl":
                            if (int.TryParse(content, out int n))
                            {
                                Logger.Info(author.Username, $"Changed user <@{requestedUser.Id}>'s level from {user.Level} to {content}");
                                user.Level = Convert.ToInt32(content);
                                user.Xp = 0;
                                await ReplyAsync($"Success: <@{requestedUser.Id}>'s level set to `{user.Level}`");
                            }
                            else
                            {
                                Logger.Warning(author.Username, "Failed database set level command");
                                await ReplyAsync($"Error: Level requires a number to set. You gave: `{content}`");
                            }
                            break;
                        case "xp":
                            if (int.TryParse(content, out int o))
                            {
                                Logger.Info(author.Username, $"Changed user <@{requestedUser.Id}>'s xp from {user.Xp} to {content}");
                                user.Xp = Convert.ToInt32(content);
                                await ReplyAsync($"Success: <@{requestedUser.Id}>'s xp set to `{user.Xp}`");
                            }
                            else
                            {
                                Logger.Warning(author.Username, "Failed database set xp command");
                                await ReplyAsync($"Error: XP requires a number to set. You gave: `{content}`");
                            }
                            break;
                        case "age":
                        case "a":
                            if (int.TryParse(content, out int m))
                            {
                                Logger.Info(author.Username, $"Changed user <@{requestedUser.Id}>'s age from {user.Age} to {content}");
                                user.Age = Convert.ToInt32(content);
                                await ReplyAsync($"Success: <@{requestedUser.Id}>'s age set to `{user.Age}`");
                            }
                            else
                            {
                                Logger.Warning(author.Username, "Failed database set age command");
                                await ReplyAsync($"Error: Age requires a number to set. You gave: `{content}`");
                            }
                            break;
                        case "gender":
                        case "g":
                            Predicate<SocketRole> genderFinder;
                            SocketRole gender;
                            SocketTextChannel channel = Context.Channel as SocketTextChannel;
                            List<SocketRole> roles = channel.Guild.Roles.ToList();
                            SocketGuildUser discordUser = requestedUser as SocketGuildUser;

                            User.Genders genderEnum = Utilities.StringToGender(content);
                            if (genderEnum == User.Genders.None)
                            {
                                await ReplyAsync("I'm sorry I couldn't understand your message. Make sure the gender is either male, female, trans-male, trans-female, or other.\n" +
                                        $"You gave: {content}");

                                return;
                            }

                            // Remove old role
                            genderFinder = (SocketRole sr) => { return sr.Name == user.Gender.ToString().ToLower(); };
                            gender = roles.Find(genderFinder);
                            if (gender == null)
                            {
                                Logger.Error("System", $"Could not find user gender {user.Gender.ToString().ToString()}");
                            }
                            else
                            {
                                await discordUser.RemoveRoleAsync(gender);
                            }

                            // Add new role
                            genderFinder = (SocketRole sr) => { return sr.Name == genderEnum.ToString().ToLower(); };
                            gender = roles.Find(genderFinder);
                            await discordUser.AddRoleAsync(gender);

                            user.Gender = genderEnum;
                            db.SaveChanges();

                            Logger.Info(author.Username, $"Changed user <@{requestedUser.Id}>'s gender from {user.Gender} to {content}");
                            await ReplyAsync($"Success: <@{requestedUser.Id}>'s gender set to `{user.Gender}`");
                            break;
                        case "orientation":
                        case "o":
                            Predicate<SocketRole> orientationFinder;
                            SocketRole orientation;
                            channel = Context.Channel as SocketTextChannel;
                            roles = channel.Guild.Roles.ToList();
                            discordUser = requestedUser as SocketGuildUser;

                            User.Orientation orientationEnum = Utilities.StringToOrientation(content);
                            if (orientationEnum == User.Orientation.None)
                            {
                                await ReplyAsync("Couldn't understand that gender... it can either be\n" +
                                    "```\n" +
                                    "- Male\n" +
                                    "- Female\n" +
                                    "- Trans-Female\n" +
                                    "- Transe-Male\n" +
                                    "- Other\n" +
                                    "```");

                                return;
                            }

                            // Remove old role
                            orientationFinder = (SocketRole sr) => { return sr.Name == user.orientation.ToString().ToLower(); };
                            orientation = roles.Find(orientationFinder);
                            if (orientation == null)
                            {
                                Logger.Error("System", $"Could not find user orientation {user.orientation.ToString().ToString()}");
                            }
                            else
                            {
                                await discordUser.RemoveRoleAsync(orientation);
                            }

                            // Add new role
                            orientationFinder = (SocketRole sr) => { return sr.Name == orientationEnum.ToString().ToLower(); };
                            orientation = roles.Find(orientationFinder);
                            await discordUser.AddRoleAsync(orientation);

                            user.orientation = orientationEnum;
                            db.SaveChanges();

                            Logger.Info(author.Username, $"Changed user <@{requestedUser.Id}>'s orientation to {user.orientation.ToString()}");
                            await ReplyAsync($"Success: <@{requestedUser.Id}>'s orientation set to `{user.orientation.ToString()}`");
                            break;

                        case "ref":
                        case "r":
                            if (Uri.TryCreate(content, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                            {
                                Logger.Info(author.Username, $"Changed user <@{requestedUser.Id}>'s ref from {user.Ref} to {content}");
                                user.Ref = content;
                                await ReplyAsync($"Success: <@{requestedUser.Id}>'s ref has been updated");
                            }
                            else
                            {
                                Logger.Warning(author.Username, "Failed database set ref command");
                                await ReplyAsync($"Error: Ref sheet must be a link. You gave: `{content}`");
                            }
                            break;
                        default:
                            Logger.Warning(author.Username, "Failed database set command.");
                            await ReplyAsync($"Error: Could not find attribute {attribute}. Check you syntax!");
                            return;
                    }

                    db.SaveChanges();
                    Logger.Verbose(author.Username, $"Updated data for {userId}");

                    return;
                }

                Logger.Verbose(author.Username, $"Failed to find user: {userId}");
                await ReplyAsync($"Failed to find user: {userId}");

                //Logger.Verbose(author.Username, "Created User");
                //ReplyAsync("Created User");

                Logger.Verbose("System", $"Updated information for user {author}");
            }
        }

        [Command("registerall")]
        public async Task RegisterAllAsync()
        {
            using (DiscordContext db = new DiscordContext())
            {
                SocketUser author = Context.User;
                ulong userId = author.Id;

                if (db.Users.Where(x => x.DiscordId == userId).FirstOrDefault().Privilege == User.Privileges.User)
                {
                    Logger.Warning(author.Username, "Failed RegisterAll command");
                    await ReplyAsync("You're not a moderator, go away.");

                    return;
                }

                Logger.Info(author.Username, "Fixing Registrations");
                await ReplyAsync("Fixing registrations...");

                SocketGuildChannel channel = Context.Channel as SocketGuildChannel;
                List<SocketGuildUser> users = channel.Guild.Users.ToList();

                foreach (SocketGuildUser u in users)
                {
                    if (db.Users.Where(x => x.DiscordId == u.Id).Count() == 0)
                    {
                        Logger.Verbose(author.Username, $"Creating User Data for {u.Username}");

                        User newUser = new User();
                        newUser.DiscordId = u.Id;
                        newUser.Level = 1;
                        newUser.Privilege = 0;
                        newUser.TutorialFinished = false;
                        newUser.Gender = User.Genders.None;
                        db.Users.Add(newUser);
                        var list = db.Users.ToList();

                        Logger.Verbose(author.Username, $"Created User {newUser.ID.ToString()}");

                    }
                }

                db.SaveChanges();

                await ReplyAsync("Finished registering users.");
                Logger.Info(author.Username, "Finished registering users.");

            }
        }

        [Command("forcetut")]
        public async Task ForcetutAsync(IUser requestedUser)
        {
            // User to forcetut
            ulong parsedUserId = requestedUser.Id;

            using (DiscordContext db = new DiscordContext())
            {
                ulong userId = Context.User.Id;
                SocketGuildUser discordUser = requestedUser as SocketGuildUser;

                if (db.Users.Where(x => x.DiscordId == userId).FirstOrDefault().Privilege == User.Privileges.User)
                {
                    Logger.Warning(Context.User.Id.ToString(), "User tried to use forcetut command and failed");
                    await ReplyAsync($"Nice try. Dont want me calling your parents, right?");
                    return;
                }

                RegisterCommand registerCommand = new RegisterCommand();
                User user = db.Users.Where(x => x.DiscordId == parsedUserId).FirstOrDefault();

                //Reset database entry for user
                user.ResetUser();

                // Register user in database
                registerCommand.manualRegister(discordUser);

                SocketGuildChannel channel = Context.Channel as SocketGuildChannel;
                IReadOnlyCollection<SocketRole> guildRoles = channel.Guild.Roles;

                SocketRole role = guildRoles.Where(x => x.Name.Equals("Newbie")).FirstOrDefault();

                await channel.Guild.GetUser((ulong)parsedUserId).AddRoleAsync(role);

                // Creat intro room
                RestTextChannel introRoom = await channel.Guild.CreateTextChannelAsync($"intro-{parsedUserId}");

                await Task.Run(async () =>
                {
                    // Make room only visible to new user, staff, and Luna
                    await introRoom.AddPermissionOverwriteAsync(discordUser, Permissions.userPerm);

                    // Start interaction with user. Sleeps are for humanizing the bot.
                    await introRoom.SendMessageAsync("Welcome to the server! Lets get you settled, alright?");
                    Thread.Sleep(1000);
                    await introRoom.SendMessageAsync("Firstly, what should we call you?");
                }).ConfigureAwait(false);

                db.SaveChanges();
            }
        }

        [Command("timeout")]
        public async Task TimeoutAsync(IUser requestedUser, int time)
        {
            using (DiscordContext db = new DiscordContext())
            {
                ulong userId = Context.User.Id;
                if (db.Users.Where(x => x.DiscordId == userId).FirstOrDefault().Privilege == 0)
                {
                    Logger.Warning(Context.User.Username, "Failed timout command. Not enough privileges.");
                    await ReplyAsync("You're not a moderator, go away.");

                    return;
                }

                await MuteUserHelper.MuteAsync(Context.Channel as SocketTextChannel, requestedUser as SocketGuildUser, time);

            }
        }
    }
}