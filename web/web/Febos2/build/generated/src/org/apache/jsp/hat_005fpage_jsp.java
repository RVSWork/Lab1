package org.apache.jsp;

import javax.servlet.*;
import javax.servlet.http.*;
import javax.servlet.jsp.*;
import java.util.ResourceBundle;
import java.util.Locale;

public final class hat_005fpage_jsp extends org.apache.jasper.runtime.HttpJspBase
    implements org.apache.jasper.runtime.JspSourceDependent {

  private static final JspFactory _jspxFactory = JspFactory.getDefaultFactory();

  private static java.util.List<String> _jspx_dependants;

  private org.glassfish.jsp.api.ResourceInjector _jspx_resourceInjector;

  public java.util.List<String> getDependants() {
    return _jspx_dependants;
  }

  public void _jspService(HttpServletRequest request, HttpServletResponse response)
        throws java.io.IOException, ServletException {

    PageContext pageContext = null;
    HttpSession session = null;
    ServletContext application = null;
    ServletConfig config = null;
    JspWriter out = null;
    Object page = this;
    JspWriter _jspx_out = null;
    PageContext _jspx_page_context = null;

    try {
      response.setContentType("text/html;charset=UTF-8");
      pageContext = _jspxFactory.getPageContext(this, request, response,
      			null, true, 8192, true);
      _jspx_page_context = pageContext;
      application = pageContext.getServletContext();
      config = pageContext.getServletConfig();
      session = pageContext.getSession();
      out = pageContext.getOut();
      _jspx_out = out;
      _jspx_resourceInjector = (org.glassfish.jsp.api.ResourceInjector) application.getAttribute("com.sun.appserv.jsp.resource.injector");

      out.write("\n");
      out.write("\n");
      out.write("\n");
      out.write("\n");
      out.write("\n");

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
           
           
      out.write("\n");
      out.write("<!DOCTYPE html>\n");
      out.write("<html>\n");
      out.write("    <head>\n");
      out.write("        <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n");
      out.write("        <link rel=\"stylesheet\" type=\"text/css\" href=\"style/franklin-gothic-demi-cond.ttf\">\n");
      out.write("        <link rel=\"stylesheet\" type=\"text/css\" href=\"style/hat_page.css\">\n");
      out.write("    </head>\n");
      out.write("    <body>\n");
      out.write("        <div id=\"hat\">\n");
      out.write("            \n");
      out.write("             <div id=\"right_block\">\n");
      out.write("                 <a href=\"BasketPage.jsp\"><span>");
      out.print(res.getString("header_1"));
      out.write("</span></a>\n");
      out.write("            ");
      out.write("\n");
      out.write("            ");
 if(!user){
            
      out.write("\n");
      out.write("                 <a href=\"Authorization.jsp\"><span>");
      out.print(res.getString("header_3"));
      out.write("</span></a>\n");
      out.write("             ");
}
      out.write("\n");
      out.write("            <div>\n");
      out.write("            <form  id=\"form_lang\" name=\"lang_form\" action=\"");
      out.print(parant);
      out.write("\" method=\"get\"> \n");
      out.write("                    <select onchange=\"document.lang_form.submit()\" name=\"lang\" id=\"chooseLand\">\n");
      out.write("                        \n");
      out.write("                        ");
 for(int i=0; res.containsKey("lang_"+i); i++)
                        {
                            
      out.write(" <option value=");
      out.print(res.getString("l_"+i));
      out.write('>');
      out.print(res.getString("lang_"+i));
      out.write("</option> \n");
      out.write("                        ");
}
      out.write("\n");
      out.write("                        </select>\n");
      out.write("                   </form>\n");
      out.write("            </div>\n");
      out.write("            \n");
      out.write("        </div> \n");
      out.write("                        <h1>");
      out.print(res.getString("title_site"));
      out.write("</h1>\n");
      out.write("        </div>\n");
      out.write("        <div id=\"fon_top_menu\">\n");
      out.write("                    <div id=\"menu-contater\">\n");
      out.write("                    <nav>\n");
      out.write("                        <ul class=\"menu_list horizontal_list\" id=\"top_menu\">\n");
      out.write("                         ");
 for(int i=0; res.containsKey("navigation_menu_item_"+i);i++)
                         {
      out.write("\n");
      out.write("                         <li class=\"navigation-bar-item\">\n");
      out.write("                             <a class=navigerion-bar-link href=\"\">\n");
      out.write("                                 <span class=navigerion-bar-span>");
      out.print(res.getString("navigation_menu_item_"+i));
      out.write("</span>\n");
      out.write("                             </a>\n");
      out.write("                         </li>\n");
      out.write("                         ");
}
      out.write("\n");
      out.write("                         </ul>\n");
      out.write("                    </nav>\n");
      out.write("                    </div>\n");
      out.write("               </div>\n");
      out.write("                         \n");
      out.write("    </body>\n");
      out.write("</html>\n");
    } catch (Throwable t) {
      if (!(t instanceof SkipPageException)){
        out = _jspx_out;
        if (out != null && out.getBufferSize() != 0)
          out.clearBuffer();
        if (_jspx_page_context != null) _jspx_page_context.handlePageException(t);
        else throw new ServletException(t);
      }
    } finally {
      _jspxFactory.releasePageContext(_jspx_page_context);
    }
  }
}
