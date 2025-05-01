using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Offsets
    {


        // BASES

        public static int LocalPlayer = 0x691D94;

        public static int EntityList = 0x6ADF2C;

        public static int dwViewMatrix = 0xE9AC0;

        public static int MouseBase = 0x93E74;

        public static int Players = 0x131C38;

        public static int LocationBase = 0x131C38;


        // STUFF

        public static int Health = 0x90;

        public static int ViewMatrixOffset = 0x4;

        public static int MaxHealth = 0x94;

        public static int PlayerAmount = 0x00;


        // XYZ

        public static int XCoordinate = 0x27c;

        public static int YCoordinate = 0x278;

        public static int ZCoordinate = 0x280;

        public static int MouseX = 0x04;

        public static int MouseY = 0x00;
    }
}
