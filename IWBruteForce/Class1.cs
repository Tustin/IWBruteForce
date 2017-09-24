using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS4MacroAPI;
using System.IO;
using System.Windows.Forms;

namespace IWBruteForce
{
    public class Script : ScriptBase
    {
		public Script()
		{
			Config.Name = "IW Beast BruteForce";
			Config.LoopDelay = 800;
		}

		List<List<DualShockState>> states = new List<List<DualShockState>>();
		List<DualShockState> currentState = new List<DualShockState>();

		// Called when the user pressed play
		public override void Start()
		{
			base.Start();

			var sequences = File.ReadAllLines("combos.txt");
			foreach (var line in sequences)
			{
				List<DualShockState> newStateSequence = new List<DualShockState>();

				foreach (char button in line)
				{
					DualShockState newAction = null; //Wont ever add null due to the default case
					switch (button)
					{
						case 'U':
						newAction = new DualShockState() { DPad_Up = true };
						break;
						case 'D':
						newAction = new DualShockState() { DPad_Down = true };
						break;
						case 'L':
						newAction = new DualShockState() { DPad_Left = true };
						break;
						case 'R':
						newAction = new DualShockState() { DPad_Right = true };
						break;
						default:
						throw new Exception("Invalid button");
					}
					newStateSequence.Add(newAction);
				}

				states.Add(newStateSequence);
			}
			MessageBox.Show($"Loaded {states.Count} actions");
			currentState = states[0];
		}

		void PlayNextCombo()
		{
			foreach (var button in currentState)
			{
				Press(button);
			}

			var id = states.IndexOf(currentState);
			if (id == states.Count - 1)
			{
				MessageBox.Show("done");
				return;
			}
			currentState = states[++id];
		}

		// Called every interval set by LoopDelay
		public override void Update()
		{
			PlayNextCombo();
			Sleep(1000);
		}
	}
}
