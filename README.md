# PEW: Play a post-apocalyptic robot shooting game in VR with your brain!
**For Neureality Hackathon 2024**

[DevPost here](https://devpost.com/software/pew-r0eipd)
#### Created by: Eris Gao, Christopher Ades, Qazi Ashikin, Leah Kim, Pranay Talla


### Build Instructions for VR
Open the folder PEWProject in Unity, go to Build Settings and select Android as the Build platform. Connect a VR headset and click "Build and Run."

### Set up instructions for getting EEG signal through PhysioLabXR
**something soemthing**

### How to connect the two platforms
In Unity, the application will be running as a UDP server, constantly listening for new messages.

On the BCI signal processing side, when you run the **something.py script**, it will act as a UDP client, sending messages to the Unity UDP server. After the EMG signal is processed, the “client” will attempt to bind to the “server."

To be able to communicate successfully, both the VR headset and the computer that is running the Python file must be connected to the same network. The user running the Python file should make sure to update the IP address in the UDP client with the **IPv4 address** of the VR headset. The ports should also be the same. 

Once both are up and running, whenever the BCI side receives a trigger (for example, an arm moving enough to get a EMG signal spike) the client side UDP server will send a message to the Unity application to do something.

### Communication protocol
Currently, the UDP client should send '1' to make the player inside Unity switch weapons. It should send '2' to call AOE and send '3' to destroy a robot that is flashing.

We mapped the EMG signal to '1' so that when the user has electrodes taped on their arm, a sudden arm movement will trigger the sending of '1' to the Unity application, resulting in weapon switching happening once.
