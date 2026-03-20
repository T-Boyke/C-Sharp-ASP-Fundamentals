using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace _10_Filmdatenbank.PlaywrightTests.Infrastructure
{
    public class TestHost<TProgram> : IDisposable where TProgram : class
    {
        private Process? _process;
        public string BaseUrl { get; } = "http://127.0.0.1:5018";

        public TestHost()
        {
            try
            {
                var baseDir = AppContext.BaseDirectory;
                var rootDir = baseDir;
                while (rootDir != null && !Directory.Exists(Path.Combine(rootDir, "src")))
                {
                    rootDir = Path.GetDirectoryName(rootDir);
                }

                if (rootDir == null) throw new Exception("Could not find root directory with 'src' folder.");

                var projectDir = Path.Combine(rootDir, "src", "_10_Filmdatenbank.Web");
                
                var dbName = $"E2E_Db_{Guid.NewGuid()}";
                _process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"run --project \"{projectDir}\" --environment E2ETesting --urls {BaseUrl}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };
                _process.StartInfo.EnvironmentVariables["E2E_DB_NAME"] = dbName;

                _process.OutputDataReceived += (s, e) => { if (e.Data != null) Console.WriteLine($"[WebProcess] {e.Data}"); };
                _process.ErrorDataReceived += (s, e) => { if (e.Data != null) Console.WriteLine($"[WebError] {e.Data}"); };

                _process.Start();
                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();
                Console.WriteLine($"[TestHost] Starting background process in {projectDir}");
                
                // Wait for port 5018 to be open
                bool isReady = false;
                for (int i = 0; i < 30; i++) // 30 seconds max
                {
                    try
                    {
                        using var client = new TcpClient("127.0.0.1", 5018);
                        isReady = true;
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }

                if (!isReady)
                {
                    throw new Exception("Timed out waiting for port 5018 to be open.");
                }

                Console.WriteLine("[TestHost] Port 5018 is OPEN. Host is ready.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TestHost] Critical failure: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_process != null && !_process.HasExited)
            {
                try { _process.Kill(true); } catch { }
                _process.Dispose();
            }
        }
    }
}
