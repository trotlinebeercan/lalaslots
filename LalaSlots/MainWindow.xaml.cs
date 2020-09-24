namespace LalaSlots
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Threading;

    public partial class MainWindow : Window
    {
        private LalaSlot LalaOne;
        private LalaSlot LalaTwo;
        private LalaSlot LalaThree;

        private GameLogic TheGame;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            TheGame = new GameLogic();
            TheGame.OnUpdate += delegate (object o, LalaUpdate update)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { UpdateSystem(update); }));
            };

            FFXIVMemory.InitializeMemory();
        }

        private void UpdateSystem(LalaUpdate update)
        {
            if (update.State == GameState.Initialized)
            {
                this.label_LalaOne_FinalNumber.Content   = update.Data.LalaOneFinalNumber;
                this.label_LalaTwo_FinalNumber.Content   = update.Data.LalaTwoFinalNumber;
                this.label_LalaThree_FinalNumber.Content = update.Data.LalaThreeFinalNumber;
            }

            this.MakeTheLalasDoTheThing(update.Data.LalaOneAction, update.Data.LalaTwoAction, update.Data.LalaThreeAction);
        }

        private void MakeTheLalasDoTheThing(Enums.KeybindAction act1, Enums.KeybindAction act2, Enums.KeybindAction act3)
        {
            if (act1 == act2 && act1 == act3)
            {
                this.cbox_LalaAll_SelectEmote.SelectedValue   = act1;
            }
            else
            {
                this.cbox_LalaOne_SelectEmote.SelectedValue   = act1;
                this.cbox_LalaTwo_SelectEmote.SelectedValue   = act2;
                this.cbox_LalaThree_SelectEmote.SelectedValue = act3;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        // UI Element Event Handlers
        ///////////////////////////////////////////////////////////////////////////////////////////

        private void button_StartTheGame_Click(object sender, RoutedEventArgs e)
        {
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
                { Enums.KeybindAction.EquipOutfit1,  "Equip 1"    },
                { Enums.KeybindAction.EquipOutfit2,  "Equip 2"    },
                { Enums.KeybindAction.EquipOutfit3,  "Equip 3"    },
                { Enums.KeybindAction.EquipOutfit4,  "Equip 4"    },
                { Enums.KeybindAction.EmoteCheer,    "Cheer"      },
                { Enums.KeybindAction.EmoteSurp,     "Surprised"  },
                { Enums.KeybindAction.EmoteThumbs,   "Thumbs Up"  },
                { Enums.KeybindAction.EmoteNo,       "No"         },
                { Enums.KeybindAction.EmoteFume,     "Fume"       },
                { Enums.KeybindAction.EmoteFPalm,    "Facepalm"   },
                { Enums.KeybindAction.EmoteFlex,     "Flex"       },
                { Enums.KeybindAction.EmoteAirQ,     "Air Quotes" },
                { Enums.KeybindAction.EmoteDance,    "Dance"      },
                { Enums.KeybindAction.EmoteSit,      "Sit"        },
                { Enums.KeybindAction.EmotePray,     "Pray"       },
                { Enums.KeybindAction.EmoteBeckon,   "Beckon"     },
                { Enums.KeybindAction.EmoteThink,    "Think"      },
                { Enums.KeybindAction.EmoteDoze,     "Doze"       },
                { Enums.KeybindAction.MacroSayRed,   "Say Red"    },
                { Enums.KeybindAction.MacroSayGreen, "Say Blue"   },
                { Enums.KeybindAction.MacroSayBlue,  "Say Green"  },
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
                    this.LalaOne.PerformAction(lalaOneAction);
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
                    this.LalaTwo.PerformAction(lalaTwoAction);
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
                    this.LalaThree.PerformAction(lalaThreeAction);
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
