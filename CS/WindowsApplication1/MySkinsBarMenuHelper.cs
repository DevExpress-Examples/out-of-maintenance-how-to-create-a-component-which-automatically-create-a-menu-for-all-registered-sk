using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Skins;
using System.Collections;

namespace WindowsApplication1
{
 class MySkinsBarMenuHelper 
 {


        BarSubItem miLookAndFeel, miSkin, miOfficeSkin, miBonusSkins;
		CheckBarItem miAllowFormSkins;
				
        BarManager manager;
        public BarManager Manager {
            get { return manager; }
        }

        DefaultLookAndFeel lookAndFeel;
        public DefaultLookAndFeel LookAndFeel { get { return lookAndFeel; } }

     

        BarSubItem MIOfficeSkin
        {
            get
            {
                if (miOfficeSkin == null)
                {
                    miOfficeSkin = new BarSubItem(this.manager, "Off&ice Skins");
                    miOfficeSkin.Popup += new EventHandler(OnPopupSkinNames);
                }
                return miOfficeSkin;
            }
        }

        public BarSubItem MILookAndFeel {
            get { return miLookAndFeel; }
        }


        public MySkinsBarMenuHelper(BarManager manager, DefaultLookAndFeel lookAndFeel) {
            if(manager == null)
                throw new ArgumentNullException("manager");
            if(lookAndFeel == null)
                throw new ArgumentNullException("lookAndFeel");

            this.lookAndFeel = lookAndFeel;
			this.manager = manager;
//            this.manager.Images = DevExpress.Utils.Controls.ImageHelper.CreateImageCollectionFromResources("DevExpress.Tutorials.MainDemo.menu.bmp", typeof(LookAndFeelMenu).Assembly, new Size(16, 16), Color.Magenta);
			this.manager.ForceInitialize();
            SetupMenu();
        }

        private void CreateOfficeSkins(BarSubItem subItem)
        {
            foreach (BarItemLink item in subItem.ItemLinks)
            {
                if (item.Caption.IndexOf("Office") > -1)
                    MIOfficeSkin.AddItem(item.Item);
            }
            if (miOfficeSkin == null) return;
            miLookAndFeel.AddItem(miOfficeSkin);
            for (int i = subItem.ItemLinks.Count - 1; i >= 0; i--)
            {
                if (subItem.ItemLinks[i].Caption.IndexOf("Office") > -1)
                    miSkin.RemoveLink(subItem.ItemLinks[i]);
            }
        }
        string bonusSkinNames = ";Coffee;Liquid Sky;London Liquid Sky;Glass Oceans;Stardust;Xmas 2008 Blue;Valentine;McSkin;Summer 2008;Pumpkin;Dark Side;Springtime;Darkroom;Foggy;High Contrast;Seven;Sharp;Sharp Plus;Seven Classic;";
        BarSubItem MIBonusSkin
        {
            get
            {
                if (miBonusSkins == null)
                {
                    miBonusSkins = new BarSubItem(manager, "&Bonus Skins");
                    miBonusSkins.Popup += OnPopupSkinNames;
                }
                return miBonusSkins;
            }
        }
        private void CreateBonusSkins(BarSubItem subItem)
        {
            foreach (BarItemLink item in subItem.ItemLinks)
            {
                if (bonusSkinNames.IndexOf(string.Format(";{0}", item.Caption)) > -1)
                    MIBonusSkin.AddItem(item.Item);
            }
            if (miBonusSkins == null) return;
            miLookAndFeel.AddItem(miBonusSkins);
            for (int i = subItem.ItemLinks.Count - 1; i >= 0; i--)
            {
                if (bonusSkinNames.IndexOf(string.Format(";{0}", subItem.ItemLinks[i].Caption)) > -1)
                    miSkin.RemoveLink(subItem.ItemLinks[i]);
            }
        }

        void SetupMenu()
        {
            string preview = Manager.IsDesignMode ? " (Preivew Only)" : "";
            miLookAndFeel = new BarSubItem(Manager, "&Look And Feel" + preview);
            miAllowFormSkins = new CheckBarItem(Manager, "Allow Form Skins", OnSwitchFormSkinStyle_Click);
            miLookAndFeel.ItemLinks.Add(miAllowFormSkins);
            miLookAndFeel.ItemLinks.Add(new CheckBarItemWithStyle(Manager, "&Flat", OnSwitchStyle_Click, ActiveLookAndFeelStyle.Flat, LookAndFeelStyle.Flat)).BeginGroup = true;
            miLookAndFeel.ItemLinks.Add(new CheckBarItemWithStyle(Manager, "&Ultra Flat", OnSwitchStyle_Click, ActiveLookAndFeelStyle.UltraFlat, LookAndFeelStyle.UltraFlat));
            miLookAndFeel.ItemLinks.Add(new CheckBarItemWithStyle(Manager, "&Style3D", OnSwitchStyle_Click, ActiveLookAndFeelStyle.Style3D, LookAndFeelStyle.Style3D));
            miLookAndFeel.ItemLinks.Add(new CheckBarItemWithStyle(Manager, "&Office2003", OnSwitchStyle_Click, ActiveLookAndFeelStyle.Office2003, LookAndFeelStyle.Office2003));
            miLookAndFeel.ItemLinks.Add(new CheckBarItemWithStyle(Manager, "&XP", OnSwitchStyle_Click, ActiveLookAndFeelStyle.WindowsXP, LookAndFeelStyle.Skin));
            miSkin = new BarSubItem(Manager, "S&kin");
            miLookAndFeel.Popup += OnPopupLookAndFeel;
            miSkin.Popup += OnPopupSkinNames;
            foreach (SkinContainer cnt in SkinManager.Default.Skins)
                miSkin.ItemLinks.Add(new CheckBarItem(Manager, cnt.SkinName, OnSwitchSkin, ActiveLookAndFeelStyle.Skin));

            miLookAndFeel.ItemLinks.Add(miSkin);

            if (Manager.MainMenu != null)
                Manager.MainMenu.ItemLinks.Add(miLookAndFeel);

            foreach (BarItemLink item in miLookAndFeel.ItemLinks)
            {
                CheckBarItemWithStyle aItem = item.Item as CheckBarItemWithStyle;
                if (aItem != null && aItem.LookAndFeelStyle == LookAndFeelStyle.Skin)
                    aItem.Enabled = DevExpress.Utils.WXPaint.Painter.ThemesEnabled;
            }

            CreateOfficeSkins(miSkin);
            CreateBonusSkins(miSkin);
        }

        void AddSkinNames()
        {
            ArrayList arr = new ArrayList();
            foreach (SkinContainer cnt in SkinManager.Default.Skins)
                arr.Add(cnt.SkinName);
            arr.Sort();
            foreach (string name in arr)
            {
                miSkin.ItemLinks.Add(new CheckBarItem(this.manager, name, new ItemClickEventHandler(OnSwitchSkin), ActiveLookAndFeelStyle.Skin));
            }
        }


        protected virtual void OnSwitchFormSkinStyle_Click(object sender, ItemClickEventArgs e) {
            if(SkinManager.AllowFormSkins)
                SkinManager.DisableFormSkins();
            else 
                SkinManager.EnableFormSkins();
			DevExpress.LookAndFeel.LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
        }
		
		private bool UsingXP {
			get { return LookAndFeel.LookAndFeel.UseWindowsXPTheme && DevExpress.Utils.WXPaint.Painter.ThemesEnabled; }
		}

		bool AvailableStyle(LookAndFeelStyle style) {
			return lookAndFeel.LookAndFeel.Style == style && !UsingXP;
		}

		void OnPopupSkinNames(object sender, EventArgs e) {
			BarSubItem items = sender as BarSubItem;
			foreach(BarItemLink item in items.ItemLinks) {
				CheckBarItem aItem = item.Item as CheckBarItem;
				if(aItem != null)
					aItem.Checked = AvailableStyle(LookAndFeelStyle.Skin) && LookAndFeel.LookAndFeel.SkinName == item.Caption;
			}
		}

		void OnPopupLookAndFeel(object sender, EventArgs e) {
			BarSubItem items = sender as BarSubItem;
			foreach(BarItemLink item in items.ItemLinks) { 
				CheckBarItemWithStyle aItem = item.Item as CheckBarItemWithStyle;
				if(aItem != null) {
					if(aItem.LookAndFeelStyle == LookAndFeelStyle.Skin) 
						aItem.Checked = UsingXP; 
					else
						aItem.Checked = AvailableStyle(aItem.LookAndFeelStyle);
				}
			}
            //miAllowFormSkins.Checked = DevExpress.Skins.SkinManager.AllowFormSkins;
		}

		void OnSwitchSkin(object sender, ItemClickEventArgs e) {
            OnSwitchStyle_Click(sender, e);
			if(LookAndFeel != null) LookAndFeel.LookAndFeel.SetSkinStyle(e.Item.Caption);
		}

        protected virtual void OnSwitchStyle_Click(object sender, ItemClickEventArgs e) {
            CheckBarItem item = e.Item as CheckBarItem;
            bool wxp = item.Style == ActiveLookAndFeelStyle.WindowsXP;
            LookAndFeel.LookAndFeel.SetStyle((LookAndFeelStyle)item.Style, wxp, LookAndFeel.LookAndFeel.UseDefaultLookAndFeel, LookAndFeel.LookAndFeel.SkinName);
        }

        class OptionBarItem : BarCheckItem {
            bool optionItem = false;
            public OptionBarItem(BarManager manager, string text, ItemClickEventHandler handler) : this(manager, text, handler, true) { }
            public OptionBarItem(BarManager manager, string text, ItemClickEventHandler handler, bool optionItem) {
                this.optionItem = optionItem;
                Manager = manager;
                Caption = text;
                ItemClick += handler;
            }
            public bool IsOption { get { return optionItem; } }
        }
        class CheckBarItem : OptionBarItem {
            ActiveLookAndFeelStyle style;
            public CheckBarItem(BarManager manager, string text, ItemClickEventHandler handler) : this(manager, text, handler, ActiveLookAndFeelStyle.Flat) { }
            public CheckBarItem(BarManager manager, string text, ItemClickEventHandler handler, ActiveLookAndFeelStyle style)
                : base(manager, text, handler, false) {
                this.style = style;
            }
            public ActiveLookAndFeelStyle Style { get { return style; } }

        }
        class CheckBarItemWithStyle : CheckBarItem {
            LookAndFeelStyle lfStyle;
            public CheckBarItemWithStyle(BarManager manager, string text, ItemClickEventHandler handler, ActiveLookAndFeelStyle style, LookAndFeelStyle lfStyle)
                : base(manager, text, handler, style) {
                this.lfStyle = lfStyle;
            }
            public LookAndFeelStyle LookAndFeelStyle { get { return lfStyle; } }
        }
    }
}

