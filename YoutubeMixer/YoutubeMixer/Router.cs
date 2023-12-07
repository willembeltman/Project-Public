using System.Threading;
using YoutubeMixer.UserControls;

namespace YoutubeMixer.Forms
{
    public class Router
    {
        public Router()
        {
            Controllers = new List<IController>();

            Thread = new Thread(new ThreadStart(Thread_Start));
        }


        private List<IController> Controllers { get; }
        private Thread Thread { get; }
        private bool Stop { get; set; }

        public void AddController(IController controller)
        {
            Controllers.Add(controller);
        }
        public void Start()
        {
            Thread.Start();
        }
        public void Quit()
        {
            Stop = true;
            Thread.Join();
        }
        private void Thread_Start()
        {
            foreach (var controller in Controllers)
                controller.Start();

            while (!Stop)
            {
                bool shortpause = true;

                foreach (var controller in Controllers)
                    shortpause = shortpause && controller.Loop();

                if (shortpause)
                    Thread.Sleep(16);
                else
                    Thread.Sleep(100);
            }

            foreach(var controller in Controllers)
                controller.Stop();
        }
    }
}