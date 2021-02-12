// Copyright (c) 2021 Muddy Boots Software Ltd.

using System;
using System.Collections.Generic;

namespace EnforcerDI.Entities
{
    public class LocationRepository
    {
        private const int LocationCount = 1_000;

        private readonly Lazy<List<Location>> _locations;

        private readonly Random _random = new();

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
                // 25% chance of being the special-case supplier ID, otherwise a random value that isn't the
                // supplier ID. Note that the ExampleSupplierId value must be less than the percentage for
                // this to work.
                var random = _random.Next(100);
                var supplierId = random < 25 ? Constants.ExampleSupplierId : random;
                var l = new Location { Id = i, SupplierId = supplierId, LocationName = Guid.NewGuid().ToString() };
                locations.Add(l);
            }

            return locations;
        }
    }
}
