using System.Threading;

namespace IoTVenice
{
    public delegate void ParameterizedWorkerThreadStart(object status);

    public class WorkerThread
    {
        private ParameterizedWorkerThreadStart workerCallback;
        private Thread internalThread;
        private object status;

        public WorkerThread(ParameterizedWorkerThreadStart workerThread)
        {
            this.workerCallback = workerThread;
        }

        public void Start(object status)
        {
            this.internalThread = new Thread(this.InternalThread);
            this.status = status;
            this.internalThread.Start();
        }

        private void InternalThread()
        {
            this.workerCallback(this.status);
        }
    }
}
