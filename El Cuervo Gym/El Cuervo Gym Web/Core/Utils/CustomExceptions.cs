namespace El_Cuervo_Gym_Web.Core.Utils
{
    public class SocioYaExisteException : Exception
    {
        public SocioYaExisteException()
        {
        }

        public SocioYaExisteException(string message)
            : base(message)
        {
        }

        public SocioYaExisteException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
