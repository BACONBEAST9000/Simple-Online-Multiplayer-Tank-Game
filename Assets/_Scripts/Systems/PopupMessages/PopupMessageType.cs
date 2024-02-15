using Fusion;

public class PopupMessageType {
    public ShutdownReason SessionShutdownReason { get; set; }
    public CustomMessageType CustomMessage { get; set; } = CustomMessageType.NONE;

    public PopupMessageType(ShutdownReason shutdownReason) => SessionShutdownReason = shutdownReason;
    public PopupMessageType(CustomMessageType customMessageType) => CustomMessage = customMessageType;

    public override string ToString() {
        return $"ShutdownReason?: {SessionShutdownReason}, CustomMessage?: {CustomMessage}";
    }

    public override bool Equals(object obj) {
        if (!(obj is PopupMessageType))
            return false;

        PopupMessageType other = (PopupMessageType)obj;
        return
            SessionShutdownReason == other.SessionShutdownReason &&
            CustomMessage == other.CustomMessage;
    }

    public override int GetHashCode() {
        unchecked {
            int hash = 17;
            hash = hash * 23 + SessionShutdownReason.GetHashCode();
            hash = hash * 23 + CustomMessage.GetHashCode();
            return hash;
        }
    }
}
