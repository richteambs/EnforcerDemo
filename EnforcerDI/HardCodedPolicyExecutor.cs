// Copyright (c) 2021 Muddy Boots Software Ltd.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnforcerDI.Entities;
using Rsk.Enforcer.PolicyModels;

namespace EnforcerDI
{
    public class HardCodedPolicyExecutor
    {
        private readonly LocationRepository _locationRepository;
        private readonly UserLocationPermissionsCache _permissionsCache;

        public HardCodedPolicyExecutor(LocationRepository locationRepository, UserLocationPermissionsCache permissionsCache)
        {
            _locationRepository = locationRepository;
            _permissionsCache = permissionsCache;
        }

        public Task<IEnumerable<Location>> GetPermittedLocations(User user)
        {
            var locations = _locationRepository.GetAll();
            var permittedLocations = new List<Location>();

            foreach (var location in locations)
            {
                if (IsPermitted(user, location))
                {
                    permittedLocations.Add(location);
                }
            }

            return Task.FromResult((IEnumerable<Location>)permittedLocations);
        }

        private bool IsPermitted(User user, Location location)
        {
            if (user.UserLevel != Constants.UserLevel.Restricted)
            {
                return true;
            }


            if (!_permissionsCache.TryGetPermittedGroups(user.Id, out var suppliers))
            {
                return false;
            }

            return suppliers.Contains(location.GroupId);
        }
    }
}
