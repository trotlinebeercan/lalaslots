﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace LalaSlots
{
    public class Nutcracker : GameBase
    {
        private GameData Data;

        public Nutcracker()
        {
            this.Type = GameType.SlotMachine;
        }

        private class GameData
        {
            public int MightNotBeNeeded;
        }

        public override void StartGame()
        {
            base.SetAnimations(this.LalaOneAnimations, this.LalaTwoAnimations, this.LalaThreeAnimations);
            base.SetThreadEndHandlers(this.LalaOneAnimationsEnded, this.LalaTwoAnimationsEnded, this.LalaThreeAnimationsEnded);

            this.Data = new GameData();

            this.State = GameState.Initialized;
            UpdateAll(Enums.KeybindAction.EquipOutfit1);

            this.State = GameState.Running;
            base.StartGame();
        }

        protected override void GameFinished()
        {
            Console.WriteLine("[NUT] - GameFinished");

            this.State = GameState.Finished;
            GameUpdate newUpdate = new GameUpdate();
            newUpdate.Assign(this.Type, this.Data, this.State);
            base.UpdateSystem(newUpdate);
        }

        private void UpdateAll(Enums.KeybindAction act, int kbWaitInMs = 50)
        {
            this.Update(TargetLala.First, act, kbWaitInMs);
            this.Update(TargetLala.Second, act, kbWaitInMs);
            this.Update(TargetLala.Third, act, kbWaitInMs);
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
            Console.WriteLine("[NUT] - InitCalled");
            game.button_StartTheGame.IsEnabled = false;
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
            Console.WriteLine("[NUT] - CloseCalled");
            game.button_StartTheGame.IsEnabled = true;
        }

        private void LalaAnimationsEnded(TargetLala target, Enums.KeybindAction act)
        {
            this.UpdateSleep(target, act);
        }

        private void LalaOneAnimationsEnded(object o, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("[NUT] - LalaOneAnimationsEnded");
            //this.LalaAnimationsEnded(TargetLala.First, Enums.KeybindAction.EmoteDoze);
        }

        private void LalaTwoAnimationsEnded(object o, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("[NUT] - LalaTwoAnimationsEnded");
            //this.LalaAnimationsEnded(TargetLala.Second, Enums.KeybindAction.EmoteDoze);
        }

        private void LalaThreeAnimationsEnded(object o, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("[NUT] - LalaThreeAnimationsEnded");
            //this.LalaAnimationsEnded(TargetLala.Third, Enums.KeybindAction.EmoteDoze);
        }

        /*
         *  45 - lalafells walk into position (15s)
            60/100 - lalafells do normal dance
            100/145 - lalafells run around and cause mayhem (to the best of your ability)
            145/155 - lalafells do /stepdance
            155 - lalafells /bow
            run offstage

            totals: 155s
        */

        const double StartOfAnimationInS = 45.0;

        private List<GameBase.Animation> LalaOneAnimations { get; } =
            new List<GameBase.Animation>()
            {
                new GameBase.Animation(Enums.KeybindAction.DiagLeft, StartOfAnimationInS, 1500),
                new GameBase.Animation(Enums.KeybindAction.EmoteThink, 5.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 10.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 9.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 9.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 9.0, 50),
                // 87s

                // here be the chaos
                new GameBase.Animation(Enums.KeybindAction.ToggleRun,    7.0, 50),
                new GameBase.Animation(Enums.KeybindAction.WalkBackward, 2.0, 400),
                new GameBase.Animation(Enums.KeybindAction.StrafeRight,  2.0, 800),
                new GameBase.Animation(Enums.KeybindAction.EmoteThumbs,  2.0, 50),
                new GameBase.Animation(Enums.KeybindAction.StrafeLeft,  15.0, 800),
                new GameBase.Animation(Enums.KeybindAction.WalkForward,  2.0, 250),
                new GameBase.Animation(Enums.KeybindAction.ToggleRun,    2.0, 50),
                // </chaos>
                // +32 = 119

                // this has to start at 145
                new GameBase.Animation(Enums.KeybindAction.EmoteFume, 26.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDoze, 10.0, 50),
            };

        private List<GameBase.Animation> LalaTwoAnimations { get; } =
            new List<GameBase.Animation>()
            {
                new GameBase.Animation(Enums.KeybindAction.WalkForward, StartOfAnimationInS, 1200),
                new GameBase.Animation(Enums.KeybindAction.EmoteThink, 5.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 10.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 9.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 9.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 9.0, 50),
                // 87s

                // should this lala also chaos?
                new GameBase.Animation(Enums.KeybindAction.EmoteAirQ,  7.0, 50),
                // </chaos>
                // +7 = 94

                // this has to start at 145
                new GameBase.Animation(Enums.KeybindAction.EmoteFume, 51.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDoze, 10.0, 50),
            };

        private List<GameBase.Animation> LalaThreeAnimations { get; } =
            new List<GameBase.Animation>()
            {
                new GameBase.Animation(Enums.KeybindAction.DiagRight, StartOfAnimationInS, 1500),
                new GameBase.Animation(Enums.KeybindAction.EmoteThink, 5.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 10.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 9.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 9.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDance, 9.0, 50),
                // 87s

                // here be the chaos
                new GameBase.Animation(Enums.KeybindAction.ToggleRun,    7.0, 50),
                new GameBase.Animation(Enums.KeybindAction.WalkBackward, 2.0, 500),
                new GameBase.Animation(Enums.KeybindAction.StrafeLeft,   2.0, 1000),
                new GameBase.Animation(Enums.KeybindAction.EmoteCheer,   2.0, 50),
                new GameBase.Animation(Enums.KeybindAction.StrafeRight, 15.0, 1000),
                new GameBase.Animation(Enums.KeybindAction.WalkForward,  2.0, 300),
                new GameBase.Animation(Enums.KeybindAction.ToggleRun,    2.0, 50),
                // </chaos>
                // +32 = 119

                // this has to start at 145
                new GameBase.Animation(Enums.KeybindAction.EmoteFume, 26.0, 50),
                new GameBase.Animation(Enums.KeybindAction.EmoteDoze, 10.0, 50),
            };
    }
}
