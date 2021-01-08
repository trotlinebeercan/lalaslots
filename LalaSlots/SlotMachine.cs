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

        public SlotMachine()
        {
            this.Type = GameType.SlotMachine;
        }

        private class GameData
        {
            public int LalaOneFinalNumber;
            public int LalaTwoFinalNumber;
            public int LalaThreeFinalNumber;
        }

        public override void StartGame()
        {
            base.SetAnimations(this.TheGameAnimations, this.TheGameAnimations, this.TheGameAnimations);
            base.SetThreadEndHandlers(this.LalaOneAnimationsEnded, this.LalaTwoAnimationsEnded, this.LalaThreeAnimationsEnded);

            this.Data = new GameData();
            this.Lewt = new LootTable();
            this.InitializeGameConditions();

            this.State = GameState.Initialized;
            UpdateAll(Enums.KeybindAction.EquipOutfit1);

            this.State = GameState.Running;
            base.StartGame();
        }

        protected override void GameFinished()
        {
            this.State = GameState.Finished;
            GameUpdate newUpdate = new GameUpdate();
            newUpdate.Assign(this.Type, this.Data, this.State);
            base.UpdateSystem(newUpdate);
        }

        private void UpdateAll(Enums.KeybindAction act, int kbWaitInMs = 50)
        {
            this.Update(TargetLala.First,  act, kbWaitInMs);
            this.Update(TargetLala.Second, act, kbWaitInMs);
            this.Update(TargetLala.Third,  act, kbWaitInMs);
        }

        private void UpdateSleep(TargetLala target, Enums.KeybindAction act)
        {
            this.Update(target, act);
            Thread.Sleep(1500);
        }

        public override void Update(TargetLala target, Enums.KeybindAction act, int kbWaitInMs = 50)
        {
            GameUpdate newUpdate = new GameUpdate();
            newUpdate.Assign(this.Type, target, act, this.Data, this.State, kbWaitInMs);
            base.UpdateSystem(newUpdate);
        }

        public override void InitCalled(GameUpdate update, MainWindow game)
        {
            game.label_LalaOne_FinalNumber.Content = (update.Data as GameData).LalaOneFinalNumber;
            game.label_LalaTwo_FinalNumber.Content = (update.Data as GameData).LalaTwoFinalNumber;
            game.label_LalaThree_FinalNumber.Content = (update.Data as GameData).LalaThreeFinalNumber;
        }

        public override void RunCalled(GameUpdate update, MainWindow game)
        {
            if (update.Target == TargetLala.First)
            {
                game.LalaOneKeybindWait = update.KeybindWaitInMs;
                game.cbox_LalaOne_SelectEmote.SelectedValue = update.Action;
            }
            else if (update.Target == TargetLala.Second)
            {
                game.LalaTwoKeybindWait = update.KeybindWaitInMs;
                game.cbox_LalaTwo_SelectEmote.SelectedValue = update.Action;
            }
            else if (update.Target == TargetLala.Third)
            {
                game.LalaThreeKeybindWait = update.KeybindWaitInMs;
                game.cbox_LalaThree_SelectEmote.SelectedValue = update.Action;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void CloseCalled(GameUpdate update, MainWindow game)
        {
            game.button_StartTheGame.IsEnabled = true;
        }

        private void LalaAnimationsEnded(TargetLala target, Enums.KeybindAction act, Enums.KeybindAction fit)
        {
            this.UpdateSleep(target, Enums.KeybindAction.EmoteSurp);
            this.UpdateSleep(target, fit);
            this.UpdateSleep(target, act);

            if (this.Data.LalaOneFinalNumber == this.Data.LalaTwoFinalNumber &&
                this.Data.LalaOneFinalNumber == this.Data.LalaThreeFinalNumber)
            {
                // all three match!
                this.UpdateSleep(target, Enums.KeybindAction.EmoteCheer);
            }
            else if (this.Data.LalaOneFinalNumber == this.Data.LalaTwoFinalNumber   ||
                     this.Data.LalaOneFinalNumber == this.Data.LalaThreeFinalNumber ||
                     this.Data.LalaTwoFinalNumber == this.Data.LalaThreeFinalNumber)
            {
                // at least two match
                this.UpdateSleep(target, Enums.KeybindAction.EmoteThumbs);
            }
            else
            {
                // none of the numbers match. unlucky.
                this.UpdateSleep(target, Enums.KeybindAction.EmoteFume);
            }
        }

        private void LalaOneAnimationsEnded(object o, RunWorkerCompletedEventArgs e)
        {
            (Enums.KeybindAction lalaOneAct, Enums.KeybindAction lalaOneFit) = FinalActionsFromFinalNumber(this.Data.LalaOneFinalNumber);
            this.LalaAnimationsEnded(TargetLala.First, lalaOneAct, lalaOneFit);
        }

        private void LalaTwoAnimationsEnded(object o, RunWorkerCompletedEventArgs e)
        {
            (Enums.KeybindAction lalaTwoAct, Enums.KeybindAction lalaTwoFit) = FinalActionsFromFinalNumber(this.Data.LalaTwoFinalNumber);
            this.LalaAnimationsEnded(TargetLala.Second, lalaTwoAct, lalaTwoFit);
        }

        private void LalaThreeAnimationsEnded(object o, RunWorkerCompletedEventArgs e)
        {
            (Enums.KeybindAction lalaThreeAct, Enums.KeybindAction lalaThreeFit) = FinalActionsFromFinalNumber(this.Data.LalaThreeFinalNumber);
            this.LalaAnimationsEnded(TargetLala.Third, lalaThreeAct, lalaThreeFit);
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
}
