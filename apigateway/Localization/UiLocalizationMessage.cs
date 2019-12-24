namespace apigateway
{
    internal static class UiLocalizationMessage
    {
        public static string SuccessfulRequest => "Successful request";
        public static string AnErrorOccurred => "An error occurred";
        public static string AuthenticationFailedInvalidUserNameOrPassword =>
            "Authentication failed. Username or password is invalid";
        public static string RefreshUserTokenFailed => "Failed to refresh user token.";
        public static string RefreshUserTokenNotFound => "Unable to find this user or token";
        public static string RefreshUserTokenExpired =>
            "Unable to perform this action because your login session has expired";
        public static string RegisterFailedCannotCreateUser => "Registration failed. Cannot create user";
        public static string RegisterFailedUserNameUsed => "Registration failed. This username has already been used";
        public static string RegisterFailedEmailUsed => "Registration failed. This email address has already been used";
        public static string AccountSuccessfullyCreated => "Your account was successfully created.";
    }
}
