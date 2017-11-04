/****************************************************************************************
 * Copyright (c) Zachary Milliron
 *
 * This source is subject to the Microsoft Public License.
 * See https://opensource.org/licenses/MS-PL.
 * All other rights worth reserving are reserved.
 ****************************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Identd
{
    /// <summary>
    /// Represents a simple identification server for handling identification requests, as defined by RFC 1413.
    /// </summary>
    /// <remarks>
    /// See https://www.rfc-editor.org/rfc/rfc1413.txt for official protocol specification.
    /// </remarks>
    public sealed class IdentServer : IDisposable
    {
        /// <summary>
        /// The default port on which the server listens for connections.
        /// </summary>
        public const int IDENTPORT = 113;
        private const string LINETERMINATOR = "\r\n";
        private const int DEFAULTTIMEOUT = 10000;

        private Thread _serverThread;
        private string _userName;
        private TcpListener _server;
        private int _timeout;
        private bool _isDisposed = false;

        /// <summary>
        /// Gets a value indicating if the server is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets or sets the duration in milliseconds the server listens for incoming connections
        /// before timing out.  Default timeout is 10 seconds.
        /// </summary>
        public int Timeout
        {
            get { return (_timeout); }
            set
            {
                if (value < 0) { throw new ArgumentOutOfRangeException(Properties.Resources.TimeoutLessThanZeroError, nameof(Timeout)); }

                _timeout = value;
            }
        }

        /// <summary>
        /// Occurs when an exception is thrown while listening for incoming connections.
        /// </summary>
        public event EventHandler<IdentErrorEventArgs> IdentError;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ident.IdentServer"/> class.
        /// </summary>
        /// <param name="userName">The username to provide in response to an identity request.</param>
        public IdentServer(string userName)
        {
            if(userName == null) { throw new ArgumentNullException(nameof(userName)); }
            if (string.IsNullOrEmpty(userName)) { throw new ArgumentException(Properties.Resources.EmptyOrWhitespaceError, nameof(userName)); }

            _userName = userName;
            Timeout = DEFAULTTIMEOUT;
        }

        /// <summary>
        /// Releases all resources used by the object.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                IdentError = null;
                Stop();
                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Main worker method.
        /// </summary>
        private void Run()
        {
            _server = new TcpListener(IPAddress.Any, IDENTPORT);
            _server.Start();

            try
            {
                while (IsRunning)
                {
                    if (_server.Pending())
                    {
                        _server.BeginAcceptTcpClient(new AsyncCallback(OnAcceptClient), null);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                IdentError?.Invoke(this, new IdentErrorEventArgs(ex));
            }
            finally
            {
                try
                {
                    _server.Stop();
                }
                catch (Exception) { }

                IsRunning = false;
            }
        }

        /// <summary>
        /// Starts the ident server.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="ObjectDisposedException"/>
        public void Start()
        {
            if (_isDisposed) { throw new ObjectDisposedException(nameof(IdentServer)); }
            if (IsRunning) { throw new InvalidOperationException(Properties.Resources.AlreadyRunningError); }

            IsRunning = true;

            _serverThread = new Thread(new ThreadStart(Run));
            _serverThread.IsBackground = true;
            _serverThread.Start();
        }

        /// <summary>
        /// Stops the ident server.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Processes an incoming client connection.
        /// </summary>
        /// <param name="result">A variable containing state information when the client connected.</param>
        private void OnAcceptClient(IAsyncResult result)
        {
            try
            {
                using (TcpClient client = _server.EndAcceptTcpClient(result))
                {
                    client.ReceiveTimeout = Timeout;
                    client.SendTimeout = Timeout;

                    using (StreamReader input = new StreamReader(client.GetStream()))
                    {
                        using (StreamWriter output = new StreamWriter(client.GetStream()) { NewLine = LINETERMINATOR })
                        {
                            string request = input.ReadLine();
                            if (!string.IsNullOrEmpty(request))
                            {
                                request = request.Trim();

                                //not actually on Unix, but who cares
                                string response = string.Format("{0} : USERID : UNIX : {1}", request, _userName);

                                output.WriteLine(response);
                                output.Flush();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IdentError?.Invoke(this, new IdentErrorEventArgs(ex));
            }
        }
    }
}
