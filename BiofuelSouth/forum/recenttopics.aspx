<%@ Page Language="C#" AutoEventWireup="true" Title="Recently updated topics" CodeBehind="recenttopics.aspx.cs" Inherits="aspnetforum.recenttopics" MasterPageFile="AspNetForumMaster.Master" %>
<%@ Import Namespace="aspnetforum.Resources" %>
<%@ Import Namespace="aspnetforum.Utils" %>

<asp:Content ContentPlaceHolderID="ContentPlaceHolderHEAD" ID="AspNetForumHead" runat="server">
<link rel="alternate" type="application/rss+xml" title="recent posts" href="recenttopics.aspx?rss=1" />
</asp:Content>

<asp:Content ContentPlaceHolderID="AspNetForumContentPlaceHolder" ID="AspNetForumContent" runat="server">
	<div class="location">
		<h2><a href="default.aspx"><%= various.Home %></a></h2>
	</div>
	<asp:repeater id="rptTopicsList" runat="server" EnableViewState="False">
		<HeaderTemplate>
			<table width="100%" class="roundedborder biglist">
			<tr>
				<th><h2><%= various.RecentTopics %> <a href="recenttopics.aspx?rss=1" runat="server" id="rssLink" enableviewstate="false"><img alt="Recently updated topics - RSS" src="images/rss.png" /></a></h2></th>
				<th nowrap="nowrap"><%= various.LatestPost %></th>
			</tr>
			<tbody>
		</HeaderTemplate>
		<ItemTemplate>
			<tr <%# Container.ItemType == ListItemType.AlternatingItem ? " class='altItem'" : "" %>>
				<td width="70%"><h2><a href='<%# Various.GetTopicURL(Eval("TopicID"), Eval("Subject")) %>'>
							<%# Eval("Subject") %>
						</a>
					</h2>
				</td>
				<td width="30%" style="white-space:nowrap" class="gray2">
					<%# Message.FormatMessageInfo(Eval("LastMessageID"), Eval("TopicID"), Eval("LastUserID"), Eval("LastUserName"), Eval("LastFirstName"), Eval("LastLastName"), Eval("Subject"), Eval("CreationDate"), null, Eval("Body"))%></td>
				</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</tbody></table>
		</FooterTemplate>
	</asp:repeater>
	
	<div class="location">
		<h2><a href="default.aspx">
			<%= various.Home %></a>
		&raquo;
		<%= various.RecentTopics%></h2>
	</div>
</asp:Content>