package ui;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */


import java.io.IOException;
import java.io.PrintWriter;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.util.Locale;
import java.util.ResourceBundle;
import java.util.Enumeration;
import java.util.List;
import json.*;
import domain.*;
import java.io.File;
import java.util.Scanner;


/**
 *
 * @author SofySmo
 */
public class CreatePage extends HttpServlet {

/**
     * Processes requests for both HTTP <code>GET</code> and <code>POST</code>
     * methods.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/html;charset=UTF-8");
        
        try (PrintWriter out = response.getWriter()) {
            /* TODO output your page here. You may use following sample code. */
            StringBuilder sb=new StringBuilder();
            String lang=request.getParameter("lang");
            
            if(lang==null)lang=getInitParameter("lang");
            Locale locale =new Locale(lang);
            Locale localeEn=new Locale("en");
            ResourceBundle res=ResourceBundle.getBundle("page_product", locale);
            ResourceBundle resEn=ResourceBundle.getBundle("page_product", localeEn);
            
            String indexPictureS = request.getParameter("index");
            int indexPicture;
            if(indexPictureS==null)
            {
                indexPicture=Integer.parseInt(getInitParameter("index"));
            }
            else indexPicture=Integer.parseInt(indexPictureS);
            
            String indexFileS=request.getParameter("container");
             int indexFile;
             if(indexFileS==null)
                 {
                    indexFile=Integer.parseInt(getInitParameter("container"));
                 }
             else indexFile=Integer.parseInt(indexFileS);
            Scanner in = new Scanner(new File("C:\\Users\\SofySmo\\Documents\\NetBeansProjects\\Febos2\\src\\java\\filename.txt"));
            String filename="";
                
            for(int i=0;i<indexFile+1&&in.hasNextLine(); i++)
                    {
                       filename=in.nextLine();
                     
                    }
            Artwork art;
            
           art=SerializeArt.deserializeArt("C:\\Users\\SofySmo\\Documents\\NetBeansProjects\\Febos2\\src\\java\\"+filename)[indexPicture];
            
           String revjson=TextFile.read("C:\\Users\\SofySmo\\Documents\\NetBeansProjects\\Febos2\\src\\java\\reviews.json");
           JSONObject jo=new JSONObject(revjson);
            JSONArray comments=jo.getJSONArray("content");
            
            
            String[] picture=new String[7];
            picture[0]=art.getAuthor();
            picture[1]=art.getName();
            picture[2]=art.getDirection();
            picture[3]=art.getGenre();
            picture[4]=art.getSize();
            picture[5]=art.getTechnique();
            picture[6]=art.getCountry();
            
           

            sb.append("<!DOCTYPE html>"
                    + "<html><head><title>"+res.getString("title_site")+"</title><meta charset=\"UTF-8\">"
                    + "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">"
                    + "<link rel=\"stylesheet\" type=\"text/css\" href=\"style/newcss.css\">"
                    + "<link rel=\"stylesheet\" type=\"text/css\" href=\"style/hat_page.css\">"
                    + "<script type=\"text/javascript\" src=\"js/pageJS.js\"></script>"
                    + "<script type=\"text/javascript\" src=\"js/buy_button.js\"></script>"
                    + "</head><body><div id=\"main_conteiner\">"
                    + "<div id=\"header-conteiner\">"
                    
                    
                    +"<div id=\"hat\">"
            
                    +"<div id=\"right_block\">");
                    //for(int i=0; res.containsKey("lang_"+i); i++)
                      //  {
                            sb.append("<a href=\"CartPage.jsp\"><span>"+ res.getString("header_1")+"</span></a> ");
                        //}
                                    
                        sb.append("<div>"
                            +"<form  id=\"form_lang\" name=\"lang_form\" action=\"CreatePage\" method=\"get\">" 
                            +"<select onchange=\"document.lang_form.submit()\" name=\"lang\" id=\"chooseLand\">");
                        
                    for(int i=0; res.containsKey("lang_"+i); i++)
                        {
                            sb.append("<option value=\""+res.getString("l_"+i)+"\">"+res.getString("lang_"+i)+"</option>");
                        }
                        sb.append("</select>"
                   +"</form>"
            +"</div>"
            
        +"</div>" 
        +"<h1>"+res.getString("title_site")+"</h1>"
        +"</div>"
        +"<div id=\"fon_top_menu\">"
                    +"<div id=\"menu-contater\">"
                    +"<nav>"
                        +"<ul class=\"menu_list horizontal_list\" id=\"top_menu\">");
                          for(int i=0; res.containsKey("navigation_menu_item_"+i);i++)
                         {
                         sb.append("<li class=\"navigation-bar-item\">"
                             +"<a class=\"navigerion-bar-link\" href=\"\">"
                             +"<span class=\"navigerion-bar-span\">"+res.getString("navigation_menu_item_"+i)+"</span>"
                             +"</a>"
                         +"</li>");
                         }
                         sb.append("</ul>"
                    +"</nav>"
                    +"</div>"
               +"</div>"
                    
                    
                    
                    
                   
                    
                    +"</div>"
                    +"<div id=big_contain>"
                    + "<div class=\" for_inline\"><div id=\"shopfilter\">"
                    +"<h3>"+res.getString("left-menu_painting")+"</h3><ul class=\"menu-list\" id=\"left-navigation\">"
                    + "<li class=\"left-navigation-item\">\n" +
"                            <a class=\"left-link\" href=\"http://localhost:8084/Febos2/ListOfProductsPage.jsp?lang="+lang+"\">"+res.getString("left-navigation-item_"+0)+"</a>\n" +
"                        </li>");
            for(int i=1;res.containsKey("left-navigation-item_"+i); i++)
                sb.append("<li class=\"left-navigation-item\">"
                    +"<a class=\"left-link\" href=\"http://localhost:8084/Febos2/ListOfProductsPage.jsp?lang="+lang+"&genre_filter="+resEn.getString("left-navigation-item_"+i)+"\">"+res.getString("left-navigation-item_"+i)+"</a></li>");
            sb.append("</ul></div>"
                    + "<div class=\"channel-homepage\">"
                    + "<img id=\"main_picture\" src=\""+art.getPathartwork()+"\" width=\"350\" alt=\""+art.getName()+"\"/>"
                    + "<div class=\"product-attributs\" id=\"main_info\">"
                    + "<h3>"+res.getString("main_charact")+"</h3>");
            
            for(int i=0;res.containsKey("info_product_"+i)&&picture.length>i; i++)
            {   
                String p=res.getString("info_product_"+i);
                 sb.append("<div><div class=\"with_points info_product_item\"><strong>"+p+"</strong></div>"
                    +"<span>"+picture[i]+"</span></div>");
                
            }
            //onclick=\"setCookie('#"+indexFile*100+indexPicture+"','1');\"
            sb.append("<div id=\"buy_div\"><span>"+  Integer.toString(art.getPrice())+"&#36  </span>"
                    
                    + "<span class=\"buy_product\" onclick=\"buyButtonFuck('"+indexFile*100+indexPicture+"');\">"+res.getString("buy")+"</span></div>"
                    + "</div><div id=\"bottom_list\"><ul class=\"horizontal_list\">"
                    + "<li  onclick=show1()>"
                    + "<span id=\"characteristic\" class=\"navigerion-bar-span\">"+res.getString("characteristic")+"</span></li>"
                    + "<li  onclick=show2()>"
                    + "<span id=\"reviews\" class=\"navigerion-bar-span\">"+res.getString("reviews")+"</span></li>"
                    + "</ul></div>"
                    + "<div class=\"product-attributs\" id=\"full_info\" >");
            
            for(int i=0;res.containsKey("info_product_"+i)&&picture.length>i; i++)
            {   
                String p=res.getString("info_product_"+i);
                sb.append("<div><div class=\"with_points info_product_item\"><strong>"+p+"</strong></div>"
                    +"<span>"+picture[i]+"</span></div>");
            }
            sb.append("</div><div id=\"reviews_conteiner\" >");
            for(int i=0;i<comments.length(); i++)
            {
                JSONObject ob=comments.getJSONObject(i);
                sb.append("<div class=\"review\">"
                        + "<div class=\"review_head\">"+ob.getString("name")+"</div>"
                        + "<p class=\"review_text\">"+ob.getString("review")+"</p>"
                        +"</div>");
            }
            sb.append("</div></div></div></div>"
                    + "<div class=\"endpage\""
                    + "<p>&copy;2015 Фебос.</p>"
                    + "</div>"
                    + "</div>");
            sb.append(" </body></html>");
            out.println(sb.toString());
            out.close();
        }
    }

    // <editor-fold defaultstate="collapsed" desc="HttpServlet methods. Click on the + sign on the left to edit the code.">
    /**
     * Handles the HTTP <code>GET</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Handles the HTTP <code>POST</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Returns a short description of the servlet.
     *
     * @return a String containing servlet description
     */
    @Override
    public String getServletInfo() {
        return "Short description";
    }// </editor-fold>

}
