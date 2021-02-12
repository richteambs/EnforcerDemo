// Copyright (c) 2021 Muddy Boots Software Ltd.

using System.Threading.Tasks;
using Rsk.Enforcer.PIP;
using Rsk.Enforcer.PolicyModels;

namespace EnforcerDI.PIP
{
    public abstract class DecoratorProxyAttributeValueProvider<TSource, TProxy> : RecordAttributeValueProvider<TProxy>
        where TProxy : IDecoratorProxy<TSource>, new()
    {
        private readonly TSource _source;

        public DecoratorProxyAttributeValueProvider(TSource source)
        {
            _source = source;
        }

        protected override Task<TProxy> GetRecordValue(IAttributeResolver attributeResolver)
        {
            var result = new TProxy();
            result.SetData(_source);
            return Task.FromResult(result);
        }
    }
}
