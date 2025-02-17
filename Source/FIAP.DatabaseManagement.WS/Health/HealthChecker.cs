using System.Net;
using System.Text;

namespace FIAP.DatabaseManagement.WS.Health
{
    public static class HealthChecker
    {
        public static void StartHealthCheckServer(int port)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://*:{port}/health/");
            listener.Start();

            Console.WriteLine($"Health check server running on http://localhost:{port}/health");

            while (true)
            {
                try
                {
                    var context = listener.GetContext();
                    var response = context.Response;

                    var buffer = Encoding.UTF8.GetBytes("OK");
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.OutputStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Health check error: {ex.Message}");
                }
            }
        }
    }
}
