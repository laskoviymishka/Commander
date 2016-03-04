// -----------------------------------------------------------------------
// <copyright file="ConsoleCommand.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander
{
    #region usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Abstractions;
    using Builders;

    #endregion

    public class ConsoleCommand : ICommand
    {
        private readonly IList<IArgument> _arguments;
        private readonly IList<ICommand> _commands;
        private readonly IList<IOption> _options;

        public ConsoleCommand()
        {
            _arguments = new List<IArgument>();
            _options = new List<IOption>();
            _commands = new List<ICommand>();
            Invoke = () => 0;
        }

        public IEnumerable<IArgument> Arguments => _arguments;
        public IEnumerable<IOption> Options => _options;
        public IEnumerable<ICommand> Commands => _commands;
        public IOption OptionHelp => _options.FirstOrDefault(t => t.ShortName == "h");
        public IOption OptionVersion => _options.FirstOrDefault(t => t.ShortName == "v");
        public string Template { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public ICommand Parent { get; set; }
        public Func<int> Invoke { get; set; }

        public void ShowHelp()
        {
        }

        public void ShowVersion()
        {
        }

        public void ShowHint()
        {
        }

        public int Execute(params string[] args)
        {
            try
            {
                return InternalExecute(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 1;
        }

        public IOption Option(IOption option)
        {
            _options.Add(option);
            return option;
        }

        public IOption Option(string template, string description)
        {
            return this.AddOption(
                template,
                description,
                template.IndexOf('<') != -1
                    ? CommandOptionType.SingleValue
                    : CommandOptionType.NoValue);
        }

        public IOption Option(string template, string description, Action<IOption> configuration)
        {
            var option = Option(template, description);
            configuration(option);
            return option;
        }

        public IArgument Argument(IArgument argument)
        {
            throw new NotImplementedException();
        }

        public IArgument Argument(string name, string description)
        {
            throw new NotImplementedException();
        }

        public IArgument Argument(string name, string description, Action<IArgument> configuration)
        {
            throw new NotImplementedException();
        }

        public ICommand Command(string name, string description, Action<ICommand> configure)
        {
            var command = new ConsoleCommand
            {
                Name = name,
                Description = description
            };

            configure(command);
            _commands.Add(command);

            return this;
        }

        public void OnExecute(Func<int> invoke)
        {
            Invoke = invoke;
        }

        public void OnExecuteAsync(Func<Task<int>> invoke)
        {
            Invoke = () => invoke().Result;
        }

        private int InternalExecute(string[] args)
        {
            ICommand command = this;
            IOption option = null;
            IEnumerator<IArgument> arguments = null;

            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                var processed = false;
                if (!processed && option == null)
                {
                    string[] optionValues = null;

                    if (arg.StartsWith("--"))
                    {
                        optionValues = arg.Substring(2).Split(new[] {':', '='}, 2);
                    }
                    else if (arg.StartsWith("-"))
                    {
                        optionValues = arg.Substring(1).Split(new[] {':', '='}, 2);
                    }

                    if (optionValues != null)
                    {
                        processed = true;
                        option = command.Options
                            .SingleOrDefault(
                                opt => string.Equals(opt.LongName, optionValues[0], StringComparison.Ordinal)
                                       || string.Equals(opt.ShortName, optionValues[0], StringComparison.Ordinal)
                                       || string.Equals(opt.SymbolName, optionValues[0], StringComparison.Ordinal));

                        if (option == null)
                        {
                            HandleUnexpectedArg(command, args, index, "option");
                            break;
                        }

                        if (command.OptionHelp == option)
                        {
                            command.ShowHelp();
                            return 0;
                        }

                        if (command.OptionVersion == option)
                        {
                            command.ShowVersion();
                            return 0;
                        }

                        if (!option.Populate(optionValues.Length == 2 ? optionValues[1] : null))
                        {
                            command.ShowHint();
                            throw new Exception(
                                $"Unexpected value '{string.Join(", ", option.Values)}' for option '{option.LongName}'");
                        }

                        option = null;
                    }
                }

                if (!processed && arguments == null)
                {
                    var currentCommand = command;
                    foreach (var subcommand in command.Commands)
                    {
                        if (string.Equals(subcommand.Name, arg, StringComparison.OrdinalIgnoreCase))
                        {
                            processed = true;
                            command = subcommand;
                            break;
                        }
                    }

                    if (command != currentCommand)
                    {
                        processed = true;
                    }
                }

                if (!processed)
                {
                    if (arguments == null)
                    {
                        arguments = new CommandArgumentEnumerator(command.Arguments.GetEnumerator());
                    }

                    if (arguments.MoveNext())
                    {
                        processed = true;
                        arguments.Current.Values.Add(arg);
                    }
                }

                if (processed) continue;
                HandleUnexpectedArg(command, args, index, "command or argument");
                break;
            }

            return command.Invoke();
        }

        private void HandleUnexpectedArg(ICommand command, string[] args, int index, string argTypeName)
        {
            command.ShowHint();
            Console.WriteLine($"Unrecognized {argTypeName} '{args[index]}'");
        }


        private class CommandArgumentEnumerator : IEnumerator<IArgument>
        {
            private readonly IEnumerator<IArgument> _enumerator;

            public CommandArgumentEnumerator(IEnumerator<IArgument> enumerator)
            {
                _enumerator = enumerator;
            }

            public IArgument Current
            {
                get { return _enumerator.Current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                _enumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (Current == null || !Current.MultipleValues)
                {
                    return _enumerator.MoveNext();
                }

                return true;
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }
    }
}