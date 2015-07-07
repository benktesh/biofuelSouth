<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="True" EnableViewState="false" Inherits="aspnetforum.forums" MasterPageFile="AspNetForumMaster.Master" %>
<%@ Import Namespace="aspnetforum.Resources" %>
<%@ Import Namespace="aspnetforum.Utils" %>
<%@ Register TagPrefix="cc" TagName="RecentPosts" Src="recentposts.ascx" %>


<asp:Content ContentPlaceHolderID="AspNetForumContentPlaceHolder" ID="AspNetForumContent" runat="server">
	<asp:Repeater ID="rptGroupsList" Runat="server" EnableViewState="False" OnItemDataBound="rptGroupsList_ItemDataBound">
		<ItemTemplate>
			<table style="width:100%;" class="roundedborder biglist">
				<tr><th colspan="2"><h2><%# Eval("GroupName") %></h2></th><th class="mobilehidden"><%= various.Threads %></th><th class="mobilehidden"><%= various.LatestPost %></th></tr>
				<tbody>
			<asp:repeater id="rptForumsList" runat="server" EnableViewState="False">
				<ItemTemplate>
					<tr <%# Container.ItemType == ListItemType.AlternatingItem ? " class='altItem'" : "" %> >
						<td align="center" style="width:10%;border-right:none;"><img alt="" src="<%# GetForumIcon(Eval("IconFile")) %>" height="32" width="32" /></td>
						<td style="width:55%;border-left:none;padding-left: 0;"><h2>
							<a href='<%# Various.GetForumURL(Eval("ForumID"), Eval("Title")) %>'><%# Eval("Title") %></a>
							</h2>
							<span class="gray2"><%# Eval("Description") %></span>
						</td>
						<td width="50" class="gray2 mobilehidden" style="text-align: center">
							<%# Eval("TopicCount") %></td>
						<td style="white-space:nowrap" class="gray2 mobilehidden">
							<%# Topic.GetTopicInfoBMessageyID(Eval("LatestMessageID"), Cmd)%></td>
					</tr>
				</ItemTemplate>
			</asp:repeater>
			</table>
		</ItemTemplate>
		<FooterTemplate></tbody></FooterTemplate>
	</asp:Repeater>
	<div ID="lblNoForums" style="margin-top:20px;" runat="server" visible="false" enableviewstate="false"><%= various.NoForums %></div>
	<div id="divNoForumsAdmin" style="margin-top:20px;" runat="server" visible="false">No forums have been created yet. Please go to the <a href="admin.aspx">administrator panel</a>.</div>

	<table style="width:100%;" cellpadding="11" cellspacing="0" class="roundedborder">
		<tr><th><h2><%= various.WhatsGoingOn %></h2></th></tr>
		<tbody>
	<tr>
		<td>
			<span class="gray"><%= various.UsersOnline %></span>
			<%= aspnetforum.Utils.User.OnlineUsersCount %>&nbsp;&nbsp;
			<span class="gray"><%= various.Members %></span>
			<%= aspnetforum.Utils.User.OnlineRegisteredUsersCount %>&nbsp;&nbsp;
			<span class="gray"><%= various.Guests %></span>
			<%= aspnetforum.Utils.User.OnlineUsersCount-aspnetforum.Utils.User.OnlineRegisteredUsersCount%>
			<br /><br />
			<span class="gray"><%= various.Threads %></span>
			<%= Various.GetStats().ThreadCount %>&nbsp;&nbsp;
			<span class="gray"><%= various.Posts %></span>
			<%= Various.GetStats().PostCount %>&nbsp;&nbsp;
			<span class="gray"><%= various.Members %></span>
			<%= Various.GetStats().MemberCount %>
		</td>
	</tr>
			</tbody>
	</table>

	<div id="divRecent" runat="server" enableviewstate="false" visible="false">
	<br />
	<cc:RecentPosts id="recentPosts" runat="server"></cc:RecentPosts>
	</div>
</asp:Content>
