using Dalamud.Game.Command;

namespace PartyFinder;

public class Commands
{
    private const string CommandName = "/xpf";

    public Commands()
    {
        Service.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Toggle the test window.",
            ShowInHelp = true,
        });
    }

    public static void Dispose()
    {
        Service.CommandManager.RemoveHandler(CommandName);
    }

    private static void OnCommand(string command, string args)
    {
        switch (command)
        {
            case CommandName when string.IsNullOrEmpty(args):
            case CommandName:
                Service.MainWindow.Toggle();
                break;
        }
    }
}
