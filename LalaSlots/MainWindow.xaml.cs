namespace LalaSlots
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Timers;
    using System.Windows;
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        private GameBase TheGame;

        // timers for the clock in the top-left window
        Timer RunTimer;
        TimeSpan Time;

        private LalaSlot LalaOne;
        private LalaSlot LalaTwo;
        private LalaSlot LalaThree;

        // this is such bad code. but, when i first wrote this, i two-way bound the members
        // to the UI elements for debugging / manual action with the dropdown menu
        // so, here we are
        public int LalaOneKeybindWait;
        public int LalaTwoKeybindWait;
        public int LalaThreeKeybindWait;

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Internals
        ///////////////////////////////////////////////////////////////////////////////////////////

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Time = new TimeSpan(0);
            this.RunTimer = new Timer();
            this.RunTimer.Interval = 500;
            this.RunTimer.Elapsed += DispatcherTimer_Tick;

            // TODO: Drop-down menu to select game
            TheGame = new Nutcracker();
            TheGame.OnUpdate += delegate (object o, GameUpdate update)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Send, new Action(() => { UpdateSystem(o, update); }));
            };

            FFXIVMemory.InitializeMemory();
        }

        void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.Time += new TimeSpan(0, 0, 0, 0, 500);
            this.Dispatcher.Invoke(() =>
            {
                label_Timer.Content = string.Format("{0:00}:{1:00}:{2:00}", this.Time.Minutes, this.Time.Seconds, this.Time.Milliseconds / 10);
            });
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // System Components
        ///////////////////////////////////////////////////////////////////////////////////////////

        private void UpdateSystem(object parent, GameUpdate update)
        {
            GameBase gameBase = (parent as GameBase);
            if (update.State == GameState.Initialized)
            {
                gameBase.InitCalled(update, this);
            }
            else if (update.State == GameState.Running)
            {
                gameBase.RunCalled(update, this);
            }
            else if (update.State == GameState.Finished)
            {
                gameBase.CloseCalled(update, this);

                this.RunTimer.Stop();
                this.Time = new TimeSpan(0);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // UI Element Event Handlers
        ///////////////////////////////////////////////////////////////////////////////////////////

        private void button_StartTheGame_Click(object sender, RoutedEventArgs e)
        {
            this.RunTimer.Start();

            this.button_StartTheGame.IsEnabled = false;
            TheGame.StartGame();
        }

        private void button_LalaOne_Select_Click(object sender, RoutedEventArgs e)
        {
            ProcessSelection processSelection = new ProcessSelection();
            Nullable<bool> dialogResult = processSelection.ShowDialog();
            if (dialogResult == true)
            {
                if (processSelection.SelectedLalaSlot != null)
                {
                    this.button_LalaOne_Select.Content = processSelection.SelectedLalaSlot.Id.ToString();
                    this.button_LalaOne_Select.IsEnabled = false;
                    this.label_LalaOne_Name.Content = processSelection.SelectedLalaSlot.Name;

                    this.LalaOne = processSelection.SelectedLalaSlot;
                }
            }
        }

        private void button_LalaTwo_Select_Click(object sender, RoutedEventArgs e)
        {
            ProcessSelection processSelection = new ProcessSelection();
            Nullable<bool> dialogResult = processSelection.ShowDialog();
            if (dialogResult == true)
            {
                if (processSelection.SelectedLalaSlot != null)
                {
                    this.button_LalaTwo_Select.Content = processSelection.SelectedLalaSlot.Id.ToString();
                    this.button_LalaTwo_Select.IsEnabled = false;
                    this.label_LalaTwo_Name.Content = processSelection.SelectedLalaSlot.Name;

                    this.LalaTwo = processSelection.SelectedLalaSlot;
                }
            }
        }

        private void button_LalaThree_Select_Click(object sender, RoutedEventArgs e)
        {
            ProcessSelection processSelection = new ProcessSelection();
            Nullable<bool> dialogResult = processSelection.ShowDialog();
            if (dialogResult == true)
            {
                if (processSelection.SelectedLalaSlot != null)
                {
                    this.button_LalaThree_Select.Content = processSelection.SelectedLalaSlot.Id.ToString();
                    this.button_LalaThree_Select.IsEnabled = false;
                    this.label_LalaThree_Name.Content = processSelection.SelectedLalaSlot.Name;

                    this.LalaThree = processSelection.SelectedLalaSlot;
                }
            }
        }

        public Dictionary<Enums.KeybindAction, string> KeybindActionEnumsWithCaptions { get; } =
            new Dictionary<Enums.KeybindAction, string>()
            {
                { Enums.KeybindAction.EquipOutfit1,  "Equip 1"     },
                { Enums.KeybindAction.EquipOutfit2,  "Equip 2"     },
                { Enums.KeybindAction.EquipOutfit3,  "Equip 3"     },
                { Enums.KeybindAction.EquipOutfit4,  "Equip 4"     },
                { Enums.KeybindAction.EmoteCheer,    "Cheer"       },
                { Enums.KeybindAction.EmoteSurp,     "Surprised"   },
                { Enums.KeybindAction.EmoteThumbs,   "Thumbs Up"   },
                { Enums.KeybindAction.EmoteNo,       "No"          },
                { Enums.KeybindAction.EmoteFume,     "Fume"        },
                { Enums.KeybindAction.EmoteFPalm,    "Harvest"     },
                { Enums.KeybindAction.EmoteFlex,     "Flex"        },
                { Enums.KeybindAction.EmoteAirQ,     "Ball Dance"  },
                { Enums.KeybindAction.EmoteDance,    "Dance"       },
                { Enums.KeybindAction.EmoteSit,      "Sit"         },
                { Enums.KeybindAction.EmotePray,     "Pray"        },
                { Enums.KeybindAction.EmoteBeckon,   "Beckon"      },
                { Enums.KeybindAction.EmoteThink,    "Think"       },
                { Enums.KeybindAction.EmoteDoze,     "Doze"        },
                { Enums.KeybindAction.EmoteBow,      "Bow"         },
                { Enums.KeybindAction.EmotePose,     "Pose"        },
                { Enums.KeybindAction.EmoteWelcome,  "Welcome"     },
                { Enums.KeybindAction.MacroSayRed,   "Say Red"     },
                { Enums.KeybindAction.MacroSayGreen, "Say Blue"    },
                { Enums.KeybindAction.MacroSayBlue,  "Say Green"   },
                { Enums.KeybindAction.EmoteStepDnc,  "Step Dance"  },
                { Enums.KeybindAction.EmoteHarvDnc,  "Harvest Dnc" },
                { Enums.KeybindAction.EmoteBallDnc,  "Ball Dance"  },
                { Enums.KeybindAction.EmoteBlowKiss, "Blow Kiss"   },
                { Enums.KeybindAction.EmoteCongrats, "Congrats"    },
                { Enums.KeybindAction.EmoteWave,     "Wave"        },
                { Enums.KeybindAction.EmoteLookout,  "Lookout"     },
                { Enums.KeybindAction.WalkForward,   "Fowards"     },
                { Enums.KeybindAction.WalkBackward,  "Backwards"   },
                { Enums.KeybindAction.ToggleRun,     "ToggleRun"   },
                { Enums.KeybindAction.StrafeLeft,    "StrafeLeft"  },
                { Enums.KeybindAction.StrafeRight,   "StrafeRight" },
                { Enums.KeybindAction.DiagLeft,      "DiagLeft"    },
                { Enums.KeybindAction.DiagRight,     "DiagRight"   },
            };

        private Enums.KeybindAction lalaAllAction = Enums.KeybindAction.EquipOutfit1;
        public Enums.KeybindAction LalaAllAction
        {
            get { return lalaAllAction; }
            set
            {
                lalaAllAction = value;
                this.cbox_LalaOne_SelectEmote.SelectedValue   = lalaAllAction;
                this.cbox_LalaTwo_SelectEmote.SelectedValue   = lalaAllAction;
                this.cbox_LalaThree_SelectEmote.SelectedValue = lalaAllAction;
            }
        }

        private Enums.KeybindAction lalaOneAction = Enums.KeybindAction.EquipOutfit1;
        public Enums.KeybindAction LalaOneAction
        {
            get { return lalaOneAction; }
            set
            {
                lalaOneAction = value;
                if (this.LalaOne != null)
                {
                    this.LalaOne.PerformAction(lalaOneAction, this.LalaOneKeybindWait);
                }
            }
        }

        private Enums.KeybindAction lalaTwoAction = Enums.KeybindAction.EquipOutfit1;
        public Enums.KeybindAction LalaTwoAction
        {
            get { return lalaTwoAction; }
            set
            {
                lalaTwoAction = value;
                if (this.LalaTwo != null)
                {
                    this.LalaTwo.PerformAction(lalaTwoAction, this.LalaTwoKeybindWait);
                }
            }
        }

        private Enums.KeybindAction lalaThreeAction = Enums.KeybindAction.EquipOutfit1;
        public Enums.KeybindAction LalaThreeAction
        {
            get { return lalaThreeAction; }
            set
            {
                lalaThreeAction = value;
                if (this.LalaThree != null)
                {
                    this.LalaThree.PerformAction(lalaThreeAction, this.LalaThreeKeybindWait);
                }
            }
        }

        private void button_ResetAll_Click(object sender, RoutedEventArgs e)
        {
            this.cbox_LalaAll_SelectEmote.SelectedIndex = -1;
            this.cbox_LalaAll_SelectEmote.SelectedIndex =  0;
        }
    }
}
