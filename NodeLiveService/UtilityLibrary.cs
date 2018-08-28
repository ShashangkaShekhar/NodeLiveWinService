using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace NodeLiveService
{
    public static class UtilityLibrary
    {
        public static bool PortInUse(int port)
        {
            bool inUse = false;

            try
            {
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

                foreach (IPEndPoint endPoint in ipEndPoints)
                {
                    if (endPoint.Port == port)
                    {
                        inUse = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error : " + ex.ToString());
            }

            return inUse;
        }

        public static void WriteErrorLog(string Message)
        {
            StreamWriter objSw = null;
            try
            {
                objSw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                objSw.WriteLine(DateTime.Now.ToString() + " : " + Message);
                objSw.Flush(); objSw.Close();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}
