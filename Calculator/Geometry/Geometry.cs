namespace Calculator
{
    // Class for the generic commands in the geometry section
    // Cordoned off from rest of code for readability
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
