using BiofuelSouth.Models;

namespace BiofuelSouth.ViewModels
{
    public class SimulatorViewModel
    {

        public General General { get; set; }

        public SimulatorViewModel()
        {
            General = new General();
        }
    }

    public class SimulatorInput
    {
        public Input Input { get; set; } 
        
        public override string ToString()
        {
            return "Simulator Input";
        }
    }

    public class SimulatorOutput
    {
        public override string ToString()
        {
            return "Simulator output";
        }

    }
}