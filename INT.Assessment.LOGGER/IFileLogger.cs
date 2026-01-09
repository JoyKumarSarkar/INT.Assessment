using INT.Assessment.ENTITY;

namespace INT.Assessment.LOGGER
{
    public interface IFileLogger
    {
        void Info (string Message, LogAssembly Assembly);
        void Warning (string Message, LogAssembly Assembly);
        void Error (string Message, LogAssembly Assembly, Exception Ex = null);
    }
}
