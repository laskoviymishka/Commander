// -----------------------------------------------------------------------
// <copyright file="SelectCommandOption.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander.Options
{
    public class SelectCommandOption : CommandOption
    {
        public SelectCommandOption(string template, string description = null) : base(template, description)
        {
        }
    }
}