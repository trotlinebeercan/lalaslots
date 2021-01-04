using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LalaSlots
{
    public abstract class GameBase
    {
        public enum GameType
        {
            SlotMachine = 0,
            Nutcracker  = 1,
        }

        public enum GameState
        {
            Initialized = 0,
            Running = 1,
            Finished = 2,
        }

        public static class RNG
        {
            private static Random random;
            public static int Random(int min, int max)
            {
                if (random == null) random = new Random();
                return random.Next(min, max);
            }
        }

        public class GameUpdate
        {
            public GameUpdate() {}

            public void Assign(GameBase.GameType type, object data, GameBase.GameState state, int kbWaitInMs)
            {
                this.Data = data;
                this.State = state;
                this.KeybindWaitInMs = kbWaitInMs;
            }

            public GameBase.GameType Type { private set; get; }
            public object Data { private set; get; }
            public GameBase.GameState State { private set; get; }
            public int KeybindWaitInMs { private set; get; }
        }

        public class Animation
        {
            public Enums.KeybindAction Action;
            public double AnimationWaitInSeconds;
            public int KeybindWaitInMillisecs;

            public Animation(Enums.KeybindAction a, double animTs, int kbTs)
            {
                this.Action = a;
                this.AnimationWaitInSeconds = animTs;
                this.KeybindWaitInMillisecs = kbTs;
            }
        }

        public List<GameBase.Animation> Animations { get; set; }

        public BackgroundWorker Worker;
        public EventHandler<object> OnUpdate;

        virtual public void StartGame()
        {
            if (this.Worker == null)
                this.Worker = new BackgroundWorker();

            this.Worker.DoWork += GameThreadStart;
            this.Worker.RunWorkerAsync();
        }

        private void GameThreadStart(object o, DoWorkEventArgs e)
        {
            foreach (var anim in this.Animations)
            {
                double deltaTimestampInS = anim.AnimationWaitInSeconds;
                Thread.Sleep((int)(deltaTimestampInS * 1000));
                this.Update(anim.Action, anim.KeybindWaitInMillisecs);
            }
        }

        abstract public void Update(Enums.KeybindAction action, int kbWaitInMs = 50, bool sleep = false);
        abstract public void Update(Enums.KeybindAction lalaOneAct, Enums.KeybindAction lalaTwoAct, Enums.KeybindAction lalaThreeAct, int kbWaitInMs = 50, bool sleep = false);
    }
}
