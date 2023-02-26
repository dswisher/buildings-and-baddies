// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;

namespace BuildingsAndBaddies.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (var game = new Game1())
            {
                try
                {
                    game.Run();
                }
                catch (Exception ex)
                {
                    // TODO - improve fatal exception handling - how to see the exception?
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
