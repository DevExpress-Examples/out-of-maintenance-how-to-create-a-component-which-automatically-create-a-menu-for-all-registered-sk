Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraBars

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits XtraForm
		Public Sub New()
			InitializeComponent()
		End Sub
	End Class
	Public Class MyDesignTimeSkinsBarMenuHelper
		Inherits Component

		Private _BarManager As BarManager
		Public Property BarManager() As BarManager
			Get
				Return _BarManager
			End Get
			Set(ByVal value As BarManager)
				_BarManager = value
				CreateMenu()
			End Set
		End Property

		Private _LookAndFeel As DefaultLookAndFeel
		Public Property LookAndFeel() As DefaultLookAndFeel
			Get
				Return _LookAndFeel
			End Get
			Set(ByVal value As DefaultLookAndFeel)
				_LookAndFeel = value
				CreateMenu()
			End Set
		End Property

		Private Sub CreateMenu()

			If BarManager IsNot Nothing AndAlso LookAndFeel IsNot Nothing Then
				If DesignMode Then
					Dim TempMySkinsBarMenuHelper As MySkinsBarMenuHelper = New MySkinsBarMenuHelper(BarManager, LookAndFeel)
				Else
					Dim form As Form = TryCast(BarManager.Form, Form)
					If form IsNot Nothing Then
						AddHandler form.Load, AddressOf form_Load
					End If
				End If
			End If

		End Sub

		Private Sub form_Load(ByVal sender As Object, ByVal e As EventArgs)
			Dim TempMySkinsBarMenuHelper As MySkinsBarMenuHelper = New MySkinsBarMenuHelper(BarManager, LookAndFeel)
		End Sub

		Public Sub New()

		End Sub
	End Class
End Namespace