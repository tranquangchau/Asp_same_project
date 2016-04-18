<%@ Page language="c#" Inherits="ImageBrowser.FillCache" EnableSessionState="False" buffer="False" enableViewState="False" CodeFile="FillCache.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FillCache</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<asp:Button id="Fill" runat="server" Text="Fill" onclick="Fill_Click"></asp:Button>
			<asp:Button id="DeleteFill" runat="server" Text="Delete &amp; Fill" onclick="DeleteFill_Click"></asp:Button>
			<asp:Button id="Delete" runat="server" Text="Delete" onclick="Delete_Click"></asp:Button>
		</form>
	</body>
</HTML>
