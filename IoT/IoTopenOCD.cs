using System;
using System.Diagnostics;

namespace IoT
{
    public class Dependencies
    {
        // Method to install Telnet using DISM
        public static bool? installTelnet()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("dism.exe", "/Online /Enable-Feature /FeatureName:TelnetClient")
            {
                RedirectStandardOutput = false,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas"  // Requires elevated permissions
            };

            try
            {
                using (Process process = Process.Start(procStartInfo))
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        Console.WriteLine(line);
                    }

                    process.WaitForExit();  // Wait for the DISM process to complete

                    // Check the exit code: 0 means success, other values indicate failure
                    return process.ExitCode == 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while trying to install Telnet: {ex.Message}");
                return null;  // Return null to indicate an error occurred
            }
        }

        // Method to check if Telnet is installed and install it if necessary
        public static bool? telnet()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("powershell.exe", "-Command \"Get-Command telnet\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            try
            {
                using (Process process = Process.Start(procStartInfo))
                {
                    bool found = false; // Flag to track if TelnetClient is found

                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        if (!string.IsNullOrEmpty(line) && line.Contains("telnet.exe"))
                        {
                            Console.WriteLine("      Telnet is installed. Doing nothing.\n");
                            found = true; // Set flag to true if TelnetClient is found
                        }
                    }

                    if (!found)
                    {
                        Console.WriteLine("      Telnet is not installed. Installing Telnet...");
                        installTelnet();
                        Console.WriteLine("      Telnet is installed...\n");
                    }

                    return found;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" An error occurred: " + ex.Message);
                return null; // Return null to indicate an error occurred
            }
        }
    }

    public class launchOCD
    {
        private static Process openOcdProcess = null;

        // Method to start OpenOCD with specified interface and target configuration
        public static int openOCD(string interfaceCFG, string targetCFG)
        {
            string openOcdPath = @"OpenOCD\bin\openocd.exe";
            string arguments = $@"-f OpenOCD\share\openocd\scripts\interface\{interfaceCFG} -f OpenOCD\share\openocd\scripts\target\{targetCFG}";
            openOcdProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = openOcdPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            try
            {
                openOcdProcess.Start();
                return 0; // Return 0 to indicate success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting OpenOCD: {ex.Message}");
                return -1; // Return -1 to indicate an error
            }
        }

        // Method to stop OpenOCD if it is running
        public static void stopOpenOCD()
        {
            if (openOcdProcess != null && !openOcdProcess.HasExited)
            {
                openOcdProcess.Kill();
                openOcdProcess = null;
            }
        }
    }

    public class telnet
    {
        // Method to start a Telnet session to 127.0.0.1 on port 4444
        public static int runTelnet()
        {
            string telnetPath = "cmd.exe";
            string telnetCommand = "telnet.exe 127.0.0.1 4444";
            string arguments = $"/K {telnetCommand}";

            Process telnetProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = telnetPath,
                    Arguments = arguments,
                    UseShellExecute = true  // Open in a new window
                }
            };

            try
            {
                telnetProcess.Start();
                return 0;  // Return 0 to indicate success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting Telnet: {ex.Message}");
                return -1;  // Return -1 to indicate an error
            }
        }
    }
}
