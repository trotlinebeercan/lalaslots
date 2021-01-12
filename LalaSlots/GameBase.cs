using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LalaSlots
{
    public enum GameType
    {
        SlotMachine = 0,
        Nutcracker = 1,
    }

    public enum GameState
    {
        Initialized = 0,
        Running = 1,
        Finished = 2,
    }

    public enum TargetLala
    {
        First = 0,
        Second = 1,
        Third = 2,
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
        public void Assign(GameType type, object data, GameState state)
        {
            this.Type = type;
            this.Data = data;
            this.State = state;
        }

        public void Assign(GameType type, TargetLala target, Enums.KeybindAction act, object data, GameState state, int kbWaitInMs)
        {
            this.Type = type;
            this.Target = target;
            this.Action = act;
            this.Data = data;
            this.State = state;
            this.KeybindWaitInMs = kbWaitInMs;
        }

        public GameType Type { private set; get; }
        public TargetLala Target { private set; get; }
        public Enums.KeybindAction Action { private set; get; }
        public object Data { private set; get; }
        public GameState State { private set; get; }
        public int KeybindWaitInMs { private set; get; }
    }

    public abstract class GameBase
    {
        public EventHandler<GameUpdate> OnUpdate;

        protected GameType Type;
        protected GameState State;

        private FullLalaDataObject LalaOne   = new FullLalaDataObject(TargetLala.First);
        private FullLalaDataObject LalaTwo   = new FullLalaDataObject(TargetLala.Second);
        private FullLalaDataObject LalaThree = new FullLalaDataObject(TargetLala.Third);

        abstract public void Update(TargetLala target, Enums.KeybindAction action, int kbWaitInMs);

        abstract public void InitCalled(GameUpdate update, MainWindow game);
        abstract public void RunCalled(GameUpdate update, MainWindow game);
        abstract public void CloseCalled(GameUpdate update, MainWindow game);

        protected class Animation
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

        private class FullLalaDataObject
        {
            public FullLalaDataObject(TargetLala target)
            {
                this.Target = target;
                this.Worker = new BackgroundWorker();
            }

            public List<GameBase.Animation> Animations { get; set; }
            public BackgroundWorker Worker;
            public TargetLala Target;
        }

        virtual public void StartGame()
        {
            this.LalaOne.Worker.DoWork   += delegate { ProcessEvents(this.LalaOne);   };
            this.LalaTwo.Worker.DoWork   += delegate { ProcessEvents(this.LalaTwo);   };
            this.LalaThree.Worker.DoWork += delegate { ProcessEvents(this.LalaThree); };

            BackgroundWorker thisWorker = new BackgroundWorker();
            thisWorker.DoWork += delegate
            {
                this.LalaOne.Worker.RunWorkerAsync();
                this.LalaTwo.Worker.RunWorkerAsync();
                this.LalaThree.Worker.RunWorkerAsync();
            };
            thisWorker.RunWorkerAsync();
        }

        abstract protected void GameFinished();

        protected void UpdateSystem(GameUpdate update)
        {
            this.OnUpdate?.Invoke(this, update);
        }

        protected void SetAnimations(List<Animation> one, List<Animation> two, List<Animation> three)
        {
            this.LalaOne.Animations   = one;
            this.LalaTwo.Animations   = two;
            this.LalaThree.Animations = three;
        }

        protected void SetThreadEndHandlers(RunWorkerCompletedEventHandler one, RunWorkerCompletedEventHandler two, RunWorkerCompletedEventHandler three)
        {
            this.LalaOne.Worker.RunWorkerCompleted   += one;
            this.LalaTwo.Worker.RunWorkerCompleted   += two;
            this.LalaThree.Worker.RunWorkerCompleted += three;

            //this.LalaThree.Worker.RunWorkerCompleted += delegate (object o, RunWorkerCompletedEventArgs e)
            //{
            //    GameFinished();
            //};
        }

        private void ProcessEvents(FullLalaDataObject lala)
        {
            foreach (var anim in lala.Animations)
            {
                double deltaTimestampInS = anim.AnimationWaitInSeconds;
                Thread.Sleep((int)(deltaTimestampInS * 1000));
                this.Update(lala.Target, anim.Action, anim.KeybindWaitInMillisecs);
            }
        }
    }
}
