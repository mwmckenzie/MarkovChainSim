namespace MarkovChainModel {
    public class AdjFuncNoChange : AdjustmentFunction {
        
        public override float Adjust(float observation) {
            history.Add(observation);
            return observation;
        }
        
    }
}