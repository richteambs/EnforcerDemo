// Copyright (c) 2021 Muddy Boots Software Ltd.

using EnforcerDI.Entities;
using Rsk.Enforcer.PIP;

namespace EnforcerDI.PIP
{
    public static class EvaluationContextFactory
    {
        public static IAttributeValueProvider GetEvaluationContext(User user, Location location, string action)
        {
            var requestContext = new AggregateAttributeValueProvider();
            requestContext.AddValueProvider(new SubjectAttributeValueProvider(user));
            requestContext.AddValueProvider(new ActionAttributeValueProvider(action));
            requestContext.AddValueProvider(new LocationAttributeValueProvider(location));

            return requestContext;
        }
    }
}
