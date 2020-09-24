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
        }

        public static (Keys, Keys) GetKeyFromKeybindAction(KeybindAction action)
        {
            switch (action)
            {
                case KeybindAction.EquipOutfit1:
                    return (Keys.D1, Keys.ShiftKey);
                case KeybindAction.EquipOutfit2:
                    return (Keys.D2, Keys.ShiftKey);
                case KeybindAction.EquipOutfit3:
                    return (Keys.D3, Keys.ShiftKey);
                case KeybindAction.EquipOutfit4:
                    return (Keys.D4, Keys.ShiftKey);
                case KeybindAction.EmoteCheer:
                    return (Keys.D5, Keys.ShiftKey);
                case KeybindAction.EmoteSurp:
                    return (Keys.D6, Keys.ShiftKey);
                case KeybindAction.EmoteThumbs:
                    return (Keys.D7, Keys.ShiftKey);
                case KeybindAction.EmoteNo:
                    return (Keys.D8, Keys.ShiftKey);
                case KeybindAction.EmoteFume:
                    return (Keys.D9, Keys.ShiftKey);
                case KeybindAction.EmoteFPalm:
                    return (Keys.D0, Keys.ShiftKey);
                case KeybindAction.EmoteFlex:
                    return (Keys.OemMinus, Keys.ShiftKey);
                case KeybindAction.EmoteAirQ:
                    return (Keys.Oemplus, Keys.ShiftKey);
                case KeybindAction.EmoteDance:
                    return (Keys.D1, Keys.Menu);
                case KeybindAction.EmoteSit:
                    return (Keys.D2, Keys.Menu);
                case KeybindAction.EmotePray:
                    return (Keys.D3, Keys.Menu);
                case KeybindAction.EmoteBeckon:
                    return (Keys.D4, Keys.Menu);
                case KeybindAction.EmoteThink:
                    return (Keys.D5, Keys.Menu);
                case KeybindAction.EmoteDoze:
                    return (Keys.D6, Keys.Menu);

                case KeybindAction.MacroSayRed:
                    return (Keys.D0, Keys.Menu);
                case KeybindAction.MacroSayGreen:
                    return (Keys.OemMinus, Keys.Menu);
                case KeybindAction.MacroSayBlue:
                    return (Keys.Oemplus, Keys.Menu);
            }

            return (Keys.None, Keys.None);
        }
    }
}
