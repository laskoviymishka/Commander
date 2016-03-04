// -----------------------------------------------------------------------
// <copyright file="IArgument.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander.Abstractions
{
    #region usings

    using System.Collections.Generic;

    #endregion

    public interface IArgument
    {
        string Name { get; set; }
        string Description { get; set; }
        IList<string> Values { get; }
        bool MultipleValues { get; set; }
        string Value { get; }
    }
}