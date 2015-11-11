<%-- 
    Document   : hat_page
    Created on : 23.10.2015, 16:48:11
    Author     : SofySmo
<link rel="stylesheet" type="text/css" href="newcss.css">
--%>

<%@page import="java.util.ResourceBundle"%>
<%@page import="java.util.Locale"%>
<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%
String lang=request.getParameter("lang");    
            if(lang==null)lang="ru";
            Locale locale =new Locale(lang);
            
           ResourceBundle res=ResourceBundle.getBundle("page_product", locale);
           String parant=request.getParameter("parant");
           Cookie cookies[]=request.getCookies();
           String cookieName="user";
           boolean user=false;
           if (cookies != null) {
                for (int i = 0; i < cookies.length; i++) {
                    if (cookies[i].getName().equals (cookieName)) {
                        user=true;
                        break;
                    }
                } 
           }
           
           %>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <link rel="stylesheet" type="text/css" href="style/franklin-gothic-demi-cond.ttf">
        <link rel="stylesheet" type="text/css" href="style/hat_page.css">
    </head>
    <body>
        <div id="hat">
            
             <div id="right_block">
                 <a href="CartPage.jsp"><span><%=res.getString("header_1")%></span></a>
            <%-- for(int i=0; res.containsKey("lang_"+i); i++)
                        {
                            %> <span> <%=res.getString("header_"+i)%></span> 
                        <%}--%>
            <% if(!user){
            %>
                 <a href="Authorization.jsp"><span><%=res.getString("header_3")%></span></a>
             <%}%>
            <div>
            <form  id="form_lang" name="lang_form" action="<%=parant%>" method="get"> 
                    <select onchange="document.lang_form.submit()" name="lang" id="chooseLand">
                        
                        <% for(int i=0; res.containsKey("lang_"+i); i++)
                        {
                            %> <option value=<%=res.getString("l_"+i)%>><%=res.getString("lang_"+i)%></option> 
                        <%}%>
                        </select>
                   </form>
            </div>
            
        </div> 
                        <h1><%=res.getString("title_site")%></h1>
        </div>
        <div id="fon_top_menu">
                    <div id="menu-contater">
                    <nav>
                        <ul class="menu_list horizontal_list" id="top_menu">
                         <% for(int i=0; res.containsKey("navigation_menu_item_"+i);i++)
                         {%>
                         <li class="navigation-bar-item">
                             <a class=navigerion-bar-link href="">
                                 <span class=navigerion-bar-span><%=res.getString("navigation_menu_item_"+i)%></span>
                             </a>
                         </li>
                         <%}%>
                         </ul>
                    </nav>
                    </div>
               </div>
                         
    </body>
</html>
