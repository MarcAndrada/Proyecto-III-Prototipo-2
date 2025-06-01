/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID BALANCELOSE = 4020120096U;
        static const AkUniqueID BALANCEMOVE = 2412137972U;
        static const AkUniqueID BALANCEWIN = 155621611U;
        static const AkUniqueID CIGARETTE = 3307530599U;
        static const AkUniqueID COINFALL = 2655758413U;
        static const AkUniqueID CROWD = 2848349012U;
        static const AkUniqueID DUMMYCRUSH = 3518705528U;
        static const AkUniqueID DUMMYDOWN = 213344387U;
        static const AkUniqueID DUMMYFLUIDS = 1059185820U;
        static const AkUniqueID DUMMYUP = 802614564U;
        static const AkUniqueID FLIPREDCOIN = 762051322U;
        static const AkUniqueID HOVERBUTTON = 2710047957U;
        static const AkUniqueID INTERRUPTORPRESS = 3446469450U;
        static const AkUniqueID JOKERLAUGH = 2607658113U;
        static const AkUniqueID LEVERPULL = 2563778470U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID PRESSBUTTON = 2561485778U;
        static const AkUniqueID SAW = 443572616U;
        static const AkUniqueID SCREEN = 804877345U;
        static const AkUniqueID SHOPBUY = 1153606107U;
        static const AkUniqueID SHOPINVENTORYFULL = 1063113708U;
        static const AkUniqueID SHOPNOMONEY = 66472324U;
        static const AkUniqueID SLOTANIMATION = 1005173321U;
        static const AkUniqueID SLOTICON = 4028009350U;
        static const AkUniqueID TURNCOINFLIP = 408639982U;
        static const AkUniqueID TURNCOINRESULT = 1628677272U;
        static const AkUniqueID VENTILATION = 3172605362U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace SCREEN
        {
            static const AkUniqueID GROUP = 804877345U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID SCREENOFF = 1092031120U;
                static const AkUniqueID SCREENON = 99209978U;
            } // namespace STATE
        } // namespace SCREEN

        namespace TURN
        {
            static const AkUniqueID GROUP = 3137665780U;

            namespace STATE
            {
                static const AkUniqueID ENEMY = 2299321487U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PLAYER = 1069431850U;
            } // namespace STATE
        } // namespace TURN

    } // namespace STATES

    namespace SWITCHES
    {
        namespace SLOTICON
        {
            static const AkUniqueID GROUP = 4028009350U;

            namespace SWITCH
            {
                static const AkUniqueID JOKER = 856429646U;
                static const AkUniqueID ROTATE = 1302771492U;
                static const AkUniqueID X1 = 1500973332U;
                static const AkUniqueID X2 = 1500973335U;
                static const AkUniqueID X3 = 1500973334U;
            } // namespace SWITCH
        } // namespace SLOTICON

        namespace TURNCOINFLIP
        {
            static const AkUniqueID GROUP = 408639982U;

            namespace SWITCH
            {
                static const AkUniqueID ENEMY = 2299321487U;
                static const AkUniqueID PLAYER = 1069431850U;
                static const AkUniqueID REDCOIN = 3334102693U;
            } // namespace SWITCH
        } // namespace TURNCOINFLIP

    } // namespace SWITCHES

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
