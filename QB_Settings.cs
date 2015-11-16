/* 
QuickBrake
Copyright 2015 Malah

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
using System.IO;
using UnityEngine;

namespace QuickBrake {
	public class QSettings : MonoBehaviour {

		public readonly static QSettings Instance = new QSettings();

		internal static string FileConfig = KSPUtil.ApplicationRootPath + "GameData/" + QuickBrake.MOD + "/Config.txt";

		[Persistent] internal bool EnableBrakeAtLaunchPad = false;
		[Persistent] internal bool EnableBrakeAtRunway = true;
		[Persistent] internal bool AlwaysBrakeLandedRover = true;
		[Persistent] internal bool AlwaysBrakeLandedBase = false;
		[Persistent] internal bool AlwaysBrakeLandedLander = false;
		[Persistent] internal bool AlwaysBrakeLandedVessel = false;

		#if GUI
		[Persistent] internal bool StockToolBar = true;
		[Persistent] internal bool BlizzyToolBar = true;
		#endif
		public void Save() {
			ConfigNode _temp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
			_temp.Save(FileConfig);
			QuickBrake.Log ("Settings Saved");
		}
		public void Load() {
			if (File.Exists (FileConfig)) {
				try {
					ConfigNode _temp = ConfigNode.Load (FileConfig);
					ConfigNode.LoadObjectFromConfig (this, _temp);
				} catch {
					Save ();
				}
				QuickBrake.Log ("Settings Loaded");
			} else {
				Save ();
			}
		}
	}
}