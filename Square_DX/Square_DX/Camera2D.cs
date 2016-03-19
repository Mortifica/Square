using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Square_DX
{
    public class Camera2D
    {
        #region Protected Variables
        protected float _zoom;
        #endregion
        #region Public Variables
        public float Rotation { get; set; }
        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
                //stops negitive zoom, which will flip the image
                if(_zoom < 0.1f)
                {
                    _zoom = 0.1f;
                }
            }
        }


        public Matrix TransformationMatrix { get; set; }
        public Vector2 Location { get; set; }
        #endregion
        public Camera2D()
        {
            //IOC instead of constructor, but this will work for now
            _zoom = 1.0f;
            Rotation = 0.0f;
            Location = Vector2.Zero;
        }
        public void MoveCamera(Vector2 amount)
        {
            Location += amount;
        }
        public Matrix GetMatrixTransformation(GraphicsDevice graphicsDevice)
        {
            var ViewportWidth = graphicsDevice.Viewport.Width;
            var ViewportHeight = graphicsDevice.Viewport.Height;

            TransformationMatrix = Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0)) *
                                                            Matrix.CreateRotationZ(Rotation) *
                                                            Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                                            Matrix.CreateTranslation(new Vector3(ViewportWidth * 0.5f, ViewportHeight * 0.5f, 0));
            return TransformationMatrix;
        }
    }
}