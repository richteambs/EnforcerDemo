// Copyright (c) 2021 Muddy Boots Software Ltd.

using System.Threading.Tasks;
using Rsk.Enforcer.PDP;
using Rsk.Enforcer.PIP;
using Rsk.Enforcer.PolicyModels;

namespace EnforcerDI.PIP
{
    public class ActionAttributeValueProvider : RecordAttributeValueProvider<ActionAttributeValueProvider>
    {
        [PolicyAttributeValue(PolicyAttributeCategories.Action, "Action",
            Sensitivity = PolicyAttributeSensitivity.NonSensitive)]
        public string Action { get; }

        public ActionAttributeValueProvider(string action)
        {
            Action = action;
        }

        protected override Task<ActionAttributeValueProvider> GetRecordValue(IAttributeResolver attributeResolver)
        {
            return Task.FromResult(this);
        }
    }
}
