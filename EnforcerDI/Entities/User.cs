// Copyright (c) 2021 Muddy Boots Software Ltd.

using System.Diagnostics;

namespace EnforcerDI.Entities
{
    [DebuggerDisplay("{UserName} ({Id})")]
    public class User
    {
        public static readonly User StandardUser =
            new() { Id = 1, UserLevel = Constants.UserLevel.Standard, UserName = "Standard User" };

        public static readonly User SupplierUser =
            new() { Id = 2, UserLevel = Constants.UserLevel.Supplier, UserName = "Supplier User" };

        public int Id { get; set; }

        public int UserLevel { get; set; }

        public string UserName { get; set; }
    }
}
