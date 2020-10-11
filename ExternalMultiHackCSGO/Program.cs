using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xNet;

namespace ExternalMultiHackCSGO
{
    class Program
    {

        public struct vector
        {
            public float x, y, z;
        }

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

        private static Memory mem;
        private static int client_dll;
        private static HttpRequest server = new HttpRequest();
        public static bool trigger = true;
        public static bool bhop = true;
        public static bool wh = true;

        static void Main(string[] args)
        {
            GetModules();

            try
            {
                while (true)
                {
                    if (trigger == true)
                    {
                        mem = new Memory("csgo");
                        int LocalPlayer = mem.Read<int>(client_dll + Offsets.dwlocalplayer);

                        int PlayerTeam = mem.Read<int>(LocalPlayer + Offsets.m_iTeamNum);

                        int ChrosshairID = mem.Read<int>(LocalPlayer + Offsets.m_iCrosshairId);

                        if (ChrosshairID > 0 && ChrosshairID <= 32)
                        {
                            int Target = mem.Read<int>(client_dll + Offsets.dwEntityList + (ChrosshairID - 1) * 0x10);
                            int TargetTeam = mem.Read<int>(Target + Offsets.m_iTeamNum);

                            if (TargetTeam != PlayerTeam)
                            {
                                mouse_event(0x2, 0, 0, 0, UIntPtr.Zero);
                                Thread.Sleep(1);
                                mouse_event(0x4, 0, 0, 0, UIntPtr.Zero);
                            }
                        }
                    }
                    if (bhop == true)
                    {
                        if (GetAsyncKeyState(32) != 0)
                        {
                            int LocalPlayer = mem.Read<int>(client_dll + Offsets.dwlocalplayer);
                            vector vel = mem.Read<vector>(LocalPlayer + Offsets.m_vecvelocity);
                            int Mag = (int)(vel.x + vel.z + vel.y);

                            if (Mag != 0)
                            {
                                mem.Write(client_dll + Offsets.dwforcejump, Flags() ? 4 : 5);
                            }
                            else
                            {
                            }
                        }
                    }
                    if (wh == true)
                    {
                        int LocalPlayer = mem.Read<int>(client_dll + Offsets.dwlocalplayer);
                        int PlayerTeam = mem.Read<int>(LocalPlayer + Offsets.m_iTeamNum);

                        for (int i = 0; i < 32; i++)
                        {
                            int EntityList = mem.Read<int>(client_dll + Offsets.dwEntityList + i * 0x10);
                            int EntityTeam = mem.Read<int>(EntityList + Offsets.m_iTeamNum);

                            if (EntityTeam != 0 && EntityTeam != PlayerTeam)
                            {
                                int GlowIndex = mem.Read<int>(EntityList + Offsets.m_iGlowIndex);

                                DrawEntity(GlowIndex, 0, 0, 255);
                            }
                            else if (EntityTeam != 0 && EntityTeam == PlayerTeam)
                            {
                                int GlowIndex = mem.Read<int>(EntityList + Offsets.m_iGlowIndex);

                                DrawEntity(GlowIndex, 255, 0, 0);
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("██████ ██████ ██████ ██████ ██████");
                Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                Console.WriteLine("██████ ████   ████   ██  ██ ████  ");
                Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                Console.WriteLine("██████ ██  ██ ██  ██ ██████ ██  ██");
                Console.ReadLine();
            }
        }
        static void DrawEntity(int ClowIndex, int red, int green, int blue)
        {
            int GlowObhect = mem.Read<int>(client_dll + Offsets.dwGlowObjectManager);
            mem.Write(GlowObhect + (ClowIndex * 0x38) + 4, red / 100f);
            mem.Write(GlowObhect + (ClowIndex * 0x38) + 8, green / 100f);
            mem.Write(GlowObhect + (ClowIndex * 0x38) + 12, blue / 100f);
            mem.Write(GlowObhect + (ClowIndex * 0x38) + 0x10, 255 / 100f);
            mem.Write(GlowObhect + (ClowIndex * 0x38) + 0x24, true);
            mem.Write(GlowObhect + (ClowIndex * 0x38) + 0x25, false);
        }

        private static void GetModules()
        {
            try
            {
                Process csgo = Process.GetProcessesByName("csgo")[0];
                mem = new Memory("csgo");
                foreach (ProcessModule module in csgo.Modules)
                {
                    if (module.ModuleName == "client.dll")
                        client_dll = (int)module.BaseAddress;
                }
                while (true)
                {
                    Console.Title = "External-Multu-Hack-CSGO By SoupCS";
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("External-Multu-Hack-CSGO By SoupCS");
                    Console.WriteLine("GitHub: https://github.com/SoupCS/External-Multi-Hack-CS-GO");
                    Console.WriteLine("Futures: WallHack, Bhop, TriggerBot");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("External-Multu-Hack-CSGO By SoupCS");
                    Console.WriteLine("GitHub: https://github.com/SoupCS/External-Multi-Hack-CS-GO");
                    Console.WriteLine("Futures: WallHack, Bhop, TriggerBot");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("External-Multu-Hack-CSGO By SoupCS");
                    Console.WriteLine("GitHub: https://github.com/SoupCS/External-Multi-Hack-CS-GO");
                    Console.WriteLine("Futures: WallHack, Bhop, TriggerBot");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("External-Multu-Hack-CSGO By SoupCS");
                    Console.WriteLine("GitHub: https://github.com/SoupCS/External-Multi-Hack-CS-GO");
                    Console.WriteLine("Futures: WallHack, Bhop, TriggerBot");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("External-Multu-Hack-CSGO By SoupCS");
                    Console.WriteLine("GitHub: https://github.com/SoupCS/External-Multi-Hack-CS-GO");
                    Console.WriteLine("Futures: WallHack, Bhop, TriggerBot");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("External-Multu-Hack-CSGO By SoupCS");
                    Console.WriteLine("GitHub: https://github.com/SoupCS/External-Multi-Hack-CS-GO");
                    Console.WriteLine("Futures: WallHack, Bhop, TriggerBot");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("External-Multu-Hack-CSGO By SoupCS");
                    Console.WriteLine("GitHub: https://github.com/SoupCS/External-Multi-Hack-CS-GO");
                    Console.WriteLine("Futures: WallHack, Bhop, TriggerBot");
                    Thread.Sleep(25);
                    Console.Clear();
                }
            }
            catch
            {
                while (true)
                {
                    Console.Title = "External-Multu-Hack-CSGO By SoupCS";
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("██████ ██████ ██████ ██████ ██████");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ████   ████   ██  ██ ████  ");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ██  ██ ██  ██ ██████ ██  ██");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("██████ ██████ ██████ ██████ ██████");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ████   ████   ██  ██ ████  ");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ██  ██ ██  ██ ██████ ██  ██");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("██████ ██████ ██████ ██████ ██████");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ████   ████   ██  ██ ████  ");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ██  ██ ██  ██ ██████ ██  ██");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("██████ ██████ ██████ ██████ ██████");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ████   ████   ██  ██ ████  ");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ██  ██ ██  ██ ██████ ██  ██");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("██████ ██████ ██████ ██████ ██████");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ████   ████   ██  ██ ████  ");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ██  ██ ██  ██ ██████ ██  ██");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("██████ ██████ ██████ ██████ ██████");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ████   ████   ██  ██ ████  ");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ██  ██ ██  ██ ██████ ██  ██");
                    Thread.Sleep(25);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("██████ ██████ ██████ ██████ ██████");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ████   ████   ██  ██ ████  ");
                    Console.WriteLine("██     ██  ██ ██  ██ ██  ██ ██  ██");
                    Console.WriteLine("██████ ██  ██ ██  ██ ██████ ██  ██");
                    Thread.Sleep(25);
                    Console.Clear();
                }
            }
        }
        private static bool Flags()
        {
            int LocalPlayer = mem.Read<int>(client_dll + Offsets.dwlocalplayer);
            int Flags = mem.Read<int>(LocalPlayer + Offsets.m_fflags);

            if (Flags == 256) return true;
            else if (Flags == 262) return true;
            else
            {
                return false;
            }
        }
        class Offsets
        {
            public const Int32 m_fflags = 0x104;
            public const Int32 m_vecvelocity = 0x114;
            public const Int32 dwforcejump = 0x51F9E44;
            public const Int32 dwlocalplayer = 0xD3BC5C;
            public const Int32 m_iTeamNum = 0xF4;
            public const Int32 dwEntityList = 0x4D5022C;
            public const Int32 m_iCrosshairId = 0xB3E4;
            public const Int32 dwForceAttack = 0x3181790;
            public const Int32 dwForceAttack2 = 0x318179C;
            public const Int32 m_iGlowIndex = 0xA438;
            public const Int32 dwGlowObjectManager = 0x5298078;
        }
    }
}
