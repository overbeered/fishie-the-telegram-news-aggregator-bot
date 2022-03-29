namespace Fishie.Core.Configuration
{
    /// <summary>
    /// Сonfigurations for connecting to telegram
    /// </summary>
    public class СlientConfiguration
    {
        /// <summary>
        /// Your app's api https://my.telegram.org/auth?to=apps
        /// </summary>
        public string? ApiId { get; set; }

        /// <summary>
        /// Apihash of your application https://my.telegram.org/auth?to=apps
        /// </summary>
        public string? ApiHash { get; set; }

        /// <summary>
        /// Phone number https://my.telegram.org/auth?to=apps
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// First name, if sign-up is required
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name, if sign-up is required
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Password, if user has enabled 2FA
        /// </summary>
        public string? Password { get; set; }

    }
}
