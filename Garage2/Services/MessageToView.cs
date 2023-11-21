namespace Garage2.Services
{
    public class MessageToView : IMessageToView
    {
        public string MessageToUser { get; private set; }

        public void ShowMessageInView(string inputMessage)
        {
            MessageToUser = inputMessage;
        }
    }
}
