<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Maintenance.WebForm1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            <%--combine all the script files into a single Composite Script--%>
            <CompositeScript>
                <Scripts>
                    <asp:ScriptReference Path="~/scripts/blah1.min.js" />
                    <asp:ScriptReference Path="~/scripts/blah2.min.js" />
                </Scripts>
            </CompositeScript>
        </asp:ToolkitScriptManager>
        <input id="Button1" type="button" value="button" onclick="fn_fake1(); fn_fake2()" />
    </form>
    <script type="text/javascript" language="javascript">
        function anotherFake() {
            alert("This is the COMMON FAKE fn()");
        }
    </script>
</body>
</html>