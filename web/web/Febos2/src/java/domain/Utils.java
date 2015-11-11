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
import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

public  class Utils {
    public static Artwork getArt(int indexFile,int indexPicture) throws FileNotFoundException
    {
    Scanner in = new Scanner(new File("C:\\Users\\SofySmo\\Documents\\NetBeansProjects\\Febos2\\src\\java\\filename.txt"));
            String filename="";
                
            for(int i=0;i<indexFile+1&&in.hasNextLine(); i++)
                    {
                       filename=in.nextLine();
                     
                    }            
           return SerializeArt.deserializeArt("C:\\Users\\SofySmo\\Documents\\NetBeansProjects\\Febos2\\src\\java\\"+filename)[indexPicture];
    }
    public static String tttt(int i)
    {
        return "gfff";
    }
}
