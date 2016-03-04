// -----------------------------------------------------------------------
// <copyright file="CommandOption.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander.Options
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    #endregion

    public class CommandOption : IOption
    {
        public CommandOption(string template, string description = null,
            CommandOptionType optionType = CommandOptionType.SingleValue)
        {
            OptionType = optionType;
            Description = description;
            Template = template;
            Values = new List<string>();

            foreach (var part in Template.Split(new[] {' ', '|'}, StringSplitOptions.RemoveEmptyEntries))
            {
                if (part.StartsWith("--"))
                {
                    LongName = part.Substring(2);
                }
                else if (part.StartsWith("-"))
                {
                    var optName = part.Substring(1);

                    if (optName.Length == 1 && !IsEnglishLetter(optName[0]))
                    {
                        SymbolName = optName;
                    }
                    else
                    {
                        ShortName = optName;
                    }
                }
                else if (part.StartsWith("<") && part.EndsWith(">"))
                {
                    ValueName = part.Substring(1, part.Length - 2);
                }
                else
                {
                    throw new ArgumentException($"Invalid template pattern '{template}'", nameof(template));
                }
            }

            if (string.IsNullOrEmpty(LongName) && string.IsNullOrEmpty(ShortName) && string.IsNullOrEmpty(SymbolName))
            {
                throw new ArgumentException($"Invalid template pattern '{template}'", nameof(template));
            }
        }

        public CommandOptionType OptionType { get; set; }

        public string Description { get; set; }
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public string SymbolName { get; set; }
        public string Template { get; set; }
        public string ValueName { get; set; }
        public IList<string> Values { get; }
        public bool HasValue() => Values.Any();

        public virtual bool Populate(string value)
        {
            switch (OptionType)
            {
                case CommandOptionType.MultipleValue:
                    if (string.IsNullOrWhiteSpace(value)) return false;
                    Values.Add(value);
                    break;
                case CommandOptionType.SingleValue:
                    if (Values.Any() || string.IsNullOrWhiteSpace(value))
                    {
                        return false;
                    }

                    Values.Add(value);
                    break;
                case CommandOptionType.NoValue:
                    if (value != null)
                    {
                        return false;
                    }

                    // Add a value to indicate that this option was specified
                    Values.Add("on");
                    break;
                default:
                    break;
            }
            return true;
        }

        public string Value() => HasValue() ? Values[0] : null;

        private bool IsEnglishLetter(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }
}