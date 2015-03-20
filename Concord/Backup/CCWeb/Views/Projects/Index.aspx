<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
        Inherits="System.Web.Mvc.ViewPage<List<Common.Projects>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Projects
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%using (Html.BeginForm())
  { %>
    <h2>Projects</h2>
    <br />
    <%=Html.ActionLink("Create New Project", "Edit", new { id = -1 })%>
    <br /><br />
    View only status: <%=Html.DropDownList("status", Common.Projects.GetStatusesItemList(0))%><input type="submit" value="Go" />
    <br /><br />
      <table style="border-color:#69523b; border-width:2px; border-style:solid;">
        <tr style="font-weight:bold;">
            <td valign="top" style="padding:5px 5px 5px 5px;display:none;">ID</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Name</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Date</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Status</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Author/Comment</td>
            <td valign="top" style="padding:5px 5px 5px 5px;">Remove?</td></tr>
<%if (Model != null && Model.Count > 0)
  {%>
  <%
    foreach (Common.Projects proj in Model)
    { %>
            <tr><td valign="top" style="padding:5px 5px 5px 5px;display:none;"><%=proj.Id%></td>
                <td valign="top" style="padding:5px 5px 5px 5px;"><%=Html.ActionLink(proj.Name, "Edit", new { id = proj.Id })%></td>
                <td valign="top" style="padding:5px 5px 5px 5px;"><%=proj.SubmitDate.ToString("g")%></td>
                <td valign="top" style="padding:5px 5px 5px 5px;"><%=proj.Status.ToString()%></td><td>
            <%if (proj.Comments != null && proj.Comments.Count > 0)
              { %>
            <table>
               <%foreach (Common.ProjectComment projCom in proj.Comments)
                 { %> 
                    <tr><td style="padding:5px 5px 5px 5px;display:none;" valign="top"><%=projCom.Id%></td>
                        <td style="padding:5px 5px 5px 5px;" valign="top"><%=projCom.Author.UserName%></td>
                        <td style="padding:5px 5px 5px 5px;"><%=projCom.Comment%></td></tr>
               <%} %>
            </table>
            <%} %>
            </td><td valign="top" style="padding:5px 5px 5px 5px;">
                    <%=Html.ActionLink("Remove", "Remove", new { id = proj.Id })%></td></tr>
    <%    }%>
    <%  }
  } %>
      </table>
</asp:Content>
