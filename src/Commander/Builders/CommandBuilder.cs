// -----------------------------------------------------------------------
// <copyright file="CommandBuilder.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander.Builders
{
    #region usings

    using Abstractions;
    using Options;

    #endregion

    public static class CommandBuilder
    {
        public static ICommand AddConfirm(this ICommand command)
        {
            return command;
        }

        public static IOption AddOption(
            this ICommand command,
            string template,
            string description = null,
            CommandOptionType type = CommandOptionType.SingleValue)
        {
            return command.Option(new CommandOption(template, description, type));
        }

        public static IOption AddPromtOption(
            this ICommand command,
            string template,
            string description = null,
            CommandOptionType type = CommandOptionType.SingleValue)
        {
            return command.Option(new PromtOption(template, description, type));
        }

        public static ICommand AddHelp(this ICommand command, string help = null)
        {
            command.Option(new CommandOption("-h|--help", help));
            return command;
        }

        public static ICommand AddVersionInfo(this ICommand command, string versionInfo = null)
        {
            command.Option(new CommandOption("-v|--version", versionInfo));
            return command;
        }
    }
}