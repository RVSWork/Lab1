package org.apache.jsp;

import javax.servlet.*;
import javax.servlet.http.*;
import javax.servlet.jsp.*;
import java.util.Scanner;
import java.io.File;
import java.util.Locale;
import java.util.ResourceBundle;
import java.util.ArrayList;
import domain.*;

public final class ListOfProductsPage_jsp extends org.apache.jasper.runtime.HttpJspBase
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

      out.write('\n');
      out.write("\n");
      out.write("\n");
      out.write("\n");
      out.write("\n");
      out.write("\n");
      out.write("\n");
      out.write("\n");
      out.write("\n");
      out.write("  \n");
      out.write(" ");
 
     Cookie cookies[]=request.getCookies();
     String lang=request.getParameter("lang");    
            if(lang==null)
                lang="ru";
            Locale locale =new Locale(lang);
            Locale localeEn=new Locale("en");
           ResourceBundle res=ResourceBundle.getBundle("page_product", locale);
           ResourceBundle resEn=ResourceBundle.getBundle("page_product", localeEn);
           String genre=request.getParameter("genre_filter");
           if (genre==null) genre="All";
           
           
            ArrayList<Artwork[]> art=new ArrayList<Artwork[]>();
                    Scanner in = new Scanner(new File("C:\\Users\\SofySmo\\Documents\\NetBeansProjects\\Febos2\\src\\java\\filename.txt"));
                    while (in.hasNextLine())
                    {
                       String filename=in.nextLine();
                       art.add(SerializeArt.deserializeArt("C:\\Users\\SofySmo\\Documents\\NetBeansProjects\\Febos2\\src\\java\\"+filename));
                    }
            String cookieName="idirection";
            String direction="Any";
            Cookie myCookie = null;
            
            String idirection=request.getParameter("direction");
            if(idirection==null)
            {
                idirection="0";
            
                if (cookies != null) {
                for (int i = 0; i < cookies.length; i++) {
                    if (cookies[i].getName().equals (cookieName)) {
                        myCookie=cookies[i]; 
                        break;
                    }
                } 
              }  
            if(myCookie!=null)
                idirection=myCookie.getValue();
            }
            if(idirection==null) idirection="0";
                direction=resEn.getString("direction_"+idirection);
                //out.println(direction);
            
            
 
      out.write("\n");
      out.write(" \n");
      out.write("\n");
      out.write("<!DOCTYPE html>\n");
      out.write("<html>\n");
      out.write("    <head>\n");
      out.write("        <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">\n");
      out.write("        <link rel=\"stylesheet\" type=\"text/css\" href=\"style/newcss.css\">\n");
      out.write("        <script type=\"text/javascript\" src=\"js/pageJS.js\"></script>\n");
      out.write("       <script type=\"text/javascript\" src=\"js/masonry.js\"></script>\n");
      out.write("        <title>");
      out.print(res.getString("title_site"));
      out.write("</title>\n");
      out.write("    </head>\n");
      out.write("    <body onunload=\"setCookie('idirection','");
      out.print(idirection);
      out.write("');\">\n");
      out.write("        \n");
      out.write("        <div id=\"main_conteiner\">\n");
      out.write("           <div id=\"header-conteiner\">\n");
      out.write("               \n");
      out.write("               <div id=\"hat_page\">\n");
      out.write("               ");
      org.apache.jasper.runtime.JspRuntimeLibrary.include(request, response, "hat_page.jsp" + "?" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode("lang", request.getCharacterEncoding())+ "=" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode(String.valueOf(lang), request.getCharacterEncoding()) + "&" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode("parant", request.getCharacterEncoding())+ "=" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode("ListOfProductsPage.jsp", request.getCharacterEncoding()), out, false);
      out.write("\n");
      out.write("               </div>\n");
      out.write("           </div>\n");
      out.write("           <div id=\"big_contain\">\n");
      out.write("            <div class=\"for_inline\">\n");
      out.write("                <div id=\"shopfilter\">\n");
      out.write("                    <h3>");
      out.print(res.getString("left-menu_painting"));
      out.write("</h3>\n");
      out.write("                    <ul class=\"menu-list\" id=\"left-navigation\">\n");
      out.write("                        <li class=\"left-navigation-item\">\n");
      out.write("                            <a class=\"left-link\" href=\"http://localhost:8084/Febos2/ListOfProductsPage.jsp?lang=");
      out.print(lang);
      out.write("&direction=");
      out.print(idirection);
      out.write('"');
      out.write('>');
      out.print(res.getString("left-navigation-item_"+0));
      out.write("</a>\n");
      out.write("                        </li>\n");
      out.write("                    ");
for(int i=1;res.containsKey("left-navigation-item_"+i); i++)
                    {
      out.write("\n");
      out.write("                        <li class=\"left-navigation-item\">\n");
      out.write("                            <a class=\"left-link\" href=\"http://localhost:8084/Febos2/ListOfProductsPage.jsp?lang=");
      out.print(lang);
      out.write("&genre_filter=");
      out.print(resEn.getString("left-navigation-item_"+i));
      out.write("&direction=");
      out.print(idirection);
      out.write('"');
      out.write('>');
      out.print(res.getString("left-navigation-item_"+i));
      out.write("</a>\n");
      out.write("                        </li>\n");
      out.write("                    ");
}
      out.write("\n");
      out.write("                    </ul>\n");
      out.write("                </div>\n");
      out.write("                <div class=\"channel-homepage\">\n");
      out.write("                    <div class=\"direction-filter grid-item\">\n");
      out.write("                        <span>");
      out.print(res.getString("info_product_2"));
      out.write("</span>\n");
      out.write("                        <form method=\"GET\">\n");
      out.write("                        <select id=\"direction_select\" onchange=\"itemsChanged('");
      out.print(lang);
      out.write("', '");
      out.print(genre);
      out.write("');\">\n");
      out.write("                        ");
 for(int i=0; res.containsKey("direction_"+i); i++)
                        {
                        if(idirection.equals(Integer.toString(i)))
                            {
      out.write(" <option value=\"");
      out.print(i);
      out.write("\" SELECTED>");
      out.print(res.getString("direction_"+i));
      out.write("</option> \n");
      out.write("                            ");
 } else{ 
      out.write("\n");
      out.write("                            <option value=\"");
      out.print(i);
      out.write('"');
      out.write('>');
      out.print(res.getString("direction_"+i));
      out.write("</option> \n");
      out.write("                        ");
}}
      out.write("\n");
      out.write("                        </select>\n");
      out.write("                        </form>\n");
      out.write("                    </div>\n");
      out.write("                    <div class=\"list_product grid js-masonry\">\n");
      out.write("                    \n");
      out.write("                ");
        
                    
                    for(int j=0; j<art.size(); j++)
                    for(int i=0;i<art.get(j).length; i++)
                    {
                if(genre.equals("All")||(art.get(j)[i].getGenre()!=null&&art.get(j)[i].getGenre().equals(genre)))
                {if(direction.equals("Any")||(art.get(j)[i].getDirection()!=null&&art.get(j)[i].getDirection().equals(direction))){
      out.write("\n");
      out.write("                <a href=\"http://localhost:8084/Febos2/CreatePage?lang=");
      out.print(lang);
      out.write("&container=");
      out.print(j);
      out.write("&index=");
      out.print(i);
      out.write("\">\n");
      out.write("                    <div class=\"product grid-item\">\n");
      out.write("                    ");
      org.apache.jasper.runtime.JspRuntimeLibrary.include(request, response, "product.jsp" + "?" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode("lang", request.getCharacterEncoding())+ "=" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode(String.valueOf(lang), request.getCharacterEncoding()) + "&" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode("name", request.getCharacterEncoding())+ "=" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode(String.valueOf(art.get(j)[i].getName()), request.getCharacterEncoding()) + "&" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode("author", request.getCharacterEncoding())+ "=" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode(String.valueOf(art.get(j)[i].getAuthor()), request.getCharacterEncoding()) + "&" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode("path", request.getCharacterEncoding())+ "=" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode(String.valueOf(art.get(j)[i].getPathartwork() ), request.getCharacterEncoding()) + "&" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode("price", request.getCharacterEncoding())+ "=" + org.apache.jasper.runtime.JspRuntimeLibrary.URLEncode(String.valueOf(art.get(j)[i].getPrice()), request.getCharacterEncoding()), out, false);
      out.write("                       \n");
      out.write("                        <span class=\"buy_product\" onclick=\"\">");
      out.print(res.getString("buy"));
      out.write("</span>\n");
      out.write("                    </div> \n");
      out.write("                </a>       \n");
      out.write("                    ");
}}}
                    
                
      out.write("\n");
      out.write("                </div>\n");
      out.write("                </div>\n");
      out.write("            </div>\n");
      out.write("           </div>\n");
      out.write("           <div class=\"endpage\">\n");
      out.write("                    <p>&copy;2015.</p>\n");
      out.write("           </div>\n");
      out.write("        </div>\n");
      out.write("    </body>\n");
      out.write("</html>\n");
      out.write("     \n");
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
