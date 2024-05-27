# IoT-Whisperer

```
         ___     ___________
        |   | ___\__    ___/                                                                .:------------:.
        |   |/     \    |                                                                   -= .::.   .:===:
        |   (   O   )   |                                                                   -++***++==+*+=+:
        |___|\____/|____|                                                                   -=----=-=--=---.
 __      __.__    .__                                                                       --=:-====--=-=-:
/  \    /  \  |__ |__| ____________   ___________   ___________                             --+-:=:==-==-+=:
\   \/\/   /  |  \|  |/  ___/\____ \_/ __ \_  __ \_/ __ \_  __ \                            -----+-+=-+=---.
 \        /|   Y  \  |\___ \ |  |_> >  ___/|  | \/\  ___/|  | \/                            .--=-+----=-=-:.
  \__/\  / |___|  /__/____  >|   __/ \___  >__|    \___  >__|                                 :--+-=--=-=.
       \/       \/        \/ |__|        \/            \/                                       .::=-:.
       by CyberYom

| IoT
| The Leahy Center
| Version: 0.0.1
└---------------------------------------------------------------------------
 Welcome to IoT Whisperer. Please pass the 'help ' command to view the help menu.

 Whisper >
```
IoT Whisperer is a versatile tool developed for the IoT team at The Leahy Center, capable of performing forensic acquisitions using UART, JTAG, SWD, SPI, and I2C interfaces. It provides a comprehensive set of commands to interact with various debugging interfaces and facilitates data dumping and analysis.

## Features

- **UART Interface**: Connect and communicate with devices over UART, detect baud rates, and send commands.
- **JTAG Interface**: Start and stop the OpenOCD server, connect to JTAG devices, and perform various OpenOCD operations.
- **I2C Interface**: Initialize I2C communication, read data from I2C devices, and save the data to a file.
- **SWD Interface**: Connect to devices using the SWD interface, set SWD speed, and dump data.
- **SPI Interface**: Read data from SPI devices and save the data to a file.

## Installation

To install the necessary dependencies and set up the environment, follow these steps:

1. Clone the repository:
    ```sh
    git clone https://github.com/lcdi/IoT-Whisperer.git
    cd IoT-Whisperer
    ```

2. Ensure you have the required DLLs for FTDI and J-Link in the appropriate directories.

3. Install the Telnet client on your system if not already installed:
    ```sh
    dism.exe /Online /Enable-Feature /FeatureName:TelnetClient
    ```
4. Run the executable IoT Whisperer.exe

## Commands

### General Commands

- **{protocol}**: Select a protocol to work with.
- **help**: Display the help menu.
- **exit**: Exit the IoT Whisperer.

### UART Commands

- **baud**: Detect the baud rate of a device.
- **connect**: Connect to a device using a specified COM port and baud rate.
- **back**: Return to the main menu from UART mode.

### JTAG Commands

- **start**: Start the OpenOCD server.
- **stop**: Stop the OpenOCD server.
- **connect**: Connect to the OpenOCD server.
- **jtagulate**: Interface with JTagulator.
- **back**: Return to the main menu from JTAG mode.

### I2C Commands

- **dump**: Read data from an I2C device and save it to a file.
- **back**: Return to the main menu from I2C mode.

### SWD Commands

- **connect**: Connect to a device using SWD, set the speed, and dump data to a file.
- **back**: Return to the main menu from SWD mode.

### SPI Commands

- **read**: Read data from an SPI device using a specified read command and address length, then save the data to a file.
- **back**: Return to the main menu from SPI mode.

## Example Usage

### Connecting via UART:

```
Whisper > uart
Enter COM port (e.g., COM3): COM3
UART > connect
Enter Baudrate (e.g., 9600): 9600
```

### Starting the OpenOCD Server for JTAG:

```
Whisper > jtag
JTag > start
Enter an interfacing CFG file: interface.cfg
Please enter a target cfg: target.cfg
```

### Dumping Data via I2C:

```
Whisper > i2c
I2C > dump
Enter I2C address (in hex, e.g., 0x40): 0x40
Enter buffer size: 256
Enter the filename to dump the data: I2Cdump.bin
```

## Contributing

Contributions are welcome! Please submit issues and pull requests to the [GitHub repository](https://github.com/lcdi/IoT-Whisperer).

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgements

- Developed by the CyberYom and the IoT team at The Leahy Center.
- Special thanks to all contributors and testers.
