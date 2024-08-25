namespace OrderingService.Utility
{
    public class StaticConfigurationManager
    {
        public static IConfiguration AppSetting
        {
            get;
        }

        static StaticConfigurationManager()
        {
            AppSetting = new ConfigurationManager()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
