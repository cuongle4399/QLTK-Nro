public interface ISession
{
	bool isConnected();

	void setHandler(IMessageHandler messageHandler);

	void connect(string host, int port);

	void sendMessage(Message message);

	void close();
}
