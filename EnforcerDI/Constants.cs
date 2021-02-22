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
                public const string GroupId = "locationGroupId";
            }

            public static class Subject
            {
                public const string Level = "subjectLevel";
                public const string Id = "subjectId";
                public const string LocationPermissions = "subjectLocationPermissions";
            }
        }

        public static class Actions
        {
            public const string Read = nameof(Read);
        }

        public static class UserLevel
        {
            public const int Standard = 3;
            public const int Restricted = 9;
        }

        public static class UserId
        {
            public const int Standard = 1;
            public const int Restricted = 2;
        }

        public const int ExampleGroupId = 17;
    }
}
