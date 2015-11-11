/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package domain;

/**
 *
 * @author SofySmo
 */

public class OrderItem {
    private int id;
    private int count;
    public OrderItem(int newid, int newcount)
    {
        id=newid;
        count=newcount;
    }
    public void setId(int newid)
    {
        id=newid;
    }
    public void setCount(int newcount)
    {
        count=newcount;
    }
   public int getId()
   {
       return id;
   }
   public int getCount()
           {
               return count;
           }
}
