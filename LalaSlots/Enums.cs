namespace LalaSlots
{
    using System;
    using System.Windows.Forms;

    public class Enums
    {
        public enum KeybindAction
        {
            // expects shift
            EquipOutfit1,
            EquipOutfit2,
            EquipOutfit3,
            EquipOutfit4,
            EmoteCheer,
            EmoteSurp,
            EmoteThumbs,
            EmoteNo,
            EmoteFume,
            EmoteFPalm,
            EmoteFlex,
            EmoteAirQ,

            // expects alt
            EmoteDance,
            EmoteSit,
            EmotePray,
            EmoteBeckon,
            EmoteThink,
            EmoteDoze,
            EmoteBow,
            EmotePose,
            EmoteWelcome,
            MacroSayRed,
            MacroSayGreen,
            MacroSayBlue,

            // expects ctrl+shift
            EmoteStepDnc,
            EmoteHarvDnc,
            EmoteBallDnc,
            EmoteBlowKiss,
            EmoteCongrats,
            EmoteWave,
            EmoteLookout,
            EmoteHappy,
            EmotePanic,

            // experimental walking actions
            WalkForward,
            WalkBackward,
            ToggleRun,
            StrafeLeft,
            StrafeRight,

            // special cases
            DiagLeft,
            DiagRight,
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
                case KeybindAction.EmoteBow:
                    return Keys.D7 | Keys.Alt;
                case KeybindAction.EmotePose:
                    return Keys.D8 | Keys.Alt;
                case KeybindAction.EmoteWelcome:
                    return Keys.D9 | Keys.Alt;
                case KeybindAction.MacroSayRed:
                    return Keys.D0 | Keys.Alt;
                case KeybindAction.MacroSayGreen:
                    return Keys.OemMinus | Keys.Alt;
                case KeybindAction.MacroSayBlue:
                    return Keys.Oemplus | Keys.Alt;

                case KeybindAction.EmoteStepDnc:
                    return Keys.D1 | Keys.Control | Keys.Shift;
                case KeybindAction.EmoteHarvDnc:
                    return Keys.D2 | Keys.Control | Keys.Shift;
                case KeybindAction.EmoteBallDnc:
                    return Keys.D3 | Keys.Control | Keys.Shift;
                case KeybindAction.EmoteBlowKiss:
                    return Keys.D4 | Keys.Control | Keys.Shift;
                case KeybindAction.EmoteCongrats:
                    return Keys.D5 | Keys.Control | Keys.Shift;
                case KeybindAction.EmoteWave:
                    return Keys.D6 | Keys.Control | Keys.Shift;
                case KeybindAction.EmoteLookout:
                    return Keys.D7 | Keys.Control | Keys.Shift;
                case KeybindAction.EmoteHappy:
                    return Keys.D8 | Keys.Control | Keys.Shift;
                case KeybindAction.EmotePanic:
                    return Keys.D9 | Keys.Control | Keys.Shift;

                case KeybindAction.WalkForward:
                    return Keys.W;
                case KeybindAction.WalkBackward:
                    return Keys.S;
                case KeybindAction.StrafeLeft:
                    return Keys.Q;
                case KeybindAction.StrafeRight:
                    return Keys.E;

                case KeybindAction.ToggleRun:
                    return Keys.Oemtilde;

                // these two are handled differently, since they are combinations
                case KeybindAction.DiagLeft:
                    return Keys.None;
                case KeybindAction.DiagRight:
                    return Keys.None;

                default:
                    throw new Exception("Internal error - missing case");
            }
        }
    }
}
