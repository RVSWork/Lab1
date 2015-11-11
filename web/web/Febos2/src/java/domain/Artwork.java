package domain;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */


/**
 *
 * @author SofySmo
 */
public class Artwork implements java.io.Serializable {
    private String name;
    //private String som;
    private String author;
    private int price;
    private String direction;
    private String size;
    private String technique;
    private String country;
    private String pathartwork;
    private String genre;
    
    
    public Artwork(){}
    
   public void setName(String t){name=t;}
  // public void setSom(String t){name=t;}
    public void setAuthor(String t){ author=t;}
    public void setPrice(int t){ price=t;}
    public void setDirection(String t){direction=t;}
    public void setSize(String t){size=t;}
    public void setTechnique(String t){technique=t;}
    public void setCountry(String t){country=t;}
    public void setPathartwork(String t){pathartwork=t;}
    public void setGenre(String t){genre=t;}
    
    public String getName(){return name;}
    public String getAuthor(){return author;}
    public int getPrice(){return price;}
    public String getDirection(){return direction;}
    public String getSize(){return size;}
    public String getTechnique(){return technique;}
    public String getCountry(){return country;}
    public String getPathartwork(){return pathartwork;}
    public String getGenre(){return genre;}
}
