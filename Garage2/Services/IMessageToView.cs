namespace Garage2.Services;

public interface IMessageToView
{
	public string MessageToUser { get; }
	void ShowMessageInView(string inputMessage);
}