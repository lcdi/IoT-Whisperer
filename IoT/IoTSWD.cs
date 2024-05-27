using System;
using System.IO;
using System.Runtime.InteropServices;

namespace IoT
{
    public static class SwdHandler
    {
        // J-Link DLL imports
        [DllImport("JLink_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int JLINKARM_Open();

        [DllImport("JLink_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int JLINKARM_Close();

        [DllImport("JLink_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int JLINKARM_Connect();

        [DllImport("JLink_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int JLINKARM_TIF_Select(int tif);

        [DllImport("JLink_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int JLINKARM_GetSN();

        [DllImport("JLink_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int JLINKARM_SetSpeed(int speed);

        [DllImport("JLink_x64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int JLINKARM_Reset();

        // Method to connect to the SWD interface and dump data to a file
        public static void Connect(out byte[] data, string fileName)
        {
            data = null; // Initialize the data array to null

            Console.Write("Enter the SWD speed (in kHz): ");
            if (!int.TryParse(Console.ReadLine(), out int swdSpeed))
            {
                Console.WriteLine("Invalid speed value. Please enter an integer value.");
                return;
            }

            // Open connection to J-Link
            int openResult = JLINKARM_Open();
            if (openResult < 0)
            {
                Console.WriteLine("Failed to open J-Link connection.");
                return;
            }

            // Select SWD interface (interface number 1 for SWD)
            int tifResult = JLINKARM_TIF_Select(1);
            if (tifResult < 0)
            {
                Console.WriteLine("Failed to select SWD interface.");
                JLINKARM_Close();
                return;
            }

            // Set the SWD speed (in kHz)
            int speedResult = JLINKARM_SetSpeed(swdSpeed);
            if (speedResult < 0)
            {
                Console.WriteLine("Failed to set SWD speed.");
                JLINKARM_Close();
                return;
            }

            // Connect to the target device
            int connectResult = JLINKARM_Connect();
            if (connectResult < 0)
            {
                Console.WriteLine("Failed to connect to target device.");
                JLINKARM_Close();
                return;
            }

            // Reset the target device
            int resetResult = JLINKARM_Reset();
            if (resetResult < 0)
            {
                Console.WriteLine("Failed to reset target device.");
                JLINKARM_Close();
                return;
            }

            // Get and display the serial number of the connected J-Link
            int serialNumber = JLINKARM_GetSN();
            Console.WriteLine("Connected to J-Link with serial number: " + serialNumber);

            // Dummy data for example purposes
            data = new byte[256];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)i;
            }

            // Write the data to the specified file
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

            // Close the connection
            JLINKARM_Close();
            Console.WriteLine("Connection closed.");
        }
    }
}
