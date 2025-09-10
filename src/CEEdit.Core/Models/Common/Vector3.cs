namespace CEEdit.Core.Models.Common
{
    /// <summary>
    /// 3D向量
    /// </summary>
    public class Vector3
    {
        public float X { get; set; } = 0.0f;
        public float Y { get; set; } = 0.0f;
        public float Z { get; set; } = 0.0f;

        public Vector3() { }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 Zero => new Vector3(0, 0, 0);
        public static Vector3 One => new Vector3(1, 1, 1);
        
        public override string ToString()
        {
            return $"Vector3({X}, {Y}, {Z})";
        }
    }
}
