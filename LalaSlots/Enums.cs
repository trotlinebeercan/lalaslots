namespace LalaSlots
{
    using System.Windows.Forms;

    public class Enums
    {
        public enum KeybindAction
        {
            // expects shift
            EquipOutfit1  = 1,
            EquipOutfit2  = 2,
            EquipOutfit3  = 3,
            EquipOutfit4  = 4,
            EmoteCheer    = 5,
            EmoteSurp     = 6,
            EmoteThumbs   = 7,
            EmoteNo       = 8,
            EmoteFume     = 9,
            EmoteFPalm    = 10,
            EmoteFlex     = 11,
            EmoteAirQ     = 12,

            // expects alt
            EmoteDance    = 13,
            EmoteSit      = 14,
            EmotePray     = 15,
            EmoteBeckon   = 16,
            EmoteThink    = 17,
            EmoteDoze     = 18,

            MacroSayRed   = 22,
            MacroSayGreen = 23,
            MacroSayBlue  = 24,

            // experimental walking actions
            WalkForward   = 25,
        }

        public static Keys GetKeyFromKeybindAction(KeybindAction action)
        {
            switch (action)
            {
                case KeybindAction.EquipOutfit1:
                    return Keys.D1 | Keys.Shift;
                case KeybindAction.EquipOutfit2:
                    return Keys.D2 | Keys.Shift;
                case KeybindAction.EquipOutfit3:
                    return Keys.D3 | Keys.Shift;
                case KeybindAction.EquipOutfit4:
                    return Keys.D4 | Keys.Shift;
                case KeybindAction.EmoteCheer:
                    return Keys.D5 | Keys.Shift;
                case KeybindAction.EmoteSurp:
                    return Keys.D6 | Keys.Shift;
                case KeybindAction.EmoteThumbs:
                    return Keys.D7 | Keys.Shift;
                case KeybindAction.EmoteNo:
                    return Keys.D8 | Keys.Shift;
                case KeybindAction.EmoteFume:
                    return Keys.D9 | Keys.Shift;
                case KeybindAction.EmoteFPalm:
                    return Keys.D0 | Keys.Shift;
                case KeybindAction.EmoteFlex:
                    return Keys.OemMinus | Keys.Shift;
                case KeybindAction.EmoteAirQ:
                    return Keys.Oemplus | Keys.Shift;
                case KeybindAction.EmoteDance:
                    return Keys.D1 | Keys.Alt;
                case KeybindAction.EmoteSit:
                    return Keys.D2 | Keys.Alt;
                case KeybindAction.EmotePray:
                    return Keys.D3 | Keys.Alt;
                case KeybindAction.EmoteBeckon:
                    return Keys.D4 | Keys.Alt;
                case KeybindAction.EmoteThink:
                    return Keys.D5 | Keys.Alt;
                case KeybindAction.EmoteDoze:
                    return Keys.D6 | Keys.Alt;

                case KeybindAction.MacroSayRed:
                    return Keys.D0 | Keys.Alt;
                case KeybindAction.MacroSayGreen:
                    return Keys.OemMinus | Keys.Alt;
                case KeybindAction.MacroSayBlue:
                    return Keys.Oemplus | Keys.Alt;

                case KeybindAction.WalkForward:
                    return Keys.W;
            }

            return Keys.None;
        }
    }
}
