using System;
using System.IO.Ports;

namespace IoT
{
    public class IoTUART
    {
        static SerialPort mySerialPort; // Serial port instance
        static string lastCommand = ""; // Stores the last command sent

        // Function to handle UART communication
        public static void uartFunction(string comPort, int baudrate)
        {
            try
            {
                // Initialize serial port with the provided COM port and baud rate
                mySerialPort = new SerialPort(comPort)
                {
                    BaudRate = baudrate,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None
                };

                // Attach event handler for data received event
                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                mySerialPort.Open(); // Open the serial port

                Console.WriteLine("Power on your device... ('<exit>' to close connection):");
                while (true)
                {
                    // Check if a key is pressed
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true); // Read the key without displaying it

                        // Check for exit command
                        if (key.Key == ConsoleKey.X && lastCommand.Equals("<exit>", StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }

                        // Send the key character to the serial port
                        SendCommand(key.KeyChar.ToString());
                    }

                    Thread.Sleep(10); // Reduce CPU usage
                }

                mySerialPort.Close(); // Close the serial port
                Console.WriteLine("Connection closed.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to the port is denied.");
            }
            catch (IOException)
            {
                Console.WriteLine("An I/O error occurred while attempting to access the specified COM port.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("The port name does not begin with 'COM'.");
            }
            catch (SystemException ex) when (ex is InvalidOperationException || ex is ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Ensure the serial port is closed
                if (mySerialPort != null && mySerialPort.IsOpen)
                {
                    mySerialPort.Close();
                    Console.WriteLine("Serial port closed.");
                }
            }
        }

        // Event handler for data received from the serial port
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting(); // Read existing data

            Console.Write(indata); // Display the data received from the serial port
        }

        // Function to send a command to the serial port
        private static void SendCommand(string command)
        {
            if (mySerialPort != null && mySerialPort.IsOpen)
            {
                mySerialPort.Write(command); // Write command to the serial port
                lastCommand = command; // Store the last command sent
            }
            else
            {
                Console.WriteLine("Serial port is not open.");
            }
        }
    }

    public class baud
    {
        private static readonly int[] baudRates = { 50, 75, 110, 134, 150, 200, 300, 600, 1200, 1800, 2400, 4800, 9600, 19200, 28800, 38400, 57600, 76800, 115200, 230400, 460800, 576000, 921600, 1500000, 3000000 }; // Common baud rates
        private static readonly char[] Whitespace = { ' ', '\t', '\r', '\n' }; // Whitespace characters
        private static readonly char[] Punctuation = { '.', ',', ':', ';', '?', '!' }; // Punctuation characters
        private static readonly char[] Vowels = { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' }; // Vowel characters

        // Function to detect baud rates
        public static void baudDetect(string comPort, bool verbosityFlag)
        {
            Console.WriteLine("Please power your device and then press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();

            List<int> successfulBaudRates = new List<int>(); // List to store successful baud rates

            if (!verbosityFlag)
            {
                Console.WriteLine("Detecting Baud Rates... This may take a minute...\n");
            }

            // Iterate through common baud rates to find a successful connection
            foreach (int baudRate in baudRates)
            {
                using (SerialPort serialPort = new SerialPort(comPort, baudRate))
                {
                    try
                    {
                        serialPort.ReadTimeout = 500;
                        serialPort.Open(); // Open the serial port

                        // Display baud rate being attempted if verbosity is enabled
                        if (verbosityFlag)
                        {
                            Console.Write($"Attempting to connect with baud rate:");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(baudRate);
                            Console.ResetColor();
                            Console.WriteLine("...");
                        }

                        Thread.Sleep(500); // Wait for data to come in

                        if (serialPort.BytesToRead > 0)
                        {
                            string data = serialPort.ReadExisting();
                            if (verbosityFlag)
                            {
                                Console.WriteLine("\n" + data + "\n");
                            }

                            // Check if the data received is valid
                            if (IsValidData(data))
                            {
                                if (verbosityFlag)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("Successful Connection");
                                    Console.ResetColor();
                                }
                                successfulBaudRates.Add(baudRate); // Add to successful list
                            }
                        }
                    }
                    catch (TimeoutException)
                    {
                        Console.WriteLine($"Timeout at baud rate: {baudRate}, trying the next one.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed at baud rate: {baudRate} with error: {ex.Message}");
                    }
                    finally
                    {
                        if (serialPort.IsOpen)
                        {
                            serialPort.Close(); // Close the serial port
                        }
                    }
                }
            }

            // Display successful baud rates if any
            if (successfulBaudRates.Count > 0)
            {
                Console.WriteLine($"\nSuccessfully connected with the following baud rates: {string.Join(", ", successfulBaudRates)}");
            }
            else
            {
                Console.WriteLine("No successful baud rates found.");
            }
        }

        // Function to validate the data received from the serial port
        private static bool IsValidData(string data)
        {
            int whitespaceCount = 0, punctuationCount = 0, vowelCount = 0, validCharCount = 0;

            // Iterate through each character in the data
            foreach (char c in data)
            {
                if (c > 127) continue; // Skip non-ASCII characters

                validCharCount++;
                if (Array.IndexOf(Whitespace, c) >= 0) whitespaceCount++;
                else if (Array.IndexOf(Punctuation, c) >= 0) punctuationCount++;
                else if (Array.IndexOf(Vowels, c) >= 0) vowelCount++;
            }

            // Return true if the data contains valid characters, whitespace, punctuation, and vowels
            return validCharCount > 25 && whitespaceCount > 0 && punctuationCount > 0 && vowelCount > 0;
        }
    }
}
