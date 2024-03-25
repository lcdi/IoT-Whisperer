using System;
using System.IO;
using System.Linq;
using System.IO.Ports;
using System.Threading;

namespace IoT
{
    class Execution
    {
        static void help()
        {
            Console.WriteLine(" \n+------------------------------------+ Help Page +------------------------------------+\n");
            Console.WriteLine(" Welcome to the IoT Whisperer, a tool developed in house for the IoT team at The Leahy Center.\n");
            Console.WriteLine(" Info:");
            Console.WriteLine(" | This tool is capable of performing UART, JTag, SWD, SPI, and i2c");
            Console.WriteLine(" | You must use the select command, along with whatever debugging interface you desire to work with\n | Note that once a debugging interface is specified, help will offer protocol-specific commands.\n └──────────────────────────────────────────────────────────────────────────────────────────────────\n");
            Console.WriteLine("    Supported Interfaces:\n +-------------------------------+");
            Console.WriteLine("    UART, JTag, I2C, SWD, & SPI\n");
            Console.WriteLine(" To select a specific interface, pass the name of the interface\n     Ex. UART\n\n");
            Console.WriteLine(" Additional help:\n |Please add an issue or slack Tom\n └───────https://github.com/lcdi/IoT-Whisperer/issues\n\n");
            Console.WriteLine(" Version: 0.0.1");
            Console.WriteLine(" https://github.com/lcdi/IoT-Whisperer\n");
        }
        static void firstRun()
        {
            string[] iotLines = {
                "         ___     ___________        ",
                "        |   | ___\\__    ___/        ",
                "        |   |/     \\    |           ",
                "        |   (   O   )   |           ",
                "        |___|\\____/|____|           "
            };

            string[] whispererLines = {
                " __      __.__    .__               ",
                "/  \\    /  \\  |__ |__| ____________   ___________   ___________ ",
                "\\   \\/\\/   /  |  \\|  |/  ___/\\____ \\_/ __ \\_  __ \\_/ __ \\_  __ \\",
                " \\        /|   Y  \\  |\\___ \\ |  |_> >  ___/|  | \\/\\  ___/|  | \\/",
                "  \\__/\\  / |___|  /__/____  >|   __/ \\___  >__|    \\___  >__|   ",
                "       \\/       \\/        \\/ |__|        \\/            \\/       "
            };

            string[] badgeLines = {
                "   .:------------:.  ",
                "   -= .::.   .:===:  ",
                "   -++***++==+*+=+:  ",
                "   -=----=-=--=---.  ",
                "   --=:-====--=-=-:  ",
                "   --+-:=:==-==-+=:  ",
                "   -----+-+=-+=---.  ",
                "   .--=-+----=-=-:.  ",
                "     :--+-=--=-=.    ",
                "       .::=-:.       "
            };

            int maxLines = Math.Max(iotLines.Length + whispererLines.Length, badgeLines.Length);
            int badgeStartLine = (iotLines.Length + whispererLines.Length) - badgeLines.Length;

            for (int i = 0; i < maxLines; i++)
            {
                if (i < iotLines.Length)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(iotLines[i]);
                    Console.ResetColor();
                }
                else if (i < iotLines.Length + whispererLines.Length)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(whispererLines[i - iotLines.Length]);
                    Console.ResetColor();
                }

                // Adjust the space needed to align the badge on the right and add extra space on its right
                int extraRightSpace = 10; // Increase this value for more space to the right of the badge
                int spaceNeeded = Console.WindowWidth - (badgeLines[0].Length + extraRightSpace + (i < iotLines.Length ? iotLines[i].Length : whispererLines[i - iotLines.Length].Length));

                Console.Write(new string(' ', Math.Max(spaceNeeded, 0))); // Ensure spaceNeeded is not negative

                if (i >= badgeStartLine && i < badgeStartLine + badgeLines.Length)
                {
                    Console.ForegroundColor = i - badgeStartLine < 3 ? ConsoleColor.Green : ConsoleColor.Blue;
                    Console.WriteLine(badgeLines[i - badgeStartLine]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(); // Move to the next line if no badge line to print
                }
            }

            Console.WriteLine("       by CyberYom\n");
            Console.WriteLine(" | IoT\n| The Leahy Center\n| Version: 0.0.1\n└---------------------------------------------------------------------------");
            Console.Write(" Welcome to IoT Whisperer. Please pass the '");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("help");
            Console.ResetColor();
            Console.WriteLine(" ' command to view the help menu.\n");
        }



        static void Main(string[] args)
        {
            firstRun();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" Whisper");
                Console.ResetColor();
                Console.Write(" > ");
                string userInput = Console.ReadLine();

                if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(" Exiting IoT Whisperer...");
                    Environment.Exit(0); // Exit the application
                }

                switch (userInput.ToLower())
                {
                    case "help":
                        help();
                        break;


                    case "uart":
                        Console.Write(" Enter COM port (e.g., COM3): ");
                        string comPort = Console.ReadLine(); // Store user input as COM port
                        string[] portNames = SerialPort.GetPortNames();
                        if (Array.IndexOf(portNames, comPort) == -1)
                        {
                            Console.WriteLine($" The specified COM port '{comPort}' is not connected. Try {string.Join(", ", portNames)}\n");
                            return;
                        }

                        Console.WriteLine("\n UART has been selected as the target interface. Type 'help' to view available commands.\n");
                        int baudRate = 0; // Variable to store entered baud rate

                        while (true) // Start a new loop for UART mode
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" UART");
                            Console.ResetColor();
                            Console.Write(" > ");
                            string uartInput = Console.ReadLine()?.ToLower(); // Ensure input is case-insensitive

                            if (uartInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine(" Exiting IoT Whisperer...");
                                Environment.Exit(0); // Exit the application
                            }

                            if (uartInput == "help")
                            {
                                Console.WriteLine("\n +------------------------------------+ UART +------------------------------------+\n");
                                Console.WriteLine(" To connect to the UART interface, please ensure the following connections have been made:\n");
                                Console.ForegroundColor = ConsoleColor.Green; Console.Write("                         RX"); Console.ResetColor(); Console.Write("     --->     "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("TX\n"); Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Blue; Console.Write("                         TX"); Console.ResetColor(); Console.Write("     --->     "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("RX\n"); Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Red; Console.Write("                         GND"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("GND\n\n"); Console.ResetColor();
                                Console.WriteLine("      Supported Commands:");
                                Console.WriteLine(" baud      -  Used to find the baudrate of an IoT device.");
                                Console.WriteLine(" connect   -  Connect to the IoT device");
                                Console.WriteLine(" quit      -  Quit the UART mode and return to main menu\n\n");
                                Console.WriteLine(" Additional help:\n |Please add an issue or slack Tom\n └───────https://github.com/lcdi/IoT-Whisperer/issues\n\n");
                                Console.WriteLine(" Version: 0.0.1");
                                Console.WriteLine(" https://github.com/lcdi/IoT-Whisperer\n");
                            }
                            else if (uartInput == "baud")
                            {
                                baud.baudDetect(comPort);
                            }
                            else if (uartInput == "connect")
                            {
                                Console.Write("Enter Baudrate (e.g., 9600): ");
                                if (int.TryParse(Console.ReadLine(), out int enteredBaudRate))
                                {
                                    baudRate = enteredBaudRate; // Store user input as baud rate if it's a valid integer
                                    Console.WriteLine($"Connecting to {comPort} with baudrate {baudRate}...\n");
                                    IoTUART.uartFunction(comPort, baudRate);
                                }
                                else
                                {
                                    Console.WriteLine(" Invalid baud rate entered. Please enter a numeric value.");
                                }
                            }
                            else if (uartInput == "quit")
                            {
                                break; // Exit the UART loop
                            }
                            else
                            {
                                Console.WriteLine(" Unknown command. Type 'help' for available commands.");
                            }
                        }
                        break;

                    case "jtag":
                        Console.WriteLine(" JTag has been selected as the target interface. Type 'help' to view available commands.\n");

                        while (true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" JTag");
                            Console.ResetColor();
                            Console.Write(" > ");

                            string jtagInput = Console.ReadLine()?.ToLower(); // Ensure input is case-insensitive

                            if (jtagInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("  Exiting IoT Whisperer...\n");
                                launchOCD.stopOpenOCD();
                                Environment.Exit(0); // Exit the application
                            }

                            if (jtagInput == "help")
                            {
                                Console.WriteLine("\n +------------------------------------+ JTag +------------------------------------+\n");
                                Console.WriteLine(" To connect to the JTag interface, please ensure the following connections have been made:\n");
                                Console.ForegroundColor = ConsoleColor.Red; Console.Write("                         GND"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("GND\n"); Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Blue; Console.Write("                         TMS"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("TMS\n"); Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Green; Console.Write("                         TCK"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("TCK\n"); Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("                         TDI"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("TDI\n"); Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("                         TDO"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("TDO\n\n"); Console.ResetColor();
                                Console.WriteLine("      Supported Commands:");
                                Console.WriteLine("  start       -  Starts the OpenOCD server.         stop        -  Stops the OpenOCD server.");
                                Console.WriteLine("  jtagulate   -  Interface with JTagulator.         connect     -  Connect to OpenOCD server.");
                                Console.WriteLine("  exit        -  Exit IoT Whisperer.                quit        -  Exit JTag mode.\n\n");
                                Console.WriteLine("      OpenOCD Commands:");
                                Console.WriteLine("  halt        -  Stops the CPU in whatever state it is in.");
                                Console.WriteLine("  reset init  -  Reset the CPU into a known state.");
                                Console.WriteLine("  flash banks -  Reveal information about flash memory .");
                                Console.WriteLine("  reset       -  Resets the CPU.\n");
                                Console.WriteLine("  flash write_image erase data.bin 0x08000000\n       -  Used to write a binary image to a device. data.bin is the data to write and 0x08000000 is the address to start writing data to.");
                                Console.WriteLine("  verify_image data.bin 0x0800000\n       -  Used to verify an image write after writing an image. data.bin is the data that was written to the device, and 0x08000000 is where the data starts on the device.");
                                Console.WriteLine("  dump_image dataOut.bin 0x0800000 0x00010000\n       -  Used to dump a firmware image from a device. dataOut.bin is the output file for the data, 0x08000000 is where to start dumping data, and 0x00010000 is the size of the data to dump.\n\n");
                                Console.WriteLine("  Additional help:\n |Please add an issue or slack Tom\n └───────https://github.com/lcdi/IoT-Whisperer/issues\n");
                                Console.WriteLine("  Version: 0.0.1");
                                Console.WriteLine("  https://github.com/lcdi/IoT-Whisperer\n");
                            }

                            else if (jtagInput == "start")
                            {
                                Console.WriteLine(" Checking Dependancies...");
                                Dependencies.telnet();

                                string icfgPath = "OpenOCD\\share\\openocd\\scripts\\interface\\";
                                Console.Write(" Enter an interfacing CFG file: ");
                                string interfaceCFG = ReadLineWithTabCompletion(icfgPath).ToLower();

                                string tcfgPath = "OpenOCD\\share\\openocd\\scripts\\target\\";
                                Console.Write("\n Please enter a target cfg: ");
                                string targetCFG = ReadLineWithTabCompletion(tcfgPath).ToLower();

                                Console.WriteLine("\n Starting OpenOCD server on port 4444...\n");
                                Thread openOcdThread = new Thread(() => launchOCD.openOCD(interfaceCFG, targetCFG));
                                openOcdThread.Start();
                            }

                            else if (jtagInput == "stop")
                            {
                                Console.WriteLine(" Stopping OpenOCD server...\n");
                                launchOCD.stopOpenOCD();
                            }

                            else if (jtagInput == "connect")
                            {
                                Console.WriteLine(" Please be sure you have started the server...\n");
                                Console.WriteLine(" Connecting to 127.0.0.1...");
                                telnet.runTelnet();
                                Console.WriteLine("      Connection Successful\n");
                            }

                            else if (jtagInput == "jtagulate")
                            {
                                Console.WriteLine(" jtagulator");
                                baudRate = 115200;
                                Console.Write(" Enter COM port (e.g., COM3): ");
                                string comPortJT = Console.ReadLine(); // Store user input as COM port

                                Console.WriteLine("Please enter the COM");
                                Console.WriteLine($"Connecting to JTagulator on port {comPortJT}...\n");
                                IoTUART.uartFunction(comPortJT, baudRate);
                            }

                            else if (jtagInput == "quit")
                            {
                                // Additional command to quit JTAG mode and return to main menu
                                Console.WriteLine(" Exiting JTag mode...\n");
                                launchOCD.stopOpenOCD();
                                break;
                            }
                            else
                            {
                                Console.WriteLine(" Unknown command. Type 'help' for available commands.\n");
                            }
                        }
                        break;

                }
            }
            static string ReadLineWithTabCompletion(string basePath)
            {
                string input = "";
                List<string> matches = new List<string>();
                int matchIndex = -1;
                string currentInput = "";
                ConsoleKeyInfo keyInfo;  // Declare outside the loop

                do
                {
                    keyInfo = Console.ReadKey(intercept: true);  // Assign inside the loop

                    if (keyInfo.Key == ConsoleKey.Tab)
                    {
                        // Tab key logic
                        if (matchIndex == -1 || matches.Count == 0)
                        {
                            matches = Directory.GetFileSystemEntries(basePath, input + "*")
                                               .Select(Path.GetFileName)
                                               .ToList();
                            matchIndex = 0;
                        }
                        else
                        {
                            matchIndex = (matchIndex + 1) % matches.Count;
                        }

                        if (matches.Count > 0)
                        {
                            currentInput = matches[matchIndex];
                            input = currentInput;  // Synchronize input with the current match

                            // Clear the line and print the current match
                            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
                            Console.Write("Enter an interfacing CFG file: " + currentInput);
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        // Backspace key logic
                        if (currentInput.Length > 0)
                        {
                            currentInput = currentInput[..^1];
                            input = currentInput;
                            matchIndex = -1;  // Reset match index

                            // Clear the line and print the current input
                            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
                            Console.Write("Enter an interfacing CFG file: " + currentInput);
                        }
                    }
                    else if (keyInfo.Key != ConsoleKey.Enter)
                    {
                        // Handle other keys
                        if (!char.IsControl(keyInfo.KeyChar))
                        {
                            currentInput += keyInfo.KeyChar;
                            input = currentInput;  // Synchronize input with the current input
                            Console.Write(keyInfo.KeyChar);
                            matchIndex = -1;  // Reset match index
                        }
                    }
                } while (keyInfo.Key != ConsoleKey.Enter);

                return currentInput;
            }




        }
    }
}
 
