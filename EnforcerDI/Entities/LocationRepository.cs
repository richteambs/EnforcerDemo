// Copyright (c) 2021 Muddy Boots Software Ltd.

using System;
using System.Collections.Generic;

namespace EnforcerDI.Entities
{
    public class LocationRepository
    {
        private const int LocationCount = 1_000;

        private readonly Lazy<List<Location>> _locations;

        private readonly Random _random = new(43);

        public LocationRepository()
        {
            _locations = new Lazy<List<Location>>(GenerateLocations);
        }

        public IEnumerable<Location> GetAll()
        {
            return _locations.Value;
        }

        private List<Location> GenerateLocations()
        {
            var locations = new List<Location>();
            for (var i = 0; i < LocationCount; i++)
            {
                // 25% chance of being assigned to the special-case group ID, otherwise a random value that isn't the
                // group ID. Note that the ExampleGroupId value must be less than the percentage for
                // this to work.
                var random = _random.Next(99);
                var groupId = random < 25 ? Constants.ExampleGroupId : random;
                var l = new Location { Id = i, GroupId = groupId, LocationName = Guid.NewGuid().ToString() };
                locations.Add(l);
            }

            return locations;
        }
    }
}
