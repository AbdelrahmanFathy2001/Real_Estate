namespace Project.DAL.AppMetaData
{
    public static class Router
    {
        public const string Root = "Api";
        public const string Version = "v1";
        public const string Rule = Root + "/" + Version + "/";

        public static class ClientRouting
        {
            public const string Prefix = Rule + "Clients/";
            public const string List = Prefix + "List";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string Create = Prefix + "Create";
            public const string Edit = Prefix + "Edit/{id:int}";
            public const string Delete = Prefix + "Delete/{id:int}";
            public const string Pagination = Prefix + "Pagination";

        }

        public static class RealEstateAdRouting
        {
            public const string Prefix = Rule + "RealEstateAds/";
            public const string List = Prefix + "List";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string Create = Prefix + "Create";
            public const string Edit = Prefix + "Edit/{id:int}";
            public const string Delete = Prefix + "Delete/{id:int}";
            public const string Pagination = Prefix + "Pagination";

        }


        public static class RealEstateAd
        {
            public const string Prefix = Rule + "RealEstateAd/";
            public const string List = Prefix + "List";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string Pagination = Prefix + "Pagination";

        }

        public static class AccountRouting
        {
            public const string Prefix = Rule + "Account/";
            public const string Register = Prefix + "Register";
            public const string List = Prefix + "List";
            public const string AddAcount = Prefix + "AddUser";
            public const string AddUserImage = Prefix + "AddUserImage";
            public const string changePassword = Prefix + "changePassword";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string GetUser = Prefix + "GetUser";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string Create = Prefix + "Create";
            public const string Edit = Prefix + "Edit";
            public const string DeleteAcount = Prefix + "Delete Account";
            public const string DeleteUser = Prefix + "Delete User";
            public const string UserActive = Prefix + "User Activation";
            public const string Pagination = Prefix + "Pagination";

        }
        public static class OfficeRouting
        {
            public const string Prefix = Rule + "Office/";
            public const string List = Prefix + "List";
            public const string GetById = Prefix + "GetById/id";
        }


        public static class AuthenticationRouting
        {
            public const string Prefix = Rule + "Authentication/";
            public const string SignIn = Prefix + "SignIn";
            public const string RefreshToken = Prefix + "RefreshToken";
            public const string SendCodeResetPassword = Prefix + "SendCodeResetPassword";
            public const string ResetPassword = Prefix + "ResetPassword";
            public const string ConfirmResetPassword = Prefix + "ConfirmResetPassword";
            public const string ConfirmEmail = "/Api/Authentication/ConfirmEmail";
            public const string GetById = Prefix + "GetById/{id:int}";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string Create = Prefix + "Create";
            public const string Edit = Prefix + "Edit";
            public const string Delete = Prefix + "Delete";
            public const string Pagination = Prefix + "Pagination";

        }

        public static class AuthorizationRouting
        {
            public const string Prefix = Rule + "Authorization/";
            public const string AddRole = Prefix + "AddRole";
            public const string ManageUserRole = Prefix + "ManageUserRole";
            public const string GetAllRoles = Prefix + "GetAllRoles";
            public const string GetUsersWithoutRoles = Prefix + "GetUsersWithoutRoles";
            public const string UpdateRole = Prefix + "UpdateRole";
            public const string UpdateUserRole = Prefix + "UpdateUserRole";
            public const string DeleteRole = Prefix + "DeleteRole";
            public const string GetByName = Prefix + "GetByName/{name:alpha}";
            public const string GetById = Prefix + "GetByName/{id:int}";
            public const string Pagination = Prefix + "Pagination";

        }


    }
}
