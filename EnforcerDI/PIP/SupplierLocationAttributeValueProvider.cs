// Copyright (c) 2021 Muddy Boots Software Ltd.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rsk.Enforcer.PDP;
using Rsk.Enforcer.PIP;
using Rsk.Enforcer.PolicyModels;

namespace EnforcerDI.PIP
{
    public class SupplierLocationData
    {
        public SupplierLocationData(IEnumerable<long> supplierLocations)
        {
            SupplierLocations = supplierLocations;
        }

        [PolicyAttributeValue(PolicyAttributeCategories.Subject, Constants.CustomAttributes.Subject.SupplierPermissions,
            Sensitivity = PolicyAttributeSensitivity.NonSensitive)]
        public IEnumerable<long> SupplierLocations { get; }
    }

    public class SupplierLocationAttributeValueProvider : RecordAttributeValueProvider<SupplierLocationData>
    {
        private static readonly PolicyAttribute SubjectId = new(Constants.CustomAttributes.Subject.Id,
            PolicyValueType.Integer, PolicyAttributeCategories.Subject);

        private readonly SupplierPermissionsCache _permissionsCache;

        public SupplierLocationAttributeValueProvider(SupplierPermissionsCache permissionsCache)
        {
            _permissionsCache = permissionsCache;
        }

        protected override async Task<SupplierLocationData> GetRecordValue(IAttributeResolver attributeResolver)
        {
            var resourceValues = await attributeResolver.Resolve<long>(SubjectId);

            var subjectId = resourceValues.Single();

            var suppliers = _permissionsCache.GetSuppliers(subjectId);

            return new SupplierLocationData(suppliers);
        }
    }
}
