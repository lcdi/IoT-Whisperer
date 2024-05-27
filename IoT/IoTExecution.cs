using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
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
            Console.WriteLine(" | This tool is capable of performing forensic acquisitions with UART, JTag, SWD, SPI, and I2C");
            Console.WriteLine(" | Note that once a debugging interface is specified, help will offer protocol-specific commands.\n └──────────────────────────────────────────────────────────────────────────────────────────────────\n");
            Console.WriteLine("    Supported Interfaces:\n +-------------------------------+");
            Console.WriteLine("    UART, JTag, I2C, SWD, & SPI\n");
            Console.Write(" To select a specific interface, pass the name of the interface\n     Ex. "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("Whisper"); Console.ResetColor(); Console.WriteLine(" > UART\n\n");
            Console.WriteLine(" Additional help:\n |Please ann an issue\n └───────https://github.com/lcdi/IoT-Whisperer/issues\n\n");
            Console.WriteLine(" Version: 1.0.0");
            Console.WriteLine(" https://github.com/lcdi/IoT-Whisperer");
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

                int extraRightSpace = 10;
                int spaceNeeded = Console.WindowWidth - (badgeLines[0].Length + extraRightSpace + (i < iotLines.Length ? iotLines[i].Length : whispererLines[i - iotLines.Length].Length));

                Console.Write(new string(' ', Math.Max(spaceNeeded, 0)));

                if (i >= badgeStartLine && i < badgeStartLine + badgeLines.Length)
                {
                    Console.ForegroundColor = i - badgeStartLine < 3 ? ConsoleColor.Green : ConsoleColor.Blue;
                    Console.WriteLine(badgeLines[i - badgeStartLine]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine();
                }
            }

            Console.WriteLine("       by CyberYom\n");
            Console.WriteLine("| IoT\n| The Leahy Center\n| Version: 1.0.0\n└---------------------------------------------------------------------------");
            Console.Write(" Welcome to IoT Whisperer. Please pass the '");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("help");
            Console.ResetColor();
            Console.WriteLine("' command to view the help menu.\n");
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
                    Environment.Exit(0);
                }

                switch (userInput.ToLower())
                {
                    case "help":
                        help();
                        break;

                    case "uart":
                        HandleUART();
                        break;

                    case "jtag":
                        HandleJTag();
                        break;

                    case "i2c":
                        HandleI2C();
                        break;

                    case "swd":
                        HandleSWD();
                        break;

                    case "spi":
                        HandleSPI();
                        break;

                    default:
                        Console.WriteLine(" Unknown command. Type 'help' for available commands.");
                        break;
                }
            }
        }

        static void HandleUART()
        {
            Console.Write(" Enter COM port (e.g., COM3): ");
            string comPort = Console.ReadLine();
            string[] portNames = SerialPort.GetPortNames();
            if (Array.IndexOf(portNames, comPort) == -1)
            {
                Console.WriteLine($" The specified COM port '{comPort}' is not connected. Try {string.Join(", ", portNames)}\n");
                return;
            }

            Console.WriteLine("\n UART has been selected as the target interface. Type 'help' to view available commands.\n");
            int baudRate = 0;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" UART");
                Console.ResetColor();
                Console.Write(" > ");
                string uartInput = Console.ReadLine()?.ToLower();

                if (uartInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(" Exiting IoT Whisperer...");
                    Environment.Exit(0);
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
                    Console.WriteLine(" back      -  Back out of the UART mode and return to main menu\n\n");
                    Console.WriteLine(" Additional help:\n |Please ann an issue\n └───────https://github.com/lcdi/IoT-Whisperer/issues\n\n");
                    Console.WriteLine(" Version: 1.0.0");
                    Console.WriteLine(" https://github.com/lcdi/IoT-Whisperer");
                }
                else if (uartInput == "baud")
                {
                    baud.baudDetect(comPort, false); // Passing false for verbosityFlag
                }
                else if (uartInput == "connect")
                {
                    Console.Write("Enter Baudrate (e.g., 9600): ");
                    if (int.TryParse(Console.ReadLine(), out int enteredBaudRate))
                    {
                        baudRate = enteredBaudRate;
                        Console.WriteLine($"Connecting to {comPort} with baudrate {baudRate}...\n");
                        IoTUART.uartFunction(comPort, baudRate);
                    }
                    else
                    {
                        Console.WriteLine(" Invalid baud rate entered. Please enter a numeric value.");
                    }
                }
                else if (uartInput == "back")
                {
                    break;
                }
                else
                {
                    Console.WriteLine(" Unknown command. Type 'help' for available commands.");
                }
            }
        }

        static void HandleJTag()
        {
            Console.WriteLine(" JTag has been selected as the target interface. Type 'help' to view available commands.\n");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" JTag");
                Console.ResetColor();
                Console.Write(" > ");

                string jtagInput = Console.ReadLine()?.ToLower();

                if (jtagInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("  Exiting IoT Whisperer...\n");
                    launchOCD.stopOpenOCD();
                    Environment.Exit(0);
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
                    Console.WriteLine("  exit        -  Exit IoT Whisperer.                back        -  Back out of JTag mode.\n\n");
                    Console.WriteLine("      OpenOCD Commands:");
                    Console.WriteLine("  halt        -  Stops the CPU in whatever state it is in.");
                    Console.WriteLine("  reset init  -  Reset the CPU into a known state.");
                    Console.WriteLine("  flash banks -  Reveal information about flash memory .");
                    Console.WriteLine("  reset       -  Resets the CPU.\n");
                    Console.WriteLine("  flash write_image erase data.bin 0x08000000\n       -  Used to write a binary image to a device. data.bin is the data to write and 0x08000000 is the address to start writing data to.");
                    Console.WriteLine("  verify_image data.bin 0x0800000\n       -  Used to verify an image write after writing an image. data.bin is the data that was written to the device, and 0x08000000 is where the data starts on the device.");
                    Console.WriteLine("  dump_image dataOut.bin 0x0800000 0x00010000\n       -  Used to dump a firmware image from a device. dataOut.bin is the output file for the data, 0x08000000 is where to start dumping data, and 0x00010000 is the size of the data to dump.\n\n");
                    Console.WriteLine("  Additional help:\n |Please ann an issue\n └───────https://github.com/lcdi/IoT-Whisperer/issues\n");
                    Console.WriteLine("  Version: 1.0.0");
                    Console.WriteLine("  https://github.com/lcdi/IoT-Whisperer");
                }

                else if (jtagInput == "start")
                {
                    Console.WriteLine(" Checking Dependencies...");
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
                    int baudRate = 115200;
                    Console.Write(" Enter COM port (e.g., COM3): ");
                    string comPortJT = Console.ReadLine();

                    Console.WriteLine("Please enter the COM");
                    Console.WriteLine($"Connecting to JTagulator on port {comPortJT}...\n");
                    IoTUART.uartFunction(comPortJT, baudRate);
                }

                else if (jtagInput == "back")
                {
                    launchOCD.stopOpenOCD();
                    break;
                }
                else
                {
                    Console.WriteLine(" Unknown command. Type 'help' for available commands.\n");
                }
            }
        }

        static void HandleI2C()
        {
            Console.WriteLine(" I2C has been selected as the target interface. Type 'help' to view available commands.\n");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" I2C");
                Console.ResetColor();
                Console.Write(" > ");

                string i2cInput = Console.ReadLine()?.ToLower();

                if (i2cInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(" Exiting IoT Whisperer...");
                    Environment.Exit(0);
                }

                if (i2cInput == "help")
                {
                    Console.WriteLine("\n +------------------------------------+ I2C +------------------------------------+\n");
                    Console.WriteLine(" To connect to the I2C interface, please ensure the following connections have been made:\n");
                    Console.ForegroundColor = ConsoleColor.Red; Console.Write("                         GND"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("GND\n"); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green; Console.Write("                         SDA"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("SDA\n"); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Blue; Console.Write("                         SCL"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("SCL\n\n"); Console.ResetColor();
                    Console.WriteLine("      Supported Commands:");
                    Console.WriteLine(" dump       -  Dump data from the I2C device");
                    Console.WriteLine(" back       -  Back out of the I2C mode and return to main menu\n\n");
                    Console.WriteLine(" Additional help:\n |Please ann an issue\n └───────https://github.com/lcdi/IoT-Whisperer/issues\n\n");
                    Console.WriteLine(" Version: 1.0.0");
                    Console.WriteLine(" https://github.com/lcdi/IoT-Whisperer");
                }
                else if (i2cInput == "dump")
                {
                    Console.Write("Enter I2C address (in hex, e.g., 0x40): ");
                    string addressInput = Console.ReadLine();
                    byte address = Convert.ToByte(addressInput, 16);

                    Console.Write("Enter buffer size: ");
                    int bufferSize = int.Parse(Console.ReadLine());

                    Console.Write("Enter the filename to dump the data: ");
                    string fileName = Console.ReadLine();

                    try
                    {
                        I2CCommunication i2cComm = new I2CCommunication(address);
                        i2cComm.InitializeI2C();
                        byte[] data = i2cComm.DumpData(bufferSize, fileName);

                        string directoryPath = "I2C";
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string filePath = Path.Combine(directoryPath, "I2Cdump.bin");
                        File.WriteAllBytes(filePath, data);

                        Console.WriteLine("Data successfully dumped to I2Cdump.bin:");
                        foreach (byte b in data)
                        {
                            Console.Write($"{b:X2} ");
                        }
                        Console.WriteLine();
                        Console.WriteLine($"Data successfully written to {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else if (i2cInput == "back")
                {
                    break;
                }
                else
                {
                    Console.WriteLine(" Unknown command. Type 'help' for available commands.");
                }
            }
        }

        static void HandleSWD()
        {
            Console.WriteLine("SWD has been selected as the target interface. Type 'help' to view available commands.\n");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" SWD");
                Console.ResetColor();
                Console.Write(" > ");
                string swdInput = Console.ReadLine()?.ToLower();

                if (swdInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(" Exiting IoT Whisperer...");
                    Environment.Exit(0);
                }

                if (swdInput == "help")
                {
                    Console.WriteLine("\n +------------------------------------+ SWD +------------------------------------+\n");
                    Console.WriteLine(" To connect to the SWD interface, please ensure the following connections have been made:\n");
                    Console.ForegroundColor = ConsoleColor.Red; Console.Write("                         GND"); Console.ResetColor(); Console.Write("      --->    "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("GND\n"); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green; Console.Write("                         SWDIO"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("SWDIO\n"); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Blue; Console.Write("                         SWCLK"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("SWCLK\n\n"); Console.ResetColor();
                    Console.WriteLine("      Supported Commands:");
                    Console.WriteLine(" connect   -  Connect to the target device using SWD.");
                    Console.WriteLine(" back      -  Back out of the SWD mode and return to main menu\n\n");
                    Console.WriteLine(" Additional help:\n |Please ann an issue\n └───────https://github.com/lcdi/IoT-Whisperer/issues\n\n");
                    Console.WriteLine(" Version: 1.0.0");
                    Console.WriteLine(" https://github.com/lcdi/IoT-Whisperer");
                }
                else if (swdInput == "connect")
                {
                    Console.Write("Enter the filename to dump the data: ");
                    string fileName = Console.ReadLine();

                    try
                    {
                        SwdHandler.Connect(out byte[] data, fileName);

                        string directoryPath = "SWD";
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string filePath = Path.Combine(directoryPath, "SWDdump.bin");
                        File.WriteAllBytes(filePath, data);

                        Console.WriteLine("Data successfully dumped to SWDdump.bin:");
                        foreach (byte b in data)
                        {
                            Console.Write($"{b:X2} ");
                        }
                        Console.WriteLine();
                        Console.WriteLine($"Data successfully written to {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else if (swdInput == "back")
                {
                    break;
                }
                else
                {
                    Console.WriteLine(" Unknown command. Type 'help' for available commands.");
                }
            }
        }

        static void HandleSPI()
        {
            Console.WriteLine("SPI has been selected as the target interface. Type 'help' to view available commands.\n");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" SPI");
                Console.ResetColor();
                Console.Write(" > ");
                string spiInput = Console.ReadLine()?.ToLower();

                if (spiInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(" Exiting IoT Whisperer...");
                    Environment.Exit(0);
                }

                if (spiInput == "help")
                {
                    Console.WriteLine("\n +------------------------------------+ SPI +------------------------------------+\n");
                    Console.WriteLine(" To connect to the SPI interface, please ensure the following connections have been made:\n");
                    Console.ForegroundColor = ConsoleColor.Red; Console.Write("                         GND"); Console.ResetColor(); Console.Write("     --->    "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("GND\n"); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Blue; Console.Write("                         MISO"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("MISO\n"); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green; Console.Write("                         MOSI"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("MOSI\n"); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("                         CS"); Console.ResetColor(); Console.Write("      --->    "); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("CS\n"); Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("                         SCLK"); Console.ResetColor(); Console.Write("    --->    "); Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("SCLK\n\n"); Console.ResetColor();
                    Console.WriteLine("      Supported Commands:");
                    Console.WriteLine(" read      -  Read data from the SPI device.");
                    Console.WriteLine(" back      -  Back out of the SPI mode and return to main menu\n\n");
                    Console.WriteLine(" Additional help:\n |Please ann an issue\n └───────https://github.com/lcdi/IoT-Whisperer/issues\n\n");
                    Console.WriteLine(" Version: 1.0.0");
                    Console.WriteLine(" https://github.com/lcdi/IoT-Whisperer");
                }
                else if (spiInput == "read")
                {
                    Console.Write("Enter SPI flash read command (in hex, e.g., 0x03): ");
                    string readCommandInput = Console.ReadLine();
                    byte readCommand = Convert.ToByte(readCommandInput, 16);

                    Console.Write("Enter SPI flash address length (in bytes, e.g., 3): ");
                    int addressLength = int.Parse(Console.ReadLine());

                    SPIFlash spiFlash = new SPIFlash(readCommand, addressLength);

                    Console.Write("Enter read count (number of bytes to read): ");
                    uint count = uint.Parse(Console.ReadLine());

                    Console.Write("Enter SPI flash address (in hex, e.g., 0x000000): ");
                    string addressInput = Console.ReadLine();
                    uint address = Convert.ToUInt32(addressInput, 16);

                    try
                    {
                        byte[] data = spiFlash.Read(count, address);
                        Console.WriteLine("Data read from SPI flash:");

                        string directoryPath = "SPI";
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string filePath = Path.Combine(directoryPath, "SPIdata.bin");
                        File.WriteAllBytes(filePath, data);

                        foreach (byte b in data)
                        {
                            Console.Write($"{b:X2} ");
                        }
                        Console.WriteLine();
                        Console.WriteLine($"Data successfully written to {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    finally
                    {
                        spiFlash.Close();
                    }
                }
                else if (spiInput == "back")
                {
                    break;
                }
                else
                {
                    Console.WriteLine(" Unknown command. Type 'help' for available commands.");
                }
            }
        }

        static string ReadLineWithTabCompletion(string basePath)
        {
            string input = "";
            var matches = new List<string>();
            int matchIndex = -1;
            string currentInput = "";
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Tab)
                {
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
                        input = currentInput;

                        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
                        Console.Write("Enter an interfacing CFG file: " + currentInput);
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (currentInput.Length > 0)
                    {
                        currentInput = currentInput[..^1];
                        input = currentInput;
                        matchIndex = -1;

                        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
                        Console.Write("Enter an interfacing CFG file: " + currentInput);
                    }
                }
                else if (keyInfo.Key != ConsoleKey.Enter)
                {
                    if (!char.IsControl(keyInfo.KeyChar))
                    {
                        currentInput += keyInfo.KeyChar;
                        input = currentInput;
                        Console.Write(keyInfo.KeyChar);
                        matchIndex = -1;
                    }
                }
            } while (keyInfo.Key != ConsoleKey.Enter);

            return currentInput;
        }
    }
}
