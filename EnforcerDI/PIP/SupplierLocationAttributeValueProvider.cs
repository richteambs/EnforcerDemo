// Copyright (c) 2021 Muddy Boots Software Ltd.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rsk.Enforcer.PDP;
using Rsk.Enforcer.PIP;
using Rsk.Enforcer.PolicyModels;

namespace EnforcerDI.PIP
{
    public class UserLocationData
    {
        public UserLocationData(IEnumerable<long> userLocations)
        {
            UserLocations = userLocations;
        }

        [PolicyAttributeValue(PolicyAttributeCategories.Subject, Constants.CustomAttributes.Subject.LocationPermissions,
            Sensitivity = PolicyAttributeSensitivity.NonSensitive)]
        public IEnumerable<long> UserLocations { get; }
    }

    public class SupplierLocationAttributeValueProvider : RecordAttributeValueProvider<UserLocationData>
    {
        private static readonly PolicyAttribute SubjectId = new(Constants.CustomAttributes.Subject.Id,
            PolicyValueType.Integer, PolicyAttributeCategories.Subject);

        private readonly UserLocationPermissionsCache _permissionsCache;

        public SupplierLocationAttributeValueProvider(UserLocationPermissionsCache permissionsCache)
        {
            _permissionsCache = permissionsCache;
        }

        protected override async Task<UserLocationData> GetRecordValue(IAttributeResolver attributeResolver)
        {
            var resourceValues = await attributeResolver.Resolve<long>(SubjectId);

            var subjectId = resourceValues.Single();

            var suppliers = _permissionsCache.GetPermittedGroups(subjectId);

            return new UserLocationData(suppliers);
        }
    }
}
