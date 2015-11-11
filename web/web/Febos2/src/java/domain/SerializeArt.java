package domain;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */


import domain.Artwork;
import java.beans.XMLDecoder;
import java.beans.XMLEncoder;
import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;

/**
 *
 * @author SofySmo
 */
public class SerializeArt{
    public SerializeArt(){}
    public static void serializeArt(Artwork[] arr, String filename) throws FileNotFoundException
    {
        XMLEncoder encoder = new XMLEncoder(new BufferedOutputStream(

            new FileOutputStream(filename, false)));
        encoder.writeObject(arr);
        encoder.close();        
    }
    public static Artwork[] deserializeArt(String filename) throws FileNotFoundException
    {
        
        Artwork[] list;
        int ob=0;
        XMLDecoder decoder = new XMLDecoder(new BufferedInputStream(
            new FileInputStream(filename)));
        Artwork[] obj = (Artwork[])decoder.readObject();
        decoder.close();
        return obj;
    }
    
}
