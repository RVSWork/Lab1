package org.apache.jsp.WEB_002dINF.jsp;

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
      response.setContentType("text/html");
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
            
            
 
      out.write('\n');
      out.write(' ');
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
