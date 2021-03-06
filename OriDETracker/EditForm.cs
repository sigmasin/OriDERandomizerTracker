﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OriDE.Memory;

namespace OriDETracker
{
    public partial class EditForm : Form
    {
        protected Tracker parent;
        public EditForm(Tracker p)
        {
            this.parent = p;
            InitializeComponent();
        }

        private bool rando = true;

        public void ChangeMapstone(bool new_val)
        {
            numeric_mapstone.Enabled = new_val;
            button_down.Enabled = new_val;
            button_up.Enabled = new_val;
        }
        public void ChangeShards(bool new_val)
        {
            cb_shard_wv1.Enabled = new_val;
            cb_shard_wv2.Enabled = new_val;

            cb_shard_gs1.Enabled = new_val;
            cb_shard_gs2.Enabled = new_val;

            cb_shard_ss1.Enabled = new_val;
            cb_shard_ss2.Enabled = new_val;
        }
        public void ChangeWarmth(bool new_val)
        {
            cb_event_warmth.Enabled = new_val;
        }
        public void Reset()
        {
            Clear();
            cb_shard_wv1.Enabled = false;
            cb_shard_wv2.Enabled = false;

            cb_shard_gs1.Enabled = false;
            cb_shard_gs2.Enabled = false;

            cb_shard_ss1.Enabled = false;
            cb_shard_ss2.Enabled = false;

            cb_event_warmth.Enabled = false;

        }
        public void Clear()
        {
            cb_skill_sein.Checked = false;
            cb_skill_wj.Checked = false;
            cb_skill_cflame.Checked = false;
            cb_skill_djump.Checked = false;
            cb_skill_bash.Checked = false;
            cb_skill_stomp.Checked = false;
            cb_skill_glide.Checked = false;
            cb_skill_climb.Checked = false;
            cb_skill_cjump.Checked = false;
            cb_skill_grenade.Checked = false;
            cb_skill_dash.Checked = false;

            cb_tree_sein.Checked = false;
            cb_tree_wj.Checked = false;
            cb_tree_cflame.Checked = false;
            cb_tree_djump.Checked = false;
            cb_tree_bash.Checked = false;
            cb_tree_stomp.Checked = false;
            cb_tree_glide.Checked = false;
            cb_tree_climb.Checked = false;
            cb_tree_cjump.Checked = false;
            cb_tree_grenade.Checked = false;
            cb_tree_dash.Checked = false;

            cb_event_cleanwater.Checked = false;
            cb_event_wind.Checked = false;
            cb_event_warmth.Checked = false;
            cb_event_watervein.Checked = false;
            cb_event_gumonseal.Checked = false;
            cb_event_sunstone.Checked = false;

            cb_shard_wv1.Checked = false;
            cb_shard_wv2.Checked = false;

            cb_shard_gs1.Checked = false;
            cb_shard_gs2.Checked = false;
            
            cb_shard_ss1.Checked = false;
            cb_shard_ss2.Checked = false;

            numeric_mapstone.Value = 0;

        }

        public void UpdateSkill(Skill sk, bool b)
        {
            switch (sk)
            {
                case Skill.Sein:
                    cb_skill_sein.Checked = b;
                    break;
                case Skill.WallJump:
                    cb_skill_wj.Checked = b;
                    break;
                case Skill.ChargeFlame:
                    cb_skill_cflame.Checked = b;
                    break;
                case Skill.DoubleJump:
                    cb_skill_djump.Checked = b;
                    break;
                case Skill.Bash:
                    cb_skill_bash.Checked = b;
                    break;
                case Skill.Stomp:
                    cb_skill_stomp.Checked = b;
                    break;
                case Skill.Glide:
                    cb_skill_glide.Checked = b;
                    break;
                case Skill.Climb:
                    cb_skill_climb.Checked = b;
                    break;
                case Skill.ChargeJump:
                    cb_skill_cjump.Checked = b;
                    break;
                case Skill.Grenade:
                    cb_skill_grenade.Checked = b;
                    break;
                case Skill.Dash:
                    cb_skill_dash.Checked = b;
                    break;
                case Skill.None:
                    break;
            }
        }
        public void UpdateTree(Skill sk, bool b)
        {
            switch (sk)
            {
                case Skill.Sein:
                    cb_tree_sein.Checked = b;
                    break;
                case Skill.WallJump:
                    cb_tree_wj.Checked = b;
                    break;
                case Skill.ChargeFlame:
                    cb_tree_cflame.Checked = b;
                    break;
                case Skill.DoubleJump:
                    cb_tree_djump.Checked = b;
                    break;
                case Skill.Bash:
                    cb_tree_bash.Checked = b;
                    break;
                case Skill.Stomp:
                    cb_tree_stomp.Checked = b;
                    break;
                case Skill.Glide:
                    cb_tree_glide.Checked = b;
                    break;
                case Skill.Climb:
                    cb_tree_climb.Checked = b;
                    break;
                case Skill.ChargeJump:
                    cb_tree_cjump.Checked = b;
                    break;
                case Skill.Grenade:
                    cb_tree_grenade.Checked = b;
                    break;
                case Skill.Dash:
                    cb_tree_dash.Checked = b;
                    break;
                case Skill.None:
                    break;
            }
        }
        public void UpdateEvent(String ev, bool b)
        {
            switch(ev)
            {
                case "Water Vein":
                    cb_event_watervein.Checked = b;
                    break;
                case "Gumon Seal":
                    cb_event_gumonseal.Checked = b;
                    break;
                case "Sunstone":
                    cb_event_sunstone.Checked = b;
                    break;
                case "Warmth Returned":
                    if (rando)
                    {
                        cb_event_cleanwater.Checked = b;
                    }
                    else
                    {
                        cb_event_warmth.Checked = b;
                    }
                    break;
                case "Wind Restored":
                    cb_event_wind.Checked = b;
                    break;
                case "Clean Water":
                    cb_event_cleanwater.Checked = b;
                    break;
            }
        }
        public void UpdateShard(String ev, bool b)
        {
            switch (ev)
            {
                case "Water Vein 1":
                    cb_shard_wv1.Checked = b;
                    break;
                case "Water Vein 2":
                    cb_shard_wv2.Checked = b;
                    break;
                case "Gumon Seal 1":
                    cb_shard_gs1.Checked = b;
                    break;
                case "Gumon Seal 2":
                    cb_shard_gs2.Checked = b;
                    break;
                case "Sunstone 1":
                    cb_shard_ss1.Checked = b;
                    break;
                case "Sunstone 2":
                    cb_shard_ss2.Checked = b;
                    break;

            }
        }
        public void UpdateMapstones(int ms)
        {
            this.numeric_mapstone.Value = ms;
        }

        protected void SendSkillChange(Skill sk, bool b)
        {
            parent.haveSkill[sk] = b;
            parent.Refresh();
        }
        protected void SendTreeChange(Skill sk, bool b)
        {
            parent.haveTree[sk] = b;
            parent.Refresh();
        }
        protected void SendEventChange(String ev, bool b)
        {
            if (ev == "Clean Water" && rando)
            {
                parent.haveEvent["Warmth Returned"] = b;
            }
            else
            {
                parent.haveEvent[ev] = b;
            }
            parent.Refresh();

        }
        protected void SendShardChange(String ev, bool b)
        {
            parent.haveShards[ev] = b;
            parent.Refresh();

        }
        protected void SendMapstoneChange(int ms)
        {
            parent.MapstoneCount = ms;
            parent.Refresh();
        }


        #region Skills
        private void cb_skill_sein_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.Sein, cb_skill_sein.Checked);
        }

        private void cb_skill_wj_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.WallJump, cb_skill_wj.Checked);
        }

        private void cb_skill_cflame_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.ChargeFlame, cb_skill_cflame.Checked);

        }

        private void cb_skill_djump_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.DoubleJump, cb_skill_djump.Checked);
        }

        private void cb_skill_bash_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.Bash, cb_skill_bash.Checked);

        }

        private void cb_skill_stomp_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.Stomp, cb_skill_stomp.Checked);
        }

        private void cb_skill_glide_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.Glide, cb_skill_glide.Checked);

        }

        private void cb_skill_climb_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.Climb, cb_skill_climb.Checked);

        }

        private void cb_skill_cjump_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.ChargeJump, cb_skill_cjump.Checked);

        }

        private void cb_skill_grenade_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.Grenade, cb_skill_grenade.Checked);

        }

        private void cb_skill_dash_CheckedChanged(object sender, EventArgs e)
        {
            SendSkillChange(Skill.Dash, cb_skill_dash.Checked);

        }
        #endregion

        #region Trees
        private void cb_tree_sein_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.Sein, cb_tree_sein.Checked);
        }

        private void cb_tree_wj_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.WallJump, cb_tree_wj.Checked);
        }

        private void cb_tree_cflame_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.ChargeFlame, cb_tree_cflame.Checked);

        }

        private void cb_tree_djump_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.DoubleJump, cb_tree_djump.Checked);
        }

        private void cb_tree_bash_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.Bash, cb_tree_bash.Checked);

        }

        private void cb_tree_stomp_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.Stomp, cb_tree_stomp.Checked);
        }

        private void cb_tree_glide_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.Glide, cb_tree_glide.Checked);

        }

        private void cb_tree_climb_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.Climb, cb_tree_climb.Checked);

        }

        private void cb_tree_cjump_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.ChargeJump, cb_tree_cjump.Checked);

        }

        private void cb_tree_grenade_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.Grenade, cb_tree_grenade.Checked);

        }

        private void cb_tree_dash_CheckedChanged(object sender, EventArgs e)
        {
            SendTreeChange(Skill.Dash, cb_tree_dash.Checked);

        }
        #endregion

        #region Events
        private void cb_event_watervein_CheckedChanged(object sender, EventArgs e)
        {
            SendEventChange("Water Vein", cb_event_watervein.Checked);

        }

        private void cb_event_gumonseal_CheckedChanged(object sender, EventArgs e)
        {
            SendEventChange("Gumon Seal", cb_event_gumonseal.Checked);

        }

        private void cb_event_sunstone_CheckedChanged(object sender, EventArgs e)
        {
            SendEventChange("Sunstone", cb_event_sunstone.Checked);

        }

        private void cb_event_cleanwater_CheckedChanged(object sender, EventArgs e)
        {
            SendEventChange("Clean Water", cb_event_cleanwater.Checked);

        }

        private void cb_event_wind_CheckedChanged(object sender, EventArgs e)
        {
            SendEventChange("Wind Restored", cb_event_wind.Checked);

        }

        private void cb_event_warmth_CheckedChanged(object sender, EventArgs e)
        {
            SendEventChange("Warmth Returned", cb_event_warmth.Checked);

        }
        #endregion

        #region Shards
        private void cb_shard_wv1_CheckedChanged(object sender, EventArgs e)
        {
            SendShardChange("Water Vein 1", cb_shard_wv1.Checked);

        }

        private void cb_shard_wv2_CheckedChanged(object sender, EventArgs e)
        {
            SendShardChange("Water Vein 2", cb_shard_wv2.Checked);

        }

        private void cb_shard_gs1_CheckedChanged(object sender, EventArgs e)
        {
            SendShardChange("Gumon Seal 1", cb_shard_gs1.Checked);

        }
        
        private void cb_shard_gs2_CheckedChanged(object sender, EventArgs e)
        {
            SendShardChange("Gumon Seal 2", cb_shard_gs2.Checked);

        }

        private void cb_shard_ss1_CheckedChanged(object sender, EventArgs e)
        {
            SendShardChange("Sunstone 1", cb_shard_ss1.Checked);

        }

        private void cb_shard_ss2_CheckedChanged(object sender, EventArgs e)
        {
            SendShardChange("Sunstone 2", cb_shard_ss2.Checked);

        }
        #endregion

        #region Other
        private void button_down_Click(object sender, EventArgs e)
        {
            if (numeric_mapstone.Value > 0)
            {
                numeric_mapstone.Value -= 1;
                SendMapstoneChange((int)numeric_mapstone.Value);
            }
        }
        private void button_up_Click(object sender, EventArgs e)
        {
            if (numeric_mapstone.Value < 9)
            {
                numeric_mapstone.Value += 1;
                SendMapstoneChange((int)numeric_mapstone.Value);
            }
        }
        private void numeric_mapstone_ValueChanged(object sender, EventArgs e)
        {
            SendMapstoneChange((int)numeric_mapstone.Value);
        }

        private void EditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!(e.CloseReason == CloseReason.ApplicationExitCall || e.CloseReason == CloseReason.FormOwnerClosing))
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        #endregion

        private void cb_shards_CheckedChanged(object sender, EventArgs e)
        {
            parent.DisplayShards = cb_shards.Checked;
            parent.ChangeShards();          
        }
    }
}
