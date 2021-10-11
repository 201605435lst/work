using Volo.Abp.Settings;

namespace MyCompanyName.MyProjectName.Settings
{
    public class MyProjectNameSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            context.Add(
                new SettingDefinition("Smtp.Host", "127.0.0.1"),
                new SettingDefinition("Smtp.Port", "25"),
                new SettingDefinition("Smtp.UserName"),
                new SettingDefinition("Smtp.Password", isEncrypted: true),
                new SettingDefinition("Smtp.EnableSsl", "false")
            );
        }
    }
}
