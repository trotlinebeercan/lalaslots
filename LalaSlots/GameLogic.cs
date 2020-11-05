using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace LalaSlots
{
    class GameLogic
    {
        public EventHandler<LalaUpdate> OnUpdate;

        private LootTable Lewt;
        private BackgroundWorker Worker;
        private GameData Data;
        private GameState State;

        public void StartGame()
        {
            this.Data = new GameData();
            this.Lewt = new LootTable();
            this.InitializeGameConditions();
            this.State = GameState.Initialized;

            this.Worker = new BackgroundWorker();
            this.Worker.DoWork += GameThreadStart;
            this.Worker.RunWorkerCompleted += GameThreadEnd;

            Update(Enums.KeybindAction.EquipOutfit1);

            this.Worker.RunWorkerAsync();
        }

        private void Update(Enums.KeybindAction allAct, bool sleep = false)
        {
            Update(allAct, allAct, allAct, sleep);
        }

        private void Update(Enums.KeybindAction lalaOneAct, Enums.KeybindAction lalaTwoAct, Enums.KeybindAction lalaThreeAct, bool sleep = false)
        {
            this.Data.LalaOneAction = lalaOneAct;
            this.Data.LalaTwoAction = lalaTwoAct;
            this.Data.LalaThreeAction = lalaThreeAct;

            if (sleep) Thread.Sleep(1500);

            this.OnUpdate?.Invoke(this, new LalaUpdate(this.Data, this.State));
        }

        private void GameThreadStart(object o, DoWorkEventArgs e)
        {
            this.State = GameState.Running;
            foreach (var anim in this.DanceAnimationActions)
            {
                double deltaTimestampInS = anim.DeltaTimestampInSeconds;
                Thread.Sleep((int)(deltaTimestampInS * 1000));
                this.Update(anim.Action);
            }

            (Enums.KeybindAction lalaOneAct, Enums.KeybindAction lalaOneFit) = FinalActionsFromFinalNumber(this.Data.LalaOneFinalNumber);
            (Enums.KeybindAction lalaTwoAct, Enums.KeybindAction lalaTwoFit) = FinalActionsFromFinalNumber(this.Data.LalaTwoFinalNumber);
            (Enums.KeybindAction lalaThreeAct, Enums.KeybindAction lalaThreeFit) = FinalActionsFromFinalNumber(this.Data.LalaThreeFinalNumber);

            
            this.Update(Enums.KeybindAction.EmoteSurp, true);
            this.Update(lalaOneFit, lalaTwoFit, lalaThreeFit, true);
            this.Update(lalaOneAct, lalaTwoAct, lalaThreeAct, true);
        }

        private void GameThreadEnd(object o, RunWorkerCompletedEventArgs e)
        {
            this.State = GameState.Finished;
            
            if (this.Data.LalaOneFinalNumber == this.Data.LalaTwoFinalNumber &&
                this.Data.LalaOneFinalNumber == this.Data.LalaThreeFinalNumber)
            {
                // all three match!
                this.Update(Enums.KeybindAction.EmoteCheer, true);
            }
            else if (this.Data.LalaOneFinalNumber == this.Data.LalaTwoFinalNumber   ||
                     this.Data.LalaOneFinalNumber == this.Data.LalaThreeFinalNumber ||
                     this.Data.LalaTwoFinalNumber == this.Data.LalaThreeFinalNumber)
            {
                // at least two match
                this.Update(Enums.KeybindAction.EmoteThumbs, true);
            }
            else
            {
                // none of the numbers match. unlucky.
                this.Update(Enums.KeybindAction.EmoteFume, true);
            }
        }

        private void InitializeGameConditions()
        {
            Loot chosenLoot = this.Lewt.NextRandomItem();

            int final0 = 0, final1 = 0, final2 = 0;
            switch (chosenLoot)
            {
                case Loot.Three:
                    final0 = final1 = final2 = RNG.Random(2, 5);
                    break;

                case Loot.Two:
                    final0 = final1 = final2 = RNG.Random(2, 5);
                    while (final2 == final0) final2 = RNG.Random(2, 5);
                    break;

                case Loot.None:
                    final0 = RNG.Random(2, 5);
                    if (final0 == 2)
                    {
                        final1 = 3;
                        final2 = 4;
                    }
                    else if (final0 == 3)
                    {
                        final1 = 4;
                        final2 = 2;
                    }
                    else if (final0 == 4)
                    {
                        final1 = 3;
                        final2 = 2;
                    }
                    break;
            }
            this.Data.LalaOneFinalNumber   = final0;
            this.Data.LalaTwoFinalNumber   = final1;
            this.Data.LalaThreeFinalNumber = final2;
        }

        private (Enums.KeybindAction, Enums.KeybindAction) FinalActionsFromFinalNumber(int number)
        {
            if (number == 2) return (Enums.KeybindAction.MacroSayRed,   Enums.KeybindAction.EquipOutfit2);
            if (number == 3) return (Enums.KeybindAction.MacroSayGreen, Enums.KeybindAction.EquipOutfit3);
            if (number == 4) return (Enums.KeybindAction.MacroSayBlue,  Enums.KeybindAction.EquipOutfit4);

            return (Enums.KeybindAction.EmoteThink, Enums.KeybindAction.EquipOutfit1);
        }

        private static class RNG
        {
            private static Random random;
            public static int Random(int min, int max)
            {
                if (random == null) random = new Random();
                return random.Next(min, max);
            }
        }

        private class Animation
        {
            public Enums.KeybindAction Action;
            public double DeltaTimestampInSeconds;

            public Animation(Enums.KeybindAction a, double ts)
            {
                this.Action = a;
                this.DeltaTimestampInSeconds = ts;
            }
        }

        private List<Animation> DanceAnimationActions { get; } =
            new List<Animation>()
            {
                new Animation(Enums.KeybindAction.EmoteThink,   0.0),
                new Animation(Enums.KeybindAction.EquipOutfit2, 0.25),
                new Animation(Enums.KeybindAction.EquipOutfit3, 1.25),
                new Animation(Enums.KeybindAction.EquipOutfit4, 1.25),

                new Animation(Enums.KeybindAction.EmoteDance,   0.25),
                new Animation(Enums.KeybindAction.EquipOutfit2, 0.25),
                new Animation(Enums.KeybindAction.EquipOutfit3, 1.25),
                new Animation(Enums.KeybindAction.EquipOutfit4, 1.25),
                new Animation(Enums.KeybindAction.EquipOutfit2, 1.25),
                new Animation(Enums.KeybindAction.EquipOutfit3, 1.25),
                new Animation(Enums.KeybindAction.EquipOutfit4, 1.25),
                new Animation(Enums.KeybindAction.EquipOutfit1, 1.25),

                new Animation(Enums.KeybindAction.EmotePray, 0.25),
                new Animation(Enums.KeybindAction.EquipOutfit1, 2.5),
            };
    }

    public class GameData
    {
        public int LalaOneFinalNumber;
        public Enums.KeybindAction LalaOneAction;
        public int LalaTwoFinalNumber;
        public Enums.KeybindAction LalaTwoAction;
        public int LalaThreeFinalNumber;
        public Enums.KeybindAction LalaThreeAction;
    }

    public enum GameState
    {
        Initialized = 0,
        Running     = 1,
        Finished    = 2,
    }

    public class LalaUpdate
    {
        public LalaUpdate(GameData data, GameState state)
        {
            Data = data;
            State = state;
        }
        public readonly GameData Data;
        public readonly GameState State;
    }
}
