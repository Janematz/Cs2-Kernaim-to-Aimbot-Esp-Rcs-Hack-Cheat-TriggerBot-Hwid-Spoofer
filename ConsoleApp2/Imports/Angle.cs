using System;
using System.Numerics;

namespace calc
{
    public struct Angle
    {
        const double DEGREE2RAD = -Math.PI / 180f;

        public float Pitch { get; set; }
        public float Yaw { get; set; }

        public Angle(float pitch, float yaw)
        {
            Pitch = pitch;
            Yaw = yaw;
        }

        public Angle(double pitch, double yaw)
        {
            Pitch = (float)pitch;
            Yaw = (float)yaw;
        }

        public void Absolute()
        {
            if (Yaw < 0)
            {
                Yaw = -Yaw;
            }
            if (Pitch < 0)
            {
                Pitch = -Pitch;
            }
        }

        public void Normalize()
        {
            if (Yaw < -180)
            {
                Yaw += 360;
            }
            else if (Yaw > 180)
            {
                Yaw -= 360;
            }

            if (Pitch > 89)
            {
                Pitch = 89;
            }
            else if (Pitch < -89)
            {
                Pitch = -89;
            }
        }

        public void Zero()
        {
            Yaw = 0;
            Pitch = 0;
        }

        public Vector3 ToVector3()
        {
            double pitchInRad = Pitch * DEGREE2RAD;
            double yawInRad = Yaw * DEGREE2RAD;
            return new Vector3((float)(Math.Cos(pitchInRad) * Math.Cos(yawInRad)), -(float)(Math.Cos(pitchInRad) * Math.Sin(yawInRad)), (float)Math.Sin(pitchInRad));
        }

        public static Angle operator +(Angle a, Angle b)
            => new Angle(a.Pitch + b.Pitch, a.Yaw + b.Yaw);

        public static Angle operator -(Angle a, Angle b)
            => new Angle(a.Pitch - b.Pitch, a.Yaw - b.Yaw);

        public static Angle operator *(Angle a, float b)
            => new Angle(a.Pitch * b, a.Yaw * b);
    }
}
