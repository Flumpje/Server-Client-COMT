using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
using System.Diagnostics;
using System.Threading;

  
public class SynchronousSocketClient {  
  
    public static void StartClient(String[] args) {  
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];  
        int iNrBytes;
        IPAddress ipAddress;

        // Go parse input variables
        int.TryParse(args[1], out iNrBytes);
        IPAddress.TryParse(args[0], out ipAddress);

        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < iNrBytes; i++)
        {
            //sb.AppendLine(i.ToString());
            sb.Append('W');
        }
        sb.Append("<EOF>");
        System.Console.WriteLine(sb.ToString());

        // get public ip
        string externalip = new WebClient().DownloadString("http://icanhazip.com");            
        Console.WriteLine(externalip);

        // Set stopwacht to get elpased time
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        // Connect to a remote device.  
        try {  
            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());  
            //IPAddress ipAddress = ipHostInfo.AddressList[0];  
            IPEndPoint remoteEP = new IPEndPoint(ipAddress,80);  
  
            // Create a TCP/IP  socket.  
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp );  
  
            // Connect the socket to the remote endpoint. Catch any errors.  
            try {  
                sender.Connect(remoteEP);  
  
                Console.WriteLine("Socket connected to {0}",  
                    sender.RemoteEndPoint.ToString());  
  
                // Encode the data string into a byte array.  
                //byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");  
                byte[] msg = Encoding.ASCII.GetBytes(sb.ToString());
                Console.WriteLine("String is in the msg");
  
                // Send the data through the socket.  
                int bytesSent = sender.Send(msg);  
                //Console.WriteLine("String is in the msg");

                // Receive the response from the remote device.  
                int bytesRec = sender.Receive(bytes);  
                Console.WriteLine("Echoed test = {0}",  
                    Encoding.ASCII.GetString(bytes,0,bytesRec));  
  
                // Release the socket.  
                sender.Shutdown(SocketShutdown.Both);  
                sender.Close();  

                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
  
            } catch (ArgumentNullException ane) {  
                Console.WriteLine("ArgumentNullException : {0}",ane.ToString());  
            } catch (SocketException se) {  
                Console.WriteLine("SocketException : {0}",se.ToString());  
            } catch (Exception e) {  
                Console.WriteLine("Unexpected exception : {0}", e.ToString());  
            }  
  
        } catch (Exception e) {  
            Console.WriteLine( e.ToString());  
        }  
    }  

    public static bool CheckArgs(String[] args) {
        bool bfault = false;
        int value;
        IPAddress address;
        // check if there are args
        if(args.Length==0) {
            bfault = true; 
            Console.WriteLine("No args given");

        // check if ip addres is valid
        } else if (!(IPAddress.TryParse(args[0], out address))) {
            Console.WriteLine("First arg not a valid ip address");

        // check if first arg is int
        } else if (!(int.TryParse(args[1], out value))) {
            Console.WriteLine("Second arg not a int");
        }

        Console.WriteLine("All checks completed");
        

        return bfault;
    }
  
    public static int Main(String[] args) { 
        bool bCheck = !CheckArgs(args);
        
        if (true) {
            StartClient(args); 
        }
        
        return 0;  
    }  
}