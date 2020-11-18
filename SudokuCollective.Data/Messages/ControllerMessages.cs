namespace SudokuCollective.Data.Messages
{
    public static class ControllerMessages
    {
        public static string InvalidLicenseRequestMessage = "Status Code 400: Invalid Request On This License";
        public static string NotOwnerMessage = "Status Code 400: You Are Not The Owner Of This App";
        public static string IdIncorrectMessage = "Status Code 400: Id Incorrect";

        public static string StatusCode200(string serviceMessage) 
        {
            string result = string.Format("Status Code 200: {0}", serviceMessage);

            return result;
        }

        public static string StatusCode201(string serviceMessage)
        {
            string result = string.Format("Status Code 201: {0}", serviceMessage);

            return result;
        }

        public static string StatusCode400(string serviceMessage)
        {
            string result = string.Format("Status Code 400: {0}", serviceMessage);

            return result;
        }

        public static string StatusCode404(string serviceMessage)
        {
            string result = string.Format("Status Code 404: {0}", serviceMessage);

            return result;
        }
    }
}
