// -----------------------------------------------------------------------
// <copyright file="Program.cs">
// Copyright © Andrei Tserakhau. All Rights Reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Commander.Example
{
    #region usings

    using System;
    using System.Linq;
    using Abstractions;
    using Builders;

    #endregion

    public class Program
    {
        public static void Main(params string[] args)
        {
            args = args.Any() ? args : new[] {"test", "subTest", "-p=asdasd"};
            CommandLineApplication
                .Create(application =>
                {
                    application
                        .Command("test", "test", command =>
                        {
                            var test = command.AddPromtOption("-t|--test", "test option with promt");

                            command.Command("subTest", "dsfasdf", subCommand =>
                            {
                                var test2 = subCommand.AddPromtOption("-p|--pppp", "test option with promt",
                                    CommandOptionType.MultipleValue);
                                subCommand.OnExecute(() =>
                                {
                                    Console.WriteLine(string.Join(", ", test2.Values));
                                    return 0;
                                });
                            })
                                .OnExecute(() =>
                                {
                                    Console.WriteLine(test.Value());
                                    return 0;
                                });
                        })
                        .Command("test2", "test2", command =>
                        {
                            var test = command.AddOption("-t|--test", "test option with promt");
                            command.OnExecute(() =>
                            {
                                Console.WriteLine(string.Join(", ", test.Values));
                                return 0;
                            });
                        });
                })
                .Execute(args);

            Console.ReadLine();
        }
    }
}