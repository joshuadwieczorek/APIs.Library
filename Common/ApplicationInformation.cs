using Microsoft.Extensions.Configuration;

namespace APIs.Library.Common
{
    public static class ApplicationInformation
    {
        public static string ApplicationName { get; private set; }
        public static string ApplicationType { get; private set; }
        public static string ApplicationVersion { get; private set; }
        public static string ApplicationDeploymentEnvironment { get; private set; }
        public static string BugSnagApiKey { get; private set; }


        public static void Initialize(IConfiguration configuration)
        {
            ApplicationName = configuration["AppName"];
            ApplicationType = configuration["AppType"];
            ApplicationVersion = configuration["AppReleaseVersion"];
            ApplicationDeploymentEnvironment = configuration["AppReleaseStage"];
            BugSnagApiKey = configuration["BugsnagApiKey"];
        }
    }
}