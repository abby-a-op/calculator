namespace Calculator
{
    public static class Geometry
    {
        public static Real DegToRad(Real degrees)
        {
            double rad = degrees.Value * Math.PI / 180.0;

            return new Real(rad);
        }

        public static Real RadToDeg(Real rad)
        {
            double deg = rad.Value / Math.PI * 180.0;

            return new Real(deg);
        }
    }
}
