// Copyright (c) 2021 Muddy Boots Software Ltd.

namespace EnforcerDI
{
    public static class Constants
    {
        public static class ResourceTypes
        {
            public const string Location = "GLQC:Location";
        }

        public static class CustomAttributes
        {
            public static class Location
            {
                public const string Name = "locationName";
                public const string SupplierId = "locationSupplierId";
            }

            public static class Subject
            {
                public const string Level = "subjectLevel";
                public const string Id = "subjectId";
                public const string SupplierPermissions = "subjectSupplierPermissions";
            }
        }

        public static class Actions
        {
            public const string Read = nameof(Read);
        }

        public static class UserLevel
        {
            public const int Standard = 3;
            public const int Supplier = 9;
        }

        public static class UserId
        {
            public const int Standard = 1;
            public const int Supplier = 2;
        }

        public const int ExampleSupplierId = 17;
    }
}
