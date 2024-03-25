using System;
using System.IO.Ports;

namespace IoT
{
    public class IoTUART
    {
        static SerialPort mySerialPort;
        static string lastCommand = "";

        public static void uartFunction(string comPort, int baudrate)
        {
            try
            {
                mySerialPort = new SerialPort(comPort);

                mySerialPort.BaudRate = baudrate;
                mySerialPort.Parity = Parity.None;
                mySerialPort.StopBits = StopBits.One;
                mySerialPort.DataBits = 8;
                mySerialPort.Handshake = Handshake.None;

                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                mySerialPort.Open();

                Console.WriteLine("Power on your device... ('<exit>' to close connection):");
                while (true)
                {
                    if (Console.KeyAvailable) // Check if a key is pressed
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

                mySerialPort.Close();
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
                if (mySerialPort != null && mySerialPort.IsOpen)
                {
                    mySerialPort.Close();
                    Console.WriteLine("Serial port closed.");
                }
            }
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();

            Console.Write(indata); // Display the data received from the serial port
        }

        private static void SendCommand(string command)
        {
            if (mySerialPort != null && mySerialPort.IsOpen)
            {
                mySerialPort.Write(command);
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
        private static readonly char[] Whitespace = { ' ', '\t', '\r', '\n' };
        private static readonly char[] Punctuation = { '.', ',', ':', ';', '?', '!' };
        private static readonly char[] Vowels = { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };

        public static void baudDetect(string comPort)
        {
            Console.WriteLine(" Please power your device and then press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();

            foreach (int baudRate in baudRates)
            {
                using (SerialPort serialPort = new SerialPort(comPort, baudRate))
                {
                    try
                    {
                        serialPort.ReadTimeout = 500;
                        serialPort.Open();
                        Console.WriteLine($"Attempting to connect with baud rate: {baudRate}...");

                        Thread.Sleep(1000); // Wait for data to come in

                        if (serialPort.BytesToRead > 0)
                        {
                            string data = serialPort.ReadExisting();

                            if (IsValidData(data))
                            {
                                Console.WriteLine($"Success. Baudrate: {baudRate}");
                                break;
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
                            serialPort.Close();
                        }
                    }
                }
            }
        }

        private static bool IsValidData(string data)
        {
            int whitespaceCount = 0, punctuationCount = 0, vowelCount = 0, validCharCount = 0;

            foreach (char c in data)
            {
                if (c > 127) continue; // Not an ASCII character

                validCharCount++;
                if (Array.IndexOf(Whitespace, c) >= 0) whitespaceCount++;
                else if (Array.IndexOf(Punctuation, c) >= 0) punctuationCount++;
                else if (Array.IndexOf(Vowels, c) >= 0) vowelCount++;
            }

            return validCharCount > 25 && whitespaceCount > 0 && punctuationCount > 0 && vowelCount > 0;
        }
    }
}
