using System;
using System.IO;
using System.Runtime.InteropServices;

namespace IoT
{
    public static class MpsseWrapper
    {
        // Import the I2C functions from the DLL
        [DllImport("libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr I2C_OpenChannel(int index);

        [DllImport("libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2C_InitChannel(IntPtr handle, int mode);

        [DllImport("libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2C_DeviceWrite(IntPtr handle, byte deviceAddress, byte[] buffer, int bufferSize);

        [DllImport("libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int I2C_DeviceRead(IntPtr handle, byte deviceAddress, byte[] buffer, int bufferSize);

        [DllImport("libmpsse.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void I2C_CloseChannel(IntPtr handle);
    }

    public class I2CCommunication
    {
        private IntPtr ftdiHandle; // Handle for the FTDI device
        private byte i2cAddress; // Address of the I2C device

        public I2CCommunication(byte address)
        {
            this.i2cAddress = address;
        }

        // Initialize the I2C communication channel
        public void InitializeI2C()
        {
            ftdiHandle = MpsseWrapper.I2C_OpenChannel(0); // Open the first FTDI device
            if (ftdiHandle == IntPtr.Zero)
            {
                throw new Exception("Failed to open FTDI device.");
            }

            int result = MpsseWrapper.I2C_InitChannel(ftdiHandle, 1); // Initialize I2C channel with mode 1
            if (result != 0)
            {
                throw new Exception("Failed to initialize I2C channel.");
            }
        }

        // Dump data from the I2C device to a file
        public byte[] DumpData(int bufferSize, string fileName)
        {
            byte[] readBuffer = new byte[bufferSize]; // Buffer to store the read data
            int result = MpsseWrapper.I2C_DeviceRead(ftdiHandle, i2cAddress, readBuffer, bufferSize);

            if (result != 0)
            {
                throw new Exception("Failed to read from I2C device.");
            }

            // Ensure the directory exists
            string directoryPath = "I2C";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Write the read data to the specified file
            string filePath = Path.Combine(directoryPath, fileName);
            File.WriteAllBytes(filePath, readBuffer);

            Console.WriteLine($"Dumped Data to file: {filePath}");

            return readBuffer;
        }

        // Close the I2C communication channel
        public void Close()
        {
            MpsseWrapper.I2C_CloseChannel(ftdiHandle);
        }
    }
}
