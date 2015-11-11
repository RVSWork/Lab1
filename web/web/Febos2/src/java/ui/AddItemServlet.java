/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ui;

import domain.*;
import java.io.IOException;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import org.springframework.web.bind.annotation.RequestMapping;

/**
 *
 * @author SofySmo
 */
@WebServlet(name = "AddItemServlet", urlPatterns = {"/AddItemServlet"})
public class AddItemServlet extends HttpServlet {

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
       HttpSession session=request.getSession();
        
       int count=0;
       try
       {
           if(request.getParameter("count")!=null)
           {
               count=Integer.parseInt(request.getParameter("count"));
           }
           else
           {response.sendError(401); return;}
       }catch(NumberFormatException e)
       {
           response.sendError(422);
           return;
       }
      // Order order=null;
       Order order=(Order)request.getSession().getAttribute("order");
            if(order==null)
            {
                order=new Order();
                request.getSession().setAttribute("order", order); 
            }
       
       if(request.getParameter("add")!=null)
       {     
           int id=Integer.parseInt(request.getParameter("add"));
           if (!order.existItem(id))
            order.addItem(id,count);
            else order.changeItem(id, count);
            response.sendError(200);
            return;
       }else
           if (request.getParameter("remove")!=null)
               {
                   int id=Integer.parseInt(request.getParameter("remove"));
                   order.removeItem(id,count);
                   
                   //request.getSession();
               }
           else response.sendError(400);
           response.sendError(200);
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
