using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace LalaSlots
{
    public class SlotMachine : GameBase
    {
        private LootTable Lewt;
        private GameData Data;
        private GameBase.GameState State;

        public class GameData
        {
            public int LalaOneFinalNumber;
            public Enums.KeybindAction LalaOneAction;
            public int LalaTwoFinalNumber;
            public Enums.KeybindAction LalaTwoAction;
            public int LalaThreeFinalNumber;
            public Enums.KeybindAction LalaThreeAction;
        }

        public override void StartGame()
        {
            this.Animations = this.TheGameAnimations;
            this.Data = new GameData();
            this.Lewt = new LootTable();
            this.InitializeGameConditions();

            this.State = GameBase.GameState.Initialized;
            Update(Enums.KeybindAction.EquipOutfit1);

            this.Worker = new BackgroundWorker();
            this.Worker.RunWorkerCompleted += GameThreadEnd;

            this.State = GameBase.GameState.Running;
            base.StartGame();
        }

        public override void Update(Enums.KeybindAction allAct, int kbWaitInMs = 50, bool sleep = false)
        {
            Update(allAct, allAct, allAct, kbWaitInMs, sleep);
        }

        public override void Update(Enums.KeybindAction lalaOneAct, Enums.KeybindAction lalaTwoAct, Enums.KeybindAction lalaThreeAct, int kbWaitInMs = 50, bool sleep = false)
        {
            this.Data.LalaOneAction = lalaOneAct;
            this.Data.LalaTwoAction = lalaTwoAct;
            this.Data.LalaThreeAction = lalaThreeAct;

            if (sleep) Thread.Sleep(1500);

            this.OnUpdate?.Invoke(this, new LalaUpdate(this.Data, this.State, kbWaitInMs));
        }

        private void GameThreadEnd(object o, RunWorkerCompletedEventArgs e)
        {
            (Enums.KeybindAction lalaOneAct, Enums.KeybindAction lalaOneFit) = FinalActionsFromFinalNumber(this.Data.LalaOneFinalNumber);
            (Enums.KeybindAction lalaTwoAct, Enums.KeybindAction lalaTwoFit) = FinalActionsFromFinalNumber(this.Data.LalaTwoFinalNumber);
            (Enums.KeybindAction lalaThreeAct, Enums.KeybindAction lalaThreeFit) = FinalActionsFromFinalNumber(this.Data.LalaThreeFinalNumber);

            this.Update(Enums.KeybindAction.EmoteSurp, 50, true);
            this.Update(lalaOneFit, lalaTwoFit, lalaThreeFit, 50, true);
            this.Update(lalaOneAct, lalaTwoAct, lalaThreeAct, 50, true);

            this.State = GameBase.GameState.Finished;
            
            if (this.Data.LalaOneFinalNumber == this.Data.LalaTwoFinalNumber &&
                this.Data.LalaOneFinalNumber == this.Data.LalaThreeFinalNumber)
            {
                // all three match!
                this.Update(Enums.KeybindAction.EmoteCheer, 50, true);
            }
            else if (this.Data.LalaOneFinalNumber == this.Data.LalaTwoFinalNumber   ||
                     this.Data.LalaOneFinalNumber == this.Data.LalaThreeFinalNumber ||
                     this.Data.LalaTwoFinalNumber == this.Data.LalaThreeFinalNumber)
            {
                // at least two match
                this.Update(Enums.KeybindAction.EmoteThumbs, 50, true);
            }
            else
            {
                // none of the numbers match. unlucky.
                this.Update(Enums.KeybindAction.EmoteFume, 50, true);
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

        private List<GameBase.Animation> TheGameAnimations { get; } =
            new List<GameBase.Animation>()
            {
                new GameBase.Animation(Enums.KeybindAction.EmoteThink,   0.00, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit2, 0.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit3, 1.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit4, 1.25, 50),

                new GameBase.Animation(Enums.KeybindAction.EmoteDance,   0.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit2, 0.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit3, 1.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit4, 1.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit2, 1.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit3, 1.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit4, 1.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit1, 1.25, 50),

                new GameBase.Animation(Enums.KeybindAction.EmotePray,    0.25, 50),
                new GameBase.Animation(Enums.KeybindAction.EquipOutfit1, 2.5, 50),
            };
    }

    public class LalaUpdate : GameBase.GameUpdate
    {
        public LalaUpdate(SlotMachine.GameData data, GameBase.GameState state, int kbWaitInMs)
        {
            this.Assign(GameBase.GameType.SlotMachine, data, state, kbWaitInMs);
        }
    }
}
