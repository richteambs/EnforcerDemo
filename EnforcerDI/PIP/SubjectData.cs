// Copyright (c) 2021 Muddy Boots Software Ltd.

using EnforcerDI.Entities;
using Rsk.Enforcer.PDP;
using Rsk.Enforcer.PIP;
using Rsk.Enforcer.PolicyModels;

namespace EnforcerDI.PIP
{
    public class SubjectData : IDecoratorProxy<User>
    {
        private User _user;

        [PolicyAttributeValue(PolicyAttributeCategories.Subject, Constants.CustomAttributes.Subject.Id,
            Sensitivity = PolicyAttributeSensitivity.NonSensitive)]
        public int? Id => _user.Id;

        [PolicyAttributeValue(PolicyAttributeCategories.Subject, Constants.CustomAttributes.Subject.Level,
            Sensitivity = PolicyAttributeSensitivity.NonSensitive)]
        public int? Level => _user.UserLevel;

        public void SetData(User source)
        {
            _user = source;
        }
    }

    public class SubjectAttributeValueProvider : DecoratorProxyAttributeValueProvider<User, SubjectData>
    {
        public SubjectAttributeValueProvider(User source) : base(source)
        {
        }
    }
}
