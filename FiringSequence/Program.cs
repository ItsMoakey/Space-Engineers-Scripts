﻿using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {

        string[] groupNames = new string[] {
            "Group1", "Group2", "Group3", "Group4", "Group5","Group6", "Group7", "Group8", "Group9"
            //Add, Rename Or Remove Groups Here
        };

        // Delay
        const double intervalSeconds = 0.5;

        //Looping
        bool Loop = false;

        // Current time tracking !!!Dont Change!!!
        double elapsedTime = 0;
        int currentGroupIndex = 0;

        public Program()
        {
            if (Loop == false)
            {
                Runtime.UpdateFrequency = UpdateFrequency.None;
            } else
            {
                Runtime.UpdateFrequency = UpdateFrequency.Update10;
            }
                        
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (argument.Equals("Fire", StringComparison.OrdinalIgnoreCase) && Loop == false)
            {
                // Start firing sequence
                Runtime.UpdateFrequency = UpdateFrequency.Update10;
                Echo("Firing sequence started.");
                return;
            }

            if (updateSource == UpdateType.Update10)
            {
                // Add elapsed time
                elapsedTime += Runtime.TimeSinceLastRun.TotalSeconds;

                if (elapsedTime > intervalSeconds)
                {
                    elapsedTime = 0;

                    string groupName = groupNames[currentGroupIndex];

                    var group = GridTerminalSystem.GetBlockGroupWithName(groupName);
                    if (group != null)
                    {

                        List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                        group.GetBlocks(blocks);

                        foreach (var block in blocks)
                        {

                            block.ApplyAction("ShootOnce");

                            Echo($"Fired weapons in group: {groupName}");
                        }
                    }
                    else
                    {
                        Echo($"Group '{groupName}' not found.");
                    }

                    currentGroupIndex = (currentGroupIndex + 1) % groupNames.Length;

                    if ((currentGroupIndex == 0) && Loop == false)
                    {
                        Echo("Firing sequence complete. Program halted.");
                        Runtime.UpdateFrequency = UpdateFrequency.None;
                    }

                }
                else
                {
                    // Display debug information
                    Echo($"Waiting... Next group: {groupNames[currentGroupIndex]} in {intervalSeconds - elapsedTime:F2}s");
                }
            }
        }

    }
}
