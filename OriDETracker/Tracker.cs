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
using System.Threading;

namespace OriDETracker
{
    public enum TrackerLayout
    {
        RandomizerAllTrees,
        RandomizerAllEvents,
        AllSkills,
        AllCells,
        ReverseEventOrder
    }

    public partial class Tracker : Form
    {
        public Tracker()
        {
            InitializeComponent();

            scaling = Properties.Settings.Default.Scaling;
            current_layout = Properties.Settings.Default.Layout;
            this.Opacity = Properties.Settings.Default.Opacity;
            this.display_shards = Properties.Settings.Default.Shards;
            this.image_pixel_size = Properties.Settings.Default.Pixels;
            
            edit_form = new EditForm(this);
            edit_form.Visible = false;

            settings = new SettingsLayout(this);
            settings.Visible = false;


            mem = new OriDE.Memory.OriMemory();
            th = new Thread(UpdateLoop);
            th.IsBackground = true;

            scaledSize = new Size(image_pixel_size, image_pixel_size);

            this.UpdateImages();
            this.ChangeLayout(current_layout);

            int tmp = 1;
            foreach (FontFamily ff in FontFamily.Families)
            {
                if (ff.Name.ToLower() == "amatic sc")
                {
                    map_font = new Font(new FontFamily("Amatic SC"), 24, FontStyle.Bold);
                    tmp = 0;
                    break;
                }
            }

            if (tmp == 1)
            {
                if (this.fontDialog_mapstone.ShowDialog() == DialogResult.OK)
                {
                    map_font = fontDialog_mapstone.Font;
                    map_font = new Font(fontDialog_mapstone.Font.FontFamily, 24f, FontStyle.Bold);
                }
            }
        }          

        protected OriMemory mem { get; set; }
        protected Thread th;
        protected SettingsLayout settings;
        protected TrackerLayout current_layout;
        protected EditForm edit_form;
        protected Graphics selfGraphics;

        public float Scaling { get { return scaling; } set { scaling = value; } }
        public int ImagePixelSize { get { return image_pixel_size; } set { image_pixel_size = value; } }
        public bool DisplayShards { get { return display_shards; } set { display_shards = value; } }
        public Font MapFont { set { map_font = value;  } }

        protected const int TOL = 25;
        protected bool auto_update = false;
        protected bool draggable = false;

        protected int mapstone_count = 0;

        protected float scaling = 1.0f;
        protected int image_pixel_size = 600;
        protected Size scaledSize;

        protected bool display_mapstone = false;
        protected bool display_shards = false;

        public Brush font_brush = new SolidBrush(Color.White);

        protected Font map_font;
        

        #region FrameMoving
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        #endregion

        #region LogicDictionary
        //general: Skills and Events
        public Dictionary<Skill, bool> haveSkill;
        public Dictionary<String, bool> haveEvent;

        //All Trees
        public Dictionary<Skill, bool> hitTree;
        public Dictionary<Skill, bool> haveTree;

        //Shards
        public Dictionary<String, bool> haveShards;

        //Mapstone
        public int MapstoneCount { get { return mapstone_count; } set { mapstone_count = value; } }

        /*
        //All Events
        private Dictionary<String, bool> haveEventLocation;
        private Dictionary<String, bool> hitEventLocation;
        */
        #endregion

        #region Images


        protected String DIR = @"Assets_400/";

        protected Image imageSpiritFlame ;
        protected Image imageWallJump ; 
        protected Image imageChargeFlame ; 
        protected Image imageDoubleJump ;
        protected Image imageBash ; 
        protected Image imageStomp ;
        protected Image imageGlide ; 
        protected Image imageClimb ; 
        protected Image imageChargeJump ;
        protected Image imageLightGrenade ; 
        protected Image imageDash ;

        /*
        protected Image imageGSpiritFlame   ;
        protected Image imageGWallJump      ;
        protected Image imageGChargeFlame   ; 
        protected Image imageGDoubleJump    ;
        protected Image imageGBash          ; 
        protected Image imageGStomp         ; 
        protected Image imageGChargeJump    ; 
        protected Image imageGClimb         ;
        protected Image imageGGlide         ; 
        protected Image imageGLightGrenade  ; 
        protected Image imageGDash          ; 
        */

        protected Image imageTreeSpiritFlame ;
        protected Image imageTreeWallJump ;
        protected Image imageTreeChargeFlame ; 
        protected Image imageTreeDoubleJump ;
        protected Image imageTreeBash ;
        protected Image imageTreeStomp ; 
        protected Image imageTreeChargeJump ; 
        protected Image imageTreeGlide ; 
        protected Image imageTreeClimb ; 
        protected Image imageTreeLightGrenade ; 
        protected Image imageTreeDash ; 

        /*
        protected Image imageGTreeSpiritFlame  ; + @"GTSpiritFlame.png");
        protected Image imageGTreeWallJump     ; + @"GTWallJump.png");
        protected Image imageGTreeChargeFlame  ; + @"GTChargeFlame.png");
        protected Image imageGTreeDoubleJump   ; + @"GTDoubleJump.png");
        protected Image imageGTreeBash         ; + @"GTBash.png");
        protected Image imageGTreeStomp        ; + @"GTStomp.png");
        protected Image imageGTreeChargeJump   ; + @"GTChargeJump.png");
        protected Image imageGTreeGlide        ; + @"GTGlide.png");
        protected Image imageGTreeClimb        ; + @"GTClimb.png");
        protected Image imageGTreeLightGrenade ; + @"GTLightGrenade.png");
        protected Image imageGTreeDash         ; + @"GTDash.png");
        */

        protected Image imageWaterVein ; 
        protected Image imageGumonSeal ; 
        protected Image imageSunstone ; 
        protected Image imageCleanWater ; 
        protected Image imageWindRestored ; 
        protected Image imageWarmthReturned ;

        protected Image imageGWaterVein ; 
        protected Image imageGGumonSeal ; 
        protected Image imageGSunstone ; 
        protected Image imageGCleanWater ;
        protected Image imageGWindRestored ; 
        protected Image imageGWarmthReturned ;

        protected Image imageWindRestoredRando ;
        protected Image imageGWindRestoredRando ;

        protected Image imageWaterVeinShard1 ;
        protected Image imageWaterVeinShard2 ;

        protected Image imageGumonSealShard1 ; 
        protected Image imageGumonSealShard2 ; 

        protected Image imageSunstoneShard1 ;
        protected Image imageSunstoneShard2 ; 

        protected Image imageSkillWheel ;
        protected Image imageSkillWheelDouble ; 
        protected Image imageSkillWheelTriple ; 
        protected Image imageBlackBackground ;
        protected Image imageGSkills ; 
        protected Image imageGTrees ;

        protected Image imageMapStone;

        public void UpdateImages()
        {
            DIR = "Assets_" + image_pixel_size.ToString() + @"/";

            imageSpiritFlame = Image.FromFile(DIR + @"SpiritFlame.png");
            imageWallJump = Image.FromFile(DIR + @"WallJump.png");
            imageChargeFlame = Image.FromFile(DIR + @"ChargeFlame.png");
            imageDoubleJump = Image.FromFile(DIR + @"DoubleJump.png");
            imageBash = Image.FromFile(DIR + @"Bash.png");
            imageStomp = Image.FromFile(DIR + @"Stomp.png");
            imageGlide = Image.FromFile(DIR + @"Glide.png");
            imageClimb = Image.FromFile(DIR + @"Climb.png");
            imageChargeJump = Image.FromFile(DIR + @"ChargeJump.png");
            imageLightGrenade = Image.FromFile(DIR + @"LightGrenade.png");
            imageDash = Image.FromFile(DIR + @"Dash.png");

            /*
            imageGSpiritFlame   = Image.FromFile(DIR + @"GSpiritFlame.png");
            imageGWallJump      = Image.FromFile(DIR + @"GWallJump.png");
            imageGChargeFlame   = Image.FromFile(DIR + @"GChargeFlame.png");
            imageGDoubleJump    = Image.FromFile(DIR + @"GDoubleJump.png");
            imageGBash          = Image.FromFile(DIR + @"GBash.png");
            imageGStomp         = Image.FromFile(DIR + @"GStomp.png");
            imageGChargeJump    = Image.FromFile(DIR + @"GChargeJump.png");
            imageGClimb         = Image.FromFile(DIR + @"GClimb.png");
            imageGGlide         = Image.FromFile(DIR + @"GGlide.png");
            imageGLightGrenade  = Image.FromFile(DIR + @"GLightGrenade.png");
            imageGDash          = Image.FromFile(DIR + @"GDash.png");
            */

            imageTreeSpiritFlame = Image.FromFile(DIR + @"TSpiritFlame.png");
            imageTreeWallJump = Image.FromFile(DIR + @"TWallJump.png");
            imageTreeChargeFlame = Image.FromFile(DIR + @"TChargeFlame.png");
            imageTreeDoubleJump = Image.FromFile(DIR + @"TDoubleJump.png");
            imageTreeBash = Image.FromFile(DIR + @"TBash.png");
            imageTreeStomp = Image.FromFile(DIR + @"TStomp.png");
            imageTreeChargeJump = Image.FromFile(DIR + @"TChargeJump.png");
            imageTreeGlide = Image.FromFile(DIR + @"TGlide.png");
            imageTreeClimb = Image.FromFile(DIR + @"TClimb.png");
            imageTreeLightGrenade = Image.FromFile(DIR + @"TLightGrenade.png");
            imageTreeDash = Image.FromFile(DIR + @"TDash.png");

            /*
            imageGTreeSpiritFlame  = Image.FromFile(DIR + @"GTSpiritFlame.png");
            imageGTreeWallJump     = Image.FromFile(DIR + @"GTWallJump.png");
            imageGTreeChargeFlame  = Image.FromFile(DIR + @"GTChargeFlame.png");
            imageGTreeDoubleJump   = Image.FromFile(DIR + @"GTDoubleJump.png");
            imageGTreeBash         = Image.FromFile(DIR + @"GTBash.png");
            imageGTreeStomp        = Image.FromFile(DIR + @"GTStomp.png");
            imageGTreeChargeJump   = Image.FromFile(DIR + @"GTChargeJump.png");
            imageGTreeGlide        = Image.FromFile(DIR + @"GTGlide.png");
            imageGTreeClimb        = Image.FromFile(DIR + @"GTClimb.png");
            imageGTreeLightGrenade = Image.FromFile(DIR + @"GTLightGrenade.png");
            imageGTreeDash         = Image.FromFile(DIR + @"GTDash.png");
            */

            imageWaterVein = Image.FromFile(DIR + @"WaterVein.png");
            imageGumonSeal = Image.FromFile(DIR + @"GumonSeal.png");
            imageSunstone = Image.FromFile(DIR + @"Sunstone.png");
            imageCleanWater = Image.FromFile(DIR + @"CleanWater.png");
            imageWindRestored = Image.FromFile(DIR + @"WindRestored.png");
            imageWarmthReturned = Image.FromFile(DIR + @"WarmthReturned.png");

            imageGWaterVein = Image.FromFile(DIR + @"GWaterVein.png");
            imageGGumonSeal = Image.FromFile(DIR + @"GGumonSeal.png");
            imageGSunstone = Image.FromFile(DIR + @"GSunstone.png");
            imageGCleanWater = Image.FromFile(DIR + @"GCleanWater.png");
            imageGWindRestored = Image.FromFile(DIR + @"GWindRestored.png");
            imageGWarmthReturned = Image.FromFile(DIR + @"GWarmthReturned.png");

            imageSkillWheel = Image.FromFile(DIR + @"SkillRing_Single.png");
            imageSkillWheelDouble = Image.FromFile(DIR + @"SkillRing_Double.png");
            imageSkillWheelTriple = Image.FromFile(DIR + @"SkillRing_Triple.png");

            imageBlackBackground = Image.FromFile(DIR + @"BlackBackground.png");
            imageGSkills = Image.FromFile(DIR + @"GreySkillTree.png");
            imageGTrees = Image.FromFile(DIR + @"GreyTrees.png");

            imageMapStone = Image.FromFile(DIR + @"MapStone.png");

            imageWindRestoredRando = Image.FromFile(DIR + @"WindRestoredRando.png");
            imageGWindRestoredRando = Image.FromFile(DIR + @"GWindRestoredRando.png");

            imageWaterVeinShard1 = Image.FromFile(DIR + @"WaterVeinShard1.png");
            imageWaterVeinShard2 = Image.FromFile(DIR + @"WaterVeinShard2.png");

            imageGumonSealShard1 = Image.FromFile(DIR + @"GumonSealShard1.png");
            imageGumonSealShard2 = Image.FromFile(DIR + @"GumonSealShard2.png");

            imageSunstoneShard1 = Image.FromFile(DIR + @"SunstoneShard1.png");
            imageSunstoneShard2 = Image.FromFile(DIR + @"SunstoneShard2.png");
        }

        protected Dictionary<Skill, Image> skillImages = new Dictionary<Skill, Image>();
        //protected Dictionary<Skill, Image> skillGreyImages = new Dictionary<Skill, Image>();

        protected Dictionary<String, Image> eventImages = new Dictionary<String, Image>();
        protected Dictionary<String, Image> eventGreyImages = new Dictionary<String, Image>();

        protected Dictionary<Skill, Image> treeImages = new Dictionary<Skill, Image>();

        protected Dictionary<String, Image> shardImages = new Dictionary<string, Image>();
        //protected Dictionary<Skill, Image> treeGreyImages = new Dictionary<Skill, Image>();

        #endregion

        #region Hitbox
        //Game hitboxes for trees and events
        private bool checkTreeHitbox = false;
        private bool checkEventHitbox = false;

        private Dictionary<Skill, HitBox> treeHitboxes = new Dictionary<Skill, HitBox>()
        {
            { Skill.Sein,        new HitBox("-165,-262,1,2") },
            { Skill.WallJump,    new HitBox("-317,-301,5,6") },
            { Skill.ChargeFlame, new HitBox("-53,-153,5,6") },
            { Skill.Dash,        new HitBox("293,-251,5,6") },
            { Skill.DoubleJump,  new HitBox("785,-404,5,6") },
            { Skill.Bash,        new HitBox("532,334,5,6") },
            { Skill.Stomp,       new HitBox("859,-88,5,6") },
            { Skill.Glide,       new HitBox("-458,-13,5,6") },
            { Skill.Climb,       new HitBox("-1189,-95,5,6") },
            { Skill.ChargeJump,  new HitBox("-697,413,5,6") },
            { Skill.Grenade,     new HitBox("69,-373,5,6") }
        }; 
           
        //placeholder until I get the actual coordinates
        private Dictionary<String, HitBox> eventHitboxes = new Dictionary<String, HitBox>()
        {
            {"Water Vein",      new HitBox( "0,0,1,1") },
            {"Gumon Seal",      new HitBox( "0,0,1,1") },
            {"Sunstone",        new HitBox( "0,0,1,1") },
            {"Clean Water",     new HitBox( "0,0,1,1") },
            {"Wind Restored",   new HitBox( "0,0,1,1") },
            {"Warmth Returned", new HitBox( "0,0,1,1") },
        };
        #endregion        

        //points for mouse clicks (with certain tolerance defined by TOL)
        private Dictionary<Skill, Point> skillMousePoint = new Dictionary<Skill, Point>();
        private Dictionary<String, Point> eventMousePoint = new Dictionary<string, Point>();
        private Dictionary<Skill, Point> treeMouseLocation = new Dictionary<Skill, Point>();
        private Point mapstoneMousePoint = new Point(305, 343);
        //private Dictionary<String, Point> eventMouseLocation;

        #region SetLayout
        public void ChangeLayout(TrackerLayout layout)
        {
            this.current_layout = layout;
            switch (layout)
            {
                case TrackerLayout.AllCells:
                    SetLayoutAllCells();
                    break;
                case TrackerLayout.AllSkills:
                    SetLayoutAllSkills();
                    break;
                case TrackerLayout.ReverseEventOrder:
                    SetLayoutReverseEventOrder();
                    break;
                case TrackerLayout.RandomizerAllTrees:
                    SetLayoutRandomizerAllTrees();
                    break;
                case TrackerLayout.RandomizerAllEvents:
                    SetLayoutRandomizerAllEvents();
                    break;
                default:
                    break;
            }
        }
        private void SetLayoutRandomizerAllTrees()
        {
            SetLayoutDefaults();

            display_mapstone = true;
            ChangeMapstone();

            hitTree = new Dictionary<Skill, bool>()
            {
                { Skill.Sein,        false },
                { Skill.WallJump,    false },
                { Skill.ChargeFlame, false },
                { Skill.DoubleJump,  false },
                { Skill.Bash,        false },
                { Skill.Stomp,       false },
                { Skill.Glide,       false },
                { Skill.Climb,       false },
                { Skill.ChargeJump,  false },
                { Skill.Grenade,     false },
                { Skill.Dash ,       false }
            };

            haveTree = new Dictionary<Skill, bool>()
            {
                { Skill.Sein,        false },
                { Skill.WallJump,    false },
                { Skill.ChargeFlame, false },
                { Skill.DoubleJump,  false },
                { Skill.Bash,        false },
                { Skill.Stomp,       false },
                { Skill.Glide,       false },
                { Skill.Climb,       false },
                { Skill.ChargeJump,  false },
                { Skill.Grenade,     false },
                { Skill.Dash ,       false }
            };
            haveEvent = new Dictionary<String, bool>()
            {
                { "Water Vein",      false },
                { "Gumon Seal",      false },
                { "Sunstone",        false },
                { "Warmth Returned", false }, //this is actually clean water
                { "Wind Restored",   false }
            };

            haveShards = new Dictionary<string, bool>()
            {
                { "Water Vein 1",     false },
                { "Water Vein 2",     false },
                { "Gumon Seal 1",     false },
                { "Gumon Seal 2",     false },
                { "Sunstone 1",      false },
                { "Sunstone 2",      false },
            };

            eventImages = new Dictionary<String, Image>()
            {
                { "Water Vein",       imageWaterVein },
                { "Gumon Seal",      imageGumonSeal },
                { "Sunstone",        imageSunstone },
                { "Wind Restored",    imageWindRestoredRando },
                { "Warmth Returned", imageCleanWater }
            };

            eventGreyImages = new Dictionary<String, Image>()
            {
                { "Water Vein",      imageGWaterVein },
                { "Gumon Seal",      imageGGumonSeal },
                { "Sunstone",        imageGSunstone },
                { "Wind Restored",   imageGWindRestoredRando },
                { "Warmth Returned", imageGCleanWater}
            };

            shardImages = new Dictionary<string, Image>()
            {
                { "Water Vein 1",     imageWaterVeinShard1 },
                { "Water Vein 2",     imageWaterVeinShard2 },
                { "Gumon Seal 1",     imageGumonSealShard1 },
                { "Gumon Seal 2",     imageGumonSealShard2 },
                { "Sunstone 1",      imageSunstoneShard1 },
                { "Sunstone 2",      imageSunstoneShard2 },
            };

            treeImages = new Dictionary<Skill, Image>()
            {
                { Skill.Sein,        imageTreeSpiritFlame },
                { Skill.WallJump,    imageTreeWallJump },
                { Skill.ChargeFlame, imageTreeChargeFlame },
                { Skill.Dash,        imageTreeDash },
                { Skill.DoubleJump,  imageTreeDoubleJump },
                { Skill.Bash,        imageTreeBash },
                { Skill.Stomp,       imageTreeStomp },
                { Skill.Glide,       imageTreeGlide },
                { Skill.Climb,       imageTreeClimb },
                { Skill.ChargeJump,  imageTreeChargeJump },
                { Skill.Grenade,     imageTreeLightGrenade }
            };

            /*
            treeGreyImages = new Dictionary<Skill, Image>()
            {
                { Skill.Sein,        imageGTreeSpiritFlame },
                { Skill.WallJump,    imageGTreeWallJump },
                { Skill.ChargeFlame, imageGTreeChargeFlame },
                { Skill.Dash,        imageGTreeDash },
                { Skill.DoubleJump,  imageGTreeDoubleJump },
                { Skill.Bash,        imageGTreeBash },
                { Skill.Stomp,       imageGTreeStomp },
                { Skill.Glide,       imageGTreeGlide },
                { Skill.Climb,       imageGTreeClimb },
                { Skill.ChargeJump,  imageGTreeChargeJump },
                { Skill.Grenade,     imageGTreeLightGrenade }
            };
            */

            eventMousePoint = new Dictionary<string, Point>()
            {
                { "Water Vein", new Point(206, 240) },
                { "Gumon Seal", new Point(300, 202) },
                { "Sunstone",   new Point(393, 233) },
                { "Wind Restored", new Point(391, 342) },
                { "Warmth Returned", new Point(205, 343) }
            };

            checkTreeHitbox = true;
            checkEventHitbox = false;
        }

        private void SetLayoutDefaults()
        {
            SetMouseLocations();
            checkTreeHitbox = false;
            checkEventHitbox = false;

            #region Logic
            haveSkill = new Dictionary<Skill, bool>()
            {
                { Skill.Sein,        false },
                { Skill.WallJump,    false },
                { Skill.Dash ,       false },
                { Skill.ChargeFlame, false },
                { Skill.DoubleJump,  false },
                { Skill.Bash,        false },
                { Skill.Stomp,       false },
                { Skill.Glide,       false },
                { Skill.Climb,       false },
                { Skill.ChargeJump,  false },
                { Skill.Grenade,     false }
            };
            haveEvent = new Dictionary<String, bool>()
            {
                { "Water Vein",      false },
                { "Gumon Seal",      false },
                { "Sunstone",        false },
                { "Clean Water",     false },
                { "Warmth Returned", false },
                { "Wind Restored",   false }
            };

            #endregion         

            skillImages = new Dictionary<Skill, Image>()
            {
                { Skill.Sein,        imageSpiritFlame },
                { Skill.WallJump,    imageWallJump },
                { Skill.ChargeFlame, imageChargeFlame },
                { Skill.Dash,        imageDash },
                { Skill.DoubleJump,  imageDoubleJump },
                { Skill.Bash,        imageBash },
                { Skill.Stomp,       imageStomp },
                { Skill.Glide,       imageGlide },
                { Skill.Climb,       imageClimb },
                { Skill.ChargeJump,  imageChargeJump },
                { Skill.Grenade,     imageLightGrenade }
            };

            /*
            skillGreyImages = new Dictionary<Skill, Image>()
            {
                { Skill.Sein,        imageGSpiritFlame },
                { Skill.WallJump,    imageGWallJump },
                { Skill.ChargeFlame, imageGChargeFlame },
                { Skill.Dash,        imageGDash },
                { Skill.DoubleJump,  imageGDoubleJump },
                { Skill.Bash,        imageGBash },
                { Skill.Stomp,       imageGStomp },
                { Skill.Glide,       imageGGlide },
                { Skill.Climb,       imageGClimb },
                { Skill.ChargeJump,  imageGChargeJump },
                { Skill.Grenade,     imageGLightGrenade }
            };
            */

            eventImages = new Dictionary<String, Image>()
            {
                { "Water Vein",       imageWaterVein },
                { "Gumon Seal",      imageGumonSeal },
                { "Sunstone",        imageSunstone },
                { "Clean Water",     imageCleanWater},
                { "Wind Restored",    imageWindRestored },
                { "Warmth Returned", imageWarmthReturned }
            };

            eventGreyImages = new Dictionary<String, Image>()
            {
                { "Water Vein",      imageGWaterVein },
                { "Gumon Seal",      imageGGumonSeal },
                { "Sunstone",        imageGSunstone },
                { "Clean Water",     imageGCleanWater},
                { "Wind Restored",   imageGWindRestored },
                { "Warmth Returned", imageGWarmthReturned }
            };
        }

        private void SetLayoutRandomizerAllEvents()
        {

        }
        private void SetLayoutAllSkills()
        {

        }
        private void SetLayoutAllCells()
        {

        }
        private void SetLayoutReverseEventOrder()
        {

        }

        private void SetMouseLocations()
        {
            skillMousePoint = new Dictionary<Skill, Point>();
            treeMouseLocation = new Dictionary<Skill, Point>();

            for (int i = 0; i < 11; ++i)
            {
                skillMousePoint.Add((Skill) i, new Point((int)(300 + 194 * Math.Sin(2.0 * i * Math.PI / 11.0)),
                    (int)(300 - 194 * Math.Cos(2.0 * i * Math.PI / 11.0))));
            }
            for (int i = 0; i < 11; ++i)
            {
                treeMouseLocation.Add((Skill) i, new Point((int)(300 + 267 * Math.Sin(2.0 * i * Math.PI / 11.0)),
                    (int)(300 - 267 * Math.Cos(2.0 * i * Math.PI / 11.0))));
            }

            eventMousePoint = new Dictionary<string, Point>()
            {
                { "Water Vein", new Point(206, 240) },
                { "Gumon Seal", new Point(300, 202) },
                { "Sunstone",   new Point(393, 233) },
                { "Clean Water", new Point(205, 343) },
                { "Wind Restored", new Point(300, 404) },
                { "Warmth Returned", new Point(391, 342) }
            };
        }

        #endregion

        #region EventHandlers
        private void Tracker_MouseDown(object sender, MouseEventArgs e)
        {            
            if (e.Button == MouseButtons.Left && draggable)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        protected void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        protected void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAll();

            settings.Reset();
            edit_form.Reset();
            this.settings.Visible = false;
            
            scaling = 1.0f;
            this.Opacity = 1.0;
            this.image_pixel_size = 600;

            current_layout = TrackerLayout.RandomizerAllTrees;
            ChangeLayout(current_layout);

            auto_update = false;
            this.autoUpdateToolStripMenuItem.Checked = false;
            draggable = false;
            this.editToolStripMenuItem.Checked = false;

            this.TopMost = true;
            this.alwaysOnTopToolStripMenuItem.Checked = false;

            this.display_shards = false; 

            this.BackColor = Color.Black;
            this.font_brush = new SolidBrush(Color.White);
            this.TransparencyKey = Color.Empty;

            Refresh();
        }
        protected void autoUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            auto_update = !(auto_update);
            //autoUpdateToolStripMenuItem.Checked = auto_update;

            if (auto_update)
            {
                TurnOnAutoUpdate();
            }
            else
            {
                TurnOffAutoUpdate();
            }
        }
        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }
        protected void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draggable = !draggable;
            //editToolStripMenuItem.Checked = draggable;
        }
        protected void Tracker_MouseClick(object sender, MouseEventArgs e)
        {
            int x, y;
            x = e.X;
            y = e.Y;

            //MessageBox.Show("X: " + x + "   Y: " + y);
            ToggleMouseClick(x, y);

            Refresh();
            this.Invalidate();
        }
        protected void Tracker_Paint(object sender, PaintEventArgs e)
        {
            //UpdateGraphics(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateGraphics(e.Graphics);
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings.Show();
        }
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAll();
            Refresh();
        }
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.edit_form.Show();
        }
        #endregion  

        #region Graphics

        protected int Square(int a)
        {
            return a * a;
        }

        protected bool ToggleMouseClick(int x, int y)
        {
            double mouse_scaling = scaling * ((image_pixel_size * 1.0)/ 600.0 );
            int CUR_TOL = (int)(TOL * mouse_scaling);

            if ( display_mapstone && (Math.Sqrt(Square(x - (int)(mapstoneMousePoint.X * mouse_scaling)) + Square(y - (int)(mapstoneMousePoint.Y * mouse_scaling))) <= 2 * CUR_TOL) )
            {
                mapstone_count += 1;
                if (mapstone_count > 9)
                {
                    mapstone_count = 0;
                }
                return true;
            }

            foreach (KeyValuePair<Skill, Point> sk in skillMousePoint)
            {
                if (Math.Sqrt(Square(x - (int)(sk.Value.X*mouse_scaling)) + Square(y - (int)(sk.Value.Y * mouse_scaling))) <= 2*CUR_TOL)
                {
                    if (haveSkill.ContainsKey(sk.Key))
                    {
                        haveSkill[sk.Key] = !haveSkill[sk.Key];
                        return true;
                    }
                }
            }

            foreach (KeyValuePair<Skill, Point> sk in treeMouseLocation)
            {
                if (Math.Sqrt(Square(x - (int)(sk.Value.X * mouse_scaling)) + Square(y - (int)(sk.Value.Y * mouse_scaling))) <= CUR_TOL)
                {
                    if (haveTree.ContainsKey(sk.Key))
                    {
                        haveTree[sk.Key] = !haveTree[sk.Key];
                        return true;
                    }
                }
            }

            foreach (KeyValuePair<String, Point> sk in eventMousePoint)
            {
                if (Math.Sqrt(Square(x - (int)(sk.Value.X * mouse_scaling)) + Square(y - (int)(sk.Value.Y * mouse_scaling))) <= CUR_TOL)
                {
                    if (haveEvent.ContainsKey(sk.Key))
                    {
                        switch (sk.Key)
                        {
                            case "Water Vein":
                            case "Gumon Seal":
                            case "Sunstone":
                                if (display_shards)
                                {
                                    if (haveEvent[sk.Key])
                                    {
                                        haveShards[sk.Key + " 1"] = false;
                                        haveShards[sk.Key + " 2"] = false;
                                        haveEvent[sk.Key] = false;
                                    }
                                    else if (haveShards[sk.Key + " 2"])
                                    {
                                        haveShards[sk.Key + " 1"] = true;
                                        haveShards[sk.Key + " 2"] = true;
                                        haveEvent[sk.Key] = true;
                                    }
                                    else if (haveShards[sk.Key + " 1"])
                                    {
                                        haveShards[sk.Key + " 1"] = true;
                                        haveShards[sk.Key + " 2"] = true;
                                        haveEvent[sk.Key] = false;
                                    }
                                    else
                                    {
                                        haveShards[sk.Key + " 1"] = true;
                                        haveShards[sk.Key + " 2"] = false;
                                        haveEvent[sk.Key] = false;
                                    }
                                }
                                else
                                {
                                    haveEvent[sk.Key] = !haveEvent[sk.Key];
                                }
                                break;
                            case "Warmth Returned":
                            case "Wind Restored":
                            case "Clean Water":
                                haveEvent[sk.Key] = !haveEvent[sk.Key];
                                break;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        protected void UpdateGraphics(Graphics g)
        {
            if (this.selfGraphics != g)
                this.selfGraphics = g;

            try
            {
                /*
                 * Drawing consists of the following steps:
                 * (1) The background on which everything is drawn (this can be user selected)
                 * (2) Drawing the Skills (either grayed out or colored in)
                 * (3) Drawing the Events (same)
                 * (4) Drawing the Tree locations
                 * (5) Putting the skill wheel on top
                 * */

                scaledSize = new Size((int)(image_pixel_size * scaling), (int)(image_pixel_size * scaling));
                this.Size = scaledSize;
                Rectangle drawRect = new Rectangle(new Point(0, 0), scaledSize);

                #region Draw
                #region Skills

                this.selfGraphics.DrawImage(imageGSkills, drawRect);
                foreach (KeyValuePair<Skill, bool> sk in haveSkill)
                {
                    edit_form.UpdateSkill(sk.Key, sk.Value);

                    if (sk.Value)
                    {
                        g.DrawImage(skillImages[sk.Key], drawRect);
                    }
                }

                #endregion
                #region Events

                foreach (KeyValuePair<String, bool> ev in haveEvent)
                {
                    edit_form.UpdateEvent(ev.Key, ev.Value);

                    if (ev.Value)
                    {
                        this.selfGraphics.DrawImage(eventImages[ev.Key], drawRect);

                    }
                    else
                    {
                        this.selfGraphics.DrawImage(eventGreyImages[ev.Key], drawRect);
                    }
                }
                #endregion
                #region Tree

                this.selfGraphics.DrawImage(imageGTrees, drawRect);
                foreach (KeyValuePair<Skill, bool> sk in haveTree)
                {
                    edit_form.UpdateTree(sk.Key, sk.Value);

                    if (sk.Value)
                    {
                        this.selfGraphics.DrawImage(treeImages[sk.Key], drawRect);
                    }
                    /*
                    else
                    {
                        this.selfGraphics.DrawImage(treeGreyImages[sk.Key], drawRect);
                    }
                    */
                }

                #endregion
                #region Shards
                if (display_shards)
                {
                    foreach (KeyValuePair<String, bool> ev in haveShards)
                    {
                        edit_form.UpdateShard(ev.Key, ev.Value);

                        if (ev.Value)
                        {
                            this.selfGraphics.DrawImage(shardImages[ev.Key], drawRect);
                        }
                    }
                }
                #endregion  
                if (display_mapstone)
                {
                    edit_form.UpdateMapstones(mapstone_count);

                    this.selfGraphics.DrawImage(imageMapStone, drawRect);
                    this.selfGraphics.DrawString(mapstone_count.ToString() + "/9", map_font, font_brush, new PointF(285 * scaling, 375 * scaling));
                }
                #endregion

                this.selfGraphics.DrawImage(imageSkillWheelDouble, drawRect);
            }
            catch { }
        }

        protected void ClearAll()
        {
            for (int i = 0; i < haveSkill.Count; i++)
            {
                haveSkill[haveSkill.ElementAt(i).Key] = false;
            }
            for (int i = 0; i < haveTree.Count; i++)
            {
                haveTree[haveTree.ElementAt(i).Key] = false;
            }
            for (int i = 0; i < hitTree.Count; i++)
            {
                hitTree[hitTree.ElementAt(i).Key] = false;
            }
            for (int i = 0; i < haveEvent.Count; i++)
            {
                haveEvent[haveEvent.ElementAt(i).Key] = false;
            }
            edit_form.Clear();

            Refresh();
        }

        public void ChangeMapstone()
        {
            edit_form.ChangeMapstone(display_mapstone);
        }
        public void ChangeShards()
        {
            settings.ChangeShards(display_shards);
            edit_form.ChangeShards(display_shards);
        }

        #endregion

        #region AutoUpdate
        //these features need to be added
        bool paused = false;
        protected void TurnOnAutoUpdate()
        {
            if (paused)
            {
                th.Resume();
                paused = false;
            }
            else
            {
                th.Start();
            }
        }

        protected void TurnOffAutoUpdate()
        {
            th.Suspend();
            paused = true;
        }

        private bool CheckInGame(GameState state)
        {
            return (state != GameState.Logos && state != GameState.StartScreen && state != GameState.TitleScreen);
        }

        private bool CheckInGameWorld(GameState state)
        {
            return (CheckInGame(state) && state != GameState.Prologue && !mem.IsEnteringGame());
        }

        private void UpdateLoop()
        {
            bool lastHooked = false;
            while (true)
            {
                try
                {
                    bool hooked = mem.HookProcess();
                    if (hooked)
                    {
                        UpdateValues();
                    }
                    if (lastHooked != hooked)
                    {
                        lastHooked = hooked;
                        //MessageBox.Show("Hooked: " + hooked.ToString());
                        this.Invoke((Action)delegate () { labelBlank.Visible = false; });
                    }
                    Thread.Sleep(100);
                }
                catch { }
            }
        }

        private void UpdateValues()
        {
            if (CheckInGameWorld(mem.GetGameState()))
            {
                UpdateSkills();
                UpdateEvents();
                UpdateShards();
                UpdateMapstoneProgression();
                if (checkTreeHitbox)
                    CheckTrees();
                if (checkEventHitbox)
                    CheckEventLocations();    

                //the following works but is "incorrect"
                try
                {
                    this.Invalidate();
                    this.Update();
                }
                catch { }
            }
        }

        private void UpdateSkills()
        {
            Skill cur;
            for (int i = 0; i < haveSkill.Count; i++)
            {
                cur = haveSkill.ElementAt(i).Key;
                haveSkill[cur] = mem.GetAbility(GetSkillName(cur));
            }
        }

        private void UpdateEvents()
        {
            String cur;
            for (int i = 0; i < haveEvent.Count; i++)
            {
                cur = haveEvent.ElementAt(i).Key;
                switch (cur)
                {
                    case "Water Vein":
                    case "Gumon Seal":
                    case "Sunstone":
                        haveEvent[cur] = mem.GetKey(cur);
                        break;
                    case "Warmth Returned":
                    case "Wind Restored":
                    case "Clean Water":
                        haveEvent[cur] = mem.GetEvent(cur);
                        break;
                }
            }
        }

        private void UpdateShards()
        {
            DisplayShards = mem.WaterVeinShards() > 0 || mem.GumonSealShards() > 0 || mem.SunstoneShards() > 0;
            haveShards["Water Vein 1"] = mem.WaterVeinShards() > 0;
            haveShards["Water Vein 2"] = mem.WaterVeinShards() > 1;
            haveShards["Gumon Seal 1"] = mem.GumonSealShards() > 0;
            haveShards["Water Vein 2"] = mem.GumonSealShards() > 1;
            haveShards["Sunstone 1"] = mem.SunstoneShards() > 0;
            haveShards["Sunstone 2"] = mem.SunstoneShards() > 1;
        }

        private void UpdateMapstoneProgression()
        {
            MapstoneCount = mem.MapStoneProgression();
        }

        private void CheckTrees()
        {
            HitBox ori = new HitBox(mem.GetCameraTargetPosition(), 0.68f, 1.15f, true);

            Skill tree_at = Skill.None;
            bool touchingAnyTree = false;
            foreach (KeyValuePair<Skill, HitBox> tree in treeHitboxes)
            {
                if (tree.Value.Intersects(ori))
                {
                    touchingAnyTree = true;
                    if (!mem.CanMove())
                    {
                        tree_at = tree.Key;
                        touchingAnyTree = false;
                    }
                    break;
                }
            }

            if (!touchingAnyTree && tree_at != Skill.None)
            {
                hitTree[tree_at] = true;
                haveTree[tree_at] = true;
            }

            //this loops over all trees and updates the have values to the hit values

			foreach (KeyValuePair<Skill, bool> skills in hitTree)
			{
				haveTree[skills.Key] = (hitTree[skills.Key] || skills.Value);
			}

        }

        private void CheckEventLocations()
        {
            HitBox ori = new HitBox(mem.GetCameraTargetPosition(), 0.68f, 1.15f, true);

            String event_at = "";
            bool touchingAnyEvent = false;
            foreach (KeyValuePair<String, HitBox> loc in eventHitboxes)
            {
                if (loc.Value.Intersects(ori))
                {
                    touchingAnyEvent = true;
                    if (!mem.CanMove())
                    {
                        event_at = loc.Key;
                        touchingAnyEvent = false;
                    }
                    break;
                }
            }
            
            if (!touchingAnyEvent && event_at != "")
            {
                //hitEventLocation[event_at] = true;
                //haveEventLocation[event_at] = true;
            }
            /*
            //this loops over all trees and updates the have values to the hit values
            //looking at this right now, I'm not exactly sure if it is working
            foreach (KeyValuePair<String, bool> trees in hitEventLocation)
            {
                haveEventLocation[trees.Key] = (hitEventLocation[trees.Key] || trees.Value);
            }
            */

        }

        private string GetSkillName(Skill sk)
        {
            switch (sk)
            {
                case Skill.Sein: return "Spirit Flame";
                case Skill.WallJump: return "Wall Jump";
                case Skill.ChargeFlame: return "Charge Flame";
                case Skill.Dash: return "Dash";
                case Skill.DoubleJump: return "Double Jump";
                case Skill.Bash: return "Bash";
                case Skill.Stomp: return "Stomp";
                case Skill.Glide: return "Glide";
                case Skill.Climb: return "Climb";
                case Skill.ChargeJump: return "Charge Jump";
                case Skill.Grenade: return "Light Grenade";
            }
            return "N/A";
        }

        #endregion

        private void Tracker_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Scaling = scaling;
            Properties.Settings.Default.Layout = current_layout;
            Properties.Settings.Default.Opacity = this.Opacity;
            Properties.Settings.Default.Shards = this.display_shards;
            Properties.Settings.Default.Pixels = this.image_pixel_size;
            Properties.Settings.Default.Save();
        }


    }
}
