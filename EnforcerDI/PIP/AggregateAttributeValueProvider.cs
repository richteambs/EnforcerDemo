// Copyright (c) 2021 Muddy Boots Software Ltd.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rsk.Enforcer.PDP;
using Rsk.Enforcer.PIP;
using Rsk.Enforcer.PolicyModels;
using Rsk.Enforcer.Services.Logging;

namespace EnforcerDI.PIP
{
    public class AggregateAttributeValueProvider : IAttributeValueProvider
    {
        private readonly IList<IAttributeValueProvider> _valueProviders = new List<IAttributeValueProvider>();

        public async ValueTask<AttributeValueResult<T>> GetValue<T>(PolicyAttribute attribute,
            IAttributeResolver attributeResolver,
            IEnforcerLogger evaluationLogger)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            if (attribute.RuntimeType != typeof(T))
            {
                evaluationLogger.LogError(
                    $"{GetType().Name}: Requested attribute {attribute} does not match type requested {typeof(T).Name}");
                throw new ArgumentException("Requested attribute does not match type requested", nameof(attribute));
            }

            // Iterate through value providers and aggregate their results
            var sensitivity = PolicyAttributeSensitivity.NonSensitive;
            var values = Enumerable.Empty<T>();
            var foundValues = false;
            foreach (var provider in _valueProviders)
            {
                var valueResult = await provider.GetValue<T>(attribute, attributeResolver, evaluationLogger);
                sensitivity = MostSensitive(sensitivity, valueResult.Sensitivity);
                if (valueResult.HasValues)
                {
                    values = values.Concat(valueResult.Values);
                    foundValues = true;
                }
            }

            return foundValues
                ? new AttributeValueResult<T>(values.ToArray(), sensitivity)
                : AttributeValueResult<T>.CacheableEmpty;
        }

        public void AddValueProvider(IAttributeValueProvider valueProvider)
        {
            _valueProviders.Add(valueProvider);
        }

        private PolicyAttributeSensitivity MostSensitive(PolicyAttributeSensitivity current,
            PolicyAttributeSensitivity proposed)
        {
            return current switch
            {
                PolicyAttributeSensitivity.PII => PolicyAttributeSensitivity.PII,
                PolicyAttributeSensitivity.Sensitive => proposed == PolicyAttributeSensitivity.PII ? proposed : current,
                PolicyAttributeSensitivity.NonSensitive =>
                    // If not NonSensitive then it must be more sensitive
                    proposed != PolicyAttributeSensitivity.NonSensitive ? proposed : current,
                _ => throw new ArgumentOutOfRangeException(nameof(current), current, null),
            };
        }
    }
}
