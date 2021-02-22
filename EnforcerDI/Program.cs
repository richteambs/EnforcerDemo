// Copyright (c) 2021 Muddy Boots Software Ltd.

using System.Diagnostics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace EnforcerDI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            DebugInProcessConfig config = null;
            if (Debugger.IsAttached)
            {
                config = new DebugInProcessConfig();
            }

            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
        }
    }
}
