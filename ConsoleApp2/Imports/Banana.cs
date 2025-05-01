using System.Numerics;
using System.Runtime.InteropServices;

namespace calc
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion
    {
        public float x, y, z, w;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct matrix3x4_t
    {
        public fixed float m[12];
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct matrix4x4_t
    {
        public fixed float m[16];
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct mstudiobone_t
    {
        public int sznameindex;
        public int parent; // parent bone
        public fixed int bonecontroller[6];     // bone controller index, -1 == none
        public Vector3 pos;
        public Quaternion quat;
        public Vector3 rot;
        public Vector3 posscale;
        public Vector3 rotscale;
        public matrix3x4_t poseToBone;
        public Quaternion qAlignment;
        public int flags;
        public int proctype;
        public int procindex;                   // procedural rule
        public int physicsbone;                 // index into physically simulated bone
        public int surfacepropidx;              // index into string tablefor property name
        public int contents;                    // See BSPFlags.h for the contents flags
        public fixed int unused[8];             // remove as appropriat
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct studiohdr_t
    {
        public int id;
        public int version;
        public int checksum;
        public fixed byte name[64];
        public int length;
        public Vector3 eyeposition;
        public Vector3 illumposition;
        public Vector3 hull_min;
        public Vector3 hull_max;
        public Vector3 view_bbmin;
        public Vector3 view_bbmax;
        public int flags;
        public int numbones;                // bones
        public int boneindex;
        public int numbonecontrollers;      // bone controllers
        public int bonecontrollerindex;
        public int numhitboxsets;
        public int hitboxsetindex;
    }
}
