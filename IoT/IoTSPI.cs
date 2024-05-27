using System;
using System.IO;
using FTD2XX_NET;

namespace IoT
{
    public class SPIFlash
    {
        private FTDI _ftdiDevice; // FTDI device instance
        private byte _readCommand; // SPI flash read command
        private int _addressLength; // Length of the address for the SPI flash

        // Constructor to initialize the SPIFlash instance with a read command and address length
        public SPIFlash(byte readCommand, int addressLength)
        {
            _ftdiDevice = new FTDI();
            _readCommand = readCommand;
            _addressLength = addressLength;
            InitializeDevice();
        }

        // Method to initialize the FTDI device
        private void InitializeDevice()
        {
            // Open the FTDI device by index
            FTDI.FT_STATUS status = _ftdiDevice.OpenByIndex(0);
            if (status != FTDI.FT_STATUS.FT_OK)
            {
                throw new Exception("Failed to open FTDI device");
            }
        }

        // Method to read data from the SPI flash
        public byte[] Read(uint count, uint address)
        {
            // Create the command buffer (1 byte for command + address length)
            byte[] command = new byte[1 + _addressLength];
            command[0] = _readCommand; // Set the read command

            // Set the address in the command buffer
            for (int i = 0; i < _addressLength; i++)
            {
                command[_addressLength - i] = (byte)((address >> (8 * i)) & 0xFF);
            }

            // Send the command to the FTDI device
            uint bytesWritten = 0;
            FTDI.FT_STATUS status = _ftdiDevice.Write(command, command.Length, ref bytesWritten);
            if (status != FTDI.FT_STATUS.FT_OK || bytesWritten != command.Length)
            {
                throw new Exception("Failed to write command to FTDI device");
            }

            // Read the data from the FTDI device
            byte[] data = new byte[count];
            uint bytesRead = 0;
            status = _ftdiDevice.Read(data, count, ref bytesRead);
            if (status != FTDI.FT_STATUS.FT_OK || bytesRead != count)
            {
                throw new Exception("Failed to read data from FTDI device");
            }

            return data; // Return the read data
        }

        // Method to close the FTDI device
        public void Close()
        {
            _ftdiDevice.Close();
        }
    }
}
