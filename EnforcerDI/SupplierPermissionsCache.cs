// Copyright (c) 2021 Muddy Boots Software Ltd.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EnforcerDI
{
    public class SupplierPermissionsCache
    {
        private readonly Lazy<Dictionary<long, HashSet<long>>> _userSupplierPermissions;

        public SupplierPermissionsCache()
        {
            _userSupplierPermissions = new Lazy<Dictionary<long, HashSet<long>>>(InitPermissionsDictionary,
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IEnumerable<long> GetSuppliers(long userId)
        {
            return !TryGetSuppliers(userId, out var suppliers) ? Enumerable.Empty<long>() : suppliers;
        }

        public bool TryGetSuppliers(long userId, out IEnumerable<long> suppliers)
        {
            if (!_userSupplierPermissions.Value.TryGetValue(userId, out var supplierPermissions))
            {
                suppliers = Enumerable.Empty<long>();
                return false;
            }

            suppliers = supplierPermissions;
            return true;
        }

        private Dictionary<long, HashSet<long>> InitPermissionsDictionary()
        {
            // Generate supplier permissions for each sample user. The standard user doesn't need explicit
            // supplier permissions because they implicitly have access to all suppliers.
            var permissionsDictionary = new Dictionary<long, HashSet<long>>
            {
                { Constants.UserId.Standard, new HashSet<long>() },
                { Constants.UserId.Supplier, new HashSet<long> { Constants.ExampleSupplierId } }
            };


            return permissionsDictionary;
        }
    }
}
