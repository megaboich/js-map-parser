<script type="text/javascript">
    var rootPath = "<%=ResolveUrl("~")%>";

    function clickHandler<%=ClentID%>() {

    }

    <%
    'This should be completely ignored and not cause any errors
    %>

    

    var controlHelper<%:ClientId%> = {
        clientId: "<%ClientId%>",
        click: function () {
        }
    }

</script>

<%--<script type="text/javascript">
    
    This should be completely ignored as well

</script>--%>