﻿using System;
using System.Linq;
using Discord.WebSocket;
using LunaBot.Database;
using System.Collections.Generic;

namespace LunaBot.Commands
{
    [LunaBotCommand("adminset_Desc")]
    class AdminSetDescCommand : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {

            using (DiscordContext db = new DiscordContext())
            {
                long userId;

                if (message.MentionedUsers.Count > 0)
                {
                    userId = Convert.ToInt64(message.MentionedUsers.First().Id);
                }
                else
                {
                    userId = Convert.ToInt64(parameters[1]);
                }
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    if (user.Description == null)
                    {
                        Logger.Warning(message.Author.Username, $"user <@{userId}> description not found.");
                        message.Channel.SendMessageAsync($"<@{userId}> has no description. *Mysterious...*");

                        return;
                    }

                    Logger.Verbose(message.Author.Username, $"Setting description for {userId} to: ");
                    message.Channel.SendMessageAsync($"<@{userId}> describes themselve as: {user.Description}");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: `{message.Author}`");

            }
        }
    }

    [LunaBotCommand("adminset_Age")]
    class AdminSetAgeCommand : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {
            using (DiscordContext db = new DiscordContext())
            {
                long userId;

                if (message.MentionedUsers.Count > 0)
                {
                    userId = Convert.ToInt64(message.MentionedUsers.First().Id);
                }
                else
                {
                    userId = Convert.ToInt64(parameters[1]);
                }

                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    if (user.Age == 0)
                    {
                        Logger.Warning(message.Author.Username, $"user <@{userId}> age not set.");
                        message.Channel.SendMessageAsync($"<@{userId}> is ageless");

                        return;
                    }

                    Logger.Verbose(message.Author.Username, $"Looking for {userId} description.");
                    message.Channel.SendMessageAsync($"<@{userId}> is {user.Age} years old.");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: `{message.Author}`");

            }
        }
    }

    [LunaBotCommand("adminset_Lvl")]
    class AdminSetLvlCommand : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {

            using (DiscordContext db = new DiscordContext())
            {
                long userId;

                if (message.MentionedUsers.Count > 0)
                {
                    userId = Convert.ToInt64(message.MentionedUsers.First().Id);
                }
                else
                {
                    userId = Convert.ToInt64(parameters[1]);
                }
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    Logger.Warning(message.Author.Username, $"looking for user <@{userId}> level.");
                    message.Channel.SendMessageAsync($"<@{userId}> is level {user.Level}");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: `{message.Author}`");

            }
        }
    }

    [LunaBotCommand("adminset_xp")]
    class AdminSetXpCommand : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {

            using (DiscordContext db = new DiscordContext())
            {
                long userId;

                if (message.MentionedUsers.Count > 0)
                {
                    userId = Convert.ToInt64(message.MentionedUsers.First().Id);
                }
                else
                {
                    userId = Convert.ToInt64(parameters[1]);
                }
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    Logger.Warning(message.Author.Username, $"looking for user <@{userId}> xp.");
                    message.Channel.SendMessageAsync($"<@{userId}> has {user.Xp}xp");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: `{message.Author}`");

            }
        }
    }

    [LunaBotCommand("adminset_g")]
    class AdminSetGenderCommand : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {

            using (DiscordContext db = new DiscordContext())
            {
                long userId;

                if (message.MentionedUsers.Count > 0)
                {
                    userId = Convert.ToInt64(message.MentionedUsers.First().Id);
                }
                else
                {
                    userId = Convert.ToInt64(parameters[1]);
                }
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    Logger.Warning(message.Author.Username, $"looking for user @<{userId}> gender.");
                    message.Channel.SendMessageAsync($"<@{userId}> is `{user.Gender.ToString()}`");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: `{message.Author}`");

            }
        }
    }

    [LunaBotCommand("adminset_o")]
    class AdminSetOrientationCommand : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {

            using (DiscordContext db = new DiscordContext())
            {
                long userId;

                if (message.MentionedUsers.Count > 0)
                {
                    userId = Convert.ToInt64(message.MentionedUsers.First().Id);
                }
                else
                {
                    userId = Convert.ToInt64(parameters[1]);
                }
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    Logger.Warning(message.Author.Username, $"looking for user @<{userId}> orientation.");
                    message.Channel.SendMessageAsync($"<@{userId}> is {user.orientation.ToString()}");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: `{message.Author}`");

            }
        }
    }

    [LunaBotCommand("adminset_Fur")]
    class AdminSetFurCommand : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {

            using (DiscordContext db = new DiscordContext())
            {
                long userId;

                if (message.MentionedUsers.Count > 0)
                {
                    userId = Convert.ToInt64(message.MentionedUsers.First().Id);
                }
                else
                {
                    userId = Convert.ToInt64(parameters[1]);
                }
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    if (user.Description == null)
                    {
                        Logger.Warning(message.Author.Username, $"user <@{userId}> fur not found.");
                        message.Channel.SendMessageAsync($"<@{userId}> has no fur. Maybe they're invisible...");

                        return;
                    }

                    Logger.Verbose(message.Author.Username, $"Looking for {userId} fur.");
                    message.Channel.SendMessageAsync($"<@{userId}> is a {user.Fur}");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: `{message.Author}`");

            }
        }
    }

    [LunaBotCommand("adminset_Ref")]
    class AdminSetRefCommand : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {

            using (DiscordContext db = new DiscordContext())
            {
                long userId;

                if (message.MentionedUsers.Count > 0)
                {
                    userId = Convert.ToInt64(message.MentionedUsers.First().Id);
                }
                else
                {
                    userId = Convert.ToInt64(parameters[1]);
                }
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    if (user.Description == null)
                    {
                        Logger.Warning(message.Author.Username, $"user <@{userId}> ref not found.");
                        message.Channel.SendMessageAsync($"<@{userId}> has no ref. use this one instead -> :wolf:");

                        return;
                    }

                    Logger.Verbose(message.Author.Username, $"Looking for {userId} ref.");
                    message.Channel.SendMessageAsync($"<@{userId}> is a {user.Fur}");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: `{message.Author}`");

            }
        }
    }

    [LunaBotCommand("adminset_priv")]
    class AdminSetPrivilegeCommand : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {

            using (DiscordContext db = new DiscordContext())
            {
                long userId;

                if (message.MentionedUsers.Count > 0)
                {
                    userId = Convert.ToInt64(message.MentionedUsers.First().Id);
                }
                else
                {
                    userId = Convert.ToInt64(parameters[1]);
                }
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    Logger.Verbose(message.Author.Username, $"Looking for {userId} privilege.");
                    if (user.Privilege == User.Privileges.Admin || user.Privilege == User.Privileges.User)
                    {
                        message.Channel.SendMessageAsync($"<@{userId}> is a `{user.Privilege.ToString()}`");

                        return;
                    }

                    message.Channel.SendMessageAsync($"<@{userId}> is a `{user.Privilege.ToString()}`");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: `{message.Author}`");

            }
        }
    }

    [LunaBotCommand("set")]
    class AdminSetCommands : BaseCommand
    {
        public override void Process(SocketMessage message, string[] parameters)
        {
            // Check if command params are correct.
            if(parameters.Length < 3)
            {
                Logger.Verbose(message.Author.Username, "Failed database modify command");
                message.Channel.SendMessageAsync("Error: Wrong syntax, try !set `user` `property` `value`.");

                return;
            }

            string[] unparsedUserId = parameters[0].Split(new [] {'@', '>'});
            
            // Check if user attached is correct.
            if(unparsedUserId.Length < 2|| unparsedUserId[0] != "<" || unparsedUserId[1].Length != 18)
            {
                Logger.Verbose(message.Author.Username, "Failed database modify command");
                message.Channel.SendMessageAsync("Error: Command requires an attached `user` to command. Forgot the '@'?");

                return;
            }
            
            using (DiscordContext db = new DiscordContext())
            {
                long userId = Convert.ToInt64(message.Author.Id);
                User user = db.Users.FirstOrDefault(x => x.DiscordId == userId);
                if (user != null)
                {
                    switch (parameters[1].ToLower())
                    {
                        case "description":
                        case "desc":
                            user.Description = String.Join(" ", parameters.Skip(2).Where(s => !String.IsNullOrEmpty(s)));
                            message.Channel.SendMessageAsync($"Success: {parameters[0]}'s description updated");
                            break;
                        case "level":
                            if(int.TryParse(parameters[2], out int n))
                            {
                                Logger.Info(message.Author.Username, $"Changed user {parameters[0]}'s level from {user.Level} to {parameters[2]}");
                                user.Level = Convert.ToInt32(parameters[2]);
                                message.Channel.SendMessageAsync($"Success: {parameters[0]}'s level set to `{user.Level}`");
                            }
                            else
                            {
                                Logger.Warning(message.Author.Username, "Failed database set level command");
                                message.Channel.SendMessageAsync($"Error: Level requires a number to set. You gave: `{parameters[2]}`");
                            }
                            break;
                        case "xp":
                            if (int.TryParse(parameters[2], out int o))
                            {
                                Logger.Info(message.Author.Username, $"Changed user {parameters[0]}'s xp from {user.Xp} to {parameters[2]}");
                                user.Xp = Convert.ToInt32(parameters[2]);
                                message.Channel.SendMessageAsync($"Success: {parameters[0]}'s xp set to `{user.Xp}`");
                            }
                            else
                            {
                                Logger.Warning(message.Author.Username, "Failed database set xp command");
                                message.Channel.SendMessageAsync($"Error: XP requires a number to set. You gave: `{parameters[2]}`");
                            }
                            break;
                        case "age":
                            if (int.TryParse(parameters[2], out int m))
                            {
                                Logger.Info(message.Author.Username, $"Changed user {parameters[0]}'s age from {user.Age} to {parameters[2]}");
                                user.Age = Convert.ToInt32(parameters[2]);
                                message.Channel.SendMessageAsync($"Success: {parameters[0]}'s age set to `{user.Age}`");
                            }
                            else
                            {
                                Logger.Warning(message.Author.Username, "Failed database set age command");
                                message.Channel.SendMessageAsync($"Error: Age requires a number to set. You gave: `{parameters[2]}`");
                            }
                            break;
                        case "gender":
                            Predicate<SocketRole> genderFinder;
                            SocketRole gender;
                            SocketTextChannel channel = message.Channel as SocketTextChannel;
                            List<SocketRole> roles = channel.Guild.Roles.ToList();
                            SocketGuildUser discordUser = message.Author as SocketGuildUser;
                            switch (parameters[2].ToLower())
                            {
                                case "male":
                                case "m":
                                    genderFinder = (SocketRole sr) => { return sr.Name == "male"; };
                                    gender = roles.Find(genderFinder);
                                    discordUser.AddRoleAsync(gender);
                                    user.Gender = User.Genders.Male;
                                    break;
                                case "female":
                                case "f":
                                    genderFinder = (SocketRole sr) => { return sr.Name == "female"; };
                                    gender = roles.Find(genderFinder);
                                    discordUser.AddRoleAsync(gender);
                                    user.Gender = User.Genders.Female;
                                    break;
                                case "other":
                                case "o":
                                    genderFinder = (SocketRole sr) => { return sr.Name == "other"; };
                                    gender = roles.Find(genderFinder);
                                    discordUser.AddRoleAsync(gender);
                                    user.Gender = User.Genders.Other;
                                    break;
                                case "trans-male":
                                    genderFinder = (SocketRole sr) => { return sr.Name == "trans-male"; };
                                    gender = roles.Find(genderFinder);
                                    discordUser.AddRoleAsync(gender);
                                    user.Gender = User.Genders.TransM;
                                    break;
                                case "trans-female":
                                    genderFinder = (SocketRole sr) => { return sr.Name == "trans-female"; };
                                    gender = roles.Find(genderFinder);
                                    discordUser.AddRoleAsync(gender);
                                    user.Gender = User.Genders.TransF;
                                    break;
                                default:
                                    message.Channel.SendMessageAsync("I'm sorry I couldn't understand your message. Make sure the gender is either male, female, trans-male, trans-female, or other.\n" +
                                        $"You gave: {parameters[2]}");
                                    return;
                            }
                            Logger.Info(message.Author.Username, $"Changed user {parameters[0]}'s gender from {user.Gender} to {parameters[2]}");
                            message.Channel.SendMessageAsync($"Success: {parameters[0]}'s gender set to `{user.Gender}`");
                            break;
                        case "ref":
                            if (Uri.TryCreate(parameters[2], UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                            {
                                Logger.Info(message.Author.Username, $"Changed user {parameters[0]}'s ref from {user.Ref} to {parameters[2]}");
                                user.Ref = parameters[2];
                                message.Channel.SendMessageAsync($"Success: {parameters[0]}'s ref has been updated");
                            }
                            else
                            {
                                Logger.Warning(message.Author.Username, "Failed database set ref command");
                                message.Channel.SendMessageAsync($"Error: Ref sheet must be a link. You gave: `{parameters[2]}`");
                            }
                            break;
                        default:
                            Logger.Warning(message.Author.Username, "Failed database set command.");
                            message.Channel.SendMessageAsync($"Error: Could not find attribute {parameters[1]}. Check you syntax!");
                            return;
                    }

                    db.SaveChanges();
                    Logger.Verbose(message.Author.Username, $"Updated data for {userId}");

                    return;
                }

                Logger.Verbose(message.Author.Username, $"Failed to find user: {userId}");
                message.Channel.SendMessageAsync($"Failed to find user: {userId}");

                //Logger.Verbose(message.Author.Username, "Created User");
                //message.Channel.SendMessageAsync("Created User");

                Logger.Verbose("System", $"Updated information for user {message.Author}");
            }
        }
    }
}
