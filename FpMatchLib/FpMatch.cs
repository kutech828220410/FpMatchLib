using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Basic;
namespace FpMatchLib
{
    public enum CmdType : int
    {
        Connect = 0,
        SetDeviceType = 1,
        GetDeviceType = 2,
        DeviceCount = 3,
        OpenDevice = 4,
        CloseDevice = 5,
        Enroll = 6,
        GetFeature = 7,
        SetParameter = 8,
        GetParameter = 9,
        GetDeviceSn = 10,
        CodeToStr = 11,
        GetImage = 12
    }
    public enum retCode : int
    {
        RT_SUCCESS = 0x00,
        RT_FAIL = 0x01,
        RT_DEVICE_NOT_FOUND = 0x06,
        RT_DEVICE_BUSY = 0x07,
        RT_BAD_QUALITY = 0x21,
        RT_GENERALIZE = 0x30,
        RT_FP_CANCEL = 0x41,

     
    }
    public enum stateCode : int
    {
        NONE = 0x00,
        FAIL = 0x01,
        READY = 0x02,
        UNREADY = 0x02,
        RT_NEED_SWEEP = -1,

        RT_NEED_FIRST_SWEEP = 0xFFF1,
        RT_NEED_SECOND_SWEEP = 0xFFF2,
        RT_NEED_THIRD_SWEEP = 0xFFF3,
        RT_NEED_RELEASE_FINGER = 0xFFF4,
    }
    public class FpMatchClass
    {
        [JsonPropertyName("cmd")]
        public int cmd { get; set; }
        [JsonPropertyName("cmdStr")]
        public string 命令內容 { get; set; }
        [JsonPropertyName("deviceType")]
        public int 裝置類別 { get; set; }
        [JsonPropertyName("deviceIndex")]
        public int 裝置號碼 { get; set; }
        [JsonPropertyName("handle")]
        public int handle { get; set; }
        [JsonPropertyName("feature")]
        public string feature { get; set; }
        [JsonPropertyName("featureLen")]
        public int featureLen { get; set; }
        [JsonPropertyName("retCode")]
        public int retCode { get; set; }
        [JsonPropertyName("stateCode")]
        public int stateCode { get; set; }
    }
    public class FpMatchSoket
    {   
        
        // defines
        public const int FEATURE_SIZE = (768);

        // return values
        public const int RTC_SUCCESS = (0);
        public const int RTC_FAIL = (1);

        // exported functions
#if x86
        [DllImport("FpMatch_32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
#else
        [DllImport("FpMatch_64.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
#endif
        public static extern int FPMatch(byte[] registeredTemplate, byte[] queryFeature, int secLevel, ref int similarity);
        public static int secLevel = 3;

        static public bool ConsoleWrite = false;
        private Net.SocketClient socketClient;
        public bool IsOpen
        {
            get
            {
                return (Hanndle != -1);
            }
        }
        public bool IsBusy = false;
        public int DeviceIndex = -1;
        public int Hanndle = -1;
        private bool _abort = false;
        public stateCode StateCode = stateCode.NONE;
        public void Init(string url)
        {
            socketClient = new Net.SocketClient(url);        
        }
        public bool Open()
        {
            if (socketClient == null) socketClient = new Net.SocketClient($@"127.0.0.1:19002/ws");
            bool flag_OK = true;
            try
            {
                if (this.Hanndle != -1)
                {
                    if (CloseDevice() == false) flag_OK = false;
                }
                if (Connect() == false) flag_OK = false;
                if (SetDeviceType() == false) flag_OK = false;
                if (OpenDevice() == false) flag_OK = false;
                return flag_OK;
            }
            catch
            {
                flag_OK = false;
                return flag_OK;
            }
            finally
            {
                Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] FpMatchSoket {(flag_OK ? "sucess" : "failed")}!");           
            }
      
        }
        public async Task<bool> OpenAsync()
        {
            bool flag_OK = true;
            try
            {
                if (await ConnectAsync() == false) flag_OK = false;
                if (await SetDeviceTypeAsync() == false) flag_OK = false;
                if (await OpenDeviceAsync() == false) flag_OK = false;
                return flag_OK;
            }
            catch
            {
                flag_OK = false;
                return flag_OK;
            }
            finally
            {
                Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().DeclaringType.Name}] FpMatchSoket {(flag_OK ? "sucess" : "failed")}!");
            }
        }
        public bool Match(string registerTemplate, string queryFeature )
        {
            MyTimerBasic myTimerBasic = new MyTimerBasic(50000);
            myTimerBasic.StartTickTime();
            byte[] templateBytes = System.Text.Encoding.Default.GetBytes(registerTemplate);
            byte[] featureBytes = System.Text.Encoding.Default.GetBytes(queryFeature);
            int nRet = 0;
            int similarity = 0;
            nRet = FPMatch(templateBytes, featureBytes, secLevel, ref similarity);
            if ((nRet == RTC_SUCCESS) && (similarity > 800))
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] match success,similarity:{similarity} ,{myTimerBasic.ToString()}");
             
                return true;
            }
            else
            {
              Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] match fail, return code:{nRet} ,{myTimerBasic.ToString()}");
                return false;
            }

        }
        public void Abort()
        {
            _abort = true;
        }
        public FpMatchClass Enroll()
        {
            IsBusy = true;
            _abort = false;
            FpMatchClass fpMatchClass = new FpMatchClass();
            try
            {
           
                while (true)
                {
                    if (_abort == true)
                    {
                        CloseDevice();
                        break;
                    }
                    StateCode = enroll(ref fpMatchClass);
                    if (StateCode == stateCode.RT_NEED_FIRST_SWEEP)
                    {
                        if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] ##First Press");
                    }
                    if (StateCode == stateCode.RT_NEED_SECOND_SWEEP)
                    {
                        if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] ##Second Press");
                    }
                    if (StateCode == stateCode.RT_NEED_THIRD_SWEEP)
                    {
                        if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] ##Third Press");
                    }
                    if (StateCode == stateCode.RT_NEED_RELEASE_FINGER)
                    {
                        if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] ##Please leave finger");
                    }
                    if (StateCode == stateCode.NONE)
                    {
                        if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] featureLen:{fpMatchClass.featureLen}");
                        if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] feature:{fpMatchClass.feature}");
                        StateCode = stateCode.READY;
                        return fpMatchClass;
                    }
                    if (StateCode == stateCode.FAIL)
                    {
                        if (StateCode == stateCode.NONE)
                            return new FpMatchClass();
                    }
                     System.Threading.Thread.Sleep(50);
                }
            }
            catch(Exception e)
            {
                return new FpMatchClass();
            }
            finally
            {
                IsBusy = false;
            }
            return new FpMatchClass();
        }
        public FpMatchClass GetFeature()
        {
            IsBusy = true;
            _abort = false;
            FpMatchClass fpMatchClass = new FpMatchClass();
            try
            {

                while (true)
                {
                    if (_abort == true)
                    {
                        CloseDevice();
                        break;
                    }
                    StateCode = getFeature(ref fpMatchClass);

                    if (StateCode == stateCode.NONE)
                    {
                        if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] featureLen:{fpMatchClass.featureLen}");
                        if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] feature:{fpMatchClass.feature}");
                        StateCode = stateCode.READY;
                        return fpMatchClass;
                    }
                    if (StateCode == stateCode.RT_NEED_SWEEP)
                    {
                        if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] ##Please press finger");
                    }

                    if (StateCode == stateCode.FAIL)
                    {
                        if (StateCode == stateCode.NONE)
                            return new FpMatchClass();
                    }
                    System.Threading.Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                return new FpMatchClass();
            }
            finally
            {
                IsBusy = false;
            }
            return new FpMatchClass();
        }
        public FpMatchClass GetFeatureOnce()
        {
            IsBusy = true;
            _abort = false;
            FpMatchClass fpMatchClass = new FpMatchClass();
            StateCode = getFeature(ref fpMatchClass);
            if (StateCode == stateCode.NONE)
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] featureLen:{fpMatchClass.featureLen}");
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] feature:{fpMatchClass.feature}");
         
                return fpMatchClass;
            }
            StateCode = stateCode.READY;
            return null;
            
        }
        public bool Connect()
        {
            FpMatchClass fpMatch = new FpMatchClass();
            fpMatch.cmd = (int)CmdType.Connect;
            string json = socketClient.PostJson(fpMatch.JsonSerializationt());
            FpMatchClass fpMatch_result = json.JsonDeserializet<FpMatchClass>();
            if (fpMatch_result == null) return false;
            if ((retCode)fpMatch_result.retCode == retCode.RT_SUCCESS)
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return true;
            }
            else
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return false;
            }
        }
        public async Task<bool> ConnectAsync()
        {
            FpMatchClass fpMatch = new FpMatchClass();
            fpMatch.cmd = (int)CmdType.Connect;
            string json = await socketClient.PostJsonAsync(fpMatch.JsonSerializationt());
            FpMatchClass fpMatch_result = json.JsonDeserializet<FpMatchClass>();
            if (fpMatch_result == null) return false;
            if ((retCode)fpMatch_result.retCode == retCode.RT_SUCCESS)
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{ new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().DeclaringType.Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return true;
            }
            else
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{ new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().DeclaringType.Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return false;
            }
        }
        public bool SetDeviceType()
        {
            FpMatchClass fpMatch = new FpMatchClass();
            fpMatch.cmd = (int)CmdType.SetDeviceType;
            fpMatch.裝置類別 = 1;
            string json = socketClient.PostJson(fpMatch.JsonSerializationt());
            FpMatchClass fpMatch_result = json.JsonDeserializet<FpMatchClass>();
            if (fpMatch_result == null) return false;
            if ((retCode)fpMatch_result.retCode == retCode.RT_SUCCESS)
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return true;
            }
            else
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return false;
            }
        }
        public async Task<bool> SetDeviceTypeAsync()
        {
            FpMatchClass fpMatch = new FpMatchClass();
            fpMatch.cmd = (int)CmdType.SetDeviceType;
            fpMatch.裝置類別 = 1;
            string json = await socketClient.PostJsonAsync(fpMatch.JsonSerializationt());
            FpMatchClass fpMatch_result = json.JsonDeserializet<FpMatchClass>();
            if (fpMatch_result == null) return false;
            if ((retCode)fpMatch_result.retCode == retCode.RT_SUCCESS)
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().DeclaringType.Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return true;
            }
            else
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().DeclaringType.Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return false;
            }
        }
        private stateCode getFeature(ref FpMatchClass fpMatch)
        {
            if (this.IsOpen == false)
            {
                if (Open() == false)
                {
                    return stateCode.FAIL;
                }
            }
            fpMatch.cmd = (int)CmdType.GetFeature;
            fpMatch.裝置類別 = 1;
            fpMatch.handle = Hanndle;
            string json = socketClient.PostJson(fpMatch.JsonSerializationt());
            fpMatch = json.JsonDeserializet<FpMatchClass>();
            return (stateCode)fpMatch.stateCode;
        }
        private stateCode enroll(ref FpMatchClass fpMatch)
        {
            if(this.IsOpen == false)
            {
                if(Open() == false)
                {
                    return stateCode.FAIL;
                }
            }
            fpMatch.cmd = (int)CmdType.Enroll;
            fpMatch.裝置類別 = 1;
            fpMatch.handle = Hanndle;
            string json = socketClient.PostJson(fpMatch.JsonSerializationt());
            fpMatch = json.JsonDeserializet<FpMatchClass>();
            return (stateCode)fpMatch.stateCode;
        }   
        private bool OpenDevice()
        {
            FpMatchClass fpMatch = new FpMatchClass();
            fpMatch.cmd = (int)CmdType.OpenDevice;
            fpMatch.裝置類別 = 1;
            fpMatch.裝置號碼 = -1;
            string json = socketClient.PostJson(fpMatch.JsonSerializationt());
            FpMatchClass fpMatch_result = json.JsonDeserializet<FpMatchClass>();
            if (fpMatch_result == null) return false;
            if ((retCode)fpMatch_result.retCode == retCode.RT_SUCCESS)
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                Hanndle = fpMatch_result.handle;
                StateCode = stateCode.READY;
                return true;
            }
            else
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return false;
            }
        }
        private async Task<bool> OpenDeviceAsync()
        {
            FpMatchClass fpMatch = new FpMatchClass();
            fpMatch.cmd = (int)CmdType.OpenDevice;
            fpMatch.裝置類別 = 1;
            fpMatch.裝置號碼 = -1;

            string json = await socketClient.PostJsonAsync(fpMatch.JsonSerializationt());
            FpMatchClass fpMatch_result = json.JsonDeserializet<FpMatchClass>();
            if (fpMatch_result == null) return false;
            if ((retCode)fpMatch_result.retCode == retCode.RT_SUCCESS)
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().DeclaringType.Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                Hanndle = fpMatch_result.handle;
                return true;
            }
            else
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().DeclaringType.Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                return false;
            }
        }
        public bool CloseDevice()
        {
            FpMatchClass fpMatch = new FpMatchClass();
            fpMatch.cmd = (int)CmdType.CloseDevice;
            fpMatch.裝置類別 = 1;
            fpMatch.裝置號碼 = -1;
            string json = socketClient.PostJson(fpMatch.JsonSerializationt());
            FpMatchClass fpMatch_result = json.JsonDeserializet<FpMatchClass>();
            if (fpMatch_result == null) return false;
            if ((retCode)fpMatch_result.retCode == retCode.RT_SUCCESS)
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                Hanndle = -1;
                StateCode = stateCode.UNREADY;

                return true;
            }
            else
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{System.Reflection.MethodBase.GetCurrentMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                Hanndle = -1;
                StateCode = stateCode.UNREADY;

                return false;
            }
        }
        private async Task<bool> CloseDeviceAsync()
        {
            FpMatchClass fpMatch = new FpMatchClass();
            fpMatch.cmd = (int)CmdType.CloseDevice;
            fpMatch.裝置類別 = 1;
            fpMatch.裝置號碼 = -1;

            string json = await socketClient.PostJsonAsync(fpMatch.JsonSerializationt());
            FpMatchClass fpMatch_result = json.JsonDeserializet<FpMatchClass>();
            if (fpMatch_result == null) return false;
            if ((retCode)fpMatch_result.retCode == retCode.RT_SUCCESS)
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                Hanndle = -1;
                return true;
            }
            else
            {
                if (ConsoleWrite) Console.WriteLine($"{DateTime.Now.ToDateTimeString()} : [{new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name}] FpMatchSoket {((retCode)fpMatch_result.retCode).GetEnumName()}");
                Hanndle = -1;
                return false;
            }
        }
    }
}
