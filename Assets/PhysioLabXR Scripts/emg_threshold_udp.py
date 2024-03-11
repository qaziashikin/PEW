import numpy as np
import socket
from physiolabxr.scripting.RenaScript import RenaScript

SERVER_IP = '10.206.62.94'   # Change this to the IP address of your Unity server
SERVER_PORT = 24103
client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
class Pew(RenaScript):
    def __init__(self, *args, **kwargs):
        """
        Please do not edit this function
        """
        super().__init__(*args, **kwargs)

    # Start will be called once when the run button is hit.
    def init(self):
        # Create a UDP socket
        global client_socket
        client_socket.connect((SERVER_IP, SERVER_PORT))
        pass

    # loop is called <Run Frequency> times per second
    def loop(self):
        if 'OpenBCICyton8Channels' in self.inputs.keys():
            data = self.inputs.get_data('OpenBCICyton8Channels')
            data1 = np.array([np.abs(np.sum(data, axis=0))])
            timestamps = self.inputs.get_timestamps("OpenBCICyton8Channels")
            self.set_output(stream_name="emg_sum", data=data1, timestamp=timestamps)

            threshold = self.params['threshold'] #good at 350
            # Check if any element in the array is greater than the threshold
            is_greater_than_threshold = np.any(data > threshold)

            if is_greater_than_threshold:
                print("ABOVE THRESH")
                message = "1"
                client_socket.sendto(message.encode(), (SERVER_IP, SERVER_PORT))

        self.inputs.clear_buffer_data()

        print('Loop function is called')

    # cleanup is called when the stop button is hit
    def cleanup(self):
        global client_socket
        client_socket.close()
        print('Cleanup function is called')