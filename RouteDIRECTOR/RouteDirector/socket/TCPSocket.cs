﻿using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace RouteDirector
{
	class TCPSocket
	{
		private Socket clientSocket;

		public bool ConnectStatus { set; get; }
		public TCPSocket()
		{
			ConnectStatus = false;
		}

		public int ConnectServer(IPEndPoint ipe)
		{
			if (ConnectStatus == true)
				return 0;
			try
			{
				clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				clientSocket.Connect(ipe);
			}
			catch (Exception e)
			{
				Log.log.Error("Tcp connnect error", e);
				return -1;
			}

			ConnectStatus = true;
			return 0;
		}

		public void DisconnectServer()
		{
			try
			{
				if (ConnectStatus == true)
				{
					clientSocket.Disconnect(false);
					clientSocket.Close();
					ConnectStatus = false;
				}

			}
			catch (Exception e)
			{
				Log.log.Error("tcp disconnnect error", e);
			}
		}

		public byte[] ReceiveData()
		{
			byte[] buf = new byte[1024 * 100];
			int len;
			try
			{

				len = clientSocket.Receive(buf);
				byte[] packet = new byte[len];
				if (packet.Length < 20)
				{
					Log.log.Error("Tcp receive length is too short:" + packet.Length);
					return null;
				}
				Array.Copy(buf, packet, len);
				return packet;
			}

			catch (Exception e)
			{
				Log.log.Error("Tcp receive error", e);
				return null;
			}
		}

		public int SendData(byte[] data)
		{
			int len;
			try
			{
				len = clientSocket.Send(data);
			}
			catch (Exception e)
			{
				Log.log.Error("Tcp send error", e);
				return -1;
			}
			return len;
		}
	}
}
