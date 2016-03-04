// -----------------------------------------------------------------------
// <copyright file="CommandLineApplication.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander
{
    #region usings

    using System;
    using Abstractions;

    #endregion

    public class CommandLineApplication
    {
        public static ICommand Create(Action<ICommand> configure)
        {
            var app = new ConsoleCommand();
            configure(app);
            return app;
        }
    }
}