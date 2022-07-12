using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using PartyFinder.GUI.Main;

namespace PartyFinder;

// ReSharper disable once UnusedType.Global
public sealed class PartyFinder : IDalamudPlugin
{
    public string Name => "PartyFinder";

    private readonly WindowSystem windowSystem;

    public PartyFinder(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();

        Service.Configuration = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Service.Configuration.Initialize();

/*        IPC.Initialize();*/

        Service.Commands = new Commands();

        Service.MainWindow = new MainWindow();

        this.windowSystem = new WindowSystem("PartyFinder");
        this.windowSystem.AddWindow(Service.MainWindow);

        Service.Interface.UiBuilder.Draw += this.windowSystem.Draw;
    }

    public void Dispose()
    {
/*        IPC.Dispose();*/
        Commands.Dispose();

        Service.Interface.UiBuilder.Draw -= this.windowSystem.Draw;
    }

}
