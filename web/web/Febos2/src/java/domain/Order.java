package domain;


import java.io.IOException;
import javax.servlet.ServletException;
import java.util.*;


/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author SofySmo
 */
public class Order{
    
    private ArrayList<OrderItem> list=new ArrayList<OrderItem>();
    public void addItem(int id, int count)
    {   
        //if(list!=null)
        list.add(new OrderItem(id,count));
    }
    public void Order()
    {
       // list=new ArrayList<OrderItem>();
    }
    public void removeItem(int id, int count)
    {
        for(int i=0;i<list.size();i++)
        {
            if(id==list.get(i).getId())
            {
                if(list.get(i).getCount()>count)
                    list.get(i).setCount(list.get(i).getCount()-count);
                else {list.remove(i);}
                break;
            }
            
        }        
    }
    public boolean existItem(int id)
    {
        for(int i=0;i<list.size();i++)
        {
            if(list.get(i).getId()==id)
                return true;
        }
        return false;
    }
    public void changeItem(int id,int count)
    {
        for(int i=0;i<list.size();i++)
        {
            if(list.get(i).getId()==id)
                list.get(i).setCount(list.get(i).getCount()+count);
        }
    }
    public List<OrderItem> getItemList()
    {
        return list;
    }
   public boolean isEmpty()
   {
       return list.isEmpty();
   }
}
