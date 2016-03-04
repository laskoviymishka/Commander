// -----------------------------------------------------------------------
// <copyright file="CommandArgument.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander.Arguments
{
    #region usings

    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    #endregion

    public class CommandArgument : IArgument
    {
        public CommandArgument()
        {
            Values = new List<string>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public IList<string> Values { get; }
        public bool MultipleValues { get; set; }
        public string Value => Values.FirstOrDefault();
    }
}