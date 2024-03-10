using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UnityUDPServer : MonoBehaviour
{
    private Thread serverThread;
    private bool isRunning = true;
    public PlayerControl playerControl;
    private int weaponIndex = 1;
    private Socket listenerSocket;

    void Start()
    {
        serverThread = new Thread(new ThreadStart(RunServer));
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void RunServer()
    {
        const int port = 24103;
        var localEndPoint = new IPEndPoint(IPAddress.Any, port);

        try
        {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            listenerSocket.Bind(localEndPoint);
            Debug.Log($"Server is running on port {port}. Waiting for a message...");

            while (isRunning)
            {
                byte[] buffer = new byte[1024];
                EndPoint senderRemote = new IPEndPoint(IPAddress.Any, 0);
                int bytesRead = listenerSocket.ReceiveFrom(buffer, ref senderRemote);

                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Debug.Log($"Received message: {message} from {senderRemote.ToString()}");

                if (message == "1")
                {
                    UnityMainThreadDispatcher.Instance.Enqueue(() =>
                    {
                        playerControl.activeWeaponSwitching.SetSelectedWeapon(weaponIndex);
                        weaponIndex = (weaponIndex + 1) % 3;
                    });
                }
                if (message == "2")
                {
                    UnityMainThreadDispatcher.Instance.Enqueue(() =>
                    {
                        playerControl.childActivator.ActivateChild(0, 16);
                    });
                }
                if (message == "3")
                {
                    UnityMainThreadDispatcher.Instance.Enqueue(() =>
                        {
                            playerControl.GetComponent<PlayerControl>().MakeFlashingTargetsInvisible();
                        });

                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
        finally
        {
            // Close the listener socket when exiting the server loop
            if (listenerSocket != null)
            {
                listenerSocket.Close();
            }
        }
    }

    void OnDisable()
    {
        isRunning = false;
        if (serverThread != null)
        {
            serverThread.Join(); // Wait for thread to finish
        }
    }

    void OnApplicationQuit()
    {
        // Ensure the server thread is terminated when the application quits
        isRunning = false;
        if (serverThread != null && serverThread.IsAlive)
        {
            serverThread.Abort();
        }
    }
}
