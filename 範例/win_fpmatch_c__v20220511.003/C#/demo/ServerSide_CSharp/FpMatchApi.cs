#define x64
using System.Runtime.InteropServices;

namespace natrotech
{
    class FpMatchApi
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
    }
}
