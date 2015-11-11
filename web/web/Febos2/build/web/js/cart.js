/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
function addCost(cost, count)
{
    var sum=parseInt(document.getElementById('result_cost').innerHTML,10);
    sum+=cost*count;
    document.getElementById('result_cost').innerHTML=sum;   
}
function refuseProduct(i,id,price,count)
{
    //alert(id);
        document.getElementById("product_"+i).style.display='none';
        addCost(-price,count);
        var xhr=new XMLHttpRequest();
        xhr.open('GET', 'http://localhost:8084/Febos2/AddItemServlet?remove='+id+'&count='+count,true);
        xhr.send();
        //alert("nen");
        xhr.onreadystatechange = function() {
            if (this.readyState !== 4) return;
            if (this.status !== 200) {
              alert('Error while removing item to order'+this.status);
              return;
            } 
        } 
}


