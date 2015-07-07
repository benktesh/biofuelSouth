<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="activate.aspx.cs" Inherits="aspnetforum.activate" MasterPageFile="AspNetForumMaster.Master" %>
<%@ Import Namespace="aspnetforum.Resources" %>

<asp:Content ContentPlaceHolderID="AspNetForumContentPlaceHolder" ID="AspNetForumContent" runat="server">

	<div class="location">
		<strong><a href="default.aspx">Home</a> &raquo; Activation </strong>
	</div>
	<asp:Label ID="lblSuccess" runat="server" Visible="false"><%= various.ActivationSuccess %></asp:Label>
	<asp:Label ID="lblError" runat="server" Visible="false"><%= various.ActivationError %></asp:Label>
</asp:Content>