namespace MarkovChainModel {
    public interface IReceiver {
        public void Receive(float val);

        public void RegisterAsReceiver();
    }
}