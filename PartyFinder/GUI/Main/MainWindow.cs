using Dalamud.Game.ClientState.Conditions;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using System.Numerics;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;

namespace PartyFinder.GUI.Main;

public class MainWindow : Window
{
    public static string? DutyType = null;
    public MainWindow()
        : base("PartyFinder##PartyFinderMainWindow")
    {
        this.RespectCloseHotkey = Service.Configuration.Style.IsCloseHotkeyRespected;

        this.Flags = Service.Configuration.Style.MainWindowFlags;

        this.ResetSize();
        
        var configuration = Service.Interface.GetPluginConfig() as Configuration ?? new Configuration();
        configuration.Initialize(Service.Interface);
        
        var imagePath = Path.Combine(Service.Interface.AssemblyLocation.Directory?.FullName!, "goat.png");
        var goatImage = Service.Interface.UiBuilder.LoadImage(imagePath);
        this.PluginUi = new PluginUI(configuration, goatImage);

    }

    public override bool DrawConditions()
    {
        if (Service.Configuration.HideInCombat && Service.Condition[ConditionFlag.InCombat])
        {
            return false;
        }

        return true;
    }

    public override void OnOpen()
    {
    }

    public override void Draw()
    {
        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("Duty Type"))
            {
                if (ImGui.MenuItem("Savage")) { DutyType = "savage"; }
                if (ImGui.MenuItem("Ultimate")) { DutyType = "ultimate"; }
                if (ImGui.MenuItem("Extreme")) { DutyType = "extreme"; }
                if (ImGui.MenuItem("Shity Unreal")) { DutyType = "unreal"; }
                if (ImGui.MenuItem("ERP")) { DutyType = "erp"; }
                ImGui.EndMenu();
            }

            ImGui.EndMenuBar();
        }

        if (DutyType != null)
        {
            bool res;
            res = DutyType.Equals("savage");
            if (res)
            {
                if (ImGui.BeginMenuBar())
                {
                    if (ImGui.BeginMenu("Select Savage"))
                    {
                        if (ImGui.MenuItem("P1S")) { /* Do stuff */ }
                        if (ImGui.MenuItem("P2S")) { /* Do stuff */ }
                        if (ImGui.MenuItem("P3S")) { /* Do stuff */ }
                        if (ImGui.MenuItem("P4S")) { /* Do stuff */ }
                        ImGui.EndMenu();
                    }

                    ImGui.EndMenuBar();
                }
            }

            res = DutyType.Equals("ultimate");
            if (res)
            {
                if (ImGui.BeginMenuBar())
                {
                    if (ImGui.BeginMenu("Select Savage"))
                    {
                        if (ImGui.MenuItem("UCoB")) { /* Do stuff */ }
                        if (ImGui.MenuItem("UwU")) { /* Do stuff */ }
                        if (ImGui.MenuItem("TEA")) { /* Do stuff */ }
                        if (ImGui.MenuItem("DSR")) { /* Do stuff */ }
                        ImGui.EndMenu();
                    }

                    ImGui.EndMenuBar();
                }
            }

            res = DutyType.Equals("erp");
            if (res)
            {
                if (ImGui.BeginMenuBar())
                {
                    if (ImGui.BeginMenu("Select Savage"))
                    {
                        if (ImGui.MenuItem("Cat Girls")) { /* Do stuff */ }
                        if (ImGui.MenuItem("Femroe")) { /* Do stuff */ }
                        ImGui.EndMenu();
                    }

                    ImGui.EndMenuBar();
                }
            }
        }

        if (ImGui.BeginMenuBar())
        {
            if (ImGui.BeginMenu("Data Center"))
            {
                if (ImGui.MenuItem("Chaos")) { /* Do stuff */ }
                if (ImGui.MenuItem("Light")) { /* Do stuff */ }
                ImGui.EndMenu();
            }

            ImGui.EndMenuBar();
        }

        ImGui.Text("Work in progress");
        ImGui.Button("Hello");
        ImGui.Image(goat);
    }

    public void SetErrorMessage(string message)
    {
    }

    public void ResetSize()
    {
        if (!Service.Configuration.Style.IsSizeFixed &&
            (Service.Configuration.Style.MainWindowFlags & ImGuiWindowFlags.AlwaysAutoResize) != 0)
        {
        }
    }
}
