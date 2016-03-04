// -----------------------------------------------------------------------
// <copyright file="IOption.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander.Abstractions
{
    #region usings

    using System.Collections.Generic;

    #endregion

    public interface IOption
    {
        string Description { get; set; }
        string LongName { get; set; }
        string ShortName { get; set; }
        string SymbolName { get; set; }
        string Template { get; set; }
        string ValueName { get; set; }
        IList<string> Values { get; }
        bool HasValue();
        bool Populate(string value);
        string Value();
    }
}