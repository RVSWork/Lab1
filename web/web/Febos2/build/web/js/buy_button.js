/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
function buyButtonFuck(id)
{
    //alert(id);
    var xhr=new XMLHttpRequest();
    xhr.open('GET', 'http://localhost:8084/Febos2/AddItemServlet?add='+id+'&count=1',true);
    xhr.send();
    
    xhr.onreadystatechange = function() {
        if (this.readyState !== 4) return;


        if (this.status !== 200) {
          alert('Error while adding item to order'+this.status);
          return;
        } 
    }
}
//var buyButtons = document.getElementsByClassName("buy_product");
//for(var i = 0; i < buyButtons.length; i++){
  //  buyButtons[i].onclick = buyButtonFunc;
//}


