// Copyright (c) 2021 Muddy Boots Software Ltd.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EnforcerDI
{
    public class UserLocationPermissionsCache
    {
        private readonly Lazy<Dictionary<long, HashSet<long>>> _userLocationPermissions;

        public UserLocationPermissionsCache()
        {
            _userLocationPermissions = new Lazy<Dictionary<long, HashSet<long>>>(InitPermissionsDictionary,
                LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IEnumerable<long> GetPermittedGroups(long userId)
        {
            return !TryGetPermittedGroups(userId, out var permittedGroups) ? Enumerable.Empty<long>() : permittedGroups;
        }

        public bool TryGetPermittedGroups(long userId, out IEnumerable<long> permittedGroups)
        {
            if (!_userLocationPermissions.Value.TryGetValue(userId, out var groupPermissions))
            {
                permittedGroups = Enumerable.Empty<long>();
                return false;
            }

            permittedGroups = groupPermissions;
            return true;
        }

        private Dictionary<long, HashSet<long>> InitPermissionsDictionary()
        {
            // Generate group permissions for each sample user. The standard user doesn't need explicit
            // group permissions because they implicitly have access to all resources.
            var permissionsDictionary = new Dictionary<long, HashSet<long>>
            {
                { Constants.UserId.Standard, new HashSet<long>() },
                { Constants.UserId.Restricted, new HashSet<long> { Constants.ExampleGroupId } }
            };


            return permissionsDictionary;
        }
    }
}
