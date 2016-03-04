// -----------------------------------------------------------------------
// <copyright file="PromtOption.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander.Options
{
    #region usings

    using System;
    using System.Linq;
    using Abstractions;

    #endregion

    public class PromtOption : CommandOption
    {
        private readonly string message;
        private readonly char[] separators;

        public PromtOption(string template, string description = null,
            CommandOptionType optionType = CommandOptionType.SingleValue)
            : base(template, description, optionType)
        {
            separators = new[] {' '};
            message = $"Please provide value for option {LongName}.";
        }

        public override bool Populate(string value)
        {
            if (base.Populate(value)) return true;

            Console.WriteLine(message);
            var readLine = Console.ReadLine();
            return readLine != null && readLine.Split().All(userValue => base.Populate(userValue));
        }
    }
}