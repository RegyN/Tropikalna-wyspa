using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tropikalna_wyspa
{
    class Camera3D
    {
        public Vector3 Position { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 Forward { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }

        /// <summary>
        /// Construct a view matrix corresponding to this camera.
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                return Matrix.CreateLookAt(Position, Forward + Position, Up);
            }
        }
        
        public Camera3D(Vector3 position, Vector3 forward, Vector3 up, Matrix projectionMatrix)
        {
            Position = position;
            Forward = forward;
            Up = up;
            ProjectionMatrix = projectionMatrix;
        }

        //public void Orbit(Vector3 target, float speed)
        //{
        //    Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(speed));
        //    Position = Vector3.Transform(Position, rotationMatrix);
        //}

        public void Thrust(float amount)
        {
            Forward.Normalize();
            Position += Forward * amount;
        }


        /// <summary>
        /// Ruch w poziomie, w lewo.
        /// </summary>
        public void StrafeHorz(float amount)
        {
            var left = Vector3.Cross(Up, Forward);
            left.Normalize();
            Position += left * amount;
        }
        
        /// <summary>
        /// Ruch w pionie, do góry.
        /// </summary>
        public void StrafeVert(float amount)
        {
            Up.Normalize();
            Position += Up * amount;
        }

        /// <summary> 
        /// Obrót zgodnie z ruchem wskazówek zegara wokół osi skierowanej naprzód. 
        /// </summary>
        public void Roll(float amount)
        {
            Up.Normalize();
            var left = Vector3.Cross(Up, Forward);
            left.Normalize();

            Up = Vector3.Transform(Up, Matrix.CreateFromAxisAngle(Forward, MathHelper.ToRadians(amount)));
        }
        
        /// <summary> Obrót w lewo wokół osi skierowanej w górę.</summary>
        public void Yaw(float amount)
        {
            Forward.Normalize();

            Forward = Vector3.Transform(Forward, Matrix.CreateFromAxisAngle(Up, MathHelper.ToRadians(amount)));
        }

        /// <summary> Obrót w górę wokół osi skierowanej w lewo.</summary>
        public void Pitch(float amount)
        {
            Forward.Normalize();
            var left = Vector3.Cross(Up, Forward);
            left.Normalize();

            Forward = Vector3.Transform(Forward, Matrix.CreateFromAxisAngle(left, MathHelper.ToRadians(amount)));
            Up = Vector3.Transform(Up, Matrix.CreateFromAxisAngle(left, MathHelper.ToRadians(amount)));
        }
    }
}
