using System.Threading;
#if WINDOWS_APP
using System.Threading.Tasks;
#endif

namespace AzureSBLite.Examples
{
    public delegate void ParameterizedWorkerThreadStart(object status);

    public class WorkerThread
    {
        private ParameterizedWorkerThreadStart workerCallback;
#if !WINDOWS_APP
        private Thread internalThread;
#endif
        private object status;

        public WorkerThread(ParameterizedWorkerThreadStart workerThread)
        {
            this.workerCallback = workerThread;
        }

        public void Start(object status)
        {
#if !WINDOWS_APP
            this.internalThread = new Thread(this.InternalThread);
            this.status = status;
            this.internalThread.Start();
#else
            Task.Run(() =>
            {
                this.status = status;
                this.InternalThread();
            });
#endif
        }

        private void InternalThread()
        {
            this.workerCallback(this.status);
        }
    }
}
