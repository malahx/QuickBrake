/* 
QuickBrake
Copyright 2016 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System;
using UnityEngine;

namespace QuickBrake {

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public partial class QuickBrake : MonoBehaviour {

		public static QuickBrake Instance {
			get;
			private set;
		}
	
		public bool BrakeIsLocked {
			get {
				return InputLockManager.IsLocked (ControlTypes.GROUP_BRAKES);
			}
		}

		public bool BrakeLandedRover(Vessel vessel) {
			return QSettings.Instance.AlwaysBrakeLandedRover && vessel.situation == Vessel.Situations.LANDED && vessel.vesselType == VesselType.Rover;
		}

		public bool BrakeLandedBase(Vessel vessel) {
			return QSettings.Instance.AlwaysBrakeLandedBase && vessel.situation == Vessel.Situations.LANDED && vessel.vesselType == VesselType.Base;
		}

		public bool BrakeLandedLander(Vessel vessel) {
			return QSettings.Instance.AlwaysBrakeLandedLander && vessel.situation == Vessel.Situations.LANDED && vessel.vesselType == VesselType.Lander;
		}

		public bool BrakeLandedVessel(Vessel vessel) {
			return QSettings.Instance.AlwaysBrakeLandedVessel && vessel.situation == Vessel.Situations.LANDED;
		}

		public bool BrakeAtLaunchPad(Vessel vessel) {
			return QSettings.Instance.EnableBrakeAtLaunchPad && vessel.situation == Vessel.Situations.PRELAUNCH && vessel.landedAt == "LaunchPad";
		}

		public bool BrakeAtRunway(Vessel vessel) {
			return QSettings.Instance.EnableBrakeAtRunway && vessel.situation == Vessel.Situations.PRELAUNCH && vessel.landedAt == "Runway";
		}

		private void Awake() {
			if (!HighLogic.LoadedSceneIsGame || Instance != null) {
				Destroy (this);
			}
			Instance = this;
			GameEvents.OnFlightGlobalsReady.Add (OnFlightGlobalsReady);
			Warning ("Awake", true);
		}
			
		private void Start() {
			QSettings.Instance.Load ();
			Warning ("Start", true);
		}

		private void OnFlightGlobalsReady(bool ready) {
			Vessel _vessel = FlightGlobals.ActiveVessel;
			if (!ready || _vessel == null) {
				return;
			}
			if (BrakeLandedVessel(_vessel) || BrakeLandedRover(_vessel) || BrakeLandedBase(_vessel) || BrakeLandedLander(_vessel) || BrakeAtLaunchPad(_vessel) || BrakeAtRunway(_vessel)) {
				_vessel.ActionGroups.SetGroup (KSPActionGroup.Brakes, true);
				Log ("Brake");
			}
		}

		private void OnDestroy() {
			GameEvents.OnFlightGlobalsReady.Remove (OnFlightGlobalsReady);
			Warning ("OnDestroy", true);
		}
	}
}