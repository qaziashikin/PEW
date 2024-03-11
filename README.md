# PEW: Play a post-apocalyptic robot shooting game in VR with your brain!
**For Neureality Hackathon 2024**

[DevPost here](https://devpost.com/software/pew-r0eipd)
#### Created by: Eris Gao, Christopher Ades, Qazi Ashikin, Leah Kim, Pranay Talla


### Build Instructions for VR
Open the folder PEWProject in Unity, go to Build Settings and select Android as the Build platform. Connect a VR headset and click "Build and Run."

### Set up instructions for getting OpenBCI Cyton and g.tec Unicorn Hybrid Black signals through PhysioLabXR
For the Cyton, 
1. plug in usb dongle and turn on the cyton
2. open the PhysioLabXR application and click the scripting tab:
3. load the script PhysioLabXROpenBCICyton8ChannelsScript.py located in PEW/Assets/PhysioLabXR Scripts/ 
4. add an output named 'OpenBCICyton8Channels'
5. enter 8 for the number of channels and select LSL and float64
6. add a string parameter called 'serial_port' and its value will depend on which serial port the dongle is located in. For example ours was 'COM7'
7. Run the script and go back to the stream tab to add 'OpenBCICyton8Channels' as an LSL stream

For the Unicorn Hybrid Black,
1. plug in usb dongle and turn on the Unicorn
2. enter the scripting tab
3. load UnicornHybridBlack_DiscoverViaBluetooth.py located in PEW/Assets/PhysioLabXR Scripts/
4. add an output named 'UnicornHybridBlackLSL'
5. enter 19 for the number of channels and select LSL and float64
6. Run the script and go back to the stream tab to add 'UnicornHybridBlackLSL' as an LSL stream

### How to connect the two platforms
In Unity, the application will be running as a UDP server, constantly listening for new messages.

When you run the emg_threshold_udp.py script in PhysioLabXR, it will act as a UDP client, sending messages to the Unity UDP server. After the EMG signal is processed, the “client” will attempt to bind to the “server." Similarly, run eeg_threshold_udp.py to send data from the Unicorn.

To be able to communicate successfully, both the VR headset and the computer that is running the Python file must be connected to the same network. The user running the Python file should make sure to update the IP address in the UDP client with the **IPv4 address** of the VR headset. The ports should also be the same. 

Once both are up and running, whenever the BCI side receives a trigger (for example, an arm moving enough to get a EMG signal spike) the client side UDP server will send a message to the Unity application to do something.

### Communication protocol
Currently, the UDP client should send '1' to make the player inside Unity switch weapons. It should send '2' to call AOE and send '3' to destroy a robot that is flashing.

We mapped the EMG signal to '1' so that when the user has electrodes taped on their arm, a sudden arm movement will trigger the sending of '1' to the Unity application, resulting in weapon switching happening once.
