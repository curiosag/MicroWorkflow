
namespace DataProcessingByLinq
{
    public class DrAtom
    {
        public static string OperationId(string operationName, string fileType, string path)
        {
            return operationName + "_" + fileType + "(§)".Replace("§", path);
        }
    }
}
