using Dalamud.ContextMenu;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using PartyFinder.GUI.Config;
using PartyFinder.GUI.Main;
using PartyFinder.Manager;

namespace PartyFinder;

// ReSharper disable once UnusedType.Global
public sealed class PartyFinder : IDalamudPlugin
{
    public string Name => "PartyFinder";

    private readonly WindowSystem windowSystem;
    private readonly ContextMenu contextMenu;

    public PartyFinder(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();

        Service.Configuration = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Service.Configuration.Initialize();

        IPC.Initialize();

        Service.Commands = new Commands();
        Service.GameDataManager = new GameDataManager();
        Service.CharDataManager = new CharDataManager();
        Service.PartyListManager = new PartyListManager();
        Service.FfLogsClient = new FFLogsClient();

        Service.MainWindow = new MainWindow();
        Service.ConfigWindow = new ConfigWindow();
        this.windowSystem = new WindowSystem("PartyFinder");
        this.windowSystem.AddWindow(Service.ConfigWindow);
        this.windowSystem.AddWindow(Service.MainWindow);

        Service.ContextMenuBase = new DalamudContextMenuBase();
        this.contextMenu = new ContextMenu();

        Service.Interface.UiBuilder.OpenConfigUi += OpenConfigUi;
        Service.Interface.UiBuilder.Draw += this.windowSystem.Draw;
    }

    public void Dispose()
    {
        IPC.Dispose();
        Commands.Dispose();
        Service.ContextMenuBase.Dispose();
        this.contextMenu.Dispose();
        Service.GameDataManager.Dispose();

        Service.Interface.UiBuilder.OpenConfigUi -= OpenConfigUi;
        Service.Interface.UiBuilder.Draw -= this.windowSystem.Draw;
    }

    private static void OpenConfigUi()
    {
        Service.ConfigWindow.IsOpen = true;
    }
}
