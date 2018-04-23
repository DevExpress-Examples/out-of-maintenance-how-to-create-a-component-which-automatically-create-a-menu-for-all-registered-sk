Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraBars
Imports DevExpress.LookAndFeel
Imports DevExpress.Utils
Imports DevExpress.Skins
Imports System.Collections

Namespace WindowsApplication1
 Friend Class MySkinsBarMenuHelper


		Private miLookAndFeel_Renamed, miSkin, miOfficeSkin_Renamed, miBonusSkins As BarSubItem
		Private miAllowFormSkins As CheckBarItem

		Private manager_Renamed As BarManager
		Public ReadOnly Property Manager() As BarManager
			Get
				Return manager_Renamed
			End Get
		End Property

		Private lookAndFeel_Renamed As DefaultLookAndFeel
		Public ReadOnly Property LookAndFeel() As DefaultLookAndFeel
			Get
				Return lookAndFeel_Renamed
			End Get
		End Property



		Private ReadOnly Property MIOfficeSkin() As BarSubItem
			Get
				If miOfficeSkin_Renamed Is Nothing Then
					miOfficeSkin_Renamed = New BarSubItem(Me.manager_Renamed, "Off&ice Skins")
					AddHandler miOfficeSkin_Renamed.Popup, AddressOf OnPopupSkinNames
				End If
				Return miOfficeSkin_Renamed
			End Get
		End Property

		Public ReadOnly Property MILookAndFeel() As BarSubItem
			Get
				Return miLookAndFeel_Renamed
			End Get
		End Property


		Public Sub New(ByVal manager As BarManager, ByVal lookAndFeel As DefaultLookAndFeel)
			If manager Is Nothing Then
				Throw New ArgumentNullException("manager")
			End If
			If lookAndFeel Is Nothing Then
				Throw New ArgumentNullException("lookAndFeel")
			End If

			Me.lookAndFeel_Renamed = lookAndFeel
			Me.manager_Renamed = manager
'            this.manager.Images = DevExpress.Utils.Controls.ImageHelper.CreateImageCollectionFromResources("DevExpress.Tutorials.MainDemo.menu.bmp", typeof(LookAndFeelMenu).Assembly, new Size(16, 16), Color.Magenta);
			Me.manager_Renamed.ForceInitialize()
			SetupMenu()
		End Sub

		Private Sub CreateOfficeSkins(ByVal subItem As BarSubItem)
			For Each item As BarItemLink In subItem.ItemLinks
				If item.Caption.IndexOf("Office") > -1 Then
					MIOfficeSkin.AddItem(item.Item)
				End If
			Next item
			If miOfficeSkin_Renamed Is Nothing Then
				Return
			End If
			miLookAndFeel_Renamed.AddItem(miOfficeSkin_Renamed)
			For i As Integer = subItem.ItemLinks.Count - 1 To 0 Step -1
				If subItem.ItemLinks(i).Caption.IndexOf("Office") > -1 Then
					miSkin.RemoveLink(subItem.ItemLinks(i))
				End If
			Next i
		End Sub
		Private bonusSkinNames As String = ";Coffee;Liquid Sky;London Liquid Sky;Glass Oceans;Stardust;Xmas 2008 Blue;Valentine;McSkin;Summer 2008;Pumpkin;Dark Side;Springtime;Darkroom;Foggy;High Contrast;Seven;Sharp;Sharp Plus;Seven Classic;"
		Private ReadOnly Property MIBonusSkin() As BarSubItem
			Get
				If miBonusSkins Is Nothing Then
					miBonusSkins = New BarSubItem(manager_Renamed, "&Bonus Skins")
					AddHandler miBonusSkins.Popup, AddressOf OnPopupSkinNames
				End If
				Return miBonusSkins
			End Get
		End Property
		Private Sub CreateBonusSkins(ByVal subItem As BarSubItem)
			For Each item As BarItemLink In subItem.ItemLinks
				If bonusSkinNames.IndexOf(String.Format(";{0}", item.Caption)) > -1 Then
					MIBonusSkin.AddItem(item.Item)
				End If
			Next item
			If miBonusSkins Is Nothing Then
				Return
			End If
			miLookAndFeel_Renamed.AddItem(miBonusSkins)
			For i As Integer = subItem.ItemLinks.Count - 1 To 0 Step -1
				If bonusSkinNames.IndexOf(String.Format(";{0}", subItem.ItemLinks(i).Caption)) > -1 Then
					miSkin.RemoveLink(subItem.ItemLinks(i))
				End If
			Next i
		End Sub

		Private Sub SetupMenu()
			Dim preview As String
			If Manager.IsDesignMode Then
				preview = " (Preivew Only)"
			Else
				preview = ""
			End If
			miLookAndFeel_Renamed = New BarSubItem(Manager, "&Look And Feel" & preview)
			miAllowFormSkins = New CheckBarItem(Manager, "Allow Form Skins", AddressOf OnSwitchFormSkinStyle_Click)
			miLookAndFeel_Renamed.ItemLinks.Add(miAllowFormSkins)
			miLookAndFeel_Renamed.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&Flat", AddressOf OnSwitchStyle_Click, ActiveLookAndFeelStyle.Flat, LookAndFeelStyle.Flat)).BeginGroup = True
			miLookAndFeel_Renamed.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&Ultra Flat", AddressOf OnSwitchStyle_Click, ActiveLookAndFeelStyle.UltraFlat, LookAndFeelStyle.UltraFlat))
			miLookAndFeel_Renamed.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&Style3D", AddressOf OnSwitchStyle_Click, ActiveLookAndFeelStyle.Style3D, LookAndFeelStyle.Style3D))
			miLookAndFeel_Renamed.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&Office2003", AddressOf OnSwitchStyle_Click, ActiveLookAndFeelStyle.Office2003, LookAndFeelStyle.Office2003))
			miLookAndFeel_Renamed.ItemLinks.Add(New CheckBarItemWithStyle(Manager, "&XP", AddressOf OnSwitchStyle_Click, ActiveLookAndFeelStyle.WindowsXP, LookAndFeelStyle.Skin))
			miSkin = New BarSubItem(Manager, "S&kin")
			AddHandler miLookAndFeel_Renamed.Popup, AddressOf OnPopupLookAndFeel
			AddHandler miSkin.Popup, AddressOf OnPopupSkinNames
			For Each cnt As SkinContainer In SkinManager.Default.Skins
				miSkin.ItemLinks.Add(New CheckBarItem(Manager, cnt.SkinName, AddressOf OnSwitchSkin, ActiveLookAndFeelStyle.Skin))
			Next cnt

			miLookAndFeel_Renamed.ItemLinks.Add(miSkin)

			If Manager.MainMenu IsNot Nothing Then
				Manager.MainMenu.ItemLinks.Add(miLookAndFeel_Renamed)
			End If

			For Each item As BarItemLink In miLookAndFeel_Renamed.ItemLinks
				Dim aItem As CheckBarItemWithStyle = TryCast(item.Item, CheckBarItemWithStyle)
				If aItem IsNot Nothing AndAlso aItem.LookAndFeelStyle = LookAndFeelStyle.Skin Then
					aItem.Enabled = DevExpress.Utils.WXPaint.Painter.ThemesEnabled
				End If
			Next item

			CreateOfficeSkins(miSkin)
			CreateBonusSkins(miSkin)
		End Sub

		Private Sub AddSkinNames()
			Dim arr As New ArrayList()
			For Each cnt As SkinContainer In SkinManager.Default.Skins
				arr.Add(cnt.SkinName)
			Next cnt
			arr.Sort()
			For Each name As String In arr
				miSkin.ItemLinks.Add(New CheckBarItem(Me.manager_Renamed, name, New ItemClickEventHandler(AddressOf OnSwitchSkin), ActiveLookAndFeelStyle.Skin))
			Next name
		End Sub

		

		Protected Overridable Sub OnSwitchFormSkinStyle_Click(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			If SkinManager.AllowFormSkins Then
				SkinManager.DisableFormSkins()
			Else
				SkinManager.EnableFormSkins()
			End If
			DevExpress.LookAndFeel.LookAndFeelHelper.ForceDefaultLookAndFeelChanged()
		End Sub

		Private ReadOnly Property UsingXP() As Boolean
			Get
				Return LookAndFeel.LookAndFeel.UseWindowsXPTheme AndAlso DevExpress.Utils.WXPaint.Painter.ThemesEnabled
			End Get
		End Property

		Private Function AvailableStyle(ByVal style As LookAndFeelStyle) As Boolean
			Return lookAndFeel_Renamed.LookAndFeel.Style = style AndAlso Not UsingXP
		End Function

		Private Sub OnPopupSkinNames(ByVal sender As Object, ByVal e As EventArgs)
			Dim items As BarSubItem = TryCast(sender, BarSubItem)
			For Each item As BarItemLink In items.ItemLinks
				Dim aItem As CheckBarItem = TryCast(item.Item, CheckBarItem)
				If aItem IsNot Nothing Then
					aItem.Checked = AvailableStyle(LookAndFeelStyle.Skin) AndAlso LookAndFeel.LookAndFeel.SkinName = item.Caption
				End If
			Next item
		End Sub

		Private Sub OnPopupLookAndFeel(ByVal sender As Object, ByVal e As EventArgs)
			Dim items As BarSubItem = TryCast(sender, BarSubItem)
			For Each item As BarItemLink In items.ItemLinks
				Dim aItem As CheckBarItemWithStyle = TryCast(item.Item, CheckBarItemWithStyle)
				If aItem IsNot Nothing Then
					If aItem.LookAndFeelStyle = LookAndFeelStyle.Skin Then
						aItem.Checked = UsingXP
					Else
						aItem.Checked = AvailableStyle(aItem.LookAndFeelStyle)
					End If
				End If
			Next item
			'miAllowFormSkins.Checked = DevExpress.Skins.SkinManager.AllowFormSkins;
		End Sub

		Private Sub OnSwitchSkin(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			OnSwitchStyle_Click(sender, e)
			If LookAndFeel IsNot Nothing Then
				LookAndFeel.LookAndFeel.SetSkinStyle(e.Item.Caption)
			End If
		End Sub

		Protected Overridable Sub OnSwitchStyle_Click(ByVal sender As Object, ByVal e As ItemClickEventArgs)
			Dim item As CheckBarItem = TryCast(e.Item, CheckBarItem)
			Dim wxp As Boolean = item.Style = ActiveLookAndFeelStyle.WindowsXP
			LookAndFeel.LookAndFeel.SetStyle(CType(item.Style, LookAndFeelStyle), wxp, LookAndFeel.LookAndFeel.UseDefaultLookAndFeel, LookAndFeel.LookAndFeel.SkinName)
		End Sub

		Private Class OptionBarItem
			Inherits BarCheckItem
			Private optionItem As Boolean = False
			Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler)
				Me.New(manager, text, handler, True)
			End Sub
			Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler, ByVal optionItem As Boolean)
				Me.optionItem = optionItem
				Me.Manager = manager
				Caption = text
				AddHandler ItemClick, handler
			End Sub
			Public ReadOnly Property IsOption() As Boolean
				Get
					Return optionItem
				End Get
			End Property
		End Class
		Private Class CheckBarItem
			Inherits OptionBarItem
			Private style_Renamed As ActiveLookAndFeelStyle
			Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler)
				Me.New(manager, text, handler, ActiveLookAndFeelStyle.Flat)
			End Sub
			Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler, ByVal style As ActiveLookAndFeelStyle)
				MyBase.New(manager, text, handler, False)
				Me.style_Renamed = style
			End Sub
			Public ReadOnly Property Style() As ActiveLookAndFeelStyle
				Get
					Return style_Renamed
				End Get
			End Property

		End Class
		Private Class CheckBarItemWithStyle
			Inherits CheckBarItem
			Private lfStyle As LookAndFeelStyle
			Public Sub New(ByVal manager As BarManager, ByVal text As String, ByVal handler As ItemClickEventHandler, ByVal style As ActiveLookAndFeelStyle, ByVal lfStyle As LookAndFeelStyle)
				MyBase.New(manager, text, handler, style)
				Me.lfStyle = lfStyle
			End Sub
			Public ReadOnly Property LookAndFeelStyle() As LookAndFeelStyle
				Get
					Return lfStyle
				End Get
			End Property
		End Class
 End Class
End Namespace

