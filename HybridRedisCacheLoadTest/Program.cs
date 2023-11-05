using HybridRedisCacheLoadTest;

public class Program
{


    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine(@"Clear All : 1
Test Storage : 2
Test Read : 3
Add Key : 4
Get Key : 5
Remove Key : 6");

            switch (Console.ReadLine())
            {
                case "1":
                    await (new LoadTest()).ClearAll();
                    break;
                case "2":
                    await (new LoadTest()).TestStorage();
                    break;
                case "3":
                    await (new LoadTest()).TestReadStorage();
                    break;
                case "4":
                    await (new LoadTest()).AddKey(Console.ReadLine(), Console.ReadLine());
                    break;
                case "5":
                    await (new LoadTest()).GetKey(Console.ReadLine());
                    break;
                case "6":
                    await (new LoadTest()).RemoveKey(Console.ReadLine());
                    break;
            }
        }

        Console.ReadLine();
    }



    private static async Task TestMultipleServerValue()
    {
        var loadTest = new LoadTest();
        var type = Console.ReadLine();
        if (type == "1")
            await loadTest.TestSetValue();
        else
            await loadTest.TestGetValue();

    }


    private static async Task TestLoad()
    {
        var loadTest = new LoadTest();
        await loadTest.TestReadStorage();
    }



    //public static void TstTcp()
    //{
    //    const string endPoint = @"";
    //    var times = new List<double>();
    //    for (int i = 0; i < 4; i++)
    //    {
    //        var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
    //        {
    //            Blocking = false,
    //            DontFragment = ConsoleTraceListener string endPoint,
    //            DualMode = false,
    //            EnableBroadcast = false,
    //            ExclusiveAddressUse = false,
    //            LingerState = null,
    //            MulticastLoopback = false,
    //            NoDelay = false,
    //            ReceiveBufferSize = 0,
    //            ReceiveTimeout = 0,
    //            SendBufferSize = 0,
    //            SendTimeout = 0,
    //            Ttl = 0
    //        }
    //            asd;
    //        sock.Blocking = true;

    //        var stopwatch = new Stopwatch();

    //        // Measure the Connect call only
    //        stopwatch.Start();
    //        sock.Connect(endPoint);
    //        stopwatch.Stop();

    //        double t = stopwatch.Elapsed.TotalMilliseconds;
    //        Console.WriteLine("{0:0.00}ms", t);
    //        times.Add(t);

    //        sock.Close();

    //        Thread.Sleep(1000);
    //    }
    //}

}