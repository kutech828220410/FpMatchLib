

using System;

namespace natrotech
{
    class Demo
    {

        public static void DoMatch()
        {

            string registerTemplate = "+lwSLbB2Pfic7//qx78flN5K+mdNQ3IrTyaa/JicqdcSZyVCtcVmOQ5CBgdnq0JczHJu7i1UkDpFTwsutdeGR/zqwtqDEZ6a5cXm3MMFL3KVWBwtXCozrWNF2ChsSN102POCl7bSfzTx3a1zOHVl1+PPmjxz0CvbgNEAcllIqO3ja7zFekDwimCh2rFCLrJhJSyk4GkprYpD70lTOJKHax1hkw1ZjM7OJlwHXg52CbpYMlwNr0/PWQw57llYtoxD8y+uzDT2a+HIfUOCq0DQ2ZMhyBNprkXWxGfXbCNONdXuleIiNmxJnM9CNTu2KSl3toc8rY5hr7eze/2Bz8OUlwtoriKhUnt6BZudrTNQwbxRQ71XcDV3qmXP84DKbUANPhwPvEUiPr9wSC1KcwUyAMzqUbgcP2LBkYdkbnoOfQbRgJOeqS8hFlhgvaOM3EVzx5A3LluwbVyIPvVPfRF5+U1rIcyMRCuJMBPKJxtkGVerpC/i2rvR9/9owlw7S+16HOmqK6ZTyRAvX6kbuaKBTSvZ6XQ3mS0vUqWOR+9TVHXF8WNFin/KQi4axCEk5EEOtoOWvcltUatEmv3c3u36VdyY24ow+kPBczo1lWGVqV1uFfgiIg/9Lx/EU+JVXhTaouioXNgIIzlZcwuo5GRR9tfoDGuT9VUyQanU4xw9ydBLKa4aspE8tqyREwNb+BEMD5WpkRkr/IgkcCivTB9/VUNjwWCZvZFcUojqibfAr6y46zmPxOFSq0I/zSQ6xIJW";
            string queryFeature= "WY4vVDiCWc7o7l3b4C5KdHfe9L7nbV4yF4EeAYzgdhvBh/mfWA6k76T8tKCBKvVK030wsHbk7Jx6gXmLBa9WqQZaJMVBLA9BFVU5dJ0T6nQtS9vegCjq+LcQWUbyZhbO1nDOmHSNsoYXRoep5/LUy48UICAa7Dg1hN1DV7K/FFo2QJOi3dNmMjefe4I4rt/ehTmhdnY+lt8IKXrE3a/5hyHJ76/Qm3jS5rooW0fIVWn+Tqcnz95yqku2il6Jm3KwZUfifr4F1FQ6N+VQ4pXFryk3QmIuvkcBwimbLYlbw3h11Jpy4eBJd3RTRfphZrrctS3JgSaTugJgQOvsdED9ZIlieIg+bN0Vvk9+1eODaCh7k4AwUxFXqcuDE7sp5BU2pKwN855j6TH8rUqMdGzIVv4PlXoDgRnaP0egh38Mtu1Ofavmxg6hPJc49r2Vy+cWgWboMpUiCKxPkNa6AYnodqAdpelS6r4Gu45oyPMuaOgYxicGRBzEBI9SLZJ2kIBEzPftbitQbcLTdxDeREQUy4I01mfuSMJBY9R5dKFr/PjsGQWrBDiEi9E4t/BTfCQf9ZUCtpkdMG+Mh9oKoPJUtKfFE3L9kvcklJ0gmq3CshSMx11IbFrAe3qRnZ+P/G+PlTmu+r2Pnj2wScc1+OMpO+pj74pcSmEFlxrXjRugLXuPwJjm3XX+8ijiHX0lu3Ci4ihs03Buxi/7y4okfHxy9QImh9p7FSyRpOmJs5MfU4bQV5bTrMNl8FWcixDu1Sps";

            byte[] templateBytes = System.Text.Encoding.Default.GetBytes(registerTemplate);
            byte[] featureBytes = System.Text.Encoding.Default.GetBytes(queryFeature);
            int nRet = 0;
            int secLevel = 3; // 默认为3 
            int[] similarity = new int[1];


            nRet = FpMatchApi.FPMatch(templateBytes, featureBytes, secLevel, ref similarity[0]);
            if ((nRet == FpMatchApi.RTC_SUCCESS) && (similarity[0] > 0))
            {
                Console.WriteLine("match success,similarity:{0}", similarity[0]);
            }
            else
            {
                Console.WriteLine("match fail, return code:{0}", nRet);
            }

        }


    }
}
