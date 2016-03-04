// -----------------------------------------------------------------------
// <copyright file="ICommand.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander.Abstractions
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #endregion

    public interface ICommand
    {
        IEnumerable<IArgument> Arguments { get; }
        IEnumerable<IOption> Options { get; }
        IEnumerable<ICommand> Commands { get; }
        IOption OptionHelp { get; }
        IOption OptionVersion { get; }
        string Template { get; set; }
        string Name { get; set; }
        string Symbol { get; set; }
        string Description { get; set; }
        ICommand Parent { get; set; }
        Func<int> Invoke { get; set; }
        IOption Option(IOption option);
        IOption Option(string template, string description);
        IOption Option(string template, string description, Action<IOption> configuration);
        IArgument Argument(IArgument argument);
        IArgument Argument(string name, string description);
        IArgument Argument(string name, string description, Action<IArgument> configuration);
        ICommand Command(string mame, string description, Action<ICommand> configure);
        void OnExecute(Func<int> invoke);
        void OnExecuteAsync(Func<Task<int>> invoke);
        int Execute(params string[] args);
        void ShowHelp();
        void ShowVersion();
        void ShowHint();
    }
}