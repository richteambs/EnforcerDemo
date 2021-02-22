// Copyright (c) 2021 Muddy Boots Software Ltd.

using EnforcerDI.Entities;
using Rsk.Enforcer.PDP;
using Rsk.Enforcer.PIP;
using Rsk.Enforcer.PolicyModels;

namespace EnforcerDI.PIP
{
    public class LocationData : IDecoratorProxy<Location>
    {
        private Location _location;

        [PolicyAttributeValue(PolicyAttributeCategories.Resource, Constants.CustomAttributes.Location.Name,
            Sensitivity = PolicyAttributeSensitivity.NonSensitive)]
        public string Name => _location.LocationName;

        [PolicyAttributeValue(PolicyAttributeCategories.Resource, "ResourceType",
            Sensitivity = PolicyAttributeSensitivity.NonSensitive)]
        public string ResourceType => Constants.ResourceTypes.Location;

        [PolicyAttributeValue(PolicyAttributeCategories.Resource, Constants.CustomAttributes.Location.GroupId,
            Sensitivity = PolicyAttributeSensitivity.NonSensitive)]
        public int? GroupId => _location.GroupId;

        public void SetData(Location source)
        {
            _location = source;
        }
    }

    public class LocationAttributeValueProvider : DecoratorProxyAttributeValueProvider<Location, LocationData>
    {
        public LocationAttributeValueProvider(Location location) : base(location)
        {
        }
    }
}
