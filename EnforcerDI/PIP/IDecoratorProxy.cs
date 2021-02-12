// Copyright (c) 2021 Muddy Boots Software Ltd.

namespace EnforcerDI.PIP
{
    public interface IDecoratorProxy<in T>
    {
        void SetData(T source);
    }
}
