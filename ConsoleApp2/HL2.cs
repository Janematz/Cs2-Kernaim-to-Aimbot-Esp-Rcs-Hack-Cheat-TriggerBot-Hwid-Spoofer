using calc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp2
{
    internal class HL2
    {
        #region Variables

        // BASES
        public static IntPtr LocalPlayerBase;
        public static IntPtr EntityBase;
        public static IntPtr ViewAngelsBase;
        public static IntPtr client;
        public static IntPtr engine;

        // VECTORS
        public static Vector3 Enemy = new Vector3();
        public static Vector3 LocalPlayer = new Vector3();
        public static Vector3 AimAngles = new Vector3();
        public static Vector3 DeltaVec = new Vector3();

        // INT
        public static int i;
        public static int EntityHealth;
        public static int EntityMaxHealth;
        public static int PlayerAmount;

        // DOUBLE
        public static double AngleOutX;
        public static double AngleOutY;

        // ANGLES
        public static Angle viewAngles;

        // MATRIX
        public static matrix4x4_t ViewMatrix;

        #endregion

        #region Imports

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(Keys ArrowKeys);

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        #endregion

        #region StartThread
        public static void Main()
        {

            if (Memory.Attatch("hl2")) 
            {
                engine = Memory.GetModuleAddress("engine.dll");
                client = Memory.GetModuleAddress("client.dll");

                while (true)
                {
                    IntPtr PlayerBase = Memory.Read<IntPtr>(client + Offsets.Players);
                    PlayerAmount = Memory.Read<int>(PlayerBase + Offsets.PlayerAmount);

                    for (i = 0; i < PlayerAmount; i++)
                    {
                        ReadValues();
                        Thread.Sleep(10);
                    }
                }
            }
        }
        #endregion

        #region UpdateAimbotTarget
            static void UpdateCurrentTarget()
            {
                #region ViewAngels
                viewAngles.Yaw = Memory.Read<float>(ViewAngelsBase + Offsets.MouseX);
                viewAngles.Pitch = Memory.Read<float>(ViewAngelsBase + Offsets.MouseY);
                #endregion

                #region Update XYZ
                Enemy.X = Memory.Read<float>(EntityBase + Offsets.XCoordinate);
                Enemy.Y = Memory.Read<float>(EntityBase + Offsets.YCoordinate);
                Enemy.Z = Memory.Read<float>(EntityBase + Offsets.ZCoordinate);

                LocalPlayer.X = Memory.Read<float>(LocalPlayerBase + Offsets.XCoordinate);
                LocalPlayer.Y = Memory.Read<float>(LocalPlayerBase + Offsets.YCoordinate);
                LocalPlayer.Z = Memory.Read<float>(LocalPlayerBase + Offsets.ZCoordinate);

                DeltaVec = Enemy - LocalPlayer;
                #endregion

                #region Update CalcAngel & Health
                EntityHealth = Memory.Read<int>(EntityBase + Offsets.Health);
                AngleOutX = (Math.Atan2(DeltaVec.X, DeltaVec.Y) * (180 / Math.PI));
                AngleOutY = -(Math.Atan2(DeltaVec.Z, DeltaVec.Length()) * (180 / Math.PI) - 0.9f);

                viewAngles.Yaw = (float)AngleOutX;
                viewAngles.Pitch = (float)AngleOutY;
                #endregion
            }

        #endregion

            static void ReadValues()
            {
                #region Read Bases

                EntityBase = Memory.Read<IntPtr>(client + Offsets.EntityList + i * 0x10);
                LocalPlayerBase = Memory.Read<IntPtr>(client + Offsets.EntityList);
                ViewAngelsBase = Memory.Read<IntPtr>(engine + Offsets.MouseBase);

                #endregion

                #region Read Values

                EntityHealth = Memory.Read<int>(EntityBase + Offsets.Health);
                EntityMaxHealth = Memory.Read<int>(EntityBase + Offsets.MaxHealth);

                viewAngles.Yaw = Memory.Read<float>(ViewAngelsBase + Offsets.MouseX);
                viewAngles.Pitch = Memory.Read<float>(ViewAngelsBase + Offsets.MouseY);

                Enemy.X = Memory.Read<float>(EntityBase + Offsets.XCoordinate);
                Enemy.Y = Memory.Read<float>(EntityBase + Offsets.YCoordinate);
                Enemy.Z = Memory.Read<float>(EntityBase + Offsets.ZCoordinate);

                LocalPlayer.X = Memory.Read<float>(LocalPlayerBase + Offsets.XCoordinate);
                LocalPlayer.Y = Memory.Read<float>(LocalPlayerBase + Offsets.YCoordinate);
                LocalPlayer.Z = Memory.Read<float>(LocalPlayerBase + Offsets.ZCoordinate);

                #endregion

                #region CalcAngle

                DeltaVec = Enemy - LocalPlayer;

                AngleOutX = (Math.Atan2(DeltaVec.X, DeltaVec.Y) * (180 / Math.PI));
                AngleOutY = -(float)Math.Atan(DeltaVec.Z / DeltaVec.Length()) * (float)(180f / Math.PI);

                viewAngles.Yaw = (float)AngleOutX;
                viewAngles.Pitch = (float)AngleOutY;


            #endregion

                #region Aimbot

                while (Control.ModifierKeys == Keys.Shift)
                {
                     UpdateCurrentTarget();
                     if (LocalPlayer == Enemy) { break; }
                     if (!(((EntityHealth >= 0 && EntityHealth <= 1000)))) { break; }
                     Memory.Write(ViewAngelsBase + Offsets.MouseX, viewAngles.Yaw);
                     Memory.Write(ViewAngelsBase + Offsets.MouseY, viewAngles.Pitch);
                     Thread.Sleep(10);
                }

                #endregion
            }


             unsafe bool WorldToScreen(float* viewMatrix, int height, int width, ref Vector3 pos)
        {
                float screenX = viewMatrix[0] * pos.X + viewMatrix[1] * pos.Y + viewMatrix[2] * pos.Z + viewMatrix[3];
                float screenY = viewMatrix[4] * pos.X + viewMatrix[5] * pos.Y + viewMatrix[6] * pos.Z + viewMatrix[7];
                float screenW = viewMatrix[12] * pos.X + viewMatrix[13] * pos.Y + viewMatrix[14] * pos.Z + viewMatrix[15];

                if (!(screenW > 0)) return false; //Basicly behind us

                pos.X = (1 + screenX / screenW) * width / 2 + 0.5f;
                pos.Y = (1 - screenY / screenW) * height / 2 + 0.5f;

                if (pos.X < 0 || pos.X > width || pos.Y < 0 || pos.Y > height) return false;

                return true;
        }
    }
}